using System.Runtime.InteropServices;
using aadog.PInvoke.Base;
using aadog.PInvoke.LibFridaCore;

namespace aadog.PInvoke.FridaCore
{
    public unsafe class Variant
    {
        public static object from_ptr(GVariant* ptr)
        {
            string variantString = LibFridaCoreFunctions.g_variant_get_type_string(ptr).readUtf8String()!;
            object obj = variantString switch
            {
                "s" => LibFridaCoreFunctions.g_variant_get_string((GVariant*)ptr, null).readUtf8String()!,
                "b" => LibFridaCoreFunctions.g_variant_get_boolean((GVariant*)ptr)!=0,
                "x" => LibFridaCoreFunctions.g_variant_get_int64((GVariant*)ptr),
                "a{sv}" => sv_array_to_map(ptr),
                "aa{sv}" => asv_array_to_maplist(ptr),
                _ => throw new ArgumentException($"{variantString}")
            };
            return obj;
        }

        public static Dictionary<string,object> sv_array_to_map(GVariant* ptr)
        {
            var ret = new Dictionary<string, object>();
            var iter = LibFridaCoreFunctions.g_variant_iter_new(ptr);
            void* key;
            GVariant* val=null;
            while (LibFridaCoreFunctions.g_variant_iter_loop(iter, "{sv}", &key,&val)!=0)
            {
                var strKey = new IntPtr(key).readUtf8String()!;
                var objVal = Variant.from_ptr(val);
                ret.Add(strKey,objVal);
            }
            LibFridaCoreFunctions.g_variant_iter_free(iter);
            return ret;
        }
        public static List<Dictionary<string, object>> asv_array_to_maplist(GVariant* ptr)
        {
            var ret = new List<Dictionary<string, object>>();
            var outer = LibFridaCoreFunctions.g_variant_iter_new(ptr);
            void* inner;
            while (LibFridaCoreFunctions.g_variant_iter_loop(outer, "a{sv}", &inner, null) != 0)
            {
                var map = new Dictionary<string, object>();
                var innerIter = (GVariantIter*)inner;
                void* key;
                GVariant* val = null;
                while (LibFridaCoreFunctions.g_variant_iter_loop(innerIter, "{sv}", &key, &val) != 0)
                {
                    var strKey = new IntPtr(key).readUtf8String()!;
                    var objVal = Variant.from_ptr(val);
                    map.Add(strKey, objVal);
                }
                ret.Add(map);
            }
            LibFridaCoreFunctions.g_variant_iter_free(outer);
            return ret;
        }
    }
}