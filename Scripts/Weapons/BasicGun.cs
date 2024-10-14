using Godot;
using System;

public partial class BasicGun : ProjectileWeapon
{
	
	private void OnAttackTimerTimeout()
	{
		Vector2 up = new Vector2(0, -1);
		Vector2 down = new Vector2(0, 1);
		Vector2 right = new Vector2(1, 0);
		Vector2 left = new Vector2(-1, 0);
		SpawnProjectile(up);
		SpawnProjectile(down);
		SpawnProjectile(right);
		SpawnProjectile(left);
	}
}
