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
                Create.Option("--outdated",
                              LocalizableStrings.CmdOutdatedDescription),
                Create.Option("--framework",
                              LocalizableStrings.CmdFrameworkDescription,
                              Accept.ExactlyOneArgument()
                                    .With(name: LocalizableStrings.CmdFramework)
                                    .ForwardAsSingle(o => $"--framework {o.Arguments.Single()}")),
                Create.Option("--deprecated",
                              LocalizableStrings.CmdDeprecatedDescription),
                Create.Option("--include-transitive",
                              LocalizableStrings.CmdTransitiveDescription),
                Create.Option("--include-prerelease",
                              LocalizableStrings.CmdPrereleaseDescription),
                Create.Option("--highest-patch",
                              LocalizableStrings.CmdHighestPatchDescription),
                Create.Option("--highest-minor",
                              LocalizableStrings.CmdHighestMinorDescription),
                Create.Option("--config",
                              LocalizableStrings.CmdConfigDescription,
                              Accept.ExactlyOneArgument()
                                    .With(name: LocalizableStrings.CmdConfig)
                                    .ForwardAsSingle(o => $"--config {o.Arguments.Single()}")),
                Create.Option("--source",
                              LocalizableStrings.CmdSourceDescription,
                              Accept.ExactlyOneArgument()
                                    .With(name: LocalizableStrings.CmdSource)
                                    .ForwardAsSingle(o => $"--source {o.Arguments.Single()}")));
    }
}
