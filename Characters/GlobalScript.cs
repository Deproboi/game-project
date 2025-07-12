using Godot;
using System;

public partial class GlobalScript : Node
{
	public static bool IsSettingFromStart = true;
	
	public static Vector2 PlayerPosition = new Vector2(363,574);
	
	public static bool CurrentDialogue = false;
	

	public override void _Ready(){
	}

	public static void NewPlayerPosition(Vector2 NewPosition){
		PlayerPosition = NewPosition;
	}

}
