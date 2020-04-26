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
using Config.Dialogs;

namespace Config
{
    /// <summary>
    /// </summary>
    public class UserPreferences
    {
        /// <summary>
        /// </summary>
        private readonly Dictionary<string, string> _replacements;

        /// <summary>
        /// </summary>
        private readonly bool _solutionProjectSameDirectory;

        /// <summary>
        /// </summary>
        private UserPreferences(IDictionary<string, string> replacementsDictionary)
        {
            _replacements = replacementsDictionary.Clone();

            ProjectType = (ProjectType) Enum.Parse(typeof(ProjectType),
                _replacements[@"$wizarddata$"]);

            _solutionProjectSameDirectory =
                string.IsNullOrWhiteSpace(SolutionDirectory) ||
                SolutionDirectory.Equals(DestinationDirectory, StringComparison.OrdinalIgnoreCase) ||
                !SolutionDirectory.Equals(Path.GetDirectoryName(DestinationDirectory), StringComparison.OrdinalIgnoreCase);

            if (_solutionProjectSameDirectory)
                SolutionDirectory = DestinationDirectory;

            // Add replacements keys
            _replacements.AddKey(@"$targetname$");
            _replacements.AddKey(@"$exportsdef$");
            _replacements.AddKey(@"$msvcdirectory$");
            _replacements.AddKey(@"$cmakeprojectname$");
            _replacements.AddKey(@"$cmakepluginprojectdirname$");
            _replacements.AddKey(@"$pluginname$");
            _replacements.AddKey(@"$pluginversion$");
            _replacements.AddKey(@"$pluginauthor$");
            _replacements.AddKey(@"$pluginlogtag$");
            _replacements.AddKey(@"$pluginurl$");
            _replacements.AddKey(@"$pluginlibrary$");
            _replacements.AddKey(@"$pluginlibclass$");
            _replacements.AddKey(@"$pluginloadable$");
            _replacements.AddKey(@"$pluginunloadable$");
            _replacements.AddKey(@"$pluginprojectdirname$");

            // Initialize replacements
            TargetName = SafeProjectName.Replace("_", string.Empty).ToLower() + ProjectType.Suffix();
            ExportsDef = SafeProjectName.Replace("_", string.Empty).ToUpper() + @"_EXPORTS";
            MsvcDirectory = Path.Combine(DestinationDirectory, @"msvc");
            CMakeProjectName = SafeProjectName.Replace("_", string.Empty).ToLower();
            PluginName = ProjectName;
            PluginVersion = @"1.0.0";
            PluginAuthor = Environment.UserName;
            PluginLogTag = PluginName.ToUpper();
            PluginUrl = "https://dev-cs.ru";
            PluginLibrary = SafeProjectName.ToLower();
            PluginLibClass = SafeProjectName.ToLower();
            PluginLoadable = MetaPluginLoadTime.AnyTime.ToCppCode();
            PluginUnloadable = MetaPluginLoadTime.AnyTime.ToCppCode();
            PluginProjectDirName = SafeProjectName.ToLower();
        }

        /// <summary>
        /// </summary>
        public Dictionary<string, string> Replacements =>
            new Dictionary<string, string>(_replacements);

        /// <summary>
        /// </summary>
        public ProjectType ProjectType { get; }

        /// <summary>
        /// </summary>
        public string ProjectName =>
            _replacements[@"$projectname$"];

        /// <summary>
        /// </summary>
        public string SafeProjectName =>
            _replacements[@"$safeprojectname$"];

        /// <summary>
        /// </summary>
        public string SolutionDirectory
        {
            get => _replacements[@"$solutiondirectory$"];
            set => _replacements[@"$solutiondirectory$"] = value;
        }

        /// <summary>
        /// </summary>
        public string DestinationDirectory
        {
            get => _replacements[@"$destinationdirectory$"];
            internal set
            {
                _replacements[@"$destinationdirectory$"] = value;
                MsvcDirectory = Path.Combine(DestinationDirectory, "msvc");
            }
        }

