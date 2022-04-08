using AnyProgram;

var pluginManager = PluginManager.Instance();
var loaded = pluginManager.LoadPlugins();
Console.WriteLine($"Plugin loaded: {loaded}");

if (loaded > 0) {
    foreach (var plugin in pluginManager.GetNamePlugins()) {
        Console.WriteLine($"- {plugin}");
    }

    foreach (var plugin in pluginManager.GetPlugins()) {
        Console.WriteLine("MethodB: " + plugin.MethodB().ToString());
    }
}
