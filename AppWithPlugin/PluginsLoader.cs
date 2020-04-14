using PluginBase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;


namespace AppWithPlugin
{
    // S'occupe du chargement des plugins
    class PluginsLoader
    {
        public static IEnumerable<IUsersPlugin> LoadPlugins() {
            string[] pluginDirs = Directory.GetDirectories("./plugins");
            List<string> pluginPaths = new List<string>();
            foreach (string pluginDir in pluginDirs) {
                string pluginName = new DirectoryInfo(pluginDir).Name;
                string dllPath = Path.Join(pluginDir, @"\bin\Debug\netcoreapp3.1", $"{pluginName}.dll");
                pluginPaths.Add(dllPath);
            }

            IEnumerable<IUsersPlugin> commands = pluginPaths.SelectMany(pluginPath =>
            {
                Assembly pluginAssembly = LoadPlugin(pluginPath);
                return CreateCommands(pluginAssembly);
            }).ToList();
            return commands;
        }

        static Assembly LoadPlugin(string relativePath)
        {
            // navigation to plugins root dir
            string pluginRoot = Path.GetFullPath(Path.Combine(
                Path.GetDirectoryName(
                    Path.GetDirectoryName(
                        Path.GetDirectoryName(
                            Path.GetDirectoryName(
                                Path.GetDirectoryName(typeof(Program).Assembly.Location)))))));

            string pluginLocation = Path.GetFullPath(Path.Combine(pluginRoot, relativePath.Replace('\\', Path.DirectorySeparatorChar)));
            Console.WriteLine($"Loading commands from: {pluginLocation}");
            PluginLoadContext loadContext = new PluginLoadContext(pluginLocation);
            return loadContext.LoadFromAssemblyName(new AssemblyName(Path.GetFileNameWithoutExtension(pluginLocation)));
        }

        static IEnumerable<IUsersPlugin> CreateCommands(Assembly assembly)
        {
            int count = 0;

            foreach (Type type in assembly.GetTypes())
            {
                if (typeof(IUsersPlugin).IsAssignableFrom(type))
                {
                    IUsersPlugin result = Activator.CreateInstance(type) as IUsersPlugin;
                    if (result != null)
                    {
                        count++;
                        yield return result;
                    }
                }
            }

            if (count == 0)
            {
                string availableTypes = string.Join(",", assembly.GetTypes().Select(t => t.FullName));
                throw new ApplicationException(
                    $"Can't find any type which implements ICommand in {assembly} from {assembly.Location}.\n" +
                    $"Available types: {availableTypes}");
            }
        }
    }
}