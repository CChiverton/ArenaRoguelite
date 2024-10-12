using Godot;
using System;

public partial class Game : Node2D
{
	[Export]
	private PackedScene _startGameArena {get; set;}
	[Export]
	private PackedScene _startGamePlayer {get; set;}
	
	private MainMenu _mainMenu;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_mainMenu = GetNode<MainMenu>("MainMenu");
		_mainMenu.StartButtonPressed += StartGame;
	}
	
	public void StartGame()
	{
		var arena = _startGameArena.Instantiate();
		var player = _startGamePlayer.Instantiate();
		AddChild(arena);
		AddChild(player);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
