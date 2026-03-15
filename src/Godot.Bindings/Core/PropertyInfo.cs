namespace Godot;

/// <summary>
/// Describes a property of a <see cref="GodotObject"/>.
/// </summary>
public sealed class PropertyInfo
{
    /// <summary>
    /// Type of the property.
    /// </summary>
    public VariantType Type { get; set; }

    /// <summary>
    /// Name of the property.
    /// </summary>
    public StringName Name { get; set; }

    /// <summary>
    /// Hint that determines how the property should be handled by the editor.
    /// </summary>
    public PropertyHint Hint { get; set; }

    /// <summary>
    /// Additional metadata for <see cref="Hint"/>.
    /// The contents and format of the string depend on the type of hint.
    /// </summary>
    public string? HintString { get; set; }

    /// <summary>
    /// Name of the property's type when <see cref="Type"/> is <see cref="VariantType.Object"/>
    /// and the type is a registered class. Otherwise, it should be <see langword="null"/>.
    /// </summary>
    public StringName? ClassName { get; set; }

    /// <summary>
    /// Flags that determine how the property should be handled by the editor.
    /// </summary>
    public PropertyUsageFlags Usage { get; set; }

    /// <summary>
    /// Constructs a new <see cref="PropertyInfo"/> with the specified name and type.
    /// </summary>
    /// <param name="name">Name of the property.</param>
    /// <param name="type">Type of the property.</param>
    public PropertyInfo(StringName name, VariantType type)
    {
        Type = type;
        Name = name;
    }
}
