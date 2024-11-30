using aadog.PInvoke.LibFridaCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aadog.PInvoke.FridaCore
{
    public unsafe class FridaApplicationQueryOptions:IDisposable
    {
        public LibFridaCore.FridaApplicationQueryOptions* NativePointer;
        public static FridaApplicationQueryOptions create()
        {
            return new FridaApplicationQueryOptions()
            {
                NativePointer = LibFridaCoreFunctions.frida_application_query_options_new()
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
