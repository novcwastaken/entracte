using Godot;

public partial class Angel : CharacterBody2D {
    [ExportGroup("Nodes")]
    [ExportSubgroup("Combat")]
    [Export] private Node2D attackParent;
    [Export] private Sprite2D attackSprite;
    [Export] private Area2D attackArea;
    [Export] private CollisionShape2D attackCollisionShape;

    private void ReadyCombat() {
        ResetAttack();
    }

    private void Attack() {
        if (!attackTimer.IsStopped()) return;
        if (inputDirection.Y != 0f && (inputDirection.Y < 0 || !IsOnFloor())) {
            attackParent.RotationDegrees = inputDirection.Y > 0 ? 90f : -90f;
        } else {
            attackParent.RotationDegrees = inputDirection.X > 0 ? 0f : 180f;
        }

        attackCollisionShape.Disabled = false;
        attackSprite.Visible = true;
        
        attackTimer.Start();
    }

    private void ResetAttack() {
        attackCollisionShape.Disabled = true;
        attackSprite.Visible = false;
    }

    private void OnAttackTimerTimeout() {
        ResetAttack();
    }
}