using Godot;
using System;


public partial class GameOver : CanvasLayer
{
	
	public override void _Ready()
	{
	}

	public override void _PhysicsProcess(double delta){
		if (Input.IsActionJustReleased("respawn")){
			GetTree().ChangeSceneToFile("res://InsideScene/main.tscn");
		}
	}
	
	
}
