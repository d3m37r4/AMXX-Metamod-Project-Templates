// ***********************************************************************
// Author           : the_hunter
// Created          : 04-01-2020
//
// Last Modified By : the_hunter
// Last Modified On : 04-01-2020
// ***********************************************************************

using System;
using System.Windows;

namespace Config.Dialogs
{
    /// <summary>
    /// </summary>
    internal class UserPrefsViewModel
    {
        /// <summary>
        /// </summary>
        private readonly UserPreferences _userPrefs;

        /// <summary>
        /// </summary>
        public UserPrefsViewModel(UserPreferences userPreferences)
        {
            _userPrefs = userPreferences ?? throw new ArgumentNullException(nameof(userPreferences));
        }

        /// <summary>
        /// </summary>
        public Visibility AmxxVisibility =>
            _userPrefs.ProjectType == ProjectType.Amxx ? Visibility.Visible : Visibility.Collapsed;

        /// <summary>
        /// </summary>
        public Visibility MetamodVisibility =>
            _userPrefs.ProjectType == ProjectType.Metamod ? Visibility.Visible : Visibility.Collapsed;

        /// <summary>
        /// </summary>
        public string Name
        {
            get => _userPrefs.PluginName;
            set => _userPrefs.PluginName = value;
        }

        /// <summary>
        /// </summary>
        public string Version
        {
            get => _userPrefs.PluginVersion;
            set => _userPrefs.PluginVersion = value;
        }

        /// <summary>
        /// </summary>
        public string Author
        {
            get => _userPrefs.PluginAuthor;
            set => _userPrefs.PluginAuthor = value;
        }

        /// <summary>
        /// </summary>
        public string LogTag
        {
            get => _userPrefs.PluginLogTag;
            set => _userPrefs.PluginLogTag = value;
        }

        /// <summary>
        /// </summary>
        public string Url
        {
            get => _userPrefs.PluginUrl;
            set => _userPrefs.PluginUrl = value;
        }

        /// <summary>
        /// </summary>
        public string Library
        {
            get => _userPrefs.PluginLibrary;
            set => _userPrefs.PluginLibrary = value;
        }

        /// <summary>
        /// </summary>
        public string LibClass
        {
            get => _userPrefs.PluginLibClass;
            set => _userPrefs.PluginLibClass = value;
        }

        /// <summary>
        /// </summary>
        public bool ReHldsApi
        {
            get => _userPrefs.ReHldsApi;
            set => _userPrefs.ReHldsApi = value;
        }

        /// <summary>
        /// </summary>
        public bool ReGameDllApi
        {
            get => _userPrefs.ReGameDllApi;
            set => _userPrefs.ReGameDllApi = value;
        }

        /// <summary>
        /// </summary>
        public bool MetamodApi
        {
            get => _userPrefs.MetamodApi;
            set => _userPrefs.MetamodApi = value;
        }

        /// <summary>
        /// </summary>
        public bool ReloadOnMapChange
        {
            get => _userPrefs.ReloadOnMapChange;
            set => _userPrefs.ReloadOnMapChange = value;
        }

        /// <summary>
        /// </summary>
        public bool Amxx182Compatibility
        {
            get => _userPrefs.Amxx182Compatibility;
            set => _userPrefs.Amxx182Compatibility = value;
        }

        /// <summary>
        /// </summary>
        public string Loadable
        {
            get => _userPrefs.PluginLoadable;
            set => _userPrefs.PluginLoadable = value;
        }

        /// <summary>
        /// </summary>
        public string Unloadable
        {
            get => _userPrefs.PluginUnloadable;
            set => _userPrefs.PluginUnloadable = value;
        }

        /// <summary>
        /// </summary>
        public string ProjectDirName
        {
            get => _userPrefs.PluginProjectDirName;
            set => _userPrefs.PluginProjectDirName = value;
        }
    }
}