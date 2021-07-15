// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace NuGet.SolutionRestoreManager
{
    /// <summary>
    /// NuGet PackageReference project update events.
    /// This API provides means of tracking project updates performance by NuGet on PackageReference projects, in particular updates to the assets file and nuget generated props/targets.
    /// All events are fired from a threadpool thread.
    /// </summary>
    [ComImport]
    [Guid("30CDDD0A-6901-482D-8CEF-6D798F1A99FC")]
    public interface IVsNuGetPackageReferenceProjectUpdateEvents
    {
        /// <summary>
        /// Raised when solution restore starts with the list of projects that will be restored.
        /// The list will not include all projects. Some projects may have been skipped in earlier up to date check, and other projects may no-op.
        /// </summary>
        /// <remarks>
        /// Just because a project is being restored that doesn't necessarily mean any actual updates will happen.
        /// Only PackageReference projects are included in this list.
        /// No heavy computation should happen in any of these methods as it'll block the NuGet progress.
        /// </remarks>
        event SolutionRestoreEventHandler SolutionRestoreStarted;

        /// <summary>
        /// Raised when solution restore finishes with the list of projects that were restored.
        /// The list will not include all projects. Some projects may have been skipped in earlier up to date check, and other projects may no-op.
        /// </summary>
        /// <remarks>
        /// Just because a project is being restored that doesn't necessarily mean any actual updates will happen.
        /// Only PackageReference projects are included in this list.
        /// No heavy computation should happen in any of these methods as it'll block the NuGet progress.
        /// </remarks>
        event SolutionRestoreEventHandler SolutionRestoreFinished;

        /// <summary>
        /// Raised when particular project is about to be updated.
        /// This means an assets file or a nuget temp msbuild file write (nuget.g.props or nuget.g.targets). The list of updated files will include the aforementioned.
        /// If a project was restore, but no file updates happen, this event will not be fired.
        /// </summary>
        /// <remarks>
        /// No heavy computation should happen in any of these methods as it'll block the NuGet progress.
        /// </remarks>
        event ProjectUpdateEventHandler ProjectUpdateStarted;

        /// <summary>
        /// Raised when particular project update has been completed.
        /// This means an assets file or a nuget temp msbuild file write (nuget.g.props or nuget.g.targets). The list of updated files will include the aforementioned.
        /// If a project was restore, but no file updates happen, this event will not be fired.
        /// </summary>
        /// <remarks>
        /// No heavy computation should happen in any of these methods as it'll block the NuGet progress.
        /// </remarks>
        event ProjectUpdateEventHandler ProjectUpdateFinished;
    }

    /// <summary>
    /// Defines an event handler delegate for PackageReference solution restore start and end.
    /// </summary>
    /// <param name="projects">List of projects that will run restore. Never <see langword="null"/>.</param>
    public delegate void SolutionRestoreEventHandler(IReadOnlyList<string> projects);

    /// <summary>
    /// Defines an event handler delegate for project updates.
    /// </summary>
    /// <param name="projectUniqueName">Project full path. Never <see langword="null"/>. </param>
    /// <param name="updatedFiles">NuGet output files that may be updated. Never <see langword="null"/>.</param>
    public delegate void ProjectUpdateEventHandler(string projectUniqueName, IReadOnlyList<string> updatedFiles);
}
