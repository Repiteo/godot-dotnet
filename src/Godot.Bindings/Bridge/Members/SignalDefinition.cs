using System.Collections.Generic;

namespace Godot.Bridge;

/// <summary>
/// Defines a signal registered for a class.
/// </summary>
public sealed class SignalDefinition
{
    /// <summary>
    /// Name of the signal.
    /// </summary>
    public StringName Name { get; }

    /// <summary>
    /// Collection of parameter information for the signal delegate.
    /// </summary>
    public List<ParameterDefinition> Parameters { get; } = [];

    /// <summary>
    /// Constructs a new <see cref="SignalDefinition"/> with the specified name.
    /// </summary>
    /// <param name="name"></param>
    public SignalDefinition(StringName name)
    {
        Name = name;
    }
}
