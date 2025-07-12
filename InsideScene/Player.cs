using Godot;
using System;
using System.ComponentModel.DataAnnotations;


public partial class Player : CharacterBody2D
{
	
	[Export] private PlayerHealthBar playerHealthBar;
	[Export] private PlayerHeal playerHeal;


	private Timer _DashTimer;
	private Timer _DashAgain;
	private Timer _WallJumpDur;
	private Timer _WallJumpTimer;
	
	private AnimatedSprite2D PlayerAnim;
	private AnimatedSprite2D EffectAnim;
	private RayCast2D WallRay;
	private CharacterBody2D player;
	private CollisionShape2D _collisionShape;
	private Area2D AttackAreaF;
	private Area2D AttackAreaD;
	private CollisionShape2D AttackCollisionF;
	private CollisionShape2D AttackCollisionD;


	public override void _Ready()
	{
		
		Position = GlobalScript.PlayerPosition;
		
		_DashTimer = GetNode<Timer>("Dash_timer");
		_DashAgain = GetNode<Timer>("Dash_Again");
		_WallJumpDur = GetNode<Timer>("Wall_Jump_Dur");
		_WallJumpTimer = GetNode<Timer>("Wall_Jump_Timer");
		
		PlayerAnim = GetNode<AnimatedSprite2D>("PlayerAnim");
		EffectAnim = GetNode<AnimatedSprite2D>("EffectAnim");
		WallRay = GetNode<RayCast2D>("WallRay");
		player = GetNode<CharacterBody2D>("Player");
		_collisionShape = GetNode<CollisionShape2D>("HitboxCollision");
		AttackAreaF = GetNode<Area2D>("AttackAreaF");
		AttackAreaD = GetNode<Area2D>("AttackAreaD");
		AttackCollisionF = AttackAreaF.GetNode<CollisionShape2D>("AttackCollisionF");
		AttackCollisionD = AttackAreaD.GetNode<CollisionShape2D>("AttackCollisionD");
		
		
	}
	
	public override void _UnhandledInput(InputEvent @event){
		//settings
		if (Input.IsActionJustReleased("Back")){
			GetTree().ChangeSceneToFile("res://OutsideScene/settings.tscn");
		}
	}
	
	//Player Stats
	public int health = 10;
	public int damage = 1;
	public int GourdHealAmount = 25;

	//DASH MOVEMENT VARIABLES
	private int last = 1;
	private const int DASH_SPEED = 20;
	private bool dashing = false;
	private bool can_dash = true;
	
	private bool IsJumping = false;
	
	private bool IsAttacking = false;
	
	public static bool HitSpike = false;
	
	private bool ded = false;

