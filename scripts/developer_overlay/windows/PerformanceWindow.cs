using System;
using Godot;
using ImGuiNET;

public partial class DeveloperOverlay : Node {
    private void PerformanceWindow() {
        KeyValueTable("PerformanceTable", () => {
            KeyValueTableRow("FPS", () => ValueDisplay(Engine.GetFramesPerSecond()));
        });
    }
}