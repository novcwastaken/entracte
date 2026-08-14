using Godot;

public partial class Angel : CharacterBody2D {
    // -------------
    // --- STATS ---
    // -------------

    // --- BASE ---
    
    // Movement
    // ...

    // Input
    public Vector2 debugInputDirection => inputDirection;
    public int debugLastDirectionX => lastDirectionX;

    // --- ABILITIES ---

    // Dash 
    public bool debugDashUnlocked => dashUnlocked;
    public int debugDashDirectionX => dashDirectionX;
    // ---
    public bool debugIsDashing => isDashing;
    public bool debugCanDash => canDash;
    public bool debugCanStartDash => canStartDash;
    public bool debugCanRegainDash => canRegainDash;
    // ---
    public Timer debugDashTimer => dashTimer;
    public Timer debugDashCooldownTimer => dashCooldownTimer;
}