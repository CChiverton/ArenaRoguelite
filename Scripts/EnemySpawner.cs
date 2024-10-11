using Godot;
using System;

public partial class EnemySpawner : Node2D
{
	[Export]
	private PackedScene _enemy {get; set;}
	private Node2D _enemiesGroup;
	
	private int _maxEnemyCount = 10;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_enemiesGroup = GetNode<Node2D>("Enemies");
	}
	
	private void SpawnEnemy()
	{
		if (_enemiesGroup.GetChildCount() < _maxEnemyCount)
		{
			Enemy enemy = _enemy.Instantiate<Enemy>();
			_enemiesGroup.AddChild(enemy);
		}
	}

	private void OnEnemySpawnerTimerTimeout()
	{
		SpawnEnemy();
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
