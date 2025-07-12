using Godot;
using System;

public partial class PlayerHealthBar : CanvasLayer
{
	[Export] private Player player;
	private HealthBar healthBar;
	
	private int health;
	private int maxHealth;
	public bool FullHealth = true;
	
	public override void _Ready()
	{
		healthBar = GetNode<HealthBar>("HealthBar");
		health = player.health;
		maxHealth = health;
		healthBar.Initialize(health);
	}

	public void PlayerTakeDamage(int damage){
		health -= damage;
		if (health <= 0){
			player.PlayerDied();
		}
		FullHealth = false;
		healthBar.SetHealth(health);
	}
	
	public void PlayerHeal(int heal){
		health += heal;
		if (health>=maxHealth){
			health = maxHealth;
			FullHealth = true;
		}
		healthBar.SetHealth(health);
	}
	
	
}
