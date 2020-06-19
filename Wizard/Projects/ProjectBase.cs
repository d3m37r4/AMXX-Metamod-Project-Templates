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
using System.Text;
using Config;
using Data;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.VCProjectEngine;

namespace Wizard.Projects
{
    /// <summary>
    /// </summary>
    internal abstract class ProjectBase
    {
        /// <summary>
        /// </summary>
        protected readonly VCProject Project;

        /// <summary>
        /// </summary>
        protected readonly UserPreferences UserPrefs;

        /// <summary>
        /// </summary>
        protected readonly ProjectSources ProjectSources;

        /// <summary>
        /// </summary>
        protected ProjectBase(VCProject project, UserPreferences userPrefs)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            Project = project ?? throw new ArgumentNullException(nameof(project));
            UserPrefs = userPrefs ?? throw new ArgumentNullException(nameof(userPrefs));
            ProjectSources = new ProjectSources(UserPrefs);
        }

        /// <summary>
        /// </summary>
        public virtual void AddSources()
        {
            CreateDirectories();
            AddProjectDirFilters();

            AddPropertySheets();
            AddReHldsApi();
            AddReGameDllApi();
            AddMainCpp();
            AddThirdParty();
            AddCMake();
            AddGitConfigs();

            SetProperties();
            ReplaceParameters(Path.Combine(UserPrefs.MsvcDirectory, @"resources.rc"));
        }

        /// <summary>
        /// </summary>
        protected virtual void CreateDirectories()
        {
            var dir = Path.Combine(UserPrefs.DestinationDirectory, UserPrefs.PluginProjectDirName, @"include",
                UserPrefs.PluginProjectDirName);

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            dir = Path.Combine(UserPrefs.DestinationDirectory, UserPrefs.PluginProjectDirName, @"src");

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
        }

        /// <summary>
        /// </summary>
        protected virtual void AddProjectDirFilters()
        {
            var include = AddFilter($@"{UserPrefs.PluginProjectDirName}\include");
            var src = AddFilter($@"{UserPrefs.PluginProjectDirName}\src");

            include.Filter = @"h;hh;hpp;hxx;hm;inl;inc;ipp;xsd";
            src.Filter = @"cpp;c;cc;cxx;def;odl;idl;hpj;bat;asm;asmx";
        }

        /// <summary>
        /// </summary>
        protected virtual void AddPropertySheets()
        {
            var propertySheets = Path.Combine(UserPrefs.MsvcDirectory, @"PropertySheets");
            ProjectSources.CopyDirectory(ProjectSources.PropertySheets, propertySheets);

            foreach (var file in Directory.EnumerateFiles(propertySheets, @"*", SearchOption.AllDirectories))
                ReplaceParameters(file);
        }

        /// <summary>
        /// </summary>
        protected virtual void AddReHldsApi()
        {
            if (!UserPrefs.ReHldsApi)
                return;

            var pluginProjectDir = Path.Combine(UserPrefs.DestinationDirectory, UserPrefs.PluginProjectDirName);
            var dest = Path.Combine(pluginProjectDir, @"include", UserPrefs.PluginProjectDirName, @"rehlds_api.h");
            ProjectSources.CopyFile(ProjectSources.ReHldsApiH, dest);
            ReplaceParameters(dest);

            var filter = AddFilter($@"{UserPrefs.PluginProjectDirName}\include");
            filter.AddFile(dest);

            dest = Path.Combine(pluginProjectDir, @"src\rehlds_api.cpp");
            ProjectSources.CopyFile(ProjectSources.ReHldsApiCpp, dest);
            ReplaceParameters(dest);

            filter = AddFilter($@"{UserPrefs.PluginProjectDirName}\src");
            filter.AddFile(dest);
        }

        /// <summary>
        /// </summary>
        protected virtual void AddReGameDllApi()
        {
            if (!UserPrefs.ReGameDllApi)
                return;

            var pluginProjectDir = Path.Combine(UserPrefs.DestinationDirectory, UserPrefs.PluginProjectDirName);
            var dest = Path.Combine(pluginProjectDir, @"include", UserPrefs.PluginProjectDirName, @"regamedll_api.h");
            ProjectSources.CopyFile(ProjectSources.ReGameDllApiH, dest);
            ReplaceParameters(dest);

            var filter = AddFilter($@"{UserPrefs.PluginProjectDirName}\include");
            filter.AddFile(dest);

            dest = Path.Combine(pluginProjectDir, @"src\regamedll_api.cpp");
            ProjectSources.CopyFile(ProjectSources.ReGameDllApiCpp, dest);
            ReplaceParameters(dest);

            filter = AddFilter($@"{UserPrefs.PluginProjectDirName}\src");
            filter.AddFile(dest);
        }

