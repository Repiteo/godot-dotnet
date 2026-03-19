using System;
using System.Collections.Generic;
using System.Threading;
using Godot.Common.CodeAnalysis;
using Microsoft.CodeAnalysis;

namespace Godot.SourceGeneration;

internal static class RegistrationSpecCollector
{
    public static GodotRegistrationSpec? Collect(Compilation compilation, INamedTypeSymbol typeSymbol, CancellationToken cancellationToken = default)
    {
        if (!typeSymbol.TryGetAttribute(KnownTypeNames.GodotClassAttribute, out var attribute))
        {
            // Classes must have the attribute to be registered.
            return null;
        }

        bool? isTool = null;

        foreach (var (key, constant) in attribute.NamedArguments)
        {
            switch (key)
            {
                case "Tool":
                    isTool = constant.Value as bool?;
                    break;
            }
        }

        var registrationKind = GodotRegistrationSpec.Kind.RuntimeClass;
        if (typeSymbol.IsAbstract)
        {
            // Abstract classes can't be registered as anything else, because
            // it would require instantiating them, which isn't possible.
            registrationKind = GodotRegistrationSpec.Kind.AbstractClass;
        }
        else if (isTool ?? false)
        {
            registrationKind = GodotRegistrationSpec.Kind.Class;
        }

        return new GodotRegistrationSpec()
        {
            SymbolName = typeSymbol.Name,
            FullyQualifiedSymbolName = typeSymbol.FullQualifiedNameWithGlobal(),
            FullyQualifiedBaseSymbolName = typeSymbol.BaseType!.FullQualifiedNameWithGlobal(),
            RegistrationKind = registrationKind,
        };
    }
}
