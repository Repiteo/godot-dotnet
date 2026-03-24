using Godot;
using Godot.Collections;

public partial class MyNode : Node
{
    public GodotArray<Node> ConcatArrays(GodotArray<Node> left, GodotArray<Node> right)
    {
        return [.. left, .. right];
    }

    public void AssignConcat(GodotArray<Node> left, GodotArray<Node> right)
    {
        GodotArray<Node> c = [.. left, .. right];
        _ = c;
    }
}
