using Godot;
using System;

public partial class CharDialogue1 : Area2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	
	private void _on_body_entered(Node body){
		Dialogue.npcID = "Link";
	}
	
}
