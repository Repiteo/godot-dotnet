using Godot;
using Godot.Collections;

public partial class MyNode : Node
{
    public Array<Node> ConcatArrays(Array<Node> left, Array<Node> right)
    {
        return {|GUA1012:left + right|};
    }

    public void AssignConcat(Array<Node> left, Array<Node> right)
    {
        Array<Node> c = {|GUA1012:left + right|};
        _ = c;
    }
}
