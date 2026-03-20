using Godot;

namespace NS;

[GodotClass]
public partial class MyNode : Node
{
    [BindMethod]
    [Rpc]
    public void ValidRpcMethod() { }

    [Rpc]
    public void {|GODOT0901:InvalidRpcMethod|}() { }
}
