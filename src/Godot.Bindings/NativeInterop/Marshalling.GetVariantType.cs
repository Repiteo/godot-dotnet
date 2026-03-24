using System;
using System.Text;
using Godot.Collections;

namespace Godot.NativeInterop;

partial class Marshalling
{
    internal static GDExtensionVariantType GetVariantType<[MustBeVariant] T>()
    {
        // `typeof(T1) == typeof(T2)` is optimized away. We cannot cache `typeof(T)` in a local variable, as it's not optimized when done like that.

        if (typeof(T) == typeof(bool))
        {
            return GDExtensionVariantType.GDEXTENSION_VARIANT_TYPE_BOOL;
        }

        if (typeof(T) == typeof(char)
         || typeof(T) == typeof(Rune)
         || typeof(T) == typeof(sbyte)
         || typeof(T) == typeof(short)
         || typeof(T) == typeof(int)
         || typeof(T) == typeof(long)
         || typeof(T) == typeof(byte)
         || typeof(T) == typeof(ushort)
         || typeof(T) == typeof(uint)
         || typeof(T) == typeof(ulong)
         || typeof(T) == typeof(nint))
        {
            return GDExtensionVariantType.GDEXTENSION_VARIANT_TYPE_INT;
        }

        if (typeof(T) == typeof(Half)
         || typeof(T) == typeof(float)
         || typeof(T) == typeof(double))
        {
            return GDExtensionVariantType.GDEXTENSION_VARIANT_TYPE_FLOAT;
        }

        if (typeof(T) == typeof(string))
        {
            return GDExtensionVariantType.GDEXTENSION_VARIANT_TYPE_STRING;
        }

        if (typeof(T) == typeof(Aabb))
        {
            return GDExtensionVariantType.GDEXTENSION_VARIANT_TYPE_AABB;
        }

        if (typeof(T) == typeof(Basis))
        {
            return GDExtensionVariantType.GDEXTENSION_VARIANT_TYPE_BASIS;
        }

        if (typeof(T) == typeof(Callable))
        {
            return GDExtensionVariantType.GDEXTENSION_VARIANT_TYPE_CALLABLE;
        }

        if (typeof(T) == typeof(Color))
        {
            return GDExtensionVariantType.GDEXTENSION_VARIANT_TYPE_COLOR;
        }

        if (typeof(T) == typeof(NodePath))
        {
            return GDExtensionVariantType.GDEXTENSION_VARIANT_TYPE_NODE_PATH;
        }

        if (typeof(T) == typeof(Plane))
        {
            return GDExtensionVariantType.GDEXTENSION_VARIANT_TYPE_PLANE;
        }

        if (typeof(T) == typeof(Projection))
        {
            return GDExtensionVariantType.GDEXTENSION_VARIANT_TYPE_PROJECTION;
        }

        if (typeof(T) == typeof(Quaternion))
        {
            return GDExtensionVariantType.GDEXTENSION_VARIANT_TYPE_QUATERNION;
        }

        if (typeof(T) == typeof(Rect2))
        {
            return GDExtensionVariantType.GDEXTENSION_VARIANT_TYPE_RECT2;
        }

        if (typeof(T) == typeof(Rect2I))
        {
            return GDExtensionVariantType.GDEXTENSION_VARIANT_TYPE_RECT2I;
        }

        if (typeof(T) == typeof(Rid))
        {
            return GDExtensionVariantType.GDEXTENSION_VARIANT_TYPE_RID;
        }

        if (typeof(T) == typeof(Signal))
        {
            return GDExtensionVariantType.GDEXTENSION_VARIANT_TYPE_SIGNAL;
        }

        if (typeof(T) == typeof(StringName))
        {
            return GDExtensionVariantType.GDEXTENSION_VARIANT_TYPE_STRING_NAME;
        }

        if (typeof(T) == typeof(Transform2D))
        {
            return GDExtensionVariantType.GDEXTENSION_VARIANT_TYPE_TRANSFORM2D;
        }

        if (typeof(T) == typeof(Transform3D))
        {
            return GDExtensionVariantType.GDEXTENSION_VARIANT_TYPE_TRANSFORM3D;
        }

        if (typeof(T) == typeof(Vector2))
        {
            return GDExtensionVariantType.GDEXTENSION_VARIANT_TYPE_VECTOR2;
        }

        if (typeof(T) == typeof(Vector2I))
        {
            return GDExtensionVariantType.GDEXTENSION_VARIANT_TYPE_VECTOR2I;
        }

        if (typeof(T) == typeof(Vector3))
        {
            return GDExtensionVariantType.GDEXTENSION_VARIANT_TYPE_VECTOR3;
        }

        if (typeof(T) == typeof(Vector3I))
        {
            return GDExtensionVariantType.GDEXTENSION_VARIANT_TYPE_VECTOR3I;
        }

        if (typeof(T) == typeof(Vector4))
        {
            return GDExtensionVariantType.GDEXTENSION_VARIANT_TYPE_VECTOR4;
        }

        if (typeof(T) == typeof(Vector4I))
        {
            return GDExtensionVariantType.GDEXTENSION_VARIANT_TYPE_VECTOR4I;
        }

        if (typeof(T) == typeof(PackedByteArray))
        {
            return GDExtensionVariantType.GDEXTENSION_VARIANT_TYPE_PACKED_BYTE_ARRAY;
        }

        if (typeof(T) == typeof(PackedInt32Array))
        {
            return GDExtensionVariantType.GDEXTENSION_VARIANT_TYPE_PACKED_INT32_ARRAY;
        }

        if (typeof(T) == typeof(PackedInt64Array))
        {
            return GDExtensionVariantType.GDEXTENSION_VARIANT_TYPE_PACKED_INT64_ARRAY;
        }

        if (typeof(T) == typeof(PackedFloat32Array))
        {
            return GDExtensionVariantType.GDEXTENSION_VARIANT_TYPE_PACKED_FLOAT32_ARRAY;
        }

        if (typeof(T) == typeof(PackedFloat64Array))
        {
            return GDExtensionVariantType.GDEXTENSION_VARIANT_TYPE_PACKED_FLOAT64_ARRAY;
        }

        if (typeof(T) == typeof(PackedStringArray))
        {
            return GDExtensionVariantType.GDEXTENSION_VARIANT_TYPE_PACKED_STRING_ARRAY;
        }

        if (typeof(T) == typeof(PackedVector2Array))
        {
            return GDExtensionVariantType.GDEXTENSION_VARIANT_TYPE_PACKED_VECTOR2_ARRAY;
        }

        if (typeof(T) == typeof(PackedVector3Array))
        {
            return GDExtensionVariantType.GDEXTENSION_VARIANT_TYPE_PACKED_VECTOR3_ARRAY;
        }

        if (typeof(T) == typeof(PackedColorArray))
        {
            return GDExtensionVariantType.GDEXTENSION_VARIANT_TYPE_PACKED_COLOR_ARRAY;
        }

        if (typeof(T) == typeof(PackedVector4Array))
        {
            return GDExtensionVariantType.GDEXTENSION_VARIANT_TYPE_PACKED_VECTOR4_ARRAY;
        }

        if (typeof(T) == typeof(GodotArray))
        {
            return GDExtensionVariantType.GDEXTENSION_VARIANT_TYPE_ARRAY;
        }

        if (typeof(T) == typeof(GodotDictionary))
        {
            return GDExtensionVariantType.GDEXTENSION_VARIANT_TYPE_DICTIONARY;
        }

        // More complex checks here at the end, to avoid screwing the simple ones in case they're not optimized away.

        // `typeof(T1).IsAssignableFrom(typeof(T2))` is optimized away.

        if (typeof(GodotObject).IsAssignableFrom(typeof(T)))
        {
            return GDExtensionVariantType.GDEXTENSION_VARIANT_TYPE_OBJECT;
        }

        // `typeof(T).IsEnum` is optimized away.

        if (typeof(T).IsEnum)
        {
            return GDExtensionVariantType.GDEXTENSION_VARIANT_TYPE_INT;
        }

        // Variant and other generic types (GodotArray<T>, GodotDictionary<K,V>) fall through to NIL.
        // NIL means "no element type constraint" (untyped).
        return GDExtensionVariantType.GDEXTENSION_VARIANT_TYPE_NIL;
    }
}
