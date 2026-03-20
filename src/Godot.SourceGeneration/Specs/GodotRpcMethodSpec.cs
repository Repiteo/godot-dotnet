using System;

namespace Godot.SourceGeneration;

/// <summary>
/// Describes the RPC configuration for a method.
/// </summary>
internal readonly record struct GodotRpcMethodSpec : IEquatable<GodotRpcMethodSpec>
{
    /// <summary>
    /// Name of the method's symbol.
    /// This is the real name of the method in the source code.
    /// </summary>
    public required string SymbolName { get; init; }

    /// <summary>
    /// Fully qualified C# expression for the RPC mode value.
    /// For example: <c>global::Godot.MultiplayerApi.RpcMode.Authority</c>.
    /// </summary>
    public required string ModeExpression { get; init; }

    /// <summary>
    /// If the method will also be called locally; otherwise, it is only called remotely.
    /// </summary>
    public bool CallLocal { get; init; }

    /// <summary>
    /// Fully qualified C# expression for the transfer mode value.
    /// For example: <c>global::Godot.MultiplayerPeer.TransferModeEnum.Reliable</c>.
    /// </summary>
    public required string TransferModeExpression { get; init; }

    /// <summary>
    /// Transfer channel for the method.
    /// </summary>
    public int TransferChannel { get; init; }
}
