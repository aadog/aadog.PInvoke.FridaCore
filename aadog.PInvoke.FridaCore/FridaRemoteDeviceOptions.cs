using aadog.PInvoke.LibFridaCore;
namespace aadog.PInvoke.FridaCore
{
    public unsafe class FridaRemoteDeviceOptions:IDisposable
    {
        public LibFridaCore.FridaRemoteDeviceOptions* NativePointer;
        public static FridaRemoteDeviceOptions create()
        {
            return new FridaRemoteDeviceOptions()
            {
                NativePointer = LibFridaCoreFunctions.frida_remote_device_options_new()
            };
        }

        public void SetToken(string value)
        {
            LibFridaCoreFunctions.frida_remote_device_options_set_token(NativePointer, value);
        }

        public void Dispose()
        {
            unRef();
        }
        public void addRef()
        {
            LibFridaCoreFunctions.frida_unref(NativePointer);
        }
        public void unRef()
        {
            LibFridaCoreFunctions.frida_unref(NativePointer);
        }
    }
}
