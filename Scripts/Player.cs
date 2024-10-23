using Godot;
using System;

public partial class Player : CharacterBody2D
{
	// Player upgradable stats
	private int _health = 100;
	private int _maxHealth = 100;
	private float _speed = 100.0f;
	private float _damageMultiplier = 1.0f;
	
	private const float _knockbackSpeed = 200.0f;
	private Vector2 _knockback = Vector2.Zero;
	private int _experience = 0;
	private int _experienceMax = 10;
	private int _level = 1;
	public Vector2 PlayerDirection {get; private set;} = new Vector2(1,0);
	private bool _playerControlEnabled = true;
	
	[Signal]
	public delegate void LevelUpEventHandler();
	[Signal]
	public delegate void OnPlayerDeathEventHandler();
	
	private ProgressBar HealthBar;
	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		HealthBar = GetNode<ProgressBar>("HealthBar");
		HealthBar.MaxValue = _maxHealth;
		HealthBar.Value = _health;
		GetNode<Node2D>("Weapons").AddChild(GetNode<Game>("/root/Game").PlayerWeapon.Instantiate<ProjectileWeapon>());
	}
	
	private void PlayerDeath()
	{
		_playerControlEnabled = false;
		Velocity = Vector2.Zero;		// Stop any existing movement
		foreach (ProjectileWeapon weapons in GetNode<Node2D>("Weapons").GetChildren())
		{
			weapons.WeaponEnabled = false;
		}
		GetNode<CollisionShape2D>("CollisionShape2D").Disabled = true;
		EmitSignal(SignalName.OnPlayerDeath);
	}
	
	// int healthDifference: 	positive if heal
	//							negative if damage
	private void ChangeHealth(int healthDifference)
	{
		_health += healthDifference;
		HealthBar.Value = _health;
		
		if ((_health <= 0))
		{
			PlayerDeath();
		}
	}
	
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
		ChangeHealth(-enemyDamage);
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
		if (_playerControlEnabled)
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
