using System;

namespace Godot;

/// <summary>
/// Registers the annotated class as an extension class within the Godot engine.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class GodotClassAttribute : Attribute
{
    /// <summary>
    /// Determines if the class is registered as a tool, instead of a runtime class.
    /// Tool classes run in the editor.
    /// </summary>
    public bool Tool { get; init; }
}
