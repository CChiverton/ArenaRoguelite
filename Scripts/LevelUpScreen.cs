using Godot;
using System;

public partial class LevelUpScreen : CanvasLayer
{
	[Signal]
	public delegate void DamageUpPressedEventHandler(int damage);
	[Signal]
	public delegate void SpeedUpPressedEventHandler(float speed);
	
	private RandomNumberGenerator _random;
	
	private enum UpgradeType {
		Damage,
		Speed
	}
	
	private struct Upgrade 
	{
		public Button button {get; set;}
		public UpgradeType upgradeType {get; set;}
		public float upgradeValue {get; set;}
	}
	
	private Upgrade _firstButton;
	private Upgrade _secondButton;
	private Upgrade _thirdButton; 
	
	// TODO Change so that it is not passed by reference. Bad practice?
	private void RandomiseButton(ref Upgrade button)
	{
		UpgradeType upgrade = (UpgradeType)_random.RandiRange((int)UpgradeType.Damage, (int)UpgradeType.Speed);
		switch(upgrade)
		{
			case UpgradeType.Damage:
				button.upgradeValue = _random.RandiRange(1, 5);
				button.button.Text = "Damage: +" + button.upgradeValue;
				break;
			case UpgradeType.Speed:
				button.upgradeValue = (float)_random.RandiRange(10, 50) / 10;
				button.button.Text = "Speed: +" + button.upgradeValue;
				break;
			default:
				GD.Print("Switch statement incorrect");
				break;
		}
		button.upgradeType = upgrade;
	}
	
	public void RandomiseButtons()
	{
		RandomiseButton(ref _firstButton);
		RandomiseButton(ref _secondButton);
		RandomiseButton(ref _thirdButton);
	}
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_random = new RandomNumberGenerator();
		_firstButton.button = GetNode<Button>("MarginContainer/CenterContainer/HBoxContainer/FirstButton");
		_secondButton.button = GetNode<Button>("MarginContainer/CenterContainer/HBoxContainer/SecondButton");
		_thirdButton.button = GetNode<Button>("MarginContainer/CenterContainer/HBoxContainer/ThirdButton");
		
		//RandomiseButtons();
	}
	
	private void ButtonSignal(Upgrade button)
	{
		switch(button.upgradeType)
		{
			case UpgradeType.Damage:
				EmitSignal(SignalName.DamageUpPressed, (int)button.upgradeValue);
				break;
			case UpgradeType.Speed:
				EmitSignal(SignalName.SpeedUpPressed, button.upgradeValue);
				break;
			default:
				GD.Print("Signal Switch Error");
				break;
		}
	}

	private void OnFirstButtonPressed()
	{
		ButtonSignal(_firstButton);
	}
	
	private void OnSecondButtonPressed()
	{
		ButtonSignal(_secondButton);
	}

	private void OnThirdButtonPressed()
	{
		ButtonSignal(_thirdButton);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
