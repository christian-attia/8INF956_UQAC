using System;
using System.Collections.Generic;
using PluginBase;

namespace UsersInMemoryPlugin
{
    public class UsersMemoryLoader : IUsersPlugin
    {
        public string Name { get => "UsersInMemoryPlugin"; }
        public string Description { get => "Load fake users"; }

        public IEnumerable<User> LoadUsers() => new List<User>{
                new User("John", "Doe", "john@doe.com"),
                new User("Terry", "Alvarez", "terry@alvarez.com"),
                new User("James", "Collin", "james@collin.com")
            };
    }
}
