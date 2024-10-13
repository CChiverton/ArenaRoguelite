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
	private LevelUpScreen _levelUpScreen;
	
	// Loaded Level
	public Player PlayerObject {get; private set;}
	private Node2D _arenaObject;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_mainMenu = GetNode<MainMenu>("MainMenu");
		_mainMenu.StartButtonPressed += StartGame;
		
		_escapeMenu = GetNode<EscapeMenu>("EscapeMenu");
		_escapeMenu.ResumeButtonPressed += ResumeGame;
		_escapeMenu.RestartButtonPressed += RestartGame;
		
		_levelUpScreen = GetNode<LevelUpScreen>("LevelUpScreen");
		_levelUpScreen.DamageUpPressed += DamageUp;
		_levelUpScreen.SpeedUpPressed += SpeedUp;
	}
	
	/********************** Player Signal Handling **********************/
	private void PlayerLevelUp()
	{
		GetTree().Paused = true;
		_levelUpScreen.RandomiseButtons();
		_levelUpScreen.Show();
	}
	
	private void LoadArena()
	{
		_arenaObject = _startGameArena.Instantiate<Node2D>();
		PlayerObject = _startGamePlayer.Instantiate<Player>();
		PlayerObject.LevelUp += PlayerLevelUp; 
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
	
	/*********************** Level Up Screen Functions *********************/
	private void ResumeFromLevelUp()
	{
		_levelUpScreen.Hide();
		GetTree().Paused = false;
	}
	
	private void DamageUp(int damage)
	{
		GD.Print("Adding " + damage + " damage");
		// TODO Add way to increase damage to player
		ResumeFromLevelUp();
	}
	
	private void SpeedUp(float speed)
	{
		GD.Print("Adding " + speed + " speed");
		PlayerObject.LevelUpSpeed(speed);
		ResumeFromLevelUp();
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
