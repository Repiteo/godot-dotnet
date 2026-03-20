using Godot.Collections;

namespace Godot.NativeInterop;

/// <summary>
/// Defines the RPC configuration for a method.
/// </summary>
public readonly struct RpcConfig
{
    /// <summary>
    /// RPC mode for the method.
    /// </summary>
    public MultiplayerApi.RpcMode Mode { get; init; }

    /// <summary>
    /// If the method will also be called locally; otherwise, it is only called remotely.
    /// </summary>
    public bool CallLocal { get; init; }

    /// <summary>
    /// Transfer mode for the method.
    /// </summary>
    public MultiplayerPeer.TransferModeEnum TransferMode { get; init; }

    /// <summary>
    /// Transfer channel for the method.
    /// </summary>
    public int TransferChannel { get; init; }

    internal GodotDictionary GetConfigDictionary()
    {
        // IMPORTANT: The keys in this dictionary must match the ones expected by the Godot engine.
        // See https://github.com/godotengine/godot/blob/4a280218fcfdd69408cceb74577c9e69086be23a/modules/multiplayer/scene_rpc_interface.cpp#L75-L103
        return new GodotDictionary()
        {
            ["rpc_mode"] = (long)Mode,
            ["transfer_mode"] = (long)TransferMode,
            ["call_local"] = CallLocal,
            ["channel"] = TransferChannel,
        };
    }
}
