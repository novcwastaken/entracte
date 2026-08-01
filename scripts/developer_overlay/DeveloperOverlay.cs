using System;
using Godot;
using ImGuiNET;

public partial class DeveloperOverlay : Node {
    [Export] private Angel angel;

    private readonly ImGuiTableFlags keyValueTableFlags = ImGuiTableFlags.Borders | ImGuiTableFlags.RowBg;
    private readonly float windowWidth = 300f;

    private bool statsWindowVisible = true;
    private bool unlocksWindowVisible = true;
    private bool cheatsWindowVisible = true;

    public override void _Process(double delta) {
        // TODO: Add F3 toggle

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

    private void BoolDisplay(bool value) {
        Color boolColor = value ? Colors.Green : Colors.Red;

        ImGui.PushStyleColor(ImGuiCol.Text, GodotColorToU32(boolColor));
        ImGui.Text(value.ToString());
        ImGui.PopStyleColor();
    }

    private void Vector2Display(Vector2 vector2) {
        ImGui.PushStyleVar(ImGuiStyleVar.CellPadding, new System.Numerics.Vector2(0f, 0f));

        if (ImGui.BeginTable("Vector2", 2)) {
            ImGui.TableNextRow();

            ImGui.TableNextColumn();
            ImGui.PushStyleColor(ImGuiCol.Text, GodotColorToU32(Colors.Magenta));
            ImGui.Text(vector2.X.ToString("0.00"));
            ImGui.PopStyleColor();

            ImGui.TableNextColumn();
            ImGui.PushStyleColor(ImGuiCol.Text, GodotColorToU32(Colors.Orange));
            ImGui.Text(vector2.Y.ToString("0.00"));
            ImGui.PopStyleColor();

            ImGui.EndTable();
        }

        ImGui.PopStyleVar();
    }
}