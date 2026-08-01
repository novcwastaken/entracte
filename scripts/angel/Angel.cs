using Godot;
using System;

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

	public Vector2 DebugPosition => GlobalPosition;
	public Vector2 DebugVelocity => Velocity;
	public Vector2 DebugInputDirection => inputDirection;
	public int DebugLastDirectionX => lastDirectionX;
	public int DebugDashDirectionX => dashDirectionX;
	public bool DebugIsDashing => isDashing;
	public bool DebugCanDash => canDash;
	public bool DebugIsGliding => isGliding;
	public string DebugWallState => wallState.ToString();
	public bool DebugCanBufferJump => canBufferJump;
	public bool DebugCanCoyoteJump => canCoyoteJump;
	public bool DebugCanStartGlide => canStartGlide;
	public bool DebugDoubleJumpUnlocked => doubleJumpUnlocked;
	public bool DebugGlideUnlocked => glideUnlocked;

	public void SetDoubleJumpUnlocked(bool enabled) {
		doubleJumpUnlocked = enabled;
	}

	public void SetGlideUnlocked(bool enabled) {
		glideUnlocked = enabled;
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
		""";
	}

	private void OnVerticalLookTimerTimeout() {
		float lookDirY = inputDirection.Y;

		if (lookDirY == 0 || isMoving) return;
		camera.targetOffset.Y = lookDirY * verticalLookOffset;
	}
}
