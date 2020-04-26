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

namespace Wizard.ProjectTemplates
{
    /// <summary>
    /// </summary>
    internal class Template
    {
        /// <summary>
        /// </summary>
        public Template(IReadOnlyList<object> customParams)
        {
            if (customParams == null)
                throw new ArgumentNullException(nameof(customParams));

            var wizardDirectory = Path.GetDirectoryName(customParams[0].ToString()) ?? string.Empty;
            var templateDirectory = Path.Combine(wizardDirectory, "Template");

            FileName = Path.Combine(templateDirectory, "Template.vstemplate");
        }

        /// <summary>
        /// </summary>
        public string FileName { get; }
    }
}