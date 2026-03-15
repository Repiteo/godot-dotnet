using System.Collections.Generic;

namespace Godot.Bridge;

/// <summary>
/// Defines a virtual method registered for a class.
/// A virtual method is a method that can be overridden in user scripts
/// but the extension class must call the script implementation using
/// <see cref="GodotObject.CallVirtualMethod(StringName)"/> or
/// <see cref="GodotObject.TryCallVirtualMethod(StringName)"/>.
/// </summary>
public partial class VirtualMethodDefinition
{
    /// <summary>
    /// Name of the method.
    /// </summary>
    public StringName Name { get; }

    /// <summary>
    /// Collection of parameter information for the method.
    /// </summary>
    public List<ParameterDefinition> Parameters { get; } = [];

    /// <summary>
    /// Return information for the method or <see langword="null"/> if the
    /// method has no return parameter.
    /// </summary>
    public ReturnDefinition? Return { get; init; }

    /// <summary>
    /// Constructs a new <see cref="VirtualMethodDefinition"/> with the specified name.
    /// </summary>
    /// <param name="name">Name of the method.</param>
    public VirtualMethodDefinition(StringName name)
    {
        Name = name;
    }
}
