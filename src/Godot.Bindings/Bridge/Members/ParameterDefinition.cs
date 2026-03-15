namespace Godot.Bridge;

/// <summary>
/// Defines a parameter in a <see cref="MethodDefinition"/>.
/// </summary>
public sealed class ParameterDefinition : PropertyDefinition
{
    /// <summary>
    /// Default value for this parameter or <see langword="null"/> if this parameter is required.
    /// </summary>
    public Variant? DefaultValue { get; }

    /// <summary>
    /// Constructs a new <see cref="ParameterDefinition"/> with the specified name and type.
    /// </summary>
    /// <param name="type">Type of the parameter.</param>
    /// <param name="name">Name of the parameter.</param>
    /// <param name="metadata">Type metadata of the parameter.</param>
    public ParameterDefinition(StringName name, VariantType type, VariantTypeMetadata metadata = VariantTypeMetadata.None) : base(name, type, metadata) { }

    /// <summary>
    /// Constructs a new <see cref="ParameterDefinition"/> with the specified name, type, and default value.
    /// </summary>
    /// <param name="type">Type of the parameter.</param>
    /// <param name="name">Name of the parameter.</param>
    /// <param name="metadata">Type metadata of the parameter.</param>
    /// <param name="defaultValue">Default value for the parameter.</param>
    public ParameterDefinition(StringName name, VariantType type, VariantTypeMetadata metadata, Variant defaultValue) : this(name, type, metadata)
    {
        DefaultValue = defaultValue;
    }
}
