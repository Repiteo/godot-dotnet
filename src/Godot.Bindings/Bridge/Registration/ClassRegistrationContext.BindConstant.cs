using System;
using System.Collections.Generic;
using Godot.NativeInterop;

namespace Godot.Bridge;

partial class ClassRegistrationContext
{
    private readonly HashSet<StringName> _registeredConstants = [];

    /// <summary>
    /// Register a constant in the class.
    /// </summary>
    /// <param name="constantDefinition">Information that describes the constant to register.</param>
    /// <exception cref="ArgumentException">
    /// A constant has already been registered with the same name.
    /// </exception>
    public unsafe void BindConstant(ConstantDefinition constantDefinition)
    {
        if (!_registeredConstants.Add(constantDefinition.Name))
        {
            throw new ArgumentException(SR.FormatArgument_ConstantAlreadyRegistered(constantDefinition.Name, ClassName), nameof(constantDefinition));
        }

        StringName enumName = constantDefinition.EnumName ?? StringName.Empty;

        if (enumName.IsEmpty && constantDefinition.IsFlagsEnum)
        {
            throw new ArgumentException(SR.FormatArgument_ConstantWithoutEnumCantBeFlag(constantDefinition.Name), nameof(constantDefinition));
        }

        _registerBindingActions.Enqueue(() =>
        {
            NativeGodotStringName constantNameNative = constantDefinition.Name.NativeValue.DangerousSelfRef;
            NativeGodotStringName enumNameNative = enumName.NativeValue.DangerousSelfRef;

            NativeGodotStringName classNameNative = ClassName.NativeValue.DangerousSelfRef;

            GodotBridge.GDExtensionInterface.classdb_register_extension_class_integer_constant(GodotBridge.LibraryPtr, &classNameNative, &enumNameNative, &constantNameNative, constantDefinition.Value, constantDefinition.IsFlagsEnum);
        });
    }
}
