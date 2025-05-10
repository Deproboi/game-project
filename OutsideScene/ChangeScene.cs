using Godot;
using System;

public partial class ChangeScene : Area2D
{
	
	Node currentScene;

	public override void _Ready()
	{
		SceneTree scenetree = GetTree();
		currentScene = scenetree.CurrentScene;
	}
	
	private void OnBodyEntered(Node body){
		if (body is Player){
			
			//Add another if statemnet for each scene change
			//the name is the name of the Node
			
			
			if (currentScene.Name == "World"){
				GlobalScript.PlayerPosition = new Vector2(363,574 );
				GetTree().ChangeSceneToFile("res://InsideScene/beginning.tscn");
			}
		}
	}

}
