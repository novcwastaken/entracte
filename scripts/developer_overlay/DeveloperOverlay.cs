using System;
using Godot;
using ImGuiNET;

public partial class DeveloperOverlay : Node {
    [Export] private Angel angel;
    [Export] private bool overlayEnabled = false;

    private readonly ImGuiTableFlags keyValueTableFlags = ImGuiTableFlags.Borders | ImGuiTableFlags.RowBg;
    private readonly float windowWidth = 350f;

    private bool statsWindowVisible = true;
    private bool unlocksWindowVisible = true;
    private bool cheatsWindowVisible = true;

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
        var viewportSize = GetViewport().GetVisibleRect().Size;
        var toggleWindowSize = new System.Numerics.Vector2(220f, 120f);

        var toggleWindowPos = new System.Numerics.Vector2(
            viewportSize.X - toggleWindowSize.X - 20f,
            viewportSize.Y - toggleWindowSize.Y - 20f
        );

        ImGui.SetNextWindowPos(toggleWindowPos, ImGuiCond.Always);
        ImGui.SetNextWindowSize(toggleWindowSize, ImGuiCond.Always);
        ImGui.Begin("Visible Windows", ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoResize);

        ImGui.Checkbox("Stats", ref statsWindowVisible);
        ImGui.Checkbox("Unlocks", ref unlocksWindowVisible);
        ImGui.Checkbox("Cheats", ref cheatsWindowVisible);

        ImGui.End();

        int windowIndex = 0;

        AddNewWindow(statsWindowVisible, "Stats", StatsWindow, windowIndex++);
        AddNewWindow(unlocksWindowVisible, "Unlocks", UnlocksWindow, windowIndex++);
        AddNewWindow(cheatsWindowVisible, "Cheats", CheatsWindow, windowIndex);
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