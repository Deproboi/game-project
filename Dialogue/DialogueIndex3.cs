using Godot;
using System;

public partial class DialogueIndex3 : Area2D
{

	public override void _Ready()
	{
	}

	private void _on_body_entered(Node body){
		Dialogue.npcID = "Instructor3";
	}
}
