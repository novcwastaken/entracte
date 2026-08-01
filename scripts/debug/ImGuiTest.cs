using Godot;
using ImGuiNET;

public partial class ImGuiTest : Node {
    public override void _Process(double delta) {
        ImGui.Begin("ImGui on Godot 4");
        ImGui.Text("Hello world!");
        ImGui.End();
    }
}