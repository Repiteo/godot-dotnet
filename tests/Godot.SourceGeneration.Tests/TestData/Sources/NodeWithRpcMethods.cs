using Godot;

namespace NS;

[GodotClass]
public partial class NodeWithRpcMethods : Node
{
    [BindMethod]
    [Rpc]
    public void MyRpcMethod() { }

    [BindMethod]
    [Rpc(MultiplayerApi.RpcMode.AnyPeer)]
    public void MyRpcMethodWithMode() { }

    [BindMethod]
    [Rpc(MultiplayerApi.RpcMode.Authority, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Unreliable, TransferChannel = 1)]
    public void MyFullyConfiguredRpcMethod() { }
}
