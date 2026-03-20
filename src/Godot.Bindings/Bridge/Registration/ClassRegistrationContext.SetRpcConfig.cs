using System.Collections.Generic;
using Godot.NativeInterop;

namespace Godot.Bridge;

partial class ClassRegistrationContext
{
    private readonly Dictionary<StringName, RpcConfig> _rpcConfigs = new(StringNameEqualityComparer.Default);

    /// <summary>
    /// Set the RPC configuration for a method in the class.
    /// If a configuration has already been set for <paramref name="methodName"/>,
    /// it will be silently replaced by the new configuration.
    /// </summary>
    /// <param name="methodName">Name of the method to configure.</param>
    /// <param name="rpcConfig">The RPC configuration to set for the method.</param>
    public void SetRpcConfig(StringName methodName, RpcConfig rpcConfig)
    {
        _rpcConfigs[methodName] = rpcConfig;
    }

    internal void RegisterRpcMethods(Node instance)
    {
        foreach (var (methodName, rpcConfig) in _rpcConfigs)
        {
            instance.RpcConfig(methodName, rpcConfig.GetConfigDictionary());
        }
    }
}
