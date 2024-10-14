using Godot;
using System;

public partial class MainMenu : Control
{
	Node2D Game;
	[Signal]
	public delegate void StartButtonPressedEventHandler();
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}
	
	private void OnStartButtonPressed()
	{
		EmitSignal(SignalName.StartButtonPressed);
		Visible = false;
	}
	
	private void OnQuitButtonPressed()
	{
		GetTree().Quit();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
