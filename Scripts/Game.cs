using Godot;
using System;

public partial class Game : Node2D
{
	[Export]
	private PackedScene _startGameArena {get; set;}
	[Export]
	private PackedScene _startGamePlayer {get; set;}
	
	private MainMenu _mainMenu;
	private EscapeMenu _escapeMenu;
	
	// Loaded Level
	public Node2D PlayerObject {get; private set;}
	private Node2D _arenaObject;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_mainMenu = GetNode<MainMenu>("MainMenu");
		_mainMenu.StartButtonPressed += StartGame;
		
		_escapeMenu = GetNode<EscapeMenu>("EscapeMenu");
		_escapeMenu.ResumeButtonPressed += ResumeGame;
		_escapeMenu.RestartButtonPressed += RestartGame;
	}
	
	private void LoadArena()
	{
		_arenaObject = _startGameArena.Instantiate<Node2D>();
		PlayerObject = _startGamePlayer.Instantiate<Player>();
		AddChild(_arenaObject);
		AddChild(PlayerObject);
	}
	
	/********************** Main Menu Functions ********************/
	private void StartGame()
	{
		LoadArena();
	}
	
	/********************* Escape Menu Functions ********************/
	private void ResumeGame()
	{
		_escapeMenu.Hide();
		GetTree().Paused = false;
	}
	
	private void RestartGame()
	{
		_arenaObject.QueueFree();
		PlayerObject.QueueFree();
		LoadArena();
		ResumeGame();
	}
	
	
	private void GetInput()
	{
		if (Input.IsActionJustPressed("Escape"))
		{
			GetTree().Paused = true;
			_escapeMenu.Show();
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		GetInput();
	}
}
