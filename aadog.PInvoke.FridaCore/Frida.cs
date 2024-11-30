
using aadog.PInvoke.LibFridaCore;
using aadog.PInvoke.Base;

namespace aadog.PInvoke.FridaCore
{

    public unsafe class Frida:IDisposable
    {
        public Frida()
        {
            LibFridaCoreFunctions.frida_init();
        }
        public void Dispose()
        {
            DeInit();
        }
        public static FridaDeviceManager DeviceManager;
        public static string VersionString()
        {
            var v = LibFridaCoreFunctions.frida_version_string();
            return v.readUtf8String()!;
        }
        public static  void Version(uint* major, uint *minor, uint *micro, uint *nano)
        {
            LibFridaCoreFunctions.frida_version(major, minor, micro,nano);
        }
        public static void Init()
        {
            LibFridaCoreFunctions.frida_init();
        }
        public static void DeInit()
        {
            LibFridaCoreFunctions.frida_deinit();
        }
        public static void Shutdown()
        {
            LibFridaCoreFunctions.frida_shutdown();
        }
    }
}
