using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using aadog.PInvoke.LibFridaCore;

namespace aadog.PInvoke.FridaCore
{
    public unsafe class FridaProcessQueryOptions :IDisposable
    {
        public LibFridaCore.FridaProcessQueryOptions* NativePointer;
        public static FridaProcessQueryOptions create()
        {
            return new FridaProcessQueryOptions()
            {
                NativePointer = LibFridaCoreFunctions.frida_process_query_options_new()
            };
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
