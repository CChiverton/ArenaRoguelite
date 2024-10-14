using Godot;
using System;

public partial class BasicGun : Node2D
{
	[Export]
	private int _bulletSpeed {get; set;} = 200;
	[Export]
	private int _bulletDamage {get;set;} = 10;
	public float _damageMultiplier = 1.0f;
	[Export]
	private PackedScene _bullet {get; set;}
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}
	
	private void SpawnBullet(Vector2 direction)
	{
		Bullet attackBullet = _bullet.Instantiate<Bullet>();
		attackBullet.GlobalPosition = GlobalPosition;		// Sets position as bullet doesn't inherit gun position
		attackBullet.Speed = _bulletSpeed;
		attackBullet.Damage = (int)(_bulletDamage * _damageMultiplier);
		attackBullet.Direction = Position.DirectionTo(direction);
		GetNode<Node>("Bullets").AddChild(attackBullet);
	}
	
	private void OnAttackTimerTimeout()
	{
		Vector2 up = new Vector2(0, -1);
		Vector2 down = new Vector2(0, 1);
		Vector2 right = new Vector2(1, 0);
		Vector2 left = new Vector2(-1, 0);
		SpawnBullet(up);
		SpawnBullet(down);
		SpawnBullet(right);
		SpawnBullet(left);
	}
	

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
