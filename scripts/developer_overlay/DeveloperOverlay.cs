using System;
using Godot;
using ImGuiNET;

public partial class DeveloperOverlay : Node {
    [Export] private Angel angel;
    [Export] private bool overlayEnabled = false;

    private readonly ImGuiTableFlags keyValueTableFlags = ImGuiTableFlags.Borders | ImGuiTableFlags.RowBg;
    private readonly float windowWidth = 350f;

    public override void _UnhandledInput(InputEvent @event) {
        if (@event.IsActionPressed("toggle_developer_overlay")) {
            overlayEnabled = !overlayEnabled;
        }
    }

    public override void _Process(double delta) {
        if (!overlayEnabled) return;
        MainOverlay();        
    }

    private void MainOverlay() {
        int windowIndex = 0;

        AddNewWindow(true, "Stats", StatsWindow, windowIndex++);
        AddNewWindow(true, "Unlocks", UnlocksWindow, windowIndex++);
        AddNewWindow(true, "Cheats", CheatsWindow, windowIndex++);
        AddNewWindow(true, "Performance", PerformanceWindow, windowIndex);
    }

    private void AddNewWindow(bool condition, string title, Action windowMethod, int windowIndex) {
        if (!condition) return;

        ImGui.SetNextWindowPos(new System.Numerics.Vector2(windowIndex * windowWidth, 0));
        ImGui.SetNextWindowSize(new System.Numerics.Vector2(windowWidth, 0));
        ImGui.Begin($"{title}##{windowIndex}", ImGuiWindowFlags.NoMove);
        windowMethod();
        ImGui.End();
    }

    private uint GodotColorToU32(Color color) {
        return ImGui.ColorConvertFloat4ToU32(new System.Numerics.Vector4(color.R, color.G, color.B, color.A));
    }
}