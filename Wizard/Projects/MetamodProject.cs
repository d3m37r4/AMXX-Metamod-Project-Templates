// ***********************************************************************
// Author           : the_hunter
// Created          : 04-01-2020
//
// Last Modified By : the_hunter
// Last Modified On : 04-01-2020
// ***********************************************************************

using System;
using System.IO;
using System.Text;
using Config;
using Microsoft.VisualStudio.VCProjectEngine;

namespace Wizard.Projects
{
    /// <summary>
    /// </summary>
    internal class MetamodProject : ProjectBase
    {
        /// <summary>
        /// </summary>
        public MetamodProject(VCProject project, UserPreferences userPrefs) : base(project, userPrefs)
        {
        }

        /// <summary>
        /// </summary>
        public override void AddSources()
        {
            base.AddSources();

            AddCsSdk();
            AddMetamodSdk();
            AddModuleDefinitionFile();
        }

        /// <summary>
        /// </summary>
        protected override void AddMetamodSdk()
        {
            base.AddMetamodSdk();

            var configPath = Path.Combine(UserPrefs.DestinationDirectory,
                @"sdk\metamod\include\metamod\metamod_config.h");

            var config = new Config(configPath);
            config.UncommentMetaAttach();
            config.UncommentMetaDetach();
            config.Save();
        }

        /// <summary>
        /// </summary>
        private class Config
        {
            /// <summary>
            /// </summary>
            private readonly string _configPath;

            /// <summary>
            /// </summary>
            private string _configFile;

            /// <summary>
            /// </summary>
            public Config(string configPath)
            {
                _configPath = configPath ?? throw new ArgumentNullException(nameof(configPath));
            }

            /// <summary>
            /// </summary>
            private string ConfigFile
            {
                get
                {
                    if (string.IsNullOrEmpty(_configFile))
                        _configFile = File.ReadAllText(_configPath, Encoding.ASCII);

                    return _configFile;
                }
                set => _configFile = value;
            }

            /// <summary>
            /// </summary>
            public void UncommentMetaAttach()
            {
                ConfigFile = ConfigFile.Replace(@"//#define META_ATTACH", @"#define META_ATTACH");
            }

            /// <summary>
            /// </summary>
            public void UncommentMetaDetach()
            {
                ConfigFile = ConfigFile.Replace(@"//#define META_DETACH", @"#define META_DETACH");
            }

            /// <summary>
            /// </summary>
            public void Save()
            {
                File.WriteAllText(_configPath, ConfigFile, Encoding.ASCII);
            }
        }
    }
}