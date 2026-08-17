using Godot;
using System;

[GlobalClass, Icon("res://addons/at-icons/node2d/comedy_mask.svg")]
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
	[Export] private Timer attackCooldownTimer;

	[ExportSubgroup("Components")]
	[Export] public HealthComponent healthComponent;

	[ExportSubgroup("Camera")]
	[Export] private Camera camera;

	[ExportGroup("Camera")]
	[Export] public float verticalLookOffset = 450;
	[Export] public float verticalLookLerpWeight = 0.15f;
	[Export] public float horizontalOffset = 200;
	[Export] public float horizontalOffsetLerpWeight = 0.025f;

	[ExportGroup("Debug")]
	[Export] private Label healthLabel;

	public override void _Ready() {
		ReadyMovement();
		ReadyCombat();
	}

	public override void _PhysicsProcess(double delta) {
		ProcessMovement(delta);
		ProcessFlip();

		UpdateHealthLabel();		
	}

	private void UpdateHealthLabel() {
		healthLabel.Text = $"{healthComponent.currentHealth}/{healthComponent.maxHealth}";
	}

	private void OnVerticalLookTimerTimeout() {
		float lookDirY = inputDirection.Y;

		if (lookDirY == 0 || isMoving) return;
		camera.targetOffset.Y = lookDirY * verticalLookOffset;
	}
}
