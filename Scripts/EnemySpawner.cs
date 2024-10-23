using Godot;
using System;

public partial class EnemySpawner : Node2D
{
	[Export]
	private PackedScene _enemy {get; set;}
	private Node2D _enemiesGroup;
	private Timer _enemySpawnerTimer;
	
	private int _spawnRangeMin = 100;
	private int _maxEnemyCount = 10;
	private int _spawnAtOnce = 1;	// The maximum number of enemies spawned per cooldown
	
	private TileMapLayer _map;
	private Godot.Collections.Array<Vector2I> _spawnableTiles = new Godot.Collections.Array<Vector2I>();
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_enemiesGroup = GetNode<Node2D>("Enemies");
		_enemySpawnerTimer = GetNode<Timer>("EnemySpawnerTimer");
		_map = GetNode<TileMapLayer>("../TileMapLayer");
		_spawnableTiles = _map.GetUsedCellsById(0);
	}
	
	private Vector2 SetSpawnLocation()
	{
		Vector2 spawnLocation;
		do
		{
			spawnLocation = _map.MapToLocal(_spawnableTiles.PickRandom());
		}
		while (spawnLocation.DistanceTo(GetNode<Game>("/root/Game").PlayerObject.GlobalPosition) < _spawnRangeMin);
		return spawnLocation;
	}
	
	private void SpawnEnemy()
	{
		for (int i = 0; i < _spawnAtOnce; i++)
		{
			if (_enemiesGroup.GetChildCount() < _maxEnemyCount)
			{
				Enemy enemy = _enemy.Instantiate<Enemy>();
				enemy.GlobalPosition = SetSpawnLocation();
				_enemiesGroup.AddChild(enemy);
			}
			else
			{
				break;
			}
		}
	}
	
	private void IncreaseDifficulty()
	{
		_enemySpawnerTimer.WaitTime -= 0.2;
		if (_enemySpawnerTimer.WaitTime < 2.5)
		{
			_spawnAtOnce += 1;
			_enemySpawnerTimer.WaitTime = 5.0;
		}
	}

	private void OnEnemySpawnerTimerTimeout()
	{
		IncreaseDifficulty();
		SpawnEnemy();
	}
	
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
