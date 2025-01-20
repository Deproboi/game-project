using Godot;
using System;
using System.ComponentModel.DataAnnotations;

public partial class CharacterBody2d : CharacterBody2D
{

	private Timer _DashTimer;
	private Timer _DashAgain;
	private Timer _WallJumpDur;

	public override void _Ready()
	{
		
		_DashTimer = GetNode<Timer>("Dash_timer");
		_DashAgain = GetNode<Timer>("Dash_Again");
		_WallJumpDur = GetNode<Timer>("Wall_Jump_Dur");
		
	}

	//DASH MOVEMENT VARIABLES
	private int last = 1;
	private const int DASH_SPEED = 20;
	private bool dashing = false;
	private bool can_dash = true;

	public override void _PhysicsProcess(double delta)
	{

		int AMOUNT = 10;
		
		//BASIC LEFT RIGHT MOVEMENT
		
		if (Input.IsPhysicalKeyPressed(Key.A)){
			this.Position += new Vector2(-AMOUNT,0);
			last = -1;
		}
		if (Input.IsPhysicalKeyPressed(Key.D)){
			this.Position += new Vector2(AMOUNT, 0);
			last = 1;
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
	
	//GRAVITY AND JUMP VARIABLES
	private const float Speed = 200f;
	private const float Gravity = 800f;
	private const float JumpForce = -350f;
	private const int Fall_Gravity = 1500;

	//WALL JUMP AND SLIDE VARIABLES
	
	private const int WallJumpPush = 15;
	private bool CanWallJump = true;
	private bool WallJumping = false;
	private int WallDir = -5;
	private bool IsWallSlide = false;
	private const float MaxFallSpeed = 150f;
	
	private void JumpMovement(double delta){
		
		
		Vector2 _velocity = Velocity;
		
		if (!IsOnFloor()){
			if (_velocity.Y <0){
				_velocity.Y += Gravity * (float)delta;
			}else{
				_velocity.Y += Fall_Gravity * (float)delta;
			}
		}

		if (Input.IsActionJustReleased("jump") && _velocity.Y <0){
			_velocity.Y = JumpForce / 4;
		}


		if((Input.IsPhysicalKeyPressed(Key.W) || Input.IsPhysicalKeyPressed(Key.Space))){
			if (IsOnFloor()){
				_velocity.Y = JumpForce;
			
			} if(IsOnWall() && Input.IsPhysicalKeyPressed(Key.D) && WallJumping == false && CanWallJump == true){
				WallDir = -1;
				WallJumping = true;
				CanWallJump = false;
				_velocity.Y = JumpForce;
				_WallJumpDur.Start();
			
			} if(IsOnWall() && Input.IsPhysicalKeyPressed(Key.A) && WallJumping == false && CanWallJump == true){
				WallDir = 1;
				WallJumping = true;
				CanWallJump = false;
				_velocity.Y = JumpForce;
				_WallJumpDur.Start();
			}
		}
		
		if (WallJumping == true){
			this.Position += new Vector2(WallDir * WallJumpPush,0);
		}

		
		if (IsOnFloor()){
			CanWallJump = true;
		}



		if (IsOnWall() && !IsOnFloor()){
			if (Input.IsPhysicalKeyPressed(Key.A) || Input.IsPhysicalKeyPressed(Key.D)){
				IsWallSlide = true;
			}else{
				IsWallSlide = false;
			}
		}else{
			IsWallSlide = false;
		}

		if (IsWallSlide == true){
			_velocity.Y = Mathf.Min(_velocity.Y + Gravity * (float)delta, MaxFallSpeed);
		}
	

		Velocity = _velocity;
		MoveAndSlide();
		

	}
	
	private void OnWallTimeout(){
		WallJumping = false;
	}
	

}
