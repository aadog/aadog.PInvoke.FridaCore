using aadog.PInvoke.Base;
using aadog.PInvoke.LibFridaCore;

namespace aadog.PInvoke.FridaCore
{
    public unsafe class FridaProcess(LibFridaCore.FridaProcess* NativePointer):IDisposable
    {
        public string getName()
        {
            return LibFridaCoreFunctions.frida_process_get_name(NativePointer).readUtf8String()!;
        }
        public uint getPid()
        {
            return LibFridaCoreFunctions.frida_process_get_pid(NativePointer);
        }
        public Dictionary<string, object> getParameters()
        {
            var result = new Dictionary<string, object>();
            GHashTable* hashTable = LibFridaCoreFunctions.frida_process_get_parameters(NativePointer);
            var fn = new LibFridaCoreFunctions.GHFunc((key, value, data) =>
            {
                var strKey = key.readUtf8String()!;

                result.Add(strKey, Variant.from_ptr((GVariant*)value.ToPointer()));
            });
            LibFridaCoreFunctions.g_hash_table_foreach(hashTable, fn, IntPtr.Zero);
            return result;
        }
        public void Dispose()
        {
            unRef();
        }
        public void addRef()
        {
            LibFridaCoreFunctions.g_object_ref(NativePointer);
        }
        public void unRef()
        {
            LibFridaCoreFunctions.g_object_unref(NativePointer);
        }
    }
}
