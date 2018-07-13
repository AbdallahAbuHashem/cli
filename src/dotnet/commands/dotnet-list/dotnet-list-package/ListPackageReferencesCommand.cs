// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Linq;
using Microsoft.Build.Evaluation;
using Microsoft.DotNet.Cli;
using Microsoft.DotNet.Cli.CommandLine;
using Microsoft.DotNet.Cli.Utils;
using Microsoft.DotNet.Tools.Common;

namespace Microsoft.DotNet.Tools.List.PackageReferences
{
    internal class ListPackageReferencesCommand : CommandBase
    {
        private readonly string _fileOrDirectory;

        public ListPackageReferencesCommand(
            AppliedOption appliedCommand,
            ParseResult parseResult) : base(parseResult)
        {
            if (appliedCommand == null)
            {
                throw new ArgumentNullException(nameof(appliedCommand));
            }

            _fileOrDirectory = appliedCommand.Arguments.Count == 0 ? PathUtility.EnsureTrailingSlash(Directory.GetCurrentDirectory()) : appliedCommand.Arguments.Single();
        }

        public override int Execute()
        {

            return 0;
        }
    }
}
