using Godot;
using System;
using Godot.Collections;



public partial class Dialogue : CanvasLayer
{
	
	
	private static Label text;
	private static Label _name;
	private static Timer typingTimer;
	

	private static string[] dialogueLines;
	private static int currentLineIndex = 0;
	
	private static string currentLine = "";
	private static int charIndex = 0;
	
	public static bool IsDialogueActive = false;
	
	public static string npcID;

	private static string TheName;
	
	private static Dictionary<string, Variant> npcDialogues = new Dictionary<string, Variant>();
	
	public override void _Ready()
	{
		text = GetNode<Label>("Text");
		typingTimer = GetNode<Timer>("TypingTimer");
		_name = GetNode<Label>("Name");
		
		 //DialogueIndex dialogueIndex = GetNode<DialogueIndex>("DialogueIndex");

		// Access the non-static member
		//npcID = dialogueIndex.DialogueNPCIndex;


	}
	



	public static void LoadDialogue(string filepath)
{
	
	// Read the JSON file
	string jsonString = System.IO.File.ReadAllText(filepath);

	// Parse the JSON string
	var jsonData = Json.ParseString(jsonString);

	// Check for errors
	

	// Cast the parsed data to a Dictionary
	npcDialogues = jsonData.AsGodotDictionary<string, Variant>();

	//Which NPC dialogue
	var npc = npcDialogues[npcID];

	var npcData = npc.AsGodotDictionary<string, Variant>();

	// Set the NPC name
	TheName = npcData["name"].AsString();
	

	// Cast the dialogue lines to an Array
	dialogueLines = (string[])npcData["dialogue1"];

	StartingDialogue();

}

	
	public static void StartingDialogue(){
		IsDialogueActive = true;
		currentLineIndex = 0;
		charIndex = 0;
		currentLine = "";
		text.Visible = true;
		IsDialogueActive = true;
		_name.Visible = true;
		_name.Text = TheName;
		StartDialogue();
	}

	
	private static void StartDialogue(){
		StartTyping(dialogueLines[currentLineIndex]);
	}

	public static void OnNextDown(){
		if (charIndex < dialogueLines[currentLineIndex].Length){
			text.Text = dialogueLines[currentLineIndex];
			charIndex = dialogueLines[currentLineIndex].Length;
		}else{
			currentLineIndex ++ ;
		
			if (currentLineIndex == dialogueLines.Length){
				text.Visible = false;
				IsDialogueActive = false;
				_name.Visible = false;
			}else{
				StartDialogue();
			}
		}
		
	}
	
	private static void StartTyping(string line){
		currentLine = line;
		charIndex = 0;
		text.Text = "";
		typingTimer.Start();
	}
	
	private void OnTypingTimeout(){
		if (charIndex < currentLine.Length){
			text.Text += currentLine[charIndex];
			charIndex++;
			typingTimer.Start();
		}
	}
	
}
