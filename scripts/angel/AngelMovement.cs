using Godot;
using System;

public partial class Angel : CharacterBody2D {
    [ExportGroup("Movement")]
    [ExportSubgroup("Base")]
    [Export] private float speed = 600.0f;
	[Export] private float gravity = 60.0f;
    [Export] private float terminalVelocity = 2000.0f;

    [ExportSubgroup("Jump")]
    [Export] private bool doubleJumpUnlocked = false;
    [Export] private float jumpVelocity = 1250.0f;
    [Export] private float jumpCutoffMultiplier = 0.3f;

    [ExportSubgroup("Dash")]
    [Export] private bool dashUnlocked = false;
    [Export] private float dashSpeed = 1200.0f;

    [ExportSubgroup("Walljump")]
    [Export] private bool walljumpUnlocked = false;
    [Export] private float wallJumpStrength = 3000.0f;
    [Export] private float wallSlideYMultiplier = 0.8f;

    [ExportSubgroup("Glide")]
    [Export] private bool glideUnlocked = false;
    [Export] private float glideYMultiplier = 0.75f;

    private Vector2 controlledVelocity;
    private Vector2 inputDirection;
    
    private bool isMoving => Velocity != Vector2.Zero;
    private int lastDirectionX = 1;
    private int dashDirectionX = 1;

    private bool canDash = true;
    private bool canRegainDash = false;
    private bool doubleJumpUsed = false;
    private bool wasOnWall = false;

    private bool isDashing => dashTimer.TimeLeft > 0f;
    private bool isGliding = false;

    private bool canBufferJump => !jumpBufferTimer.IsStopped();
    private bool canCoyoteJump => !coyoteTimer.IsStopped();
    private bool canStartDash => dashUnlocked && canDash && !isDashing && dashCooldownTimer.IsStopped() && !isAttacking;
    private bool canStartGlide => glideUnlocked && Velocity.Y > 0f && !IsOnFloor() && wallState == WallState.NONE && (doubleJumpUsed || !doubleJumpUnlocked) && !isGliding && !canCoyoteJump && !isDashing;

    private enum WallState { NONE, SLIDING, CLINGING } // NONE: on floor, ADJACENT: "sliding" down wall, not holding input, CLINGING: holding input
    private WallState wallState = WallState.NONE;

    private void ReadyMovement() {
        controlledVelocity = Velocity;
    }

    private void ProcessMovement(double delta) {

        // Gravity
		if (IsOnFloor()) {
			controlledVelocity.Y = 0f;
		} else {
            controlledVelocity.Y += gravity;

            if (walljumpUnlocked && wallState != WallState.NONE) {
                controlledVelocity.Y *= wallSlideYMultiplier;
                RegainDash();
            }
        }        

        // Terminal velocity
        controlledVelocity.Y = Mathf.Min(controlledVelocity.Y, terminalVelocity);

        // Movement
		inputDirection = Input.GetVector("left", "right", "up", "down").Normalized();
        if (inputDirection.X != 0 && !isDashing) lastDirectionX = inputDirection.X >= 0 ? 1 : -1; // Funky godot stuff (0.71)

        if (!walljumpLeaveTimer.IsStopped()) {
            controlledVelocity.X = Mathf.MoveToward(controlledVelocity.X, inputDirection.X * speed, (float)delta);
        } else if (inputDirection.X != 0) {
			if (!isDashing) controlledVelocity.X = (inputDirection.X >= 0 ? 1 : -1) * speed; // Mekanik
		} else {
			controlledVelocity.X = Mathf.MoveToward(Velocity.X, 0, speed);
		}

        UpdateWallState();

        // Jump
        controlledVelocity = ProcessJump(controlledVelocity);
        controlledVelocity = ProcessGlideTick(controlledVelocity, delta);

        // Dash
        controlledVelocity = ProcessDashTick(controlledVelocity, delta);
        if (IsOnFloor() && dashCooldownTimer.IsStopped()) RegainDash();

        // Double jump
        if (IsOnFloor()) doubleJumpUsed = false;

        Velocity = controlledVelocity;

        bool wasOnFloor = IsOnFloor();
        wasOnWall = IsOnWall();

		MoveAndSlide();

        if (wasOnFloor && !IsOnFloor()) coyoteTimer.Start();      
        if (!wasOnWall && IsOnWall() && isDashing && dashDirectionX * GetWallNormal().X < 0) {
            StopDash(true);
        }
    }

