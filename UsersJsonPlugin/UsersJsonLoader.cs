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

        public List<User> LoadUsers()
        {
            List<User> users = JsonConvert.DeserializeObject<List<User>>(File.ReadAllText(@"../users/users_default.json"));
            return users;
        }
  }
}