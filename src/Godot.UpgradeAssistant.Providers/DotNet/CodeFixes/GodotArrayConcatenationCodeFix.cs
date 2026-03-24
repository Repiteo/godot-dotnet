using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Godot.UpgradeAssistant.Providers;

[ExportCodeFixProvider(LanguageNames.CSharp)]
internal sealed class GodotArrayConcatenationCodeFix : CodeFixProvider
{
    private static DiagnosticDescriptor Rule =>
        Descriptors.GUA1012_GodotArrayConcatenation;

    public override ImmutableArray<string> FixableDiagnosticIds =>
        [Rule.Id];

    public override FixAllProvider? GetFixAllProvider() =>
        WellKnownFixAllProviders.BatchFixer;

    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

        var diagnostic = context.Diagnostics.First();
        var diagnosticSpan = diagnostic.Location.SourceSpan;

        var binaryExpression = FindSyntax<BinaryExpressionSyntax>(root, diagnosticSpan);
        if (binaryExpression is null)
        {
            // Can't apply the code fix without a syntax node.
            return;
        }

        var codeAction = CodeAction.Create(
            title: SR.GUA1012_GodotArrayConcatenation_CodeFix,
            equivalenceKey: nameof(GodotArrayConcatenationCodeFix),
            createChangedDocument: cancellationToken => ApplyFix(context.Document, binaryExpression, cancellationToken));

        context.RegisterCodeFix(codeAction, diagnostic);

        static TSyntax? FindSyntax<TSyntax>(SyntaxNode? syntaxNode, TextSpan diagnosticSpan) where TSyntax : SyntaxNode
        {
            return syntaxNode?
                .FindToken(diagnosticSpan.Start).Parent?
                .AncestorsAndSelf()
                .OfType<TSyntax>()
                .First();
        }
    }

    private static async Task<Document> ApplyFix(Document document, BinaryExpressionSyntax binaryExpression, CancellationToken cancellationToken = default)
    {
        var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
        if (root is null)
        {
            // If we couldn't get the syntax root, return the document unchanged.
            // This should be unreachable.
            return document;
        }

        // Build the collection expression: [..left, ..right]
        var leftSpread = SyntaxFactory.SpreadElement(binaryExpression.Left.WithoutTrivia());
        var rightSpread = SyntaxFactory.SpreadElement(binaryExpression.Right.WithoutTrivia());

        var collectionExpression = SyntaxFactory.CollectionExpression(
            SyntaxFactory.SeparatedList<CollectionElementSyntax>(
                [
                    leftSpread,
                    SyntaxFactory.Token(SyntaxKind.CommaToken).WithTrailingTrivia(SyntaxFactory.Whitespace(" ")),
                    rightSpread,
                ]));

        var newRoot = root.ReplaceNode(binaryExpression, collectionExpression.WithTriviaFrom(binaryExpression));
        return document.WithSyntaxRoot(newRoot);
    }
}
