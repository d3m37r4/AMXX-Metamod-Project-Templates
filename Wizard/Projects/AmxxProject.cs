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
using Data;
using Microsoft.VisualStudio.VCProjectEngine;

namespace Wizard.Projects
{
    /// <summary>
    /// </summary>
    internal class AmxxProject : ProjectBase
    {
        /// <summary>
        /// </summary>
        public AmxxProject(VCProject project, UserPreferences userPrefs) : base(project, userPrefs)
        {
        }

        /// <summary>
        /// </summary>
        public override void AddSources()
        {
            base.AddSources();
            AddAmxxSdk();

            if (UserPrefs.MetamodApi)
            {
                AddCsSdk();
                AddMetamodSdk();
                AddModuleDefinitionFile();
            }
            else if (UserPrefs.ReHldsApi || UserPrefs.ReGameDllApi)
            {
                AddCsSdk();
            }
        }

        /// <summary>
        /// </summary>
        private void AddAmxxSdk()
        {
            const string amxx = @"include\amxx\amxx.h";
            const string config = @"include\amxx\amxx_config.h";
            var sdkDir = Path.Combine(UserPrefs.DestinationDirectory, ProjectSources.AmxxSdk);
            ProjectSources.CopyDirectory(ProjectSources.AmxxSdk, sdkDir);

            foreach (var directory in Directory.EnumerateDirectories(sdkDir, @"*", SearchOption.AllDirectories))
            {
                var path = directory.Replace(@"\include\amxx", @"\include\");
                var filter = AddFilter(path.Remove(0, UserPrefs.DestinationDirectory.Length));

                foreach (var file in Directory.GetFiles(directory, @"*", SearchOption.TopDirectoryOnly))
                {
                    if (file.EndsWith(amxx, StringComparison.OrdinalIgnoreCase))
                        continue;

                    if (file.EndsWith(config, StringComparison.OrdinalIgnoreCase))
                        continue;

                    filter.AddFile(file);
                }
            }

            ReplaceParameters(Path.Combine(sdkDir, config));
            AddFilter(UserPrefs.PluginProjectDirName).AddFile(Path.Combine(sdkDir, config));
            AddFilter(@"sdk\amxx").AddFile(Path.Combine(sdkDir, amxx));

            SetupConfig();
        }

        /// <summary>
        /// </summary>
        private void SetupConfig()
        {
            var configPath = Path.Combine(UserPrefs.DestinationDirectory,
                @"sdk\amxx\include\amxx\amxx_config.h");

            var config = new Config(configPath);

            if (UserPrefs.MetamodApi)
                config.UncommentUseMetamod();

            if (UserPrefs.Amxx182Compatibility)
                config.UncommentAmxxCompatibility();

            config.SetReloadOnMapChange(UserPrefs.ReloadOnMapChange);
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
            public void UncommentUseMetamod()
            {
                ConfigFile = ConfigFile.Replace(@"//#define USE_METAMOD", @"#define USE_METAMOD");
            }

            /// <summary>
            /// </summary>
            public void UncommentAmxxCompatibility()
            {
                ConfigFile = ConfigFile.Replace(@"//#define AMXX_182_COMPATIBILITY", @"#define AMXX_182_COMPATIBILITY");
            }

            /// <summary>
            /// </summary>
            public void SetReloadOnMapChange(bool value)
            {
                ConfigFile = ConfigFile.Replace(
                    @"constexpr auto AMXX_MODULE_RELOAD_ON_MAP_CHANGE = false",
                    $@"constexpr auto AMXX_MODULE_RELOAD_ON_MAP_CHANGE = {value.ToString().ToLower()}");
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