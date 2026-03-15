namespace Godot.Bridge;

/// <summary>
/// Defines the return parameter of a <see cref="MethodDefinition"/>.
/// </summary>
public sealed class ReturnDefinition : PropertyDefinition
{
    /// <summary>
    /// Constructs a new <see cref="ReturnDefinition"/> with the specified type.
    /// </summary>
    /// <param name="type">Type of the return parameter.</param>
    /// <param name="metadata">Type metadata of the property.</param>
    public ReturnDefinition(VariantType type, VariantTypeMetadata metadata = VariantTypeMetadata.None) : base(StringName.Empty, type, metadata) { }
}
