using Godot;
using System;

public partial class Enemy : CharacterBody2D
{
	private int _health = 30;
	private int _collisionDamage = 10;
	private const float _speed = 50.0f;
	Node2D Player;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Player = GetNode<CharacterBody2D>("/root/Game/Player");
	}
	
	private void TakeDamage(int damage)
	{
		_health -= damage;
		if ((_health <= 0))
		{
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
		if (Player != null) 
		{
			Velocity = GlobalPosition.DirectionTo(Player.GlobalPosition) * _speed;
			MoveAndSlide();
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		Movement();
	}
}
