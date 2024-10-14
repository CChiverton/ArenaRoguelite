using Godot;
using System;

public partial class EscapeMenu : CanvasLayer
{
	[Signal]
	public delegate void ResumeButtonPressedEventHandler();
	[Signal]
	public delegate void RestartButtonPressedEventHandler();
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Visible = false;
	}
	
	private void OnResumeButtonPressed()
	{
		Visible = false;
		EmitSignal(SignalName.ResumeButtonPressed);
	}
	
	private void OnRestartButtonPressed()
	{
		EmitSignal(SignalName.RestartButtonPressed);
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
