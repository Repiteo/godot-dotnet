using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using Godot.Common.CodeAnalysis;
using Microsoft.CodeAnalysis;

namespace Godot.SourceGeneration;

internal static class RpcMethodSpecCollector
{
    // Default fully qualified expressions matching RpcAttribute defaults.
    private const string DefaultModeExpression = "global::Godot.MultiplayerApi.RpcMode.Authority";
    private const bool DefaultCallLocal = false;
    private const string DefaultTransferModeExpression = "global::Godot.MultiplayerPeer.TransferModeEnum.Reliable";
    private const int DefaultTransferChannel = 0;

    public static GodotRpcMethodSpec? Collect(Compilation compilation, IMethodSymbol methodSymbol, CancellationToken cancellationToken = default)
    {
        if (!methodSymbol.TryGetAttribute(KnownTypeNames.RpcAttribute, out var attribute))
        {
            // Methods must have the attribute to be registered.
            return null;
        }

        if (!methodSymbol.HasAttribute(KnownTypeNames.BindMethodAttribute))
        {
            // [Rpc] only has effect on bound methods; skip if [BindMethod] is not present.
            // The RpcAnalyzer will report a diagnostic for this case.
            return null;
        }

        string modeExpression = DefaultModeExpression;
        bool callLocal = DefaultCallLocal;
        string transferModeExpression = DefaultTransferModeExpression;
        int transferChannel = DefaultTransferChannel;

        var ctorArgs = attribute.ConstructorArguments;
        if (ctorArgs.Length >= 1)
        {
            modeExpression = GetEnumMemberExpression(ctorArgs[0], DefaultModeExpression);
        }

        foreach (var (key, constant) in attribute.NamedArguments)
        {
            switch (key)
            {
                case "CallLocal":
                    callLocal = constant.Value is true;
                    break;

                case "TransferMode":
                    transferModeExpression = GetEnumMemberExpression(constant, DefaultTransferModeExpression);
                    break;

                case "TransferChannel":
                    transferChannel = Convert.ToInt32(constant.Value, CultureInfo.InvariantCulture);
                    break;
            }
        }

        return new GodotRpcMethodSpec()
        {
            SymbolName = methodSymbol.Name,
            ModeExpression = modeExpression,
            CallLocal = callLocal,
            TransferModeExpression = transferModeExpression,
            TransferChannel = transferChannel,
        };
    }

    /// <summary>
    /// Gets the fully qualified C# expression for an enum value from a <see cref="TypedConstant"/>.
    /// Looks up the enum member name if possible, otherwise falls back to a cast expression.
    /// </summary>
    private static string GetEnumMemberExpression(TypedConstant constant, string fallback)
    {
        if (constant.Type is not INamedTypeSymbol enumType || enumType.TypeKind != TypeKind.Enum)
        {
            return fallback;
        }

        long enumValue = Convert.ToInt64(constant.Value, CultureInfo.InvariantCulture);
        string enumTypeFqn = enumType.FullQualifiedNameWithGlobal();

        // Find the enum member with this value.
        var member = enumType.GetMembers()
            .OfType<IFieldSymbol>()
            .FirstOrDefault(f => f.IsConst && f.HasConstantValue
                && Convert.ToInt64(f.ConstantValue, CultureInfo.InvariantCulture) == enumValue);

        return member is not null
            ? $"{enumTypeFqn}.{member.Name}"
            : $"({enumTypeFqn}){enumValue}L";
    }
}
