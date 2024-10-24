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
	public PackedScene PlayerWeapon {get; private set;}
	 
	// Player Weapons
	[Export]
	private PackedScene _pistol {get; set;}
	[Export]
	private PackedScene _shotgun {get; set;}
	[Export]
	private PackedScene _basicGun {get; set;}
	
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
		PlayerObject.OnPlayerDeath += PlayerDeath;
		AddChild(_arenaObject);
		AddChild(PlayerObject);
	}
	
	/********************** Main Menu Functions ********************/
	private void StartGame(int weapon)
	{
		switch(weapon)
		{
			case 0:
				PlayerWeapon = _pistol;
				break;
			case 1:
				PlayerWeapon = _shotgun;
				break;
			case 2:
				PlayerWeapon = _basicGun;
				break;
			default:
				GD.Print("Error in selecting player weapon");
				break;
		}
		
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
		PlayerObject.LevelUpDamageMultiplier(damage);
		ResumeFromLevelUp();
	}
	
	private void SpeedUp(float speed)
	{
		PlayerObject.LevelUpSpeed(speed);
		ResumeFromLevelUp();
	}
	
	private void PlayerDeath()
	{
		RestartGame();
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
