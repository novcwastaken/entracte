using Godot;
using System;

[GlobalClass, Icon("res://addons/at-icons/mesh/wing.svg")]
public partial class Angel : CharacterBody2D {
	[ExportGroup("Nodes")]
	[ExportSubgroup("Timers")]
	[Export] private Timer jumpBufferTimer;
	[Export] private Timer coyoteTimer;
	[Export] private Timer dashTimer;
	[Export] private Timer dashCooldownTimer;
	[Export] private Timer walljumpLeaveTimer;
	[Export] private Timer verticalLookTimer;
	[Export] private Timer attackTimer;

	[ExportSubgroup("Camera")]
	[Export] private Camera camera;

	[ExportGroup("Camera")]
	[Export] public float verticalLookOffset = 450;
	[Export] public float verticalLookLerpWeight = 0.15f;
	[Export] public float horizontalOffset = 200;
	[Export] public float horizontalOffsetLerpWeight = 0.025f;

	[ExportGroup("Debug")]
	[Export] private Label debugLabel;

	public override void _Ready() {
		ReadyMovement();
		ReadyCombat();
	}

	public override void _PhysicsProcess(double delta) {
		ProcessMovement(delta);
		ProcessFlip();

		UpdateDebugLabel();
	}

	private void UpdateDebugLabel() {
		debugLabel.Text = $"""
			velocity = {Velocity}

			inputDirection = {inputDirection}
			lastDirectionX = {lastDirectionX}

			jumpBufferTimer.TimeLeft = {jumpBufferTimer.TimeLeft}
			jumpBufferTimer.IsStopped = {jumpBufferTimer.IsStopped()}

			coyoteTimer.TimeLeft = {coyoteTimer.TimeLeft}
			coyoteTimer.IsStopped = {coyoteTimer.IsStopped()}

			dashTimer.TimeLeft = {dashTimer.TimeLeft}
			dashCooldownTimer.TimeLeft = {dashCooldownTimer.TimeLeft}
			isDashing = {isDashing}
			canDash = {canDash}
			dashDirectionX = {dashDirectionX}

			debugDashTimer.TimeLeft = {debugDashTimer.TimeLeft}

			wallNormal = {GetWallNormal()}
			isOnWall = {IsOnWall()}
			isOnWallOnly = {IsOnWallOnly()}
			wallState = {wallState}

			jump pressed = {Input.IsActionPressed("jump")}
			canStartGlide = {canStartGlide}
			- glideUnlocked = {glideUnlocked}
			- Velocity.Y > 0f = {Velocity.Y > 0f}
			- !IsOnFloor() = {!IsOnFloor()}
			- wallState == WallState.NONE = {wallState == WallState.NONE}
			- (doubleJumpUsed || !doubleJumpUnlocked) == {doubleJumpUsed || !doubleJumpUnlocked}
			- !isGliding = {!isGliding}
			- !canCoyoteJump = {!canCoyoteJump}
			- !canBufferJump = {!canBufferJump}

			isGliding = {isGliding}

			attackTimer.TimeLeft = {attackTimer.TimeLeft}
		""";
	}

	private void OnVerticalLookTimerTimeout() {
		float lookDirY = inputDirection.Y;

		if (lookDirY == 0 || isMoving) return;
		camera.targetOffset.Y = lookDirY * verticalLookOffset;
	}
}