	public override void _PhysicsProcess(double delta)
	{
		
		
		if (Dialogue.IsDialogueActive) // Don't move if dialogue is active
		{
			if (Input.IsActionJustReleased("Continue") && Dialogue.IsDialogueActive){
				Dialogue.OnNextDown();
			}
			return;
		}else if(ded){
			return;
		}
		
		if (Input.IsActionJustReleased("Heal")&&playerHeal.CanHeal && playerHealthBar.FullHealth == false){
			playerHealthBar.PlayerHeal(5);
			playerHeal.GourdDeplen();
		}
		
		
		
		GlobalScript.NewPlayerPosition(Position);
		

		int AMOUNT = 10;
		
		//BASIC LEFT RIGHT MOVEMENT
		
		
		if (Input.IsPhysicalKeyPressed(Key.A)){
			this.Position += new Vector2(-AMOUNT,0);
			PlayerAnim.Scale = new Vector2(-1,1);
			if (IsJumping == false&&IsAttacking == false){
				PlayerAnim.Play("Running");
			}
			WallRay.Scale = new Vector2(-1,1);
			last = -1;
		}else if (Input.IsPhysicalKeyPressed(Key.D)){
			this.Position += new Vector2(AMOUNT, 0);
			PlayerAnim.Scale = new Vector2(1,1);
			if (IsJumping == false&&IsAttacking == false){
				PlayerAnim.Play("Running");
			}
			WallRay.Scale = new Vector2(1,1);
			last = 1;
		}else{
			if (IsJumping == false&&IsAttacking == false){
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
		CharAttack(delta);
		
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
	
	
	
	private bool OnGround = true;
	
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
	private const float WallSlideGravity = 200;
	private int LastWallDir =0;
	
	
	private void JumpMovement(double delta){
		
		Vector2 _velocity = Velocity;
		
		if (HitSpike){
			_velocity.Y = JumpForce;
			HitSpike = false;
		}
		
		
		if (IsOnFloor() ){
			CanWallJump = true;
			IsJumping = false;
			OnGround = true;
			_collisionShape.Position = new Vector2((float)2.5,_collisionShape.Position.Y);
			LastWallDir = 0;
			if (PlayerAnim.Animation == "DAir"){
				IsAttacking = false;
			}
		}
		
		
		

		//Wall Sliding
		
		

		if (WallColliding()&& !IsOnFloor()){
			if (Input.IsPhysicalKeyPressed(Key.A) || Input.IsPhysicalKeyPressed(Key.D)){
				IsWallSlide = true;

			}else{
				IsWallSlide = false;
			}
		}else if(!WallColliding()){
			IsWallSlide = false;
		}
		
		
		if (!IsOnFloor()){
			if (IsWallSlide == true){
			//Wall Slide gravity
			
				if (_velocity.Y < 0){
					//_velocity.Y =0;
					
				} 
				this.Position += new Vector2(0,7);
				
				//_velocity.Y = 10;
				
				
				if (IsAttacking == false){
					PlayerAnim.Play("WallSlide");
				}

			}else if (_velocity.Y <0){
				//Regular going up gravity
				_velocity.Y += Gravity * (float)delta;
			}else{
				//Going down gravity
				_velocity.Y += Fall_Gravity * (float)delta;
				if (IsAttacking == false){ PlayerAnim.Play("Falling");}
			}
		}
		
		//Different jump sizes

		if (Input.IsActionJustReleased("jump") && _velocity.Y <0){
			_velocity.Y = JumpForce / 4;
		}
		
		
		//Jumping stuff
		if(Input.IsPhysicalKeyPressed(Key.W)){
			OnGround = false;
			if (IsOnFloor()){
				//regular jum
				_velocity.Y = JumpForce;
				AfterCanWallJump = false;
				_WallJumpTimer.Start();
				if (IsAttacking == false){PlayerAnim.Play("Jumping");}
				IsJumping = true;
			
			//Wall Jumping
			}else if(WallColliding() && AfterCanWallJump == true && (Input.IsPhysicalKeyPressed(Key.D) || Input.IsPhysicalKeyPressed(Key.A)) && WallJumping == false && LastWallDir != last){
				WallDir = -last;
				LastWallDir = last;
				WallJumping = true;
				//CanWallJump = false;
				_velocity.Y = JumpForce;
				_WallJumpDur.Start();
			
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
	
	private void CharAttack(double delta){
		
		if (Input.IsActionJustReleased("RightAttack") && IsAttacking ==false){
			PlayerAnim.Scale = new Vector2(1,1);
			EffectAnim.Position = new Vector2(33,-1);
			EffectAnim.Scale = new Vector2(1,1);
			AttackAreaF.Scale = new Vector2(1,1);
			IsAttacking = true;
			PlayerAnim.Play("FAir");
			EffectAnim.Play("FAir");
			AttackCollisionF.Disabled = false;
		}else if(Input.IsActionJustReleased("LeftAttack") && IsAttacking ==false){
			PlayerAnim.Scale = new Vector2(-1,1);
			EffectAnim.Position = new Vector2(-28,-1);
			EffectAnim.Scale = new Vector2(-1,1);
			AttackAreaF.Scale = new Vector2(-1,1);
			IsAttacking = true;
			PlayerAnim.Play("FAir");
			EffectAnim.Play("FAir");
			AttackCollisionF.Disabled = false;
		}else if(Input.IsActionJustReleased("DAir") && IsAttacking ==false && OnGround == false){
			IsAttacking = true;
			EffectAnim.Position = new Vector2(4,0);
			PlayerAnim.Play("DAir");
			EffectAnim.Play("DAir");
			AttackCollisionD.Disabled = false;
		}
		
		//MoveAndSlide();
	}
	
	private void AnimationFinished(){
		EffectAnim.Play("Nothing");
		IsAttacking = false;
		AttackCollisionF.Disabled = true;
		AttackCollisionD.Disabled = true;
	}

	public void PlayerDied(){
		ded = true;
		//died animation right here
	}
	
	

}
