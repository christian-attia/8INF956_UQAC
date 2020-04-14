using System.Collections.Generic;


namespace PluginBase
{
    public interface IUsersPlugin
    {
        string Name { get; }
        string Description { get; }

        IEnumerable<User> LoadUsers();
    }
}