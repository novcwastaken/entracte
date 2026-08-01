using System;
using Godot;
using ImGuiNET;

public partial class DeveloperOverlay : Node {
    private void UnlocksWindow() {
        ImGui.TextWrapped("Unlock tickboxes will appear here");

        // TODO: Categorize (abilities, combat, etc.)
    }
}