using Godot;
using System;

public partial class Enemy : CharacterBody2D
{

	
	public const float Speed = 50.0f;
	public const float JumpVelocity = -400.0f;
	
	Boolean isChasing = false;
	Boolean can_Attack = false;
	private CharacterBody2D player;

	private Timer _atkTimer;

	//ENEMY DATA

	private int dmg = 5;

	private int hp = 100;




    public override void _Ready()
    {
       _atkTimer = GetNode<Timer>("Atk_Timer");
    }
    public override void _PhysicsProcess(double delta)
    {
		if(can_Attack){
			GD.Print("Attacked");
			//player.takeDamage();
			can_Attack = false;
		}
        if(isChasing){
			this.Position += (player.Position - this.Position)/Speed;
		}
		MoveAndSlide();
    }



    private void playerEntered(CharacterBody2D body){
		GD.Print("dsawdjsajdklwad");
		player = body;
		isChasing = true;
		
	}
	
	private void playerExited(CharacterBody2D body){
		GD.Print("DSAHJKDAS");
		player = null;
		isChasing = false;
		
	}

	private void withinAtkRange(CharacterBody2d body){
		isChasing = false;
		_atkTimer.Start();
	}

	private void atkFinished(CharacterBody2d body){
		isChasing = true;
		_atkTimer.Stop();
		
	}

	private void OnAtkTimeout(){
		can_Attack = true;
	}

	private void attack(){
		//MERGE WITH GLOBAL VAR FILE
		//playerHp -= dmg;
	}
  
  }
