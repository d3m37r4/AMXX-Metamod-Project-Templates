// ***********************************************************************
// Author           : the_hunter
// Created          : 04-01-2020
//
// Last Modified By : the_hunter
// Last Modified On : 04-01-2020
// ***********************************************************************

using System;
using System.IO;
using System.Linq;
using Config;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.VCProjectEngine;
using Wizard.Projects;
using Wizard.ProjectTemplates;

namespace Wizard
{
    /// <summary>
    /// </summary>
    internal static class Extensions
    {
        /// <summary>
        /// </summary>
        public static ProjectBase AddFromTemplate(this Solution solution, Template template, UserPreferences userPrefs)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            solution.AddFromTemplate(template.FileName, userPrefs.MsvcDirectory, userPrefs.ProjectName);

            var project = (from Project p in solution.Projects
                where p.Name.Equals(userPrefs.ProjectName, StringComparison.Ordinal)
                select p.Object as VCProject).FirstOrDefault();

            switch (userPrefs.ProjectType)
            {
                case ProjectType.Amxx:
                    return new AmxxProject(project, userPrefs);

                case ProjectType.Metamod:
                    return new MetamodProject(project, userPrefs);

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// </summary>
        public static string Save(this Solution solution, UserPreferences userPrefs)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var fileName = string.IsNullOrWhiteSpace(solution.FullName)
                ? Path.Combine(userPrefs.SolutionDirectory, userPrefs.ProjectName + ".sln")
                : solution.FullName;

            solution.SaveAs(fileName);
            solution.Saved = true;

            return fileName;
        }
    }
}