using PluginBase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace AppWithPlugin
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                
                string[] pluginDirs = Directory.GetDirectories("../plugins");
                List<string> pluginPaths = new List<string>();
                foreach (string pluginDir in pluginDirs) {
                    string pluginName = new DirectoryInfo(pluginDir).Name;
                    string dllPath = Path.Join(pluginDir, @"\bin\Debug\netcoreapp3.1", $"{pluginName}.dll");
                    pluginPaths.Add(dllPath);
                }

                IEnumerable<IUserPlugin> commands = pluginPaths.SelectMany(pluginPath =>
                {
                    Assembly pluginAssembly = LoadPlugin(pluginPath);
                    return CreateCommands(pluginAssembly);
                }).ToList();

               
                foreach (IUserPlugin command in commands)
                {
                    Console.WriteLine($"Plugin: {command.Name}");
                    Console.WriteLine("\tFirst 10 users");
                    IEnumerable<User> users = command.LoadUsers();
                    foreach (User user in users.Take(10)) {
                        Console.WriteLine(user.ToString());
                    }
                    Console.WriteLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        static Assembly LoadPlugin(string relativePath)
        {
            string root = Path.GetFullPath(Path.Combine(
                    Path.GetDirectoryName(
                        Path.GetDirectoryName(
                            Path.GetDirectoryName(
                                Path.GetDirectoryName(typeof(Program).Assembly.Location))))));

            string pluginLocation = Path.GetFullPath(Path.Combine(root, relativePath.Replace('\\', Path.DirectorySeparatorChar)));
            Console.WriteLine($"Loading commands from: {pluginLocation}");
            PluginLoadContext loadContext = new PluginLoadContext(pluginLocation);
            return loadContext.LoadFromAssemblyName(new AssemblyName(Path.GetFileNameWithoutExtension(pluginLocation)));
        }

        static IEnumerable<IUserPlugin> CreateCommands(Assembly assembly)
        {
            int count = 0;

            foreach (Type type in assembly.GetTypes())
            {
                if (typeof(IUserPlugin).IsAssignableFrom(type))
                {
                    IUserPlugin result = Activator.CreateInstance(type) as IUserPlugin;
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