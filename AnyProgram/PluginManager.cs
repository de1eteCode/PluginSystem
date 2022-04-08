using PluginInterface;
using System.Runtime.Loader;

namespace AnyProgram;
public class PluginManager {
    #region Singleton
    private static PluginManager? _instance;
    private static object _instanceLock = new object();
    public static PluginManager Instance() {
        if (_instance == null) {
            lock (_instanceLock) {
                if (_instance == null) {
                    _instance = new PluginManager();
                }
            }
        }
        return _instance;
    }
    #endregion

    private const string _pluginExtension = "dll";
    private const string _pluginPattern = "*.dll";
    private const string _pluginDirectory = "plugins";
    private Dictionary<string, IPlugin> _plugins = new();

    private PluginManager() { }

    public int LoadPlugins() {
        int counter = 0;

        if (Directory.Exists(_pluginDirectory) == false) {
            Directory.CreateDirectory(_pluginDirectory);
        }

        var directoryFiles = Directory.GetFiles(_pluginDirectory, _pluginPattern);

        if (directoryFiles.Length < 1) {
            return counter;
        }

        foreach (var directoryFile in directoryFiles) {
            var res = LoadPlugin(Path.GetFileNameWithoutExtension(directoryFile));
            if (res) {
                counter++;
            }
        }

        return counter;
    }

    public bool LoadPlugin(string pluginName) {
        if (GetPlugin(pluginName) != null) {
#if DEBUG
            throw new Exception("Данный плагин уже загружен");
#endif
#pragma warning disable CS0162 // Обнаружен недостижимый код
            return true;
#pragma warning restore CS0162 // Обнаружен недостижимый код
        }

        string? dll = Directory.GetFiles(_pluginDirectory, String.Format("{0}.{1}", pluginName, _pluginExtension)).FirstOrDefault();

        if (string.IsNullOrEmpty(dll)) {
            return false;
        }

        dll = Path.GetFullPath(dll);

        var assemblyLoadContext = new AssemblyLoadContext(dll);
        var assembly = assemblyLoadContext.LoadFromAssemblyPath(dll);

        IPlugin? plugin = null;

        try {
            plugin = Activator.CreateInstance(assembly.GetTypes().FirstOrDefault()) as IPlugin;
            if (plugin != null) {
                _plugins[pluginName] = plugin;
                return true;
            }
            else {
                return false;
            }
        }
        catch (Exception) {
            // Обработка ошибки
        }

        return false;
    }

    public IEnumerable<string> GetNamePlugins() {
        return _plugins.Keys;
    }

    public IEnumerable<IPlugin> GetPlugins() {
        return _plugins.Values;
    }

    public IPlugin? GetPlugin(string name) {
        if (_plugins.ContainsKey(name)) {
            return _plugins[name];
        }
        return null;
    }
}
