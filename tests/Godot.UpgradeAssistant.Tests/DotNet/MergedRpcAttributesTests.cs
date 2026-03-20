using System.Threading.Tasks;
using Godot.UpgradeAssistant.Providers;

namespace Godot.UpgradeAssistant.Tests;

using Verifier = CSharpCodeFixVerifier<MergedRpcAttributesCodeFix, MergedRpcAttributesAnalyzer>;

public class MergedRpcAttributesTests
{
    [Fact]
    public static async Task UpgradeFromGodotSharp3ToGodotSharp4()
    {
        await Verifier.Verify(
            "GUA1005_MergedRpcAttributes_FromGodotSharp3.cs",
            "GUA1005_MergedRpcAttributes_FromGodotSharp3.fixed.cs",
            GodotReferenceAssemblies.GodotSharp3,
            GodotReferenceAssemblies.GodotSharp4);
    }

    [Fact]
    public static async Task UpgradeFromGodotSharp3ToGodotDotNet()
    {
        var verifier = Verifier.MakeVerifier(
            "GUA1005_MergedRpcAttributes_FromGodotSharp3.cs",
            "GUA1005_MergedRpcAttributes_FromGodotSharp3.fixed.cs",
            GodotReferenceAssemblies.GodotSharp3,
            GodotReferenceAssemblies.GodotDotNet);
        await verifier.RunAsync();
    }

    [Fact]
    public static async Task UpgradeFromGodotSharp4ToGodotDotNet()
    {
        var verifier = Verifier.MakeVerifier(
            "GUA1005_MergedRpcAttributes_FromGodotSharp4.cs",
            "GUA1005_MergedRpcAttributes_FromGodotSharp4.fixed.cs",
            GodotReferenceAssemblies.GodotSharp4,
            GodotReferenceAssemblies.GodotDotNet);
        await verifier.RunAsync();
    }

    [Fact]
    public static async Task UpgradeAlreadyLatest()
    {
        var verifier = Verifier.MakeVerifier(
            "GUA1005_MergedRpcAttributes_FromGodotSharp4.cs",
            "GUA1005_MergedRpcAttributes_FromGodotSharp4.fixed.cs",
            GodotReferenceAssemblies.GodotDotNet,
            GodotReferenceAssemblies.GodotDotNet);
        await verifier.RunAsync();
    }
}
