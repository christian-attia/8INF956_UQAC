using PluginBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;


namespace AppWithPlugin
{
    class Program
    {
        static void Main(string[] args)
        {   
            try 
            {
                IEnumerable<IUsersPlugin> plugins = PluginsLoader.LoadPlugins();
                foreach (IUsersPlugin plugin in plugins)
                {
                    Console.WriteLine($"Plugin: {plugin.Name}");
                    Console.WriteLine("\tSample of the users loaded:");
                    IEnumerable<User> users = plugin.LoadUsers();
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
    }
}