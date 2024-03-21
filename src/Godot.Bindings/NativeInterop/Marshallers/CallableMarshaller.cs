using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Godot.Bridge;

namespace Godot.NativeInterop.Marshallers;

internal unsafe static class CallableMarshaller
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteUnmanaged(NativeGodotCallable* destination, Callable value)
    {
        *destination = value.NativeValue.DangerousSelfRef;
    }

    public static NativeGodotCallable* ConvertToUnmanaged(Callable value)
    {
        NativeGodotCallable* ptr = (NativeGodotCallable*)Marshal.AllocHGlobal(sizeof(NativeGodotCallable));
        WriteUnmanaged(ptr, value);
        return ptr;
    }

    public static Callable ConvertFromUnmanaged(NativeGodotCallable* value)
    {
        Debug.Assert(value is not null);

        void* userData = GodotBridge.GDExtensionInterface.callable_custom_get_userdata(value, GodotBridge.LibraryPtr);
        if (userData is not null)
        {
            // userData will only be non-null if the Callable is a custom Callable created with our LibraryPtr,
            // and since all our custom Callables derive from the CustomCallable class, then it has to be one.
            var gcHandle = GCHandle.FromIntPtr((nint)userData);
            var customCallable = gcHandle.Target as CustomCallable;
            Debug.Assert(customCallable is not null);

            return Callable.CreateTakingOwnership(customCallable);
        }

        return Callable.CreateTakingOwnership(*value);
    }

    public static void Free(NativeGodotCallable* value)
    {
        Marshal.FreeHGlobal((nint)value);
    }

    public static NativeGodotVariant* ConvertToVariant(Callable value)
    {
        NativeGodotVariant* ptr = (NativeGodotVariant*)Marshal.AllocHGlobal(sizeof(NativeGodotVariant));
        *ptr = NativeGodotVariant.CreateFromCallableTakingOwnership(value.NativeValue.DangerousSelfRef);
        return ptr;
    }

    public static Callable ConvertFromVariant(NativeGodotVariant* value)
    {
        Debug.Assert(value is not null);
        return ConvertFromUnmanaged(NativeGodotVariant.GetOrConvertToCallable(*value).GetUnsafeAddress());
    }

    public static void FreeVariant(NativeGodotVariant* value)
    {
        Debug.Assert(value is not null);
        value->Dispose();
        Marshal.FreeHGlobal((nint)value);
    }
}
