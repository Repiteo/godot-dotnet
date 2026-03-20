using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Godot.Common.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Godot.Analyzers;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(AddBindMethodToRpcMethodCodeFix))]
internal sealed class AddBindMethodToRpcMethodCodeFix : CodeFixProvider
{
    public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create([
        Descriptors.GODOT0901_RpcMethodMustBeABoundMethod.Id,
    ]);

    public override FixAllProvider? GetFixAllProvider() =>
        WellKnownFixAllProviders.BatchFixer;

    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
        if (root is null)
        {
            return;
        }

        var diagnostic = context.Diagnostics.First();
        var diagnosticSpan = diagnostic.Location.SourceSpan;

        var syntaxNode = root
            .FindToken(diagnosticSpan.Start).Parent?
            .AncestorsAndSelf()
            .OfType<MethodDeclarationSyntax>()
            .FirstOrDefault();

        if (syntaxNode is null)
        {
            return;
        }

        var codeAction = CodeAction.Create(
            title: SR.GODOT0901_AddBindMethodAttribute_CodeFix,
            equivalenceKey: nameof(AddBindMethodToRpcMethodCodeFix),
            createChangedDocument: cancellationToken => ApplyFix(context.Document, syntaxNode, cancellationToken));

        context.RegisterCodeFix(codeAction, diagnostic);
    }

    private static async Task<Document> ApplyFix(Document document, MethodDeclarationSyntax methodDeclarationSyntax, CancellationToken cancellationToken)
    {
        var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
        if (root is null)
        {
            return document;
        }

        var bindMethodAttribute = SyntaxFactory.Attribute(
            SyntaxFactory.IdentifierName("BindMethod"));

        var newSyntaxNode = SyntaxUtils.AddAttributeList(methodDeclarationSyntax, bindMethodAttribute);

        var newRoot = root.ReplaceNode(methodDeclarationSyntax, newSyntaxNode);
        return document.WithSyntaxRoot(newRoot);
    }
}
