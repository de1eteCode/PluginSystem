using PluginInterface;

namespace Noter;
public class NoterMng : IPlugin {
    public void MethodA() { }

    public int MethodB() {
        return 22;
    }

    public void MethodC(int a) { }

    public int MethodD(int a) {
        return a * 0;
    }
}
