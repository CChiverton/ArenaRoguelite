using Godot;
using System;

public partial class MainMenu : Control
{
	Node2D Game;
	[Signal]
	public delegate void StartButtonPressedEventHandler(int weapon);
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}
	
	private void OnStartButtonPressed()
	{
		int weapon = 0;
		foreach(Button button in GetNode<GridContainer>("MarginContainer/HBoxContainer/WeaponSelect").GetChildren())
		{
			if (button.IsPressed()) {break;}
			weapon++;
		}
		EmitSignal(SignalName.StartButtonPressed, weapon);
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
