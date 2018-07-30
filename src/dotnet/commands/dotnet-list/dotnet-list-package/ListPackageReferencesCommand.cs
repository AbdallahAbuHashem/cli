// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.Build.Evaluation;
using Microsoft.DotNet.Cli;
using Microsoft.DotNet.Cli.CommandLine;
using Microsoft.DotNet.Cli.Utils;
using Microsoft.DotNet.Tools.Common;
using Microsoft.DotNet.Tools.NuGet;

namespace Microsoft.DotNet.Tools.List.PackageReferences
{
    internal class ListPackageReferencesCommand : CommandBase
    {
        private readonly string _fileOrDirectory;
        private AppliedOption _appliedCommand;

        public ListPackageReferencesCommand(
            AppliedOption appliedCommand,
            ParseResult parseResult) : base(parseResult)
        {
            if (appliedCommand == null)
            {
                throw new ArgumentNullException(nameof(appliedCommand));
            }

            _fileOrDirectory = appliedCommand.Arguments.Count == 0 ?
                               PathUtility.EnsureTrailingSlash(Directory.GetCurrentDirectory()) :
                               PathUtility.GetAbsolutePath(PathUtility.EnsureTrailingSlash(Directory.GetCurrentDirectory()),
                                                           appliedCommand.Arguments.Single());

            FileAttributes attr = File.GetAttributes(_fileOrDirectory);

            if (attr.HasFlag(FileAttributes.Directory))
            {
                _fileOrDirectory = PathUtility.EnsureTrailingSlash(_fileOrDirectory);
            }

            _appliedCommand = appliedCommand["package"];
        }

        public override int Execute()
        {
            var result = NuGetCommand.Run(TransformArgs());

            return result;
        }

        private string[] TransformArgs()
        {
            var args = new List<string>
            {
                "package",
                "list",
            };

            args.Add(GetProjectOrSolution());

            if (_appliedCommand.HasOption("outdated"))
            {
                args.Add("--outdated");
            }

            if (_appliedCommand.HasOption("deprecated"))
            {
                args.Add("--deprecated");
            }

            if (_appliedCommand.HasOption("include-transitive"))
            {
                args.Add("--include-transitive");
            }

            if (_appliedCommand.HasOption("framework"))
            {
                args.Add("--framework");
                args.Add(_appliedCommand
                .AppliedOptions["framework"]
                .Arguments
                .Single());
            }

            return args.ToArray();
        }

        private string GetProjectOrSolution()
        {
            string resultPath = _fileOrDirectory;
            FileAttributes attr = File.GetAttributes(resultPath);
            
            if (attr.HasFlag(FileAttributes.Directory))
            {
                var possibleSolutionPath = Directory.GetFiles(resultPath, "*.sln", SearchOption.TopDirectoryOnly);

                if (possibleSolutionPath.Count() > 1)
                {
                    throw new Exception(LocalizableStrings.MultipleSolutionsFound + Environment.NewLine + string.Join(Environment.NewLine, possibleSolutionPath.ToArray()));
                }
                else if (possibleSolutionPath.Count() == 1)
                {
                    return possibleSolutionPath[0];
                }
                else
                {
                    var possibleProjectPath = Directory.GetFiles(resultPath, "*.*proj", SearchOption.TopDirectoryOnly)
                                              .Where(path => !path.EndsWith(".xproj", StringComparison.OrdinalIgnoreCase))
                                              .ToList();

                    if (possibleProjectPath.Count() == 0)
                    {
                        throw new Exception(LocalizableStrings.NoProjectsOrSolutions);
                    }
                    else if (possibleProjectPath.Count() == 1)
                    {
                        return possibleProjectPath[0];
                    }
                    else
                    {
                        throw new Exception(LocalizableStrings.MultipleProjectsFound + Environment.NewLine + string.Join(Environment.NewLine, possibleProjectPath.ToArray()));
                    }
                }
            }
          
            if (!File.Exists(resultPath))
            {
                throw new FileNotFoundException();
            }
            
            return resultPath;
        }
    }
}
