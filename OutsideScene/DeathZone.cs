using Godot;
using System;

public partial class DeathZone : Area2D
{

	public override void _Ready()
	{
	}

	private void OnBodyEntered(Node body){
		
		if (body is Player){
			//Change this to last save spawnpoint later
			GlobalScript.PlayerPosition = new Vector2(363,574 );
			GetTree().ChangeSceneToFile("res://OutsideScene/game_over.tscn");
			//GD.Print("Player in hitbox");
			
		}
	}
	

}
