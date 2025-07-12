using Godot;
using System;

public partial class Enemy : CharacterBody2D
{
	[Export] private Player player;
	[Export] private PlayerHealthBar playerHealthBar;
	private HealthBar healthBar;

	
	public const float Speed = 50.0f;
	public const float JumpVelocity = -400.0f;

	private Timer AttackTimer;
	private AnimatedSprite2D EnemyAnim;
	private Area2D AttackArea;
	private CollisionShape2D AttackCollision;
	private Area2D attackRange;
	
	private float NewX;

	//ENEMY DATA


	private float playerposition = 0;

	private bool isChasing = false;
	private bool can_Attack = false;
	
	//Enemy Stats
	public int health =0;
	public int PlayerDamage;
	private int EnemyDamage = 2;


	public override void _Ready()
	{
		AttackTimer = GetNode<Timer>("AttackTimer");
		healthBar = GetNode<HealthBar>("HealthBar");
		EnemyAnim = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		AttackArea = GetNode<Area2D>("AttackArea");
		AttackCollision = AttackArea.GetNode<CollisionShape2D>("AttackCollision");
		attackRange = GetNode<Area2D>("AttackRange");
		health = 6;
		healthBar.Initialize(health);
	
		PlayerDamage = player.damage;

	}
	
	
	public override void _PhysicsProcess(double delta)
	{
		
		if(can_Attack){
			EnemyAnim.Play("Attack");
			AttackCollision.Disabled = false;
			AttackTimer.Start();
		}else if(isChasing){
			playerposition = GlobalScript.PlayerPosition.X;
			NewX = (playerposition - this.Position.X)/Speed;
			if(NewX<0){
				EnemyAnim.Scale = new Vector2(-2,2);
				AttackCollision.Position = new Vector2((float)-37,(float)56.5);
			}else if(NewX>0){
				EnemyAnim.Scale = new Vector2(2,2);
				AttackCollision.Position = new Vector2((float)44.5,(float)56.5);
			}
			this.Position = new Vector2(this.Position.X + NewX, this.Position.Y);
			EnemyAnim.Play("Idle");
		}else{
			EnemyAnim.Play("Idle");
		}
		MoveAndSlide();
	}
	
	
	 private void playerEntered(Node body){
		if (body is Player){
			isChasing = true;
		}
		
	}
	
	private void playerExited(Node body){
		if (body is Player){
			playerposition = 0;
			isChasing = false;
		}
	}

	private void withinAtkRange(Node body){
		if (body is Player){
			can_Attack = true;
		}
		
	}
	
	private void leftAtkRange(Node body){
		if (body is Player){
			AttackTimer.Stop();
		}
	}
	
	
	private void Area2DEntered(Node Area){
		if (Area.IsInGroup("Attack")){
			health -= PlayerDamage;
			healthBar.SetHealth(health);
		}
		if (health<=0){
			QueueFree();
		}
		if (Area.IsInGroup("AttackD")){
			Player.HitSpike = true;
		}
	}
	
	private void AnimationFinished(){
		can_Attack = false;
		AttackCollision.Disabled = true;
	}
	
	private void OnTimeout(){
		can_Attack = true;
	}
	
	private void OnArea2DEntered(Node Area){
		if (Area.IsInGroup("Player")){
			playerHealthBar.PlayerTakeDamage(EnemyDamage);
		}
	}
	
  
  }
