// ***********************************************************************
// Author           : the_hunter
// Created          : 04-01-2020
//
// Last Modified By : the_hunter
// Last Modified On : 04-01-2020
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;

namespace Config
{
    /// <summary>
    /// </summary>
    internal static class Extensions
    {
        /// <summary>
        /// </summary>
        public static string Suffix(this ProjectType projectType)
        {
            switch (projectType)
            {
                case ProjectType.Amxx:
                    return "_amxx";

                case ProjectType.Metamod:
                    return "_mm";

                default:
                    throw new ArgumentOutOfRangeException(nameof(projectType), projectType, null);
            }
        }

        /// <summary>
        /// </summary>
        public static MetaPluginLoadTime ToMetaPluginLoadTime(this string loadTime)
        {
            switch (loadTime)
            {
                case "MetaPluginLoadTime::Never":
                    return MetaPluginLoadTime.Never;

                case "MetaPluginLoadTime::Startup":
                    return MetaPluginLoadTime.Startup;

                case "MetaPluginLoadTime::ChangeLevel":
                    return MetaPluginLoadTime.ChangeLevel;

                case "MetaPluginLoadTime::AnyTime":
                    return MetaPluginLoadTime.AnyTime;

                case "MetaPluginLoadTime::AnyPause":
                    return MetaPluginLoadTime.AnyPause;

                default:
                    throw new ArgumentOutOfRangeException(nameof(loadTime), loadTime, null);
            }
        }

        /// <summary>
        /// </summary>
        public static string ToCppCode(this MetaPluginLoadTime loadTime)
        {
            switch (loadTime)
            {
                case MetaPluginLoadTime.Never:
                    return "MetaPluginLoadTime::Never";

                case MetaPluginLoadTime.Startup:
                    return "MetaPluginLoadTime::Startup";

                case MetaPluginLoadTime.ChangeLevel:
                    return "MetaPluginLoadTime::ChangeLevel";

                case MetaPluginLoadTime.AnyTime:
                    return "MetaPluginLoadTime::AnyTime";

                case MetaPluginLoadTime.AnyPause:
                    return "MetaPluginLoadTime::AnyPause";

                default:
                    throw new ArgumentOutOfRangeException(nameof(loadTime), loadTime, null);
            }
        }

        /// <summary>
        /// </summary>
        public static Dictionary<string, string> Clone(this IDictionary<string, string> dictionary)
        {
            return dictionary.ToDictionary(entry => entry.Key, entry => string.Copy(entry.Value));
        }

        /// <summary>
        /// </summary>
        public static void AddKey(this Dictionary<string, string> dictionary, string key)
        {
            if (!dictionary.ContainsKey(key))
                dictionary.Add(key, string.Empty);
        }
    }
}