using System.Collections.Immutable;
using Godot.Common.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;

namespace Godot.UpgradeAssistant.Providers;

[RequiresGodotDotNet]
[DiagnosticAnalyzer(LanguageNames.CSharp)]
internal sealed class GodotArrayConcatenationAnalyzer : DiagnosticAnalyzer
{
    private static DiagnosticDescriptor Rule =>
        Descriptors.GUA1012_GodotArrayConcatenation;

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
        [Rule];

    public override void Initialize(DiagnosticAnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSyntaxNodeAction(AnalyzeBinaryExpression, SyntaxKind.AddExpression);
    }

    private static void AnalyzeBinaryExpression(SyntaxNodeAnalysisContext context)
    {
        var binaryExpression = (BinaryExpressionSyntax)context.Node;

        var semanticModel = context.SemanticModel;
        if (semanticModel.GetOperation(binaryExpression) is not IBinaryOperation binaryOperation)
        {
            // Unable to get the binary operation.
            return;
        }

        var operatorMethod = binaryOperation.OperatorMethod;
        if (operatorMethod is null)
        {
            // Not a user-defined operator.
            return;
        }

        if (!IsGodotArrayAddOperator(operatorMethod))
        {
            return;
        }

        context.ReportDiagnostic(Diagnostic.Create(
            descriptor: Rule,
            location: binaryExpression.GetLocation()));
    }

    private static bool IsGodotArrayAddOperator(IMethodSymbol operatorMethod)
    {
        if (!operatorMethod.DeclaredInGodotSharp())
        {
            return false;
        }

        var containingType = operatorMethod.ContainingType;
        return containingType.EqualsType("Godot.Collections.Array", "GodotSharp")
            || containingType.EqualsGenericType("Godot.Collections.Array<T>", "GodotSharp");
    }
}
