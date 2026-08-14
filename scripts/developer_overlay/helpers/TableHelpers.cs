using System;
using Godot;
using ImGuiNET;

public partial class DeveloperOverlay : Node {
    private void KeyValueTable(string title, Action rows) {
        if (ImGui.BeginTable(title, 2, keyValueTableFlags)) {
            rows();
            ImGui.EndTable();
        }
    }

    private void KeyValueTableRow(string keyDisplay, Action valueMethod) {
        ImGui.TableNextRow();

        ImGui.TableNextColumn();
        ImGui.Text(keyDisplay);

        ImGui.TableNextColumn();
        valueMethod();
    }
}