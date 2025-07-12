using Godot;
using System;

public partial class PlayerHeal : CanvasLayer
{
	
	[Export] private Player player;
	private ProgressBar regenAmount;
	public int regenAmountValue;
	public int MaxValue = 100;
	public bool CanHeal = true;
	
	public override void _Ready()
	{
		regenAmount = GetNode<ProgressBar>("RegenAmount");
		regenAmountValue = player.GourdHealAmount;
		regenAmount.MaxValue = MaxValue;
		regenAmount.Value = MaxValue;
	}

	public void GourdHeal(){
		regenAmount.Value = regenAmountValue;
	}
	
	public void GourdDeplen(){
		double NewHealth = regenAmount.Value - regenAmountValue;
		if (NewHealth <=0){
			NewHealth = 0;
			CanHeal = false;
		}
		regenAmount.Value = NewHealth;
		
	}
	

}
