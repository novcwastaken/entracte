using System;
using System.Collections.Generic;
using Godot;
using ImGuiNET;

public partial class DeveloperOverlay : Node {
    private void StatsWindow() {       
        ImGui.TextWrapped("Stats of the Angel, like movement states, abilities and related timers, can be viewed here. This description is only here so that this widget doesn't look boring.");
        ImGui.Dummy(new System.Numerics.Vector2(0, 10f));

        if (ImGui.CollapsingHeader("Base", ImGuiTreeNodeFlags.DefaultOpen)) {
            Tree("Movement", true, () => {
                KeyValueTable("MovementTable", () => {
                    KeyValueTableRow("Position", () => Vector2Display(angel.Position));
                    KeyValueTableRow("Velocity", () => Vector2Display(angel.Velocity));
                    KeyValueTableRow("Is on floor?", () => BoolDisplay(angel.IsOnFloor()));
                    KeyValueTableRow("Is on ceiling?", () => BoolDisplay(angel.IsOnCeiling()));
                    KeyValueTableRow("Is on wall?", () => BoolDisplay(angel.IsOnWall()));
                    KeyValueTableRow("Is on wall only?", () => BoolDisplay(angel.IsOnWallOnly()));
                    KeyValueTableRow("Wall normal", () => Vector2Display(angel.GetWallNormal()));
                });
            });

            Tree("Input", true, () => {
                KeyValueTable("InputTable", () => {
                   KeyValueTableRow("Input direction", () => Vector2Display(angel.debugInputDirection));
                   KeyValueTableRow("Last direction X", () => ValueDisplay(angel.debugLastDirectionX));
                });
            });
        }

        if (ImGui.CollapsingHeader("Abilities", ImGuiTreeNodeFlags.DefaultOpen)) {
            Tree("Dash", true, () => {
                // dash unlocked?
                // dash direction x
                // ---
                // is dashing?
                // can dash?
                // can start dash?
                // can regain dash?
                // ---
                // dash timer
                // dash cooldown timer

                KeyValueTable("DashTable1", () => {
                    KeyValueTableRow("Dash unlocked?", () => BoolDisplay(angel.debugDashUnlocked));
                    KeyValueTableRow("Dash direction X", () => ValueDisplay(angel.debugDashDirectionX));
                    KeyValueTableRow("Is dashing?", () => BoolDisplay(angel.debugIsDashing));
                    KeyValueTableRow("Can dash?", () => BoolDisplay(angel.debugCanDash));
                    KeyValueTableRow("Can start dash?", () => BoolDisplay(angel.debugCanStartDash));
                    KeyValueTableRow("Can regain dash?", () => BoolDisplay(angel.debugCanRegainDash));
                    KeyValueTableRow("Dash timer",() => TimerDisplay(angel.debugDashTimer));
                    KeyValueTableRow("Dash cooldown timer",() => TimerDisplay(angel.debugDashCooldownTimer));
                });
            });

            // Double jump
            // Walljump
            // Glide
        }        
    }

    private void Tree(string title, bool defaultOpen, Action children) {
        if (defaultOpen) {
            if (ImGui.TreeNodeEx(title, ImGuiTreeNodeFlags.DefaultOpen)) {
                children();
                ImGui.TreePop();             
            }
        } else {
            if (ImGui.TreeNodeEx(title)) {
                children();
                ImGui.TreePop();       
            }
        }
    }
}