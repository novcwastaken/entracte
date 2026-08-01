using Godot;
using System;

public partial class Camera : Camera2D {
	[Export] private Angel followTarget;

	private Vector2 baseOffset; // Set this in the inspector, on the Camera2D
	public Vector2 targetOffset;

	public override void _Ready() {
		baseOffset = Offset;
		ResetOffset();
	}

	public override void _PhysicsProcess(double delta) {
		Position = followTarget.Position;
		Vector2 offset = Offset;

		offset.X = float.Lerp(offset.X, targetOffset.X, followTarget.horizontalOffsetLerpWeight);
		offset.Y = float.Lerp(offset.Y, targetOffset.Y, followTarget.verticalLookLerpWeight);

		Offset = offset;
	}

	public void ResetOffset() {
		targetOffset = baseOffset;
	}
}
