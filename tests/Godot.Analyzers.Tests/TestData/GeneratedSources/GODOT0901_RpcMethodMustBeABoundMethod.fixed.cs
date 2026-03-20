using Godot;

namespace NS;

[GodotClass]
public partial class MyNode : Node
{
    [BindMethod]
    [Rpc]
    public void ValidRpcMethod() { }

    [Rpc]
    [BindMethod]
    public void InvalidRpcMethod() { }
}
