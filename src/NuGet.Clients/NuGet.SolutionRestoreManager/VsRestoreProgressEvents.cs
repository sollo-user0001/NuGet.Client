// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Globalization;

namespace NuGet.SolutionRestoreManager
{
    [Export(typeof(IVsNuGetPackageReferenceProjectUpdateEvents))]
    [Export(typeof(IVsNuGetProgressReporter))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class VsRestoreProgressEvents : IVsNuGetPackageReferenceProjectUpdateEvents, IVsNuGetProgressReporter
    {
        public event SolutionRestoreEventHandler SolutionRestoreStarted;
        public event SolutionRestoreEventHandler SolutionRestoreFinished;
        public event ProjectUpdateEventHandler ProjectUpdateStarted;
        public event ProjectUpdateEventHandler ProjectUpdateFinished;

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
