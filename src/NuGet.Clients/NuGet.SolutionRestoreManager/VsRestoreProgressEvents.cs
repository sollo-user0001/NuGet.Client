// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Globalization;
using NuGet.ProjectManagement;

namespace NuGet.SolutionRestoreManager
{
    [Export(typeof(IVsNuGetProjectUpdateEvents))]
    [Export(typeof(IVsNuGetProgressReporter))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class VsRestoreProgressEvents : IVsNuGetProjectUpdateEvents, IVsNuGetProgressReporter
    {
        private readonly IPackageEventsProvider _packageEventsProvider;
        public event SolutionRestoreEventHandler SolutionRestoreStarted;
        public event SolutionRestoreEventHandler SolutionRestoreFinished;
        public event ProjectUpdateEventHandler ProjectUpdateStarted;
        public event ProjectUpdateEventHandler ProjectUpdateFinished;

        [ImportingConstructor]
        public VsRestoreProgressEvents(IPackageEventsProvider packageEventsProvider)
        {
            var packageEvents = _packageEventsProvider.GetPackageEvents();
            packageEvents.PackageInstalling += PackageEventsProjectUpdateStart;
            packageEvents.PackageInstalled += PackageEventsProjectUpdateEnd;
            packageEvents.PackageUninstalling += PackageEventsProjectUpdateStart;
            packageEvents.PackageUninstalled += PackageEventsProjectUpdateEnd;
        }

        private void PackageEventsProjectUpdateStart(object sender, PackageEventArgs e)
        {
            var projectPath = e.Project.GetMetadata<string>(NuGetProjectMetadataKeys.FullPath);
            ProjectUpdateStarted(projectPath, new string[] { e.InstallPath });
        }
        private void PackageEventsProjectUpdateEnd(object sender, PackageEventArgs e)
        {
            var projectPath = e.Project.GetMetadata<string>(NuGetProjectMetadataKeys.FullPath);
            ProjectUpdateFinished(projectPath, new string[] { e.InstallPath });
        }

        public void EndProjectUpdate(string projectName)
        {
            if (projectName == null)
            {
                throw new ArgumentNullException(nameof(projectName));
            }
            ProjectUpdateFinished(projectName, Array.Empty<string>());
        }

        public void StartProjectUpdate(string projectName)
        {
            if (projectName == null)
            {
                throw new ArgumentNullException(nameof(projectName));
            }
            ProjectUpdateStarted(projectName, Array.Empty<string>());
        }

        public void StartSolutionRestore(IReadOnlyList<string> projects)
        {
            if (projects == null && projects.Count == 0)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.Argument_Cannot_Be_Null_Or_Empty, nameof(projects)));
            }
            SolutionRestoreStarted(projects);
        }

        public void EndSolutionRestore(IReadOnlyList<string> projects)
        {
            if (projects == null && projects.Count == 0)
            {
                throw new ArgumentNullException(nameof(projects));
            }
            SolutionRestoreFinished(projects);
        }
    }
}
