using Godot;
using System;

public partial class HealthBar : ProgressBar
{
	
	private ProgressBar DamageBar;
	private Timer CatchUp;
	
	private float catchUpSpeed = 10.0f; 
	
	public override void _Ready()
	{
		DamageBar = GetNode<ProgressBar>("DamageBar");
		CatchUp = GetNode<Timer>("CatchUp");
	}

	public void Initialize(int health){
		this.MaxValue = health;
		this.Value = health;
		DamageBar.MaxValue = health;
		DamageBar.Value = health;
	}
	
	public void SetHealth(int health){
		if (health<=0){
			QueueFree();
		}
		this.Value = health;
		CatchUp.Start();
	}
	
	private void CatchUpTimeout(){
		DamageBar.Value = this.Value;
		/*if (DamageBar.Value > this.Value)
		{
			DamageBar.Value = Mathf.Lerp(DamageBar.Value, this.Value, catchUpSpeed * (float)CatchUp.WaitTime);
		} else {
			DamageBar.Value = Value; // Ensure it doesn't overshoot
			CatchUp.Stop(); // Stop the timer once caught up
		}*/
	}
	
	
	
}
