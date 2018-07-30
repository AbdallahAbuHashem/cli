// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Linq;
using Microsoft.DotNet.Cli.CommandLine;
using Microsoft.DotNet.Tools;
using LocalizableStrings = Microsoft.DotNet.Tools.List.PackageReferences.LocalizableStrings;

namespace Microsoft.DotNet.Cli
{
    internal static class ListPackageReferencesCommandParser
    {
        public static Command ListPackageReferences() => Create.Command(
                "package",
                LocalizableStrings.AppFullName,
                Accept.ZeroOrOneArgument(),
                CommonOptions.HelpOption(),
                Create.Option("-o|--outdated",
                              LocalizableStrings.CmdOutdatedDescription),
                Create.Option("-f|--framework",
                              LocalizableStrings.CmdFrameworkDescription,
                              Accept.ExactlyOneArgument()
                                    .With(name: LocalizableStrings.CmdFramework)
                                    .ForwardAsSingle(o => $"--framework {o.Arguments.Single()}")),
                Create.Option("-d|--deprecated",
                              LocalizableStrings.CmdDeprecatedDescription),
                Create.Option("-i|--include-transitive",
                              LocalizableStrings.CmdTransitiveDescription));
    }
}