        /// <summary>
        /// </summary>
        public string MsvcDirectory
        {
            get => _replacements[@"$msvcdirectory$"];
            internal set => _replacements[@"$msvcdirectory$"] = value;
        }

        /// <summary>
        /// </summary>
        public string TargetName
        {
            get => _replacements[@"$targetname$"];
            internal set => _replacements[@"$targetname$"] = value;
        }

        /// <summary>
        /// </summary>
        public string ExportsDef
        {
            get => _replacements[@"$exportsdef$"];
            internal set => _replacements[@"$exportsdef$"] = value;
        }

        /// <summary>
        /// </summary>
        public string CMakeProjectName
        {
            get => _replacements[@"$cmakeprojectname$"];
            internal set => _replacements[@"$cmakeprojectname$"] = value;
        }

        /// <summary>
        /// </summary>
        public string CMakePluginProjectDirName
        {
            get => _replacements[@"$cmakepluginprojectdirname$"];
            internal set => _replacements[@"$cmakepluginprojectdirname$"] = value.Trim('/', '\\') + @"/";
        }

        /// <summary>
        /// </summary>
        public string PluginName
        {
            get => _replacements[@"$pluginname$"];
            internal set => _replacements[@"$pluginname$"] = value;
        }

        /// <summary>
        /// </summary>
        public string PluginVersion
        {
            get => _replacements[@"$pluginversion$"];
            internal set => _replacements[@"$pluginversion$"] = value;
        }

        /// <summary>
        /// </summary>
        public string PluginAuthor
        {
            get => _replacements[@"$pluginauthor$"];
            internal set => _replacements[@"$pluginauthor$"] = value;
        }

        /// <summary>
        /// </summary>
        public string PluginLogTag
        {
            get => _replacements[@"$pluginlogtag$"];
            internal set => _replacements[@"$pluginlogtag$"] = value;
        }

        /// <summary>
        /// </summary>
        public string PluginUrl
        {
            get => _replacements[@"$pluginurl$"];
            internal set => _replacements[@"$pluginurl$"] = value;
        }

        /// <summary>
        /// </summary>
        public string PluginLibrary
        {
            get => _replacements[@"$pluginlibrary$"];
            internal set => _replacements[@"$pluginlibrary$"] = value;
        }

        /// <summary>
        /// </summary>
        public string PluginLibClass
        {
            get => _replacements[@"$pluginlibclass$"];
            internal set => _replacements[@"$pluginlibclass$"] = value;
        }

        /// <summary>
        /// </summary>
        public bool ReHldsApi { get; internal set; }

        /// <summary>
        /// </summary>
        public bool ReGameDllApi { get; internal set; }

        /// <summary>
        /// </summary>
        public bool MetamodApi { get; internal set; }

        /// <summary>
        /// </summary>
        public bool ReloadOnMapChange { get; internal set; }

        /// <summary>
        /// </summary>
        public bool Amxx182Compatibility { get; internal set; }

        /// <summary>
        /// </summary>
        public string PluginLoadable
        {
            get => _replacements[@"$pluginloadable$"];
            internal set => _replacements[@"$pluginloadable$"] = value;
        }

        /// <summary>
        /// </summary>
        public string PluginUnloadable
        {
            get => _replacements[@"$pluginunloadable$"];
            internal set => _replacements[@"$pluginunloadable$"] = value;
        }

        /// <summary>
        /// </summary>
        public string PluginProjectDirName
        {
            get => _replacements[@"$pluginprojectdirname$"];
            internal set
            {
                _replacements[@"$pluginprojectdirname$"] = value;

                if (!_solutionProjectSameDirectory)
                {
                    DestinationDirectory = Path.Combine(SolutionDirectory, PluginProjectDirName);
                    CMakePluginProjectDirName = PluginProjectDirName;
                }
            }
        }

        /// <summary>
        /// </summary>
        public static UserPreferences CreateInstance(IDictionary<string, string> replacementsDictionary)
        {
            var userPrefs = new UserPreferences(replacementsDictionary);

            var userPrefsView = new UserPrefsView(new UserPrefsViewModel(userPrefs));
            userPrefsView.ShowDialog();

            return userPrefs;
        }
    }
}