    private Vector2 ProcessJump(Vector2 velocity) {
        // Regular jump & WJ
        if (canBufferJump && (IsOnFloor() || canCoyoteJump || IsOnWallOnly())) { 
            jumpBufferTimer.Stop();
            coyoteTimer.Stop();

            velocity.Y = -jumpVelocity;

            // Walljump
            if (wallState != WallState.NONE) {
                velocity.X = -lastDirectionX * wallJumpStrength;
                walljumpLeaveTimer.Start();
            }
        }
        
        // Double jump mid-air
        else if (canBufferJump && !IsOnWall() && doubleJumpUnlocked && !doubleJumpUsed) { 
            jumpBufferTimer.Stop();
            coyoteTimer.Stop();
            StopDash();

            velocity.Y = -jumpVelocity;
            doubleJumpUsed = true;
        }

        // Variable jump height
        if (Input.IsActionJustReleased("jump") && velocity.Y < 0f) {
            velocity.Y *= jumpCutoffMultiplier;
        }

        if (IsOnCeiling() && velocity.Y < 0f) {
            velocity.Y = 0f;
        }

        return velocity;
    }

    private Vector2 ProcessDashTick(Vector2 velocity, double delta) {
        if (isDashing) {
            velocity = new Vector2(dashSpeed * dashDirectionX * (float)delta * 60, 0f);
        }

        return velocity;
    }

    private Vector2 ProcessGlideTick(Vector2 velocity, double delta) {
        if (isGliding && Input.IsActionPressed("jump")) {
            velocity.Y *= glideYMultiplier * (float)delta * 60;
        }

        return velocity;
    }

    private void ProcessFlip() {
        //camera.targetOffset.X = lastDirectionX * horizontalOffset * (wallState != WallState.NONE ? GetWallNormal().X : 1f);
        camera.targetOffset.X = lastDirectionX * horizontalOffset;
    }

    private void StartDash() {
        if (!canStartDash) return;

        if (wallState != WallState.NONE) {
            dashDirectionX = (int)Mathf.Sign(GetWallNormal().X);
        } else {
            dashDirectionX = lastDirectionX;
        }

        isGliding = false;

        dashTimer.Start();
        canDash = false;
    }

    private void StopDash(bool dashCheck = true) {
        if (!isDashing && dashCheck) return;

        dashTimer.Stop();
        dashCooldownTimer.Start();
    }

    private void RegainDash() {
        if (canRegainDash) {
            canDash = true;
        }
    }

    private void UpdateWallState() {        
        if (IsOnWallOnly() && !IsTouchingWallGroup("walljumpable")) {
            wallState = WallState.NONE;
            return;
        }

        bool holdingClingInput = Mathf.IsEqualApprox(inputDirection.X, -GetWallNormal().X) && inputDirection.X != 0f;

        if (IsOnWallOnly() && holdingClingInput) {
            wallState = WallState.CLINGING;
        } else if (IsOnWallOnly() && !holdingClingInput) {
            wallState = WallState.SLIDING;
        } else {
            wallState = WallState.NONE;
        }
    }

    private bool IsTouchingWallGroup(string group) {
        int slideCount = GetSlideCollisionCount();
        
        for (int i = 0; i < slideCount; i++) {
            KinematicCollision2D collision = GetSlideCollision(i);
            GodotObject collider = collision.GetCollider();

            if (collider is Node node && node.IsInGroup(group)) return true;
        }

        return false;
    }

    private void OnDashTimerTimeout() {
        StopDash(false);
    }
    
    private void OnDashCooldownTimerTimeout() {
        canRegainDash = true;
    }
}