using System.Collections.Immutable;
using Godot.Common.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Godot.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
internal sealed class RpcAnalyzer : DiagnosticAnalyzer
{
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create([
        Descriptors.GODOT0901_RpcMethodMustBeABoundMethod,
    ]);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();

        context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.Method);
    }

    private void AnalyzeSymbol(SymbolAnalysisContext context)
    {
        if (!context.Symbol.HasAttribute(KnownTypeNames.RpcAttribute))
        {
            return;
        }

        // [Rpc] requires [BindMethod] to be present on the same method.
        if (context.Symbol.HasAttribute(KnownTypeNames.BindMethodAttribute))
        {
            return;
        }

        IMethodSymbol methodSymbol = (IMethodSymbol)context.Symbol;

        context.ReportDiagnostic(Diagnostic.Create(
            Descriptors.GODOT0901_RpcMethodMustBeABoundMethod,
            methodSymbol.Locations[0],
            // Message Format parameters.
            methodSymbol.Name
        ));
    }
}
