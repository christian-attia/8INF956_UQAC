using System.Collections.Generic;


namespace PluginBase
{
    public interface IUserPlugin
    {
        string Name { get; }
        string Description { get; }

        IEnumerable<User> LoadUsers();
    }
}