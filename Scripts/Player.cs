using Godot;
using System;

public partial class Player : CharacterBody2D
{
	private const float _speed = 100.0f;
	
	private void Movement()
	{
		Vector2 Speed = Velocity;

		// Get the input direction and handle the movement/deceleration.
		Vector2 Direction = Input.GetVector("MoveLeft", "MoveRight", "MoveUp", "MoveDown");
		if (Direction != Vector2.Zero)
		{
			Speed.X = Direction.X * _speed;
			Speed.Y = Direction.Y * _speed;
		}
		else
		{
			Speed.X = Mathf.MoveToward(Velocity.X, 0, _speed);
			Speed.Y = Mathf.MoveToward(Velocity.Y, 0, _speed);
		}

		Velocity = Speed;
		MoveAndSlide();
	}

	public override void _PhysicsProcess(double delta)
	{
		Movement();
	}
}
