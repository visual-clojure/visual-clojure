﻿// MIT License Copyright 2010-2013 jmis
// See LICENSE.txt or http://opensource.org/licenses/MIT
// See AUTHORS.txt for a complete list of all contributors

using System.Collections.Generic;
using VisualClojure.Utilities;
using EnvDTE;

namespace VisualClojure.Project.Hierarchy
{
    public class ProjectFilesProvider : IProvider<List<string>>
    {
        private readonly IProvider<EnvDTE.Project> _projectProvider;

        public ProjectFilesProvider(IProvider<EnvDTE.Project> projectProvider)
        {
            _projectProvider = projectProvider;
        }

        public List<string> Get()
        {
            List<string> files = new List<string>();
            EnvDTE.Project project = _projectProvider.Get();
            if (project == null) return files;

            Queue<ProjectItem> projectItemsToLookAt = new Queue<ProjectItem>();
            foreach (ProjectItem projectItem in project.ProjectItems) projectItemsToLookAt.Enqueue(projectItem);

            while (projectItemsToLookAt.Count > 0)
            {
                ProjectItem currentProjectItem = projectItemsToLookAt.Dequeue();

                if (currentProjectItem.Object.GetType() == typeof (ClojureProjectFileNode))
                    files.Add(currentProjectItem.Properties.Item("FullPath").Value.ToString());

                if (currentProjectItem.ProjectItems != null && currentProjectItem.ProjectItems.Count > 0)
                    foreach (ProjectItem childItem in currentProjectItem.ProjectItems)
                        projectItemsToLookAt.Enqueue(childItem);
            }

            return files;
        }
    }
}