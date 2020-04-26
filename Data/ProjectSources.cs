// ***********************************************************************
// Author           : the_hunter
// Created          : 04-01-2020
//
// Last Modified By : the_hunter
// Last Modified On : 04-01-2020
// ***********************************************************************

using System;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using Config;

namespace Data
{
    /// <summary>
    /// </summary>
    public class ProjectSources
    {
        private static Stream _projectSourcesStream;
        private static ZipArchive _projectSourcesZip;

        /// <summary>
        /// </summary>
        private readonly UserPreferences _userPrefs;

        /// <summary>
        /// </summary>
        public ProjectSources(UserPreferences userPrefs)
        {
            _userPrefs = userPrefs;
        }

        /// <summary>
        /// </summary>
        private static Stream ProjectSourcesStream =>
            _projectSourcesStream ?? (_projectSourcesStream =
                Assembly.GetExecutingAssembly().GetManifestResourceStream("Data.Resources.ProjectSources.zip"));

        /// <summary>
        /// </summary>
        private static ZipArchive ProjectSourcesZip =>
            _projectSourcesZip ?? (_projectSourcesZip = new ZipArchive(ProjectSourcesStream, ZipArchiveMode.Read));

        /// <summary>
        /// </summary>
        public string PropertySheets =>
            @"msvc\PropertySheets";

        /// <summary>
        /// </summary>
        public string ModuleDefinition =>
            @"msvc\module_definition.def";

        /// <summary>
        /// </summary>
        public string AmxxSdk =>
            @"sdk\amxx";

        /// <summary>
        /// </summary>
        public string CsSdk =>
            @"sdk\cssdk";

        /// <summary>
        /// </summary>
        public string MetamodSdk =>
            @"sdk\metamod";

        /// <summary>
        /// </summary>
        public string ThirdParty =>
            @"third_party";

        /// <summary>
        /// </summary>
        public string GitAttributes =>
            @".gitattributes";

        /// <summary>
        /// </summary>
        public string GitIgnore =>
            @".gitignore";

        /// <summary>
        /// </summary>
        public string CMake
        {
            get
            {
                switch (_userPrefs.ProjectType)
                {
                    case ProjectType.Amxx:
                        return @"cmake\AMXX\cmake";

                    case ProjectType.Metamod:
                        return @"cmake\Metamod\cmake";

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        /// <summary>
        /// </summary>
        public string CMakeLists
        {
            get
            {
                switch (_userPrefs.ProjectType)
                {
                    case ProjectType.Amxx:
                        return _userPrefs.ReHldsApi || _userPrefs.ReGameDllApi
                            ? @"cmake\AMXX\CMakeLists_rehlds_regamedll.txt"
                            : @"cmake\AMXX\CMakeLists.txt";

                    case ProjectType.Metamod:
                        return _userPrefs.ReHldsApi || _userPrefs.ReGameDllApi
                            ? @"cmake\Metamod\CMakeLists_rehlds_regamedll.txt"
                            : @"cmake\Metamod\CMakeLists.txt";

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        /// <summary>
        /// </summary>
        public string Main
        {
            get
            {
                switch (_userPrefs.ProjectType)
                {
                    case ProjectType.Amxx:
                        if (_userPrefs.ReHldsApi && _userPrefs.ReGameDllApi)
                            return @"main\AMXX\main_rehlds_regamedll.cpp";

                        if (_userPrefs.ReHldsApi)
                            return @"main\AMXX\main_rehlds.cpp";

                        return _userPrefs.ReGameDllApi ? @"main\AMXX\main_regamedll.cpp" : @"main\AMXX\main.cpp";

                    case ProjectType.Metamod:
                        if (_userPrefs.ReHldsApi && _userPrefs.ReGameDllApi)
                            return @"main\Metamod\main_rehlds_regamedll.cpp";

                        if (_userPrefs.ReHldsApi)
                            return @"main\Metamod\main_rehlds.cpp";

                        return _userPrefs.ReGameDllApi ? @"main\Metamod\main_regamedll.cpp" : @"main\Metamod\main.cpp";

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        /// <summary>
        /// </summary>
        public string ReHldsApiCpp
        {
            get
            {
                switch (_userPrefs.ProjectType)
                {
                    case ProjectType.Amxx:
                        return @"rehlds\AMXX\rehlds_api.cpp";

                    case ProjectType.Metamod:
                        return @"rehlds\Metamod\rehlds_api.cpp";

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        /// <summary>
        /// </summary>
        public string ReHldsApiH
        {
            get
            {
                switch (_userPrefs.ProjectType)
                {
                    case ProjectType.Amxx:
                        return @"rehlds\AMXX\rehlds_api.h";

                    case ProjectType.Metamod:
                        return @"rehlds\Metamod\rehlds_api.h";

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        /// <summary>
        /// </summary>
        public string ReGameDllApiCpp
        {
            get
            {
                switch (_userPrefs.ProjectType)
                {
                    case ProjectType.Amxx:
                        return @"regamedll\AMXX\regamedll_api.cpp";

                    case ProjectType.Metamod:
                        return @"regamedll\Metamod\regamedll_api.cpp";

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        /// <summary>
        /// </summary>
        public string ReGameDllApiH
        {
            get
            {
                switch (_userPrefs.ProjectType)
                {
                    case ProjectType.Amxx:
                        return @"regamedll\AMXX\regamedll_api.h";

                    case ProjectType.Metamod:
                        return @"regamedll\Metamod\regamedll_api.h";

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        /// <summary>
        /// </summary>
        public static void CopyFile(string source, string dest, bool overwrite = false)
        {
            foreach (var entry in ProjectSourcesZip.Entries)
            {
                if (entry.FullName.Equals(source) && !string.IsNullOrEmpty(entry.Name))
                {
                    ExtractToFile(entry, dest, overwrite);
                    return;
                }
            }

            throw new FileNotFoundException($@"Could not find source file:\r\n{source}", source);
        }

        /// <summary>
        /// </summary>
        public static void CopyDirectory(string sourceDir, string destDir, bool overwrite = false)
        {
            var count = 0;

            if (!sourceDir.EndsWith(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal))
                sourceDir += Path.DirectorySeparatorChar;

            foreach (var entry in ProjectSourcesZip.Entries)
            {
                if (!entry.FullName.StartsWith(sourceDir) || string.IsNullOrEmpty(entry.Name))
                    continue;

                ExtractToFile(entry, Path.Combine(destDir, entry.FullName.Remove(0, sourceDir.Length)), overwrite);
                ++count;
            }

            if (count == 0)
                throw new FileNotFoundException($@"Could not find source directory:\r\n{sourceDir}", sourceDir);
        }

        /// <summary>
        /// </summary>
        private static void ExtractToFile(ZipArchiveEntry entry, string dest, bool overwrite)
        {
            var dir = Path.GetDirectoryName(dest);

            if (dir != null && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            if (overwrite)
            {
                entry.ExtractToFile(dest, true);
            }
            else if (!File.Exists(dest))
            {
                entry.ExtractToFile(dest, false);
            }
        }
    }
}