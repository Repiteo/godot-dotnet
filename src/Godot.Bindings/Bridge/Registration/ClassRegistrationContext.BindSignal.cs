using System;
using System.Collections.Generic;
using Godot.NativeInterop;

namespace Godot.Bridge;

partial class ClassRegistrationContext
{
    private readonly HashSet<StringName> _registeredSignals = new(StringNameEqualityComparer.Default);

    private const int ParameterSpanThreshold = 8;

    /// <summary>
    /// Register a signal in the class.
    /// The registered class can be emitted with
    /// <see cref="GodotObject.EmitSignal(StringName, ReadOnlySpan{Variant})"/>
    /// using the name that the signal was registered with.
    /// </summary>
    /// <param name="signalDefinition">Information that describes the signal to register.</param>
    /// <exception cref="ArgumentException">
    /// A signal has already been registered with the same name.
    /// </exception>
    public unsafe void BindSignal(SignalDefinition signalDefinition)
    {
        if (!_registeredSignals.Add(signalDefinition.Name))
        {
            throw new ArgumentException(SR.FormatArgument_SignalAlreadyRegistered(signalDefinition.Name, ClassName), nameof(signalDefinition));
        }

        _registerBindingActions.Enqueue(() =>
        {
            // Convert managed signal info to the internal unmanaged type.
            Span<GDExtensionPropertyInfo> parameters = signalDefinition.Parameters.Count <= ParameterSpanThreshold
                ? stackalloc GDExtensionPropertyInfo[ParameterSpanThreshold].Slice(0, signalDefinition.Parameters.Count)
                : new GDExtensionPropertyInfo[signalDefinition.Parameters.Count];
            for (int i = 0; i < parameters.Length; i++)
            {
                var parameterDefinition = signalDefinition.Parameters[i];

                NativeGodotStringName parameterNameNative = parameterDefinition.Name.NativeValue.DangerousSelfRef;
                NativeGodotStringName parameterClassNameNative = (parameterDefinition.ClassName?.NativeValue ?? default).DangerousSelfRef;
                NativeGodotString hintStringNative = NativeGodotString.Create(parameterDefinition.HintString);

                parameters[i] = new GDExtensionPropertyInfo()
                {
                    type = (GDExtensionVariantType)parameterDefinition.Type,
                    name = &parameterNameNative,

                    hint = (uint)parameterDefinition.Hint,
                    hint_string = &hintStringNative,
                    class_name = &parameterClassNameNative,
                    usage = (uint)parameterDefinition.Usage,
                };
            }

            NativeGodotStringName signalNameNative = signalDefinition.Name.NativeValue.DangerousSelfRef;

            NativeGodotStringName classNameNative = ClassName.NativeValue.DangerousSelfRef;

            fixed (GDExtensionPropertyInfo* parametersPtr = parameters)
            {
                GodotBridge.GDExtensionInterface.classdb_register_extension_class_signal(GodotBridge.LibraryPtr, &classNameNative, &signalNameNative, parametersPtr, parameters.Length);
            }
        });
    }
}
