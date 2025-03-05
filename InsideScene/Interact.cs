using Godot;
using System;

public partial class Interact : Area2D
{

	private bool CanDialogue = false;
	
	private Label interactText;
	
	public override void _Ready()
	{
		interactText = GetNode<Label>("InteractText");
		interactText.Visible = false;
	}

	private void OnBodyEntered(Node body){
		if (body is Player){
			CanDialogue = true;
			interactText.Visible = true;
		}
	}
	
	private void OnBodyExited(Node body){
		interactText.Visible = false;
	}
	
	public override void _Process(double delta){
		if (CanDialogue){
			if (Input.IsActionJustReleased("Dialogue")&&Dialogue.IsDialogueActive == false){
				Dialogue.LoadDialogue("C:/Users/Warre/OneDrive/Documents/Godot/trial/game-project/Dialogue/NPCdialogues.JSON");
			}
		}
	}

}
