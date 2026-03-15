using System;
using System.Collections.Generic;
using Godot.NativeInterop;

namespace Godot.Bridge;

partial class ClassRegistrationContext
{
    private readonly HashSet<StringName> _registeredProperties = new(StringNameEqualityComparer.Default);

    /// <summary>
    /// Register a property in the class.
    /// </summary>
    /// <remarks>
    /// The <paramref name="propertyDefinition"/> must include the names of the methods
    /// to be used as getter and setter. These methods must have previously been
    /// registered using <see cref="BindMethod(MethodDefinition)"/>.
    /// </remarks>
    /// <param name="propertyDefinition">Information that describes the property to register.</param>
    /// <exception cref="ArgumentException">
    /// A property has already been registered with the same name.
    /// </exception>
    public unsafe void BindProperty(PropertyDefinitionWithAccessors propertyDefinition)
    {
        if (!_registeredProperties.Add(propertyDefinition.Name))
        {
            throw new ArgumentException(SR.FormatArgument_PropertyAlreadyRegistered(propertyDefinition.Name, ClassName), nameof(propertyDefinition));
        }

        _registerBindingActions.Enqueue(() =>
        {
            // Convert managed property info to the internal unmanaged type.
            GDExtensionPropertyInfo propertyInfoNative;
            {
                NativeGodotStringName propertyNameNative = propertyDefinition.Name.NativeValue.DangerousSelfRef;
                NativeGodotStringName propertyClassNameNative = (propertyDefinition.ClassName?.NativeValue ?? default).DangerousSelfRef;
                NativeGodotString hintStringNative = NativeGodotString.Create(propertyDefinition.HintString);

                propertyInfoNative = new GDExtensionPropertyInfo()
                {
                    type = (GDExtensionVariantType)propertyDefinition.Type,
                    name = &propertyNameNative,

                    hint = (uint)propertyDefinition.Hint,
                    hint_string = &hintStringNative,
                    class_name = &propertyClassNameNative,
                    usage = (uint)propertyDefinition.Usage,
                };
            }

            NativeGodotStringName setterNameNative = propertyDefinition.SetterName.NativeValue.DangerousSelfRef;
            NativeGodotStringName getterNameNative = propertyDefinition.GetterName.NativeValue.DangerousSelfRef;

            NativeGodotStringName classNameNative = ClassName.NativeValue.DangerousSelfRef;

            GodotBridge.GDExtensionInterface.classdb_register_extension_class_property(GodotBridge.LibraryPtr, &classNameNative, &propertyInfoNative, &setterNameNative, &getterNameNative);
        });
    }

    /// <summary>
    /// Register a property in the class.
    /// </summary>
    /// <typeparam name="TInstance">Type of the class that contains the property.</typeparam>
    /// <typeparam name="TValue">Type of the property.</typeparam>
    /// <param name="propertyDefinition">Information that describes the property to register.</param>
    /// <param name="getter">Method or lambda that gets the property's value.</param>
    /// <param name="setter">Method or lambda that sets the property's value.</param>
    public void BindProperty<TInstance, [MustBeVariant] TValue>(
        PropertyDefinition propertyDefinition,
        Func<TInstance, TValue> getter,
        Action<TInstance, TValue> setter
    ) where TInstance : GodotObject
    {
        StringName getterName = new($"get_{propertyDefinition.Name}");
        StringName setterName = new($"set_{propertyDefinition.Name}");

        var returnDefinition = new ReturnDefinition(propertyDefinition.Type)
        {
            Hint = propertyDefinition.Hint,
            HintString = propertyDefinition.HintString,
            ClassName = propertyDefinition.ClassName,
            Usage = propertyDefinition.Usage,
        };

        var valueParameterDefinition = new ParameterDefinition(new StringName("value"), propertyDefinition.Type)
        {
            Hint = propertyDefinition.Hint,
            HintString = propertyDefinition.HintString,
            ClassName = propertyDefinition.ClassName,
            Usage = propertyDefinition.Usage,
        };

        BindMethod(getterName, returnDefinition, getter);
        BindMethod(setterName, valueParameterDefinition, setter);

        BindProperty(new PropertyDefinitionWithAccessors(propertyDefinition.Name, propertyDefinition.Type, getterName, setterName)
        {
            Hint = propertyDefinition.Hint,
            HintString = propertyDefinition.HintString,
            ClassName = propertyDefinition.ClassName,
            Usage = propertyDefinition.Usage,
        });
    }

    /// <summary>
    /// Register a property group in the class.
    /// </summary>
    /// <param name="groupName">Name of the group to register.</param>
    /// <param name="prefix">Prefix used by properties in the group.</param>
    public unsafe void AddPropertyGroup(string groupName, string prefix = "")
    {
        _registerBindingActions.Enqueue(() =>
        {
            using NativeGodotString groupNameNative = NativeGodotString.Create(groupName);
            using NativeGodotString prefixNative = NativeGodotString.Create(prefix);

            NativeGodotStringName classNameNative = ClassName.NativeValue.DangerousSelfRef;

            GodotBridge.GDExtensionInterface.classdb_register_extension_class_property_group(GodotBridge.LibraryPtr, &classNameNative, &groupNameNative, &prefixNative);
        });
    }

    /// <summary>
    /// Register a property subgroup in the class.
    /// </summary>
    /// <param name="subgroupName">Name of the subgroup to register.</param>
    /// <param name="prefix">Prefix used by properties in the subgroup.</param>
    public unsafe void AddPropertySubgroup(string subgroupName, string prefix = "")
    {
        _registerBindingActions.Enqueue(() =>
        {
            using NativeGodotString subgroupNameNative = NativeGodotString.Create(subgroupName);
            using NativeGodotString prefixNative = NativeGodotString.Create(prefix);

            NativeGodotStringName classNameNative = ClassName.NativeValue.DangerousSelfRef;

            GodotBridge.GDExtensionInterface.classdb_register_extension_class_property_subgroup(GodotBridge.LibraryPtr, &classNameNative, &subgroupNameNative, &prefixNative);
        });
    }
}
