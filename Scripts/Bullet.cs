using Godot;
using System;

public partial class Bullet : Area2D
{
	public int Damage = 0;
	public Vector2 Direction = Vector2.Zero;
	public int Speed = 0;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}
	
	private void OnBodyEntered(Node2D body)
	{
		if (body.IsInGroup("Enemies"))
		{
			if (body.HasMethod("TakeDamage"))
			{
				body.Call("TakeDamage", Damage);
			}
		}
		QueueFree();	// All interactions are dealt with, destroy the bullet
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		Position += (float)delta * Speed * Direction;
	}
	
	private void OnVisibleOnScreenNotifier2DScreenExited()
	{
		QueueFree();
	}
}
