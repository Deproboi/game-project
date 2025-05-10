using Godot;
using System;

public partial class ChangeSceneSettings : Label
{

	public override void _Ready()
	{
	}

	private void _on_scene_2_button_down(){
		GetTree().ChangeSceneToFile("res://InsideScene/beginning.tscn");
	}
	
}
