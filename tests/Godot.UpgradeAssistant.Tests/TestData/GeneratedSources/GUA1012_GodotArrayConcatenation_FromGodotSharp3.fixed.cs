using Godot;
using Godot.Collections;

public partial class MyNode : Node
{
    public Array<Node> ConcatArrays(Array<Node> left, Array<Node> right)
    {
        return [.. left, .. right];
    }

    public void AssignConcat(Array<Node> left, Array<Node> right)
    {
        Array<Node> c = [.. left, .. right];
        _ = c;
    }
}
