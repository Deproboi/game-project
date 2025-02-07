using Godot;
using System;

public partial class StartScreen : CanvasLayer
{


	public override void _Ready()
	{

	}

	private void OnPlayPressed(){
		GlobalScript.IsSettingFromStart = false;
		GetTree().ChangeSceneToFile("res://InsideScene/main.tscn");
	}

	private void OnQuitPressed(){
		GetTree().Quit();
	}
	
	private void OnSettingsPressed(){
		GetTree().ChangeSceneToFile("res://OutsideScene/settings.tscn");
	}

}
