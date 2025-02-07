using Godot;
using System;
using DialogueManagerRuntime;
public partial class CanDialogue : Area2D
{
	
	private Label firstlabel;
	
	public override void _Ready()
	{
		firstlabel = GetNode<Label>("FirstLabel");
		firstlabel.Visible = false;
		//GD.Print(firstlabel.Visible);
	}
	
	[Export] public Resource DialogueResource;
	[Export] public string DialogueStart = "start";
	private bool IsInBody = false;
	private static bool IsDialogueing = false;
	
	
	private void OnBodyEntered(Node body){
		if (body is Player){
			IsInBody = true;
			firstlabel.Visible = true;
		}
	}
	
	private void OnDialogueExit(Node body){
		IsInBody = false;
		firstlabel.Visible = false;
	}
	
	public override void _Process(double delta)
	{
		//GD.Print(IsInBody);
		
		if (IsInBody){
			IsDialogueing = true;
		}else{
			IsDialogueing = false;
		}
		
		if (IsDialogueing){
			if (Input.IsActionJustReleased("Dialogue")){
				DialogueManager.ShowDialogueBalloon(DialogueResource, DialogueStart);
				IsDialogueing = false;
			}
		}
	}
	



}
