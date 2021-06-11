// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using System.Linq;
using FluentAssertions;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Configuration.Test
{
    public class SearchTreeTests
    {
        [Fact]
        public void SearchTree_WithOneSource()
        {
            // Arrange
            using var mockBaseDirectory = TestDirectory.Create();
            var configPath1 = Path.Combine(mockBaseDirectory, "NuGet.Config");
            SettingsTestUtils.CreateConfigurationFile(configPath1, @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageNamespaces>
        <clear />
        <packageSource key=""nuget.org"">
            <namespace id=""stuff"" />
        </packageSource>
    </packageNamespaces>
</configuration>");
            var settings = Settings.LoadSettingsGivenConfigPaths(new string[] { configPath1 });

            // Act & Assert
            var configuration = PackageNamespacesConfiguration.GetPackageNamespacesConfiguration(settings);
            Assert.True(configuration.IsNamespacesEnabled);

            configuration.Namespaces.Should().HaveCount(1);
            var packageSourcesMatchFull = configuration.GetConfiguredPackageSources("stuff");
            Assert.Equal(1, packageSourcesMatchFull.Count);
            Assert.Equal("nuget.org", packageSourcesMatchFull.First());

            var packageSourcesMatchPartial = configuration.GetConfiguredPackageSources("stu");
            Assert.Null(packageSourcesMatchPartial);

            var packageSourcesNoMatch = configuration.GetConfiguredPackageSources("random");
            Assert.Null(packageSourcesNoMatch);
        }

        [Fact]
        public void SearchTree_WithOneSourceMultipart()
        {
            // Arrange
            using var mockBaseDirectory = TestDirectory.Create();
            var configPath1 = Path.Combine(mockBaseDirectory, "NuGet.Config");
            SettingsTestUtils.CreateConfigurationFile(configPath1, @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageNamespaces>
        <clear />
        <packageSource key=""PublicRepository"">
            <namespace id=""Contoso.Opensource.*"" />
        </packageSource>
    </packageNamespaces>
</configuration>");
            var settings = Settings.LoadSettingsGivenConfigPaths(new string[] { configPath1 });

            // Act & Assert
            var configuration = PackageNamespacesConfiguration.GetPackageNamespacesConfiguration(settings);
            Assert.True(configuration.IsNamespacesEnabled);
            configuration.Namespaces.Should().HaveCount(1);

            // No match
            var packageSourcesMatchPartial1 = configuration.GetConfiguredPackageSources("Cont");
            Assert.Null(packageSourcesMatchPartial1);

            // No match
            var packageSourcesMatchPartial2 = configuration.GetConfiguredPackageSources("Contoso.Opensource");
            Assert.Null(packageSourcesMatchPartial2);

            // Match
            var packageSourcesMatchFull1 = configuration.GetConfiguredPackageSources("Contoso.Opensource.MVC");
            Assert.Equal(1, packageSourcesMatchFull1.Count);
            Assert.Equal("publicrepository", packageSourcesMatchFull1.First());

            // Match
            var packageSourcesMatchFull2 = configuration.GetConfiguredPackageSources("Contoso.Opensource.MVC.ASP");

            Assert.Equal(1, packageSourcesMatchFull2.Count);
            Assert.Equal("publicrepository", packageSourcesMatchFull2.First());

            // No match
            var packageSourcesNoMatch = configuration.GetConfiguredPackageSources("random");
            Assert.Null(packageSourcesNoMatch);
        }

        [Fact]
        public void SearchTree_WithMultipleSources()
        {
            // Arrange
            using var mockBaseDirectory = TestDirectory.Create();
            var configPath1 = Path.Combine(mockBaseDirectory, "NuGet.Config");
            SettingsTestUtils.CreateConfigurationFile(configPath1, @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageNamespaces>
        <packageSource key=""nuget.org"">
            <namespace id=""stuff"" />
        </packageSource>
        <packageSource key=""contoso"">
            <namespace id=""moreStuff"" />
        </packageSource>
        <packageSource key=""privateRepository"">
            <namespace id=""private*"" />
        </packageSource>
    </packageNamespaces>
</configuration>");
            var settings = Settings.LoadSettingsGivenConfigPaths(new string[] { configPath1 });

            // Act & Assert
            var configuration = PackageNamespacesConfiguration.GetPackageNamespacesConfiguration(settings);
            Assert.True(configuration.IsNamespacesEnabled);
            configuration.Namespaces.Should().HaveCount(3);

            var packageSourcesMatchFull1 = configuration.GetConfiguredPackageSources("stuff");

            Assert.Equal(1, packageSourcesMatchFull1.Count);
            Assert.Equal("nuget.org", packageSourcesMatchFull1.First());

            var packageSourcesMatchPartial1 = configuration.GetConfiguredPackageSources("stu");
            Assert.Null(packageSourcesMatchPartial1);

            var packageSourcesMatchFull2 = configuration.GetConfiguredPackageSources("moreStuff");
            Assert.Equal(1, packageSourcesMatchFull2.Count);
            Assert.Equal("contoso", packageSourcesMatchFull2.First());

            var packageSourcesMatchPartial2 = configuration.GetConfiguredPackageSources("PrivateTest");
            Assert.Equal(1, packageSourcesMatchPartial2.Count);
            Assert.Equal("privaterepository", packageSourcesMatchPartial2.First());

            var packageSourcesNoMatch = configuration.GetConfiguredPackageSources("random");
            Assert.Null(packageSourcesNoMatch);
        }

        [Fact]
        public void SearchTree_WithMultipleSourcesMultiparts()
        {
            // Arrange
            using var mockBaseDirectory = TestDirectory.Create();
            var configPath1 = Path.Combine(mockBaseDirectory, "NuGet.Config");
            SettingsTestUtils.CreateConfigurationFile(configPath1, @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageNamespaces>
        <packageSource key=""PublicRepository""> 
            <namespace id=""Contoso.Public.*"" />
            <namespace id=""Contoso.Opensource.*"" />
        </packageSource>
        <packageSource key=""PrivateRepository"">
            <namespace id=""Contoso.Opensource"" />
        </packageSource>
        <packageSource key=""SharedRepository"">
            <namespace id=""Contoso.MVC*"" />
        </packageSource>
        <packageSource key=""MetaRepository"">
            <namespace id=""meta.cache*"" />
        </packageSource>
    </packageNamespaces>
</configuration>");
            var settings = Settings.LoadSettingsGivenConfigPaths(new string[] { configPath1 });

            // Act & Assert
            var configuration = PackageNamespacesConfiguration.GetPackageNamespacesConfiguration(settings);
            Assert.True(configuration.IsNamespacesEnabled);
            configuration.Namespaces.Should().HaveCount(4);

            var packageSourcesMatchPartial1 = configuration.GetConfiguredPackageSources("Contoso");
            Assert.Null(packageSourcesMatchPartial1);

            var packageSourcesMatchPartial2 = configuration.GetConfiguredPackageSources("Contoso.Opensource");
            Assert.Equal(1, packageSourcesMatchPartial2.Count);
            Assert.Equal("privaterepository", packageSourcesMatchPartial2.First());

            var packageSourcesMatchFull2 = configuration.GetConfiguredPackageSources("Contoso.MVC");
            Assert.Equal(1, packageSourcesMatchFull2.Count);
            Assert.Equal("sharedrepository", packageSourcesMatchFull2.First());

            var packageSourcesMatchFull3 = configuration.GetConfiguredPackageSources("meta.cache");
            Assert.Equal(1, packageSourcesMatchFull3.Count);
            Assert.Equal("metarepository", packageSourcesMatchFull3.First());


            var packageSourcesMatchFull4 = configuration.GetConfiguredPackageSources("meta.cache.test");
            Assert.Equal(1, packageSourcesMatchFull4.Count);
            Assert.Equal("metarepository", packageSourcesMatchFull4.First());

            var packageSourcesNoMatch = configuration.GetConfiguredPackageSources("random");
            Assert.Null(packageSourcesNoMatch);
        }

        [Fact]
        public void SearchTree_NoSources()
        {
            // Arrange
            using var mockBaseDirectory = TestDirectory.Create();
            var configPath1 = Path.Combine(mockBaseDirectory, "NuGet.Config");
            SettingsTestUtils.CreateConfigurationFile(configPath1, @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
</configuration>");
            var settings = Settings.LoadSettingsGivenConfigPaths(new string[] { configPath1 });

            // Act & Assert
            var configuration = PackageNamespacesConfiguration.GetPackageNamespacesConfiguration(settings);
            Assert.False(configuration.IsNamespacesEnabled);
            configuration.Namespaces.Should().HaveCount(0);

            var packageSourcesMatchPartial = configuration.GetConfiguredPackageSources("stuff");
            Assert.Null(packageSourcesMatchPartial);
        }
    }
}
