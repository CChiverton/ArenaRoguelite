using Godot;
using System;

public partial class EnemySpawner : Node2D
{
	[Export]
	private PackedScene _enemy {get; set;}
	private Node2D _enemiesGroup;
	
	private int _maxEnemyCount = 10;
	
	private TileMapLayer _map;
	private Godot.Collections.Array<Vector2I> _spawnableTiles = new Godot.Collections.Array<Vector2I>();
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_enemiesGroup = GetNode<Node2D>("Enemies");
		_map = GetNode<TileMapLayer>("../TileMapLayer");
		_spawnableTiles = _map.GetUsedCellsById(0);
	}
	
	private Vector2 SetSpawnLocation()
	{
		// TODO Add logic to spawn a a minimum and maximum distance away from the character
		return _map.MapToLocal(_spawnableTiles.PickRandom());
	}
	
	private void SpawnEnemy()
	{
		if (_enemiesGroup.GetChildCount() < _maxEnemyCount)
		{
			Enemy enemy = _enemy.Instantiate<Enemy>();
			enemy.GlobalPosition = SetSpawnLocation();
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
