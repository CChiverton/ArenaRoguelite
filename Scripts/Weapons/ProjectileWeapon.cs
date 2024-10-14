using Godot;
using System;

public partial class ProjectileWeapon : Node2D
{
	[Export]
	private int _projectileDamage {get;set;}		// Derived class, same assembly
	public float _damageMultiplier = 1.0f;
	[Export]
	private int _projectileSpeed {get; set;}
	[Export]
	private PackedScene _projectile {get; set;}
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}
	
	private protected void SpawnProjectile(Vector2 direction)
	{
		Projectile attackProjectile = _projectile.Instantiate<Projectile>();
		attackProjectile.GlobalPosition = GlobalPosition;		// Sets position as bullet doesn't inherit gun position
		attackProjectile.Speed = _projectileSpeed;
		attackProjectile.Damage = (int)(_projectileDamage * _damageMultiplier);
		attackProjectile.Direction = Position.DirectionTo(direction);
		GetNode<Node>("Projectiles").AddChild(attackProjectile);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
