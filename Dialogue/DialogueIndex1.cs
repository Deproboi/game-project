using Godot;
using System;

public partial class DialogueIndex1 : Area2D
{
	[Export] private string hello;
	public override void _Ready()
	{
	}

	private void _on_body_entered(Node body){
		Dialogue.npcID = hello;
	}
}
