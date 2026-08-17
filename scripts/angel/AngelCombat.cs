using System.Text.Json.Serialization;
using Godot;

public partial class Angel : CharacterBody2D {
    [ExportGroup("Combat")]
    [Export] private float pogoVelocity = 1200.0f;
    [Export] private int damage = 5;

    [ExportGroup("Nodes")]
    [ExportSubgroup("Combat")]
    [Export] private Node2D attackParent;
    [Export] private Sprite2D attackSprite;
    [Export] private Area2D attackArea;
    [Export] private CollisionShape2D attackCollisionShape;

    private bool isAttacking => !attackTimer.IsStopped();

    private void ReadyCombat() {
        ResetAttack();

        attackArea.AreaEntered += AttackAreaHit;
        attackArea.BodyEntered += AttackAreaHit;
    }

    private void Attack() {
        if (!attackTimer.IsStopped()) return;
        if (inputDirection.Y != 0f && (inputDirection.Y < 0 || !IsOnFloor())) {
            attackParent.RotationDegrees = inputDirection.Y > 0 ? 90f : -90f;
        } else {
            attackParent.RotationDegrees = (inputDirection.X > 0 || lastDirectionX > 0) ? 0f : 180f;
        }

        attackCollisionShape.Disabled = false;
        attackSprite.Visible = true;
        
        attackTimer.Start();
    }

    private void AttackAreaHit(Node node) {
        if (node == this) return;
        if (node.IsInGroup("pogoable") || node.GetParent().IsInGroup("pogoable")) {
            controlledVelocity.Y = -pogoVelocity;
            RegainDash();
        }
    }

    private void ResetAttack() {
        attackCollisionShape.Disabled = true;
        attackSprite.Visible = false;
    }

    private void OnAttackTimerTimeout() {
        ResetAttack();
    }
}

// TODO: Lock wall slash on opposite dir.
// TODO: Add attack cooldown

// TODO: Fix walljump but no horizontal boost on non-walljupmable spikes
// TODO: Fix side pogo
// TODO: Fix mini double jump?