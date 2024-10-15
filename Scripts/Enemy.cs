using Godot;
using System;

public partial class Enemy : CharacterBody2D
{
	private int _health = 30;
	private int _collisionDamage = 10;
	private const float _speed = 50.0f;
	private int _experienceValue = 1;
	Node2D PlayerObject;
	private bool _movementEnabled = false;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		PlayerObject = GetNode<Game>("/root/Game").PlayerObject;
	}
	
	private void TakeDamage(int damage)
	{
		_health -= damage;
		if ((_health <= 0))
		{
			PlayerObject.Call("AddExperience", _experienceValue);
			QueueFree();
		}
	}
	
	// Handles enemy behaviour when colliding with the player
	private int PlayerCollision()
	{
		return _collisionDamage;
	}
	
	private void Movement()
	{
		if ((PlayerObject != null) && _movementEnabled) 
		{
			Velocity = GlobalPosition.DirectionTo(PlayerObject.GlobalPosition) * _speed;
			MoveAndSlide();
		}
	}
	
	private void OnSpawnAnimationFinished()
	{
		GD.Print("Animation finished");
		_movementEnabled = true;
	}

	public override void _PhysicsProcess(double delta)
	{
		Movement();
	}
}
