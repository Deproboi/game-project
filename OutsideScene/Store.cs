using Godot;
using System;

public partial class Store : Godot.ColorRect
{
	private Button Sword;
	private Label SwordLabel;
	
	public override void _Ready()
	{
		Sword = GetNode<Button>("Sword");
		SwordLabel = Sword.GetNode<Label>("SwordLabel");
	}
	
	private void OnMouseEntered(){
		SwordLabel.Visible = true;
	}
	
	private void OnMouseExited(){
		SwordLabel.Visible = false;
	}

	public override void _Process(double delta)
	{
	}
}
