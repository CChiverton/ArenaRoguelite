using Godot;
using System;

public partial class Shotgun : ProjectileWeapon
{
	private void OnAttackTimerTimeout()
	{
		Vector2 CenterLine = GetPlayerCharacterDirection();
		Vector2 LeftProjectile = CenterLine;
		Vector2 LeftmostProjectile = CenterLine;
		Vector2 RightProjectile = CenterLine;
		Vector2 RightmostProjectile = CenterLine;
		
		if (CenterLine.X > 0)	// right
		{
			LeftProjectile += new Vector2(0, -0.1f);
			LeftmostProjectile += new Vector2(0, -0.2f);
			RightProjectile += new Vector2(0, 0.1f);
			RightmostProjectile += new Vector2(0, 0.2f);
		} else  if (CenterLine.X < 0) { // left
			LeftProjectile += new Vector2(0, 0.1f);
			LeftmostProjectile += new Vector2(0, 0.2f);
			RightProjectile += new Vector2(0, -0.1f);
			RightmostProjectile += new Vector2(0, -0.2f);
		}
		if (CenterLine.Y > 0) // down
		{
			LeftProjectile += new Vector2(0.1f, 0);
			LeftmostProjectile += new Vector2(0.2f, 0);
			RightProjectile += new Vector2(-0.1f, 0);
			RightmostProjectile += new Vector2(-0.2f, 0);
		} else if (CenterLine.Y < 0) { // up
			LeftProjectile += new Vector2(-0.1f, 0);
			LeftmostProjectile += new Vector2(-0.2f, 0);
			RightProjectile += new Vector2(0.1f, 0);
			RightmostProjectile += new Vector2(0.2f, 0);
		}
		SpawnProjectile(CenterLine);
		SpawnProjectile(LeftProjectile);
		SpawnProjectile(LeftmostProjectile);
		SpawnProjectile(RightProjectile);
		SpawnProjectile(RightmostProjectile);
	}
}
