using Godot;
using System;

public partial class Angel : CharacterBody2D {
	public override void _Input(InputEvent @event) {
		// Jump
		if (@event.IsActionPressed("jump")) {
			jumpBufferTimer.Start();
		}

		// Glide
		if (@event.IsActionPressed("jump") && canStartGlide) {
			isGliding = true;
		}
		
		if (@event.IsActionReleased("jump") && isGliding) {
			isGliding = false;
		}

		// Dash
		if (@event.IsActionPressed("dash")) {
			StartDash();
		}

		// Look
		if (@event.IsActionPressed("up") || @event.IsActionPressed("down") && !isMoving) {
			verticalLookTimer.Start();
		}

		if (@event.IsActionReleased("up") || @event.IsActionReleased("down") || isMoving) {
			camera.ResetOffset();
		}

		// Attack
		if (@event.IsActionPressed("attack")) {
			Attack();
		}
	}
}
