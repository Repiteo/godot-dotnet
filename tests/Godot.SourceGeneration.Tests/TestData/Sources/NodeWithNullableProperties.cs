#nullable enable

using Godot;
using Godot.Collections;

namespace NS;

[GodotClass]
public partial class NodeWithNullableProperties : Node
{
    [BindProperty]
    public string? MyNullableString { get; set; }

    [BindProperty]
    public string MyNonNullableString { get; set; } = string.Empty;

    [BindProperty]
    public GodotObject? MyNullableObject { get; set; }

    [BindProperty]
    public GodotObject MyNonNullableObject { get; set; } = null!;

    [BindProperty]
    public GodotArray<GodotObject?> MyGodotArrayOfNullableObject { get; set; } = [];

    [BindProperty]
    public GodotArray<GodotObject> MyGodotArrayOfNonNullableObject { get; set; } = [];
}
