using System.Threading.Tasks;
using Godot.UpgradeAssistant.Providers;

namespace Godot.UpgradeAssistant.Tests;

using Verifier = CSharpCodeFixVerifier<GodotArrayConcatenationCodeFix, GodotArrayConcatenationAnalyzer>;

public class GodotArrayConcatenationTests
{
    [Fact]
    public static async Task UpgradeFromGodotSharp3ToGodotDotNet()
    {
        await Verifier.Verify(
            "GUA1012_GodotArrayConcatenation_FromGodotSharp3.cs",
            "GUA1012_GodotArrayConcatenation_FromGodotSharp3.fixed.cs",
            GodotReferenceAssemblies.GodotSharp3,
            GodotReferenceAssemblies.GodotSharp3);
    }

    [Fact]
    public static async Task UpgradeAlreadyLatest()
    {
        await Verifier.Verify(
            "GUA1012_GodotArrayConcatenation_FromGodotDotNet.cs",
            "GUA1012_GodotArrayConcatenation_FromGodotDotNet.fixed.cs",
            GodotReferenceAssemblies.GodotDotNet,
            GodotReferenceAssemblies.GodotDotNet);
    }
}
