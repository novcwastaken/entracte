using Godot;
using ImGuiNET;

public partial class DeveloperOverlay : Node {
    private void ValueDisplay(object value) {
        ImGui.PushStyleColor(ImGuiCol.Text, GodotColorToU32(Colors.Cyan));
        ImGui.Text(value.ToString());
        ImGui.PopStyleColor();
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

    private void TimerDisplay(Timer timer) {
        if (timer.IsStopped()) {
            ImGui.PushStyleColor(ImGuiCol.Text, GodotColorToU32(Colors.DimGray));
            ImGui.Text("Stopped");
            ImGui.PopStyleColor();

            return;
        }

        float waitTime = (float)timer.WaitTime;
        float fillFraction = waitTime > 0f ? Mathf.Clamp((float)timer.TimeLeft / waitTime, 0f, 1f) : 0f;

        var avail = ImGui.GetContentRegionAvail();

        ImGui.PushStyleVar(ImGuiStyleVar.FramePadding, new System.Numerics.Vector2(0, 0));

        ImGui.PushStyleColor(ImGuiCol.PlotHistogram, GodotColorToU32(Colors.Cyan));
        ImGui.PushStyleColor(ImGuiCol.FrameBg, GodotColorToU32(Colors.Transparent));

        ImGui.ProgressBar(fillFraction, new System.Numerics.Vector2(avail.X, 12), " ");

        ImGui.PopStyleColor(2);
        ImGui.PopStyleVar();
        
    }
}