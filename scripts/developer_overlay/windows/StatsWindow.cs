using System;
using Godot;
using ImGuiNET;

public partial class DeveloperOverlay : Node {
    private void StatsWindow() {       
        ImGui.TextWrapped("Stats of the Angel, like movement states, abilities and related timers, can be viewed here. This description is only here so that this widget doesn't look boring.");
        ImGui.Dummy(new System.Numerics.Vector2(0, 10f));

        if (ImGui.CollapsingHeader("Base", ImGuiTreeNodeFlags.DefaultOpen)) {
            if (ImGui.TreeNodeEx("Movement", ImGuiTreeNodeFlags.DefaultOpen)) {
                if (ImGui.BeginTable("MovementTable", 2, keyValueTableFlags)) {
                    ImGui.TableNextRow();
                    ImGui.TableNextColumn();
                    ImGui.Text("Position");
                    ImGui.TableNextColumn();
                    Vector2Display(angel.Position);

                    ImGui.TableNextRow();
                    ImGui.TableNextColumn();
                    ImGui.Text("Velocity");
                    ImGui.TableNextColumn();
                    Vector2Display(angel.Velocity);

                    ImGui.TableNextRow();
                    ImGui.TableNextColumn();
                    ImGui.Text("Is on floor?");
                    ImGui.TableNextColumn();
                    BoolDisplay(angel.IsOnFloor());

                    // MOVEMENT
                    // Position
                    // Velocity
                    // Is on floor
                    // Is on ceiling
                    // Is on wall
                    // Is on wall only
                    // Wall normal

                    // INPUT
                    // Input direction
                    // Last direction X

                    ImGui.EndTable();
                }

                // TODO: Make helper for table (enter key string, pass in method for displaying )
                // TODO: Add Vector2 display, string display, enum display for wall state, timer display
                
                ImGui.TreePop();
            }

            if (ImGui.TreeNodeEx("Input", ImGuiTreeNodeFlags.DefaultOpen)) {
                ImGui.Text("Input Direction");

                ImGui.TreePop();
            }
        }

        if (ImGui.CollapsingHeader("Abilities", ImGuiTreeNodeFlags.DefaultOpen)) {
            if (ImGui.TreeNodeEx("Dash", ImGuiTreeNodeFlags.DefaultOpen)) {
                ImGui.Text("Dash miau");

                ImGui.TreePop();
            }
        }
    }
}