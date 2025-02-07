using Godot;
using System;
using System.ComponentModel.DataAnnotations;
using DialogueManagerRuntime;


public partial class Player : CharacterBody2D
{
	

	private Timer _DashTimer;
	private Timer _DashAgain;
	private Timer _WallJumpDur;
	private Timer _WallJumpTimer;
	
	private AnimatedSprite2D PlayerAnim;
	private RayCast2D WallRay;
	private CharacterBody2D player;

	public override void _Ready()
	{
		Position = GlobalScript.PlayerPosition;
		
		_DashTimer = GetNode<Timer>("Dash_timer");
		_DashAgain = GetNode<Timer>("Dash_Again");
		_WallJumpDur = GetNode<Timer>("Wall_Jump_Dur");
		_WallJumpTimer = GetNode<Timer>("Wall_Jump_Timer");
		
		PlayerAnim = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		WallRay = GetNode<RayCast2D>("WallRay");
		player = GetNode<CharacterBody2D>("Player");
		
		DialogueManager.Connect("on_dialogue_started", this, nameof(OnDialogueStarted));
		DialogueManager.Connect("on_dialogue_ended", this, nameof(OnDialogueEnded));
		
	}

	//DASH MOVEMENT VARIABLES
	private int last = 1;
	private const int DASH_SPEED = 20;
	private bool dashing = false;
	private bool can_dash = true;
	
	private bool IsJumping = false;
	
	private bool IsDialogueActive = false;

	public override void _PhysicsProcess(double delta)
	{
		
		if (IsDialogueActive) // Don't move if dialogue is active
		{
			return;
		}
		
		
		GlobalScript.NewPlayerPosition(Position);
		
		
		//settings
		if (Input.IsActionJustReleased("Back")){
			GetTree().ChangeSceneToFile("res://OutsideScene/settings.tscn");
		}

		int AMOUNT = 10;
		
		//BASIC LEFT RIGHT MOVEMENT
		
		
		if (Input.IsPhysicalKeyPressed(Key.A)){
			this.Position += new Vector2(-AMOUNT,0);
			PlayerAnim.Scale = new Vector2(-1,1);
			if (IsJumping == false){
				PlayerAnim.Play("Running");
			}
			WallRay.Scale = new Vector2(-1,1);
			last = -1;
		}else if (Input.IsPhysicalKeyPressed(Key.D)){
			this.Position += new Vector2(AMOUNT, 0);
			PlayerAnim.Scale = new Vector2(1,1);
			if (IsJumping == false){
				PlayerAnim.Play("Running");
			}
			WallRay.Scale = new Vector2(1,1);
			last = 1;
		}else{
			if (IsJumping == false){
				PlayerAnim.Scale = new Vector2(last, 1);
				PlayerAnim.Play("Idle");
			}
		}
	
		//DASH MOVEMENT
	
		if (Input.IsPhysicalKeyPressed(Key.Shift) && can_dash == true){
			dashing = true;
			can_dash = false;
			_DashTimer.Start();
			_DashAgain.Start();
		}
		
		if (dashing == true){
			this.Position += new Vector2(last*DASH_SPEED, 0);
		}

		JumpMovement(delta);
		
		MoveAndSlide();
		

	}
	
	//DASH TIMERS
	private void OnDashTimerTimout()
	{
		dashing = false;
	}
	
	private void OnDashAgainTimeout(){
		can_dash = true;
	}
	
	
	private void OnDialogueStarted(){
		IsDialogueActive = true;
	}
	
	
	private void OnDialogueEnded(){
		IsDialogueActive = false;
	}
	
	
	
	//GRAVITY AND JUMP VARIABLES
	private const float Speed = 200f;
	private const float Gravity = 800f;
	private const float JumpForce = -350f;
	private const int Fall_Gravity = 1500;

	//WALL JUMP AND SLIDE VARIABLES
	
	private const int WallJumpPush = 15;
	private bool CanWallJump = true;
	private bool AfterCanWallJump = false;
	private bool WallJumping = false;
	private int WallDir = -5;
	private bool IsWallSlide = false;
	private const float WallSlideGravity = 800;
	
	private int LastWallX = -123;
	
	private void JumpMovement(double delta){
		
		if (IsOnFloor() ){
			CanWallJump = true;
			LastWallX = -123;
			IsJumping = false;
		}
		
		
		Vector2 _velocity = Velocity;

		//Wall Sliding

		if (WallColliding() && !IsOnFloor()){
			if (Input.IsPhysicalKeyPressed(Key.A) || Input.IsPhysicalKeyPressed(Key.D)){
				IsWallSlide = true;
			}else{
				IsWallSlide = false;
			}
		}else{
			IsWallSlide = false;
		}
		
		if (!IsOnFloor()){
			if (IsWallSlide == true){
				//Wall Slide gravity
				//_velocity.Y += (WallSlideGravity * (float)delta);
			//_velocity.Y = Mathf.Min(_velocity.Y, WallSlideGravity);
			if (_velocity.Y >0){_velocity.Y =0;} 
			this.Position += new Vector2(0,8);

			}else if (_velocity.Y <0){
				//Regular going up gravity
				_velocity.Y += Gravity * (float)delta;
			}else{
				//Going down gravity
				_velocity.Y += Fall_Gravity * (float)delta;
			}
		}
		
		//Different jump sizes

		if (Input.IsActionJustReleased("jump") && _velocity.Y <0){
			_velocity.Y = JumpForce / 4;
		}
		
		//Jumping stuff
		if(Input.IsPhysicalKeyPressed(Key.W)){
			if (IsOnFloor()){
				//regular jum
				_velocity.Y = JumpForce;
				AfterCanWallJump = false;
				_WallJumpTimer.Start();
				PlayerAnim.Play("Jumping");
				IsJumping = true;
			
			//Wall Jumping
			}else if(WallColliding() && AfterCanWallJump == true && Input.IsPhysicalKeyPressed(Key.D) && WallJumping == false && LastWallX != (int)this.Position.X){
				WallDir = -1;
				WallJumping = true;
				//CanWallJump = false;
				_velocity.Y = JumpForce;
				_WallJumpDur.Start();
				LastWallX = (int)this.Position.X;
			
			}else if(WallColliding() && AfterCanWallJump == true && Input.IsPhysicalKeyPressed(Key.A) && WallJumping == false&& LastWallX != (int)this.Position.X){
				WallDir = 1;
				WallJumping = true;
				//CanWallJump = false;
				_velocity.Y = JumpForce;
				_WallJumpDur.Start();
				LastWallX = (int)this.Position.X;
			}
		}
		
		if (WallJumping == true){
			if (WallDir == 1 && Input.IsPhysicalKeyPressed(Key.A)){
				this.Position += new Vector2(WallDir * WallJumpPush,0);
			}else if (WallDir == -1 && Input.IsPhysicalKeyPressed(Key.D)){
				this.Position += new Vector2(WallDir * WallJumpPush,0);
			}
			//this.Position += new Vector2(WallDir * WallJumpPush,0);
		}

		

	

		Velocity = _velocity;
		MoveAndSlide();
		

	}
	
	private void OnWallTimeout(){
		WallJumping = false;
	}
	
	private void OnWallJumpTimeout(){
		AfterCanWallJump = true;
	}
	
	private bool WallColliding(){
		return WallRay.IsColliding();
	}
	
	

}
