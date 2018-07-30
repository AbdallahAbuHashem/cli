// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using FluentAssertions;
using Microsoft.Build.Construction;
using Microsoft.DotNet.Tools;
using Microsoft.DotNet.Tools.Test.Utilities;
using Msbuild.Tests.Utilities;
using System;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.DotNet.Cli.List.Package.Tests
{
    public class GivenDotnetListPackage : TestBase
    {
        private readonly ITestOutputHelper _output;

        public GivenDotnetListPackage(ITestOutputHelper output)
        {
            _output = output;
        }



        [Fact]
        public void RequestedAndResolvedVersionsMatch()
        {
            var testAsset = "TestAppSimple";
            var projectDirectory = TestAssets
                .Get(testAsset)
                .CreateInstance()
                .WithSourceFiles()
                .Root
                .FullName;

            var packageName = "Newtonsoft.Json";
            var packageVersion = "9.0.1";
            var cmd = new DotnetCommand()
                .WithWorkingDirectory(projectDirectory)
                .ExecuteWithCapturedOutput($"add package {packageName} --version {packageVersion}");
            cmd.Should().Pass();

            new RestoreCommand()
               .WithWorkingDirectory(projectDirectory)
               .Execute()
               .Should()
               .Pass()
               .And.NotHaveStdErr();

            new ListPackageCommand()
                .WithPath(projectDirectory)
                .Execute()
                .Should()
                .Pass()
                .And.NotHaveStdErr()
                .And.HaveStdOutContainingIgnoreSpaces(packageName+packageVersion+packageVersion);
        }

        [Fact]
        public void AutoReferencedPackages()
        {
            var testAsset = "TestAppSimple";
            var projectDirectory = TestAssets
                .Get(testAsset)
                .CreateInstance()
                .WithSourceFiles()
                .Root
                .FullName;

            new RestoreCommand()
               .WithWorkingDirectory(projectDirectory)
               .Execute()
               .Should()
               .Pass()
               .And.NotHaveStdErr();

            new ListPackageCommand()
                .WithPath(projectDirectory)
                .Execute()
                .Should()
                .Pass()
                .And.NotHaveStdErr()
                .And.HaveStdOutContainingIgnoreSpaces("Microsoft.NETCore.App(A)")
                .And.HaveStdOutContainingIgnoreSpaces("A:Auto-referencedpackage");
        }

        [Fact]
        public void RunOnSolution()
        {
            var sln = "TestAppWithSlnAndSolutionFolders";
            var projectDirectory = TestAssets
                .Get(sln)
                .CreateInstance()
                .WithSourceFiles()
                .Root
                .FullName;

            new RestoreCommand()
               .WithWorkingDirectory(projectDirectory)
               .Execute()
               .Should()
               .Pass()
               .And.NotHaveStdErr();

            new ListPackageCommand()
                .WithPath(projectDirectory)
                .Execute()
                .Should()
                .Pass()
                .And.NotHaveStdErr()
                .And.HaveStdOutContainingIgnoreSpaces("Microsoft.NETCore.App");
        }

    }
}
