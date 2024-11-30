using aadog.PInvoke.Base;
using aadog.PInvoke.LibFridaCore;
using aadog.PInvoke.LibFridaCore.Enums;

namespace aadog.PInvoke.FridaCore
{
    public unsafe class FridaDevice(LibFridaCore.FridaDevice* NativePointer):IDisposable
    {
        public List<FridaProcess> enumerateProcesses(FridaProcessQueryOptions? options)
        {
            var result = new List<FridaProcess>();
            GError* error;
            FridaProcessList* processes=LibFridaCoreFunctions.frida_device_enumerate_processes_sync(NativePointer,options==null?null:options.NativePointer,null,&error);
            var errorMessage = MarshalExt.ConvertLPErrorToString(error);
            if (errorMessage != null)
            {
                LibFridaCoreFunctions.g_error_free(error);
                throw new FridaCoreException(errorMessage);
            }

            var num_devices = LibFridaCoreFunctions.frida_process_list_size(processes);
            for (int i = 0; i < num_devices; i++)
            {
                var device = LibFridaCoreFunctions.frida_process_list_get(processes, i);
                result.Add(new FridaProcess(device));
            }
            LibFridaCoreFunctions.frida_unref(processes);
            processes = null;

            return result;
        }
        public List<FridaApplication> enumerateApplications(FridaApplicationQueryOptions? options)
        {
            var result = new List<FridaApplication>();
            GError* error;
            FridaApplicationList* applications = LibFridaCoreFunctions.frida_device_enumerate_applications_sync(NativePointer, options == null ? null : options.NativePointer, null, &error);
            var errorMessage = MarshalExt.ConvertLPErrorToString(error);
            if (errorMessage != null)
            {
                LibFridaCoreFunctions.g_error_free(error);
                throw new FridaCoreException(errorMessage);
            }

            var num_devices = LibFridaCoreFunctions.frida_application_list_size(applications);
            for (int i = 0; i < num_devices; i++)
            {
                var application = LibFridaCoreFunctions.frida_application_list_get(applications, i);
                result.Add(new FridaApplication(application));
            }
            LibFridaCoreFunctions.frida_unref(applications);
            applications = null;

            return result;
        }

        public string getId()
        {
            return LibFridaCoreFunctions.frida_device_get_id(NativePointer).readUtf8String()!;
        }
        public string getName()
        {
            return LibFridaCoreFunctions.frida_device_get_name(NativePointer).readUtf8String()!;
        }
        public FridaDeviceType getType()
        {
            return LibFridaCoreFunctions.frida_device_get_dtype(NativePointer);
        }
        public Dictionary<string, object> querySystemParameters()
        {
            var result=new Dictionary<string, object>();
            GError* error;
            GHashTable* hashTable=LibFridaCoreFunctions.frida_device_query_system_parameters_sync(NativePointer,null,&error);
            var errorMessage = MarshalExt.ConvertLPErrorToString(error);
            if (errorMessage != null)
            {
                LibFridaCoreFunctions.g_error_free(error);
                throw new FridaCoreException(errorMessage);
            }
            var fn = new LibFridaCoreFunctions.GHFunc((key, value, data) =>
            {
                var strKey = key.readUtf8String()!;
                
                result.Add(strKey,Variant.from_ptr((GVariant*)value.ToPointer()));
            });
            LibFridaCoreFunctions.g_hash_table_foreach(hashTable,fn ,IntPtr.Zero);
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
