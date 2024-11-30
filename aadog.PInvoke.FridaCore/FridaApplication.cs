using aadog.PInvoke.LibFridaCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using aadog.PInvoke.Base;

namespace aadog.PInvoke.FridaCore
{
    public unsafe class FridaApplication(LibFridaCore.FridaApplication* NativePointer)
    {

        public string getIdentifier()
        {
            return LibFridaCoreFunctions.frida_application_get_identifier(NativePointer).readUtf8String()!;
        }
        public string getName()
        {
            return LibFridaCoreFunctions.frida_application_get_name(NativePointer).readUtf8String()!;
        }
        public uint getPid()
        {
            return LibFridaCoreFunctions.frida_application_get_pid(NativePointer);
        }
        public Dictionary<string, object> getParameters()
        {
            var result = new Dictionary<string, object>();
            GHashTable* hashTable = LibFridaCoreFunctions.frida_application_get_parameters(NativePointer);
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
