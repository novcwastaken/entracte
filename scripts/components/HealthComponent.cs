using Godot;

[GlobalClass, Icon("res://addons/at-icons/node/heart.svg")]
public partial class HealthComponent : Node {
    [Export] public int maxHealth;
    public int currentHealth;

    public override void _Ready() {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount) {
        currentHealth = Mathf.Max(currentHealth - amount, 0);
    }

    public void Heal(int amount) {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }

    public void HealToMax() {
        Heal(maxHealth);
    }
}