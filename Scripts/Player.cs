using Godot;
using System;

public partial class Player : CharacterBody2D
{
	// Player upgradable stats
	private int _health = 100;
	private float _speed = 100.0f;
	private float _damageMultiplier = 1.0f;
	
	private const float _knockbackSpeed = 200.0f;
	private Vector2 _knockback = Vector2.Zero;
	private int _experience = 0;
	private int _experienceMax = 10;
	private int _level = 1;
	public Vector2 PlayerDirection {get; private set;} = new Vector2(1,0);
	
	[Signal]
	public delegate void LevelUpEventHandler();
	
	private void AddExperience(int exp)
	{
		_experience += exp;
		while (_experience >= _experienceMax)
		{
			_experience -= _experienceMax;
			_experienceMax += 1;
			_level += 1;
			EmitSignal(SignalName.LevelUp);
			// TODO Play animation and select upgrades
		}
	}
	
	public void LevelUpSpeed(float speed)
	{
		_speed += speed;
	}
	
	public void LevelUpDamageMultiplier(int percentDamage)
	{
		_damageMultiplier += (float)(percentDamage)/100;
		Node2D weapons = GetNode<Node2D>("Weapons");
		foreach (ProjectileWeapon projectileWeapon in weapons.GetChildren())
		{
			projectileWeapon._damageMultiplier = _damageMultiplier;
		}
	}
	
	private void OnKnockbackTimerTimeout()
	{
		_knockback = Vector2.Zero;
	}
	
	private void TakeDamage(int enemyDamage)
	{
		_health -= enemyDamage;
		if ((_health <= 0))
		{
			QueueFree();
		}
	}
	
	private void CollisionHandler()
	{
		Timer knockbackTimer = GetNode<Timer>("KnockbackTimer");
		if (knockbackTimer.IsStopped())
		{
			for (int i = 0; i < GetSlideCollisionCount(); i++)
			{
				Node2D collision = GetSlideCollision(i).GetCollider() as Node2D;
				if (collision.IsInGroup("Enemies"))
				{
					_knockback = -Position.DirectionTo(collision.GlobalPosition) * _knockbackSpeed;
					if (collision.HasMethod("TakeDamage"))
					{
						TakeDamage((int)collision.Call("PlayerCollision"));
					}
					GetNode<Timer>("KnockbackTimer").Start();
				} 
			}
		}
	}
	
	private void Movement()
	{
		Vector2 velocity = Velocity;

		// Get the input direction and handle the movement/deceleration.
		Vector2 direction = Input.GetVector("MoveLeft", "MoveRight", "MoveUp", "MoveDown");
		if (direction != Vector2.Zero)
		{
			PlayerDirection = direction;
			velocity.X = PlayerDirection.X * _speed;
			velocity.Y = PlayerDirection.Y * _speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, _speed);
			velocity.Y = Mathf.MoveToward(Velocity.Y, 0, _speed);
		}

		Velocity = velocity + _knockback;
	}

	public override void _PhysicsProcess(double delta)
	{
		Movement();
		if (MoveAndSlide())
		{
			CollisionHandler();
		}
	}
}
