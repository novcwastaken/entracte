using Godot;

public partial class ImGuiTest2 : Node {
    [Export] private NodePath angelPath;

    private bool showDebugOverlay = false;
    private Angel angel;

    public override void _Ready() {
        ImGui.OnLayout(OnLayout);

        angel = GetNodeOrNull<Angel>(angelPath);
        if (angel == null && GetTree().CurrentScene != null) {
            angel = GetTree().CurrentScene.GetNodeOrNull<Angel>("Angel");
        }
    }

    public override void _UnhandledInput(InputEvent @event) {
        if (@event.IsActionPressed("toggle_debug_overlay")) {
            showDebugOverlay = !showDebugOverlay;
        }
    }

    private void OnLayout() {
        if (!showDebugOverlay) return;

        ImGui.Begin("Angel");
        ImGui.TextColored(Colors.Fuchsia, "cool ass red text");
        ImGui.Separator();

        if (angel == null) {
            ImGui.Text("Angel not found");
            ImGui.End();
            return;
        }

        if (ImGui.BeginTabBar("AngelDebugTabs")) {
            if (ImGui.BeginTabItem("Stats")) {
                ImGui.Text($"Position: {angel.DebugPosition}");
                ImGui.Text($"Velocity: {angel.DebugVelocity}");
                ImGui.Text($"Input direction: {angel.DebugInputDirection}");
                ImGui.Text($"Last direction X: {angel.DebugLastDirectionX}");
                ImGui.Text($"Dash direction X: {angel.DebugDashDirectionX}");
                ImGui.Text($"Is dashing: {angel.DebugIsDashing}");
                ImGui.Text($"Can dash: {angel.DebugCanDash}");
                ImGui.Text($"Is gliding: {angel.DebugIsGliding}");
                ImGui.Text($"Wall state: {angel.DebugWallState}");
                ImGui.Text($"Can buffer jump: {angel.DebugCanBufferJump}");
                ImGui.Text($"Can coyote jump: {angel.DebugCanCoyoteJump}");
                ImGui.Text($"Can start glide: {angel.DebugCanStartGlide}");
                ImGui.EndTabItem();
            }

            if (ImGui.BeginTabItem("Toggles")) {
                bool doubleJumpUnlocked = angel.DebugDoubleJumpUnlocked;
                bool newDoubleJumpUnlocked = ImGui.Checkbox("Double jump unlock", doubleJumpUnlocked);
                if (newDoubleJumpUnlocked != doubleJumpUnlocked) {
                    angel.SetDoubleJumpUnlocked(newDoubleJumpUnlocked);
                }

                bool glideUnlocked = angel.DebugGlideUnlocked;
                bool newGlideUnlocked = ImGui.Checkbox("Glide unlock", glideUnlocked);
                if (newGlideUnlocked != glideUnlocked) {
                    angel.SetGlideUnlocked(newGlideUnlocked);
                }

                ImGui.EndTabItem();
            }

            ImGui.EndTabBar();
        }

        ImGui.End();
    }
}