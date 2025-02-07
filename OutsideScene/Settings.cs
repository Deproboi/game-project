using Godot;
using System;

public partial class Settings : CanvasLayer
{


	public override void _Ready()
	{

	}


	public override void _Process(double delta)
	{
		if (Input.IsActionJustReleased("Back")){
			if (GlobalScript.IsSettingFromStart == false){
				GetTree().ChangeSceneToFile("res://InsideScene/main.tscn");
			}else{
				GetTree().ChangeSceneToFile("res://OutsideScene/start_screen.tscn");
			}
		}
	}
	
	private void OnMainMenuPressed(){
		GetTree().ChangeSceneToFile("res://OutsideScene/start_screen.tscn");
	}
	
	private void OnQuitPressed(){
		GetTree().Quit();
	}
	
}
