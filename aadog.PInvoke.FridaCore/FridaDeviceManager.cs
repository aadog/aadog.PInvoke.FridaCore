using aadog.PInvoke.LibFridaCore;
using aadog.PInvoke.LibFridaCore.Enums;

namespace aadog.PInvoke.FridaCore
{
    public unsafe class FridaDeviceManager:IDisposable
    {
        public LibFridaCore.FridaDeviceManager* NativePointer;
        public static FridaDeviceManager create()
        {
            return new FridaDeviceManager()
            {
                NativePointer = LibFridaCoreFunctions.frida_device_manager_new()
            };
        }

        public FridaDevice addRemoteDevice(string address,FridaRemoteDeviceOptions options)
        {
            GError* error;
            var device=LibFridaCoreFunctions.frida_device_manager_add_remote_device_sync(NativePointer, address, options.NativePointer, null,
                &error);
            var errorMessage = MarshalExt.ConvertLPErrorToString(error);
            if (errorMessage != null)
            {
                LibFridaCoreFunctions.g_error_free(error);
                throw new FridaCoreException(errorMessage);
            }

            return new FridaDevice(device);
        }

        public List<FridaDevice> enumerateDevice()
        {
            var result = new List<FridaDevice>();
            GError* error;
            var devices = LibFridaCoreFunctions.frida_device_manager_enumerate_devices_sync(NativePointer, null, &error);
            var errorMessage = MarshalExt.ConvertLPErrorToString(error);
            if (errorMessage != null)
            {
                LibFridaCoreFunctions.g_error_free(error);
                throw new FridaCoreException(errorMessage);
            }
            var num_devices = LibFridaCoreFunctions.frida_device_list_size(devices);
            for (int i = 0; i < num_devices; i++)
            {
                var device=LibFridaCoreFunctions.frida_device_list_get(devices, i);
                result.Add(new FridaDevice(device));
            }
            LibFridaCoreFunctions.frida_unref(devices);
            devices = null;
            return result;
        }

        public FridaDevice? FindDeviceByType(FridaDeviceType type, int timeout)
        {
            GError* error;
            var device = LibFridaCoreFunctions.frida_device_manager_find_device_by_type_sync(NativePointer, type, timeout, null,
                &error);
            var errorMessage = MarshalExt.ConvertLPErrorToString(error);
            if (errorMessage != null)
            {
                LibFridaCoreFunctions._frida_g_error_free(error);
                throw new FridaCoreException(errorMessage);
            }

            if (device == null)
            {
                return null;
            }

            return new FridaDevice(device);
        }
        public FridaDevice? FindDeviceById(string id, int timeout)
        {
            GError* error;
            var device = LibFridaCoreFunctions.frida_device_manager_find_device_by_id_sync(NativePointer, id, timeout, null,
                &error);
            var errorMessage = MarshalExt.ConvertLPErrorToString(error);
            if (errorMessage != null)
            {
                LibFridaCoreFunctions._frida_g_error_free(error);
                throw new FridaCoreException(errorMessage);
            }

            if (device == null)
            {
                return null;
            }

            return new FridaDevice(device);
        }
        public FridaDevice? GetDeviceById(string id, int timeout)
        {
            GError* error;
            var device = LibFridaCoreFunctions.frida_device_manager_get_device_by_id_sync(NativePointer, id, timeout, null,
                &error);
            var errorMessage = MarshalExt.ConvertLPErrorToString(error);
            if (errorMessage != null)
            {
                LibFridaCoreFunctions._frida_g_error_free(error);
                throw new FridaCoreException(errorMessage);
            }

            if (device == null)
            {
                return null;
            }

            return new FridaDevice(device);
        }
        public void Close()
        {
            GError* error;
            LibFridaCoreFunctions.frida_device_manager_close_sync(NativePointer,null,&error);
            var errorMessage = MarshalExt.ConvertLPErrorToString(error);
            if (errorMessage != null)
            {
                LibFridaCoreFunctions.g_error_free(error);
                throw new FridaCoreException(errorMessage);
            }
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
