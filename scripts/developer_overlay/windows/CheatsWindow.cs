using System;
using Godot;
using ImGuiNET;

public partial class DeveloperOverlay : Node {
    private void CheatsWindow() {
        ImGui.TextWrapped("Cheats will appear here");
    }
}