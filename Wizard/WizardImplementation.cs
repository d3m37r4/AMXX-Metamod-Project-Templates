// ***********************************************************************
// Author           : the_hunter
// Created          : 04-01-2020
//
// Last Modified By : the_hunter
// Last Modified On : 04-01-2020
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Config;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.TemplateWizard;
using Wizard.ProjectTemplates;

namespace Wizard
{
    /// <summary>
    /// </summary>
    public class WizardImplementation : IWizard
    {
        /// <summary>
        /// </summary>
        private DTE _dte;

        /// <summary>
        /// </summary>
        private Dictionary<string, string> _replacementsDictionary;

        /// <summary>
        /// </summary>
        private Template _template;

        /// <summary>
        /// </summary>
        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary,
            WizardRunKind runKind, object[] customParams)
        {
            try
            {
                ThreadHelper.ThrowIfNotOnUIThread();

                _dte = (DTE)automationObject;
                _template = new Template(customParams);
                _replacementsDictionary = replacementsDictionary;

                // Remove dummy project directory
                var destinationDirectory = replacementsDictionary[@"$destinationdirectory$"];

                if (Directory.Exists(destinationDirectory) &&
                    !Directory.EnumerateFileSystemEntries(destinationDirectory).Any())
                    Directory.Delete(destinationDirectory, false);
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }

        /// <summary>
        /// </summary>
        public void ProjectFinishedGenerating(Project project)
        {
        }

        /// <summary>
        /// </summary>
        public void ProjectItemFinishedGenerating(ProjectItem projectItem)
        {
        }

        /// <summary>
        /// </summary>
        public bool ShouldAddProjectItem(string filePath)
        {
            return true;
        }

        /// <summary>
        /// </summary>
        public void BeforeOpeningFile(ProjectItem projectItem)
        {
        }

        /// <summary>
        /// </summary>
        public void RunFinished()
        {
            try
            {
                ThreadHelper.ThrowIfNotOnUIThread();

                var userPrefs = UserPreferences.CreateInstance(_replacementsDictionary);
                var project = _dte.Solution.AddFromTemplate(_template, userPrefs);
                project.AddSources();

                _dte.Solution.Save(userPrefs);
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }

        /// <summary>
        /// </summary>
        private static void ShowErrorMessage(Exception ex)
        {
            var message = ex.Message;

            if (ex.InnerException != null)
                message += Environment.NewLine + ex.InnerException.Message;

            MessageBox.Show(message, @"Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}