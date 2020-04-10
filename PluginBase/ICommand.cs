namespace PluginBase
{
    public interface ICommand
    {
        
        string Name { get; }
        string Description { get; }


        int Execute();

        string[] pluginPaths = new string[]
{
    // Paths to plugins to load.
};

IEnumerable<ICommand> commands = pluginPaths.SelectMany(pluginPath =>
{
    Assembly pluginAssembly = LoadPlugin(pluginPath);
    return CreateCommands(pluginAssembly);
}).ToList();
    }
}