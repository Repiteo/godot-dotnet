using System.Threading.Tasks;

namespace Godot.Analyzers.Tests;

using Verifier = CSharpCodeFixVerifier<AddBindMethodToRpcMethodCodeFix, RpcAnalyzer>;

public class RpcAnalyzerTests
{
    [Fact]
    public async Task RpcMethodMustBeABoundMethod()
    {
        await Verifier.Verify("GODOT0901_RpcMethodMustBeABoundMethod.cs", "GODOT0901_RpcMethodMustBeABoundMethod.fixed.cs");
    }
}
