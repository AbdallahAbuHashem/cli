// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

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
                Accept.ZeroOrOneArgument()
                .With(
                    name: CommonLocalizableStrings.SolutionOrProjectArgumentName,
                    description: CommonLocalizableStrings.SolutionOrProjectArgumentDescription)
                .DefaultToCurrentDirectory(),
                CommonOptions.HelpOption());
    }
}
