using Newtonsoft.Json;
using PluginBase;
using System;
using System.Collections.Generic;
using System.IO;

namespace UsersJsonPlugin
{
    public class UsersJsonLoader : IUserPlugin
    {
        public string Name { get => "UserJSONLoader"; }
        public string Description { get => "Load JSON users"; }

        public IEnumerable<User> LoadUsers()
        {   
            List<User> users = new List<User>();
            string[] jsonFiles = Directory.GetFiles("../users/", "*.json");
            var settings = new JsonSerializerSettings();
            settings.ContractResolver = new UserJsonResolver();
            foreach (string file in jsonFiles) {
                Console.WriteLine(file);
                List<User> tmp = JsonConvert.DeserializeObject<List<User>>(File.ReadAllText(@file), settings);
                users.AddRange(tmp);
            }
            return users;
        }
    }


    // Mapping des noms attributs json -> User
    public class UserJsonResolver : Newtonsoft.Json.Serialization.DefaultContractResolver
    {
        private Dictionary<string, string> PropertyMappings { get; set; }

        public UserJsonResolver()
        {
            this.PropertyMappings = new Dictionary<string, string> 
            {
                {"FirstName", "first_name"},
                {"LastName", "last_name"},
                {"Email", "email"},
            };
        }

        protected override string ResolvePropertyName(string propertyName)
        {
            string resolvedName = null;
            var resolved = this.PropertyMappings.TryGetValue(propertyName, out resolvedName);
            return (resolved) ? resolvedName : base.ResolvePropertyName(propertyName);
        }
    }
}