﻿// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Usage", "CA1816:Dispose methods should call SuppressFinalize", Justification = "<Pending>", Scope = "member", Target = "~M:NuGet.Packaging.FuncTest.AllowListVerificationProviderTests.Dispose")]
[assembly: SuppressMessage("Performance", "CA1829:Use Length/Count property instead of Count() when available", Justification = "<Pending>", Scope = "member", Target = "~M:NuGet.Packaging.FuncTest.AllowListVerificationProviderTests.GetTrustResultAsync_AuthorSignedPackage_RequirementsAsync(NuGet.Packaging.Signing.SignedPackageVerifierSettings,System.Collections.Generic.IReadOnlyCollection{NuGet.Packaging.Signing.VerificationAllowListEntry},System.Boolean,System.Boolean,System.Int32,System.Int32,System.Object[][])~System.Threading.Tasks.Task")]
[assembly: SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>", Scope = "member", Target = "~M:NuGet.Packaging.FuncTest.ClientPolicyTests.CreateSignedPackageAsync(NuGet.Test.Utility.TestDirectory,NuGet.Packaging.FuncTest.ClientPolicyTests.SigningTestType,System.Security.Cryptography.X509Certificates.X509Certificate2,System.Security.Cryptography.X509Certificates.X509Certificate2)~System.Threading.Tasks.Task{System.String}")]
[assembly: SuppressMessage("Usage", "CA1816:Dispose methods should call SuppressFinalize", Justification = "<Pending>", Scope = "member", Target = "~M:NuGet.Packaging.FuncTest.ClientPolicyTests.Dispose")]
[assembly: SuppressMessage("Performance", "CA1826:Do not use Enumerable methods on indexable collections", Justification = "<Pending>", Scope = "member", Target = "~M:NuGet.Packaging.FuncTest.SignatureTrustAndValidityVerificationProviderTests.TrustPrimaryTimestampRootCertificate(NuGet.Packaging.Signing.PrimarySignature)~System.IDisposable")]
[assembly: SuppressMessage("Performance", "CA1826:Do not use Enumerable methods on indexable collections", Justification = "<Pending>", Scope = "member", Target = "~M:NuGet.Packaging.FuncTest.SignatureTrustAndValidityVerificationProviderTests.TrustRootCertificate(NuGet.Packaging.Signing.IX509CertificateChain)~System.IDisposable")]
[assembly: SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>", Scope = "member", Target = "~M:NuGet.Packaging.FuncTest.SigningTestFixture.CreateTrustedTestCertificateThatWillExpireSoon~Test.Utility.Signing.TrustedTestCert{Test.Utility.Signing.TestCertificate}")]
[assembly: SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>", Scope = "member", Target = "~M:NuGet.Packaging.FuncTest.SigningTestFixture.CreateUntrustedTestCertificateThatWillExpireSoon~Test.Utility.Signing.TestCertificate")]
[assembly: SuppressMessage("Usage", "CA1816:Dispose methods should call SuppressFinalize", Justification = "<Pending>", Scope = "member", Target = "~M:NuGet.Packaging.FuncTest.SigningTestFixture.Dispose")]
[assembly: SuppressMessage("Performance", "CA1826:Do not use Enumerable methods on indexable collections", Justification = "<Pending>", Scope = "member", Target = "~M:NuGet.Packaging.FuncTest.TimestampProviderTests.GetTimestampAsync_AssertCompleteChain_SuccessAsync~System.Threading.Tasks.Task")]
[assembly: SuppressMessage("Performance", "CA1826:Do not use Enumerable methods on indexable collections", Justification = "<Pending>", Scope = "member", Target = "~M:NuGet.Packaging.FuncTest.TimestampTests.Timestamp_Verify_WithOfflineRevocation_ReturnsCorrectFlagsAndLogsAsync~System.Threading.Tasks.Task")]