        /// <summary>
        /// </summary>
        protected virtual void AddMainCpp()
        {
            var pluginProjectDir = Path.Combine(UserPrefs.DestinationDirectory, UserPrefs.PluginProjectDirName);
            var dest = Path.Combine(pluginProjectDir, @"src\main.cpp");

            ProjectSources.CopyFile(ProjectSources.Main, dest);
            ReplaceParameters(dest);

            var filter = AddFilter($@"{UserPrefs.PluginProjectDirName}\src");
            filter.AddFile(dest);
        }

        /// <summary>
        /// </summary>
        protected virtual void AddThirdParty()
        {
            ProjectSources.CopyDirectory(ProjectSources.ThirdParty,
                Path.Combine(UserPrefs.DestinationDirectory, ProjectSources.ThirdParty));
        }

        /// <summary>
        /// </summary>
        protected virtual void AddCMake()
        {
            var cMakeLists = Path.Combine(UserPrefs.SolutionDirectory, @"CMakeLists.txt");

            ProjectSources.CopyFile(ProjectSources.CMakeLists, cMakeLists);
            ProjectSources.CopyDirectory(ProjectSources.CMake, Path.Combine(UserPrefs.SolutionDirectory, @"cmake"));

            ReplaceParameters(cMakeLists);
            Project.AddFile(cMakeLists);
        }

        /// <summary>
        /// </summary>
        protected virtual void AddGitConfigs()
        {
            ProjectSources.CopyFile(ProjectSources.GitIgnore,
                Path.Combine(UserPrefs.SolutionDirectory, ProjectSources.GitIgnore));

            ProjectSources.CopyFile(ProjectSources.GitAttributes,
                Path.Combine(UserPrefs.SolutionDirectory, ProjectSources.GitAttributes));
        }

        /// <summary>
        /// </summary>
        protected virtual void AddCsSdk()
        {
            var sdkDir = Path.Combine(UserPrefs.DestinationDirectory, ProjectSources.CsSdk);
            ProjectSources.CopyDirectory(ProjectSources.CsSdk, sdkDir);

            foreach (var directory in Directory.EnumerateDirectories(sdkDir, @"*", SearchOption.AllDirectories))
            {
                var path = directory.Replace(@"\include\cssdk", @"\include\");
                var filter = AddFilter(path.Remove(0, UserPrefs.DestinationDirectory.Length));

                foreach (var file in Directory.GetFiles(directory, @"*", SearchOption.TopDirectoryOnly))
                    filter.AddFile(file);
            }
        }

        /// <summary>
        /// </summary>
        protected virtual void AddMetamodSdk()
        {
            const string config = @"include\metamod\metamod_config.h";
            var sdkDir = Path.Combine(UserPrefs.DestinationDirectory, ProjectSources.MetamodSdk);
            ProjectSources.CopyDirectory(ProjectSources.MetamodSdk, sdkDir);

            foreach (var directory in Directory.EnumerateDirectories(sdkDir, @"*", SearchOption.AllDirectories))
            {
                var path = directory.Replace(@"\include\metamod", @"\include\");
                var filter = AddFilter(path.Remove(0, UserPrefs.DestinationDirectory.Length));

                foreach (var file in Directory.GetFiles(directory, @"*", SearchOption.TopDirectoryOnly))
                {
                    if (file.EndsWith(config, StringComparison.OrdinalIgnoreCase))
                        continue;

                    filter.AddFile(file);
                }
            }

            ReplaceParameters(Path.Combine(sdkDir, config));
            AddFilter(UserPrefs.PluginProjectDirName).AddFile(Path.Combine(sdkDir, config));
        }

        /// <summary>
        /// </summary>
        protected virtual void AddModuleDefinitionFile()
        {
            var dest = Path.Combine(UserPrefs.MsvcDirectory, UserPrefs.TargetName + @".def");
            ProjectSources.CopyFile(ProjectSources.ModuleDefinition, dest);
            ReplaceParameters(dest);

            foreach (VCConfiguration config in Project.Configurations)
            {
                VCLinkerTool linker = ((IVCCollection)config.Tools).Item(@"VCLinkerTool");
                linker.ModuleDefinitionFile = Path.GetFileName(dest);
            }

            var filter = AddFilter(@"sdk\metamod");
            filter.AddFile(dest);
        }

        /// <summary>
        /// </summary>
        protected VCFilter AddFilter(string path)
        {
            VCFilter filter = null;
            var name = string.Empty;
            var dirs = path.Split(new[] {Path.DirectorySeparatorChar}, StringSplitOptions.RemoveEmptyEntries);

            foreach (var dir in dirs)
            {
                name = Path.Combine(name, dir);
                var item = ((IVCCollection)Project.Filters).Item(name);

                if (item != null)
                {
                    filter = item;
                    continue;
                }

                filter = filter == null ? Project.AddFilter(dir) : filter.AddFilter(dir);
            }

            return filter;
        }

        /// <summary>
        /// </summary>
        protected virtual void SetProperties()
        {
            SetPropertyValue(@"ConfigurationGeneral", @"TargetName", UserPrefs.TargetName);
            SetPropertyValue(@"Microsoft.CodeAnalysis.ClangTidy", @"ClangTidyChecks",
                @"-checks=-*,boost-*,bugprone-*,clang-analyzer-*,cppcoreguidelines-*,misc-*,modernize-*,mpi-*,openmp-*,performance-*,portability-*,readability-*,-cppcoreguidelines-avoid-c-arrays*,-cppcoreguidelines-avoid-magic-numbers*,-cppcoreguidelines-pro-type-reinterpret-cast*,-cppcoreguidelines-pro-bounds-pointer-arithmetic*,-cppcoreguidelines-pro-type-vararg*,-cppcoreguidelines-pro-bounds-array-to-pointer-decay*,-cppcoreguidelines-pro-bounds-constant-array-index*,-cppcoreguidelines-owning-memory*,-modernize-avoid-c-arrays*,-modernize-use-trailing-return-type*,-readability-implicit-bool-conversion*,-readability-magic-numbers*,-readability-named-parameter*");

            const string pageRule = @"CL";
            const string propertyName = @"PreprocessorDefinitions";

            foreach (VCConfiguration config in Project.Configurations)
            {
                var definitions = GetPropertyValue(pageRule, propertyName, false, config);
                definitions = definitions.Replace(@"$exportsdef$", UserPrefs.ExportsDef);
                SetPropertyValue(pageRule, propertyName, definitions, config);
            }
        }

        /// <summary>
        /// </summary>
        protected string GetPropertyValue(string pageRule, string propertyName, bool evaluated = true, object configName = null)
        {
            if (configName == null)
                configName = 1;

            IVCCollection configs = Project.Configurations;
            VCConfiguration config = configs.Item(configName);
            IVCRulePropertyStorage storage = config.Rules.Item(pageRule);

            return evaluated
                ? storage.GetEvaluatedPropertyValue(propertyName)
                : storage.GetUnevaluatedPropertyValue(propertyName);
        }

        /// <summary>
        /// </summary>
        protected void SetPropertyValue(string pageRule, string propertyName, string propertyValue, object configName = null)
        {
            if (configName == null)
            {
                foreach (VCConfiguration config in Project.Configurations)
                {
                    IVCRulePropertyStorage storage = config.Rules.Item(pageRule);
                    storage.SetPropertyValue(propertyName, propertyValue);
                }
            }
            else
            {
                IVCCollection configs = Project.Configurations;
                VCConfiguration config = configs.Item(configName);
                IVCRulePropertyStorage storage = config.Rules.Item(pageRule);
                storage.SetPropertyValue(propertyName, propertyValue);
            }
        }

        /// <summary>
        /// </summary>
        protected void ReplaceParameters(string file)
        {
            var encoding = GetFileEncoding(file);
            ReplaceParameters(file, encoding);
        }

        /// <summary>
        /// </summary>
        protected void ReplaceParameters(string file, Encoding encoding)
        {
            var contents = File.ReadAllText(file, encoding);

            contents = UserPrefs.Replacements.Aggregate(contents, (current, replacement) =>
                current.Replace(replacement.Key, replacement.Value));

            File.WriteAllText(file, contents, encoding);
        }

        /// <summary>
        /// </summary>
        protected static Encoding GetFileEncoding(string file)
        {
            var fileInfo = new FileInfo(file);

            if (fileInfo.Name.EndsWith(@".rc", StringComparison.OrdinalIgnoreCase))
                return Encoding.GetEncoding(1251);

            if (fileInfo.Length < 4)
                return Encoding.ASCII;

            using (var stream = fileInfo.OpenRead())
            {
                var bom = new byte[4];
                stream.Read(bom, 0, 4);

                if (bom[0] == 0x2B && bom[1] == 0x2F && bom[2] == 0x76) return Encoding.UTF7;
                if (bom[0] == 0xEF && bom[1] == 0xBB && bom[2] == 0xBF) return Encoding.UTF8;
                if (bom[0] == 0xFF && bom[1] == 0xFE) return Encoding.Unicode; // UTF-16LE
                if (bom[0] == 0xFE && bom[1] == 0xFF) return Encoding.BigEndianUnicode; // UTF-16BE
                if (bom[0] == 0 && bom[1] == 0 && bom[2] == 0xFE && bom[3] == 0xFF) return Encoding.UTF32;
            }

            return Encoding.ASCII;
        }
    }
}