using Godot;
using System;
using System.Collections.Generic;

public partial class HunterPistol : ProjectileWeapon
{
	private List<Node2D> _enemiesInRange = new List<Node2D>();
	private int _attackRange = 150;
	private Node2D _closestEnemy = null;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		CircleShape2D AttackRange = (CircleShape2D)GetNode<CollisionShape2D>("AttackRange/AttackRangeCollider").Shape;
		AttackRange.SetRadius(_attackRange);
	}
	
	private void OnAttackRangeBodyEntered(Node2D body)
	{
		_enemiesInRange.Add(body);
	}
	
	private void OnAttackRangeBodyExited(Node2D body)
	{
		_enemiesInRange.Remove(body);
		if (body == _closestEnemy)
		{
			_closestEnemy = null;
		}
	}
	
	private void TargetClosestEnemy()
	{
		if (_enemiesInRange.Count > 0)
		{
			Node2D closestEnemy = null;
			float closestDistance = _attackRange * 2;  // Distance to the centre of an enemy is greater than to the edge

			foreach (Node2D enemy in _enemiesInRange)
			{
				float distance = GlobalPosition.DistanceTo(enemy.GlobalPosition);
				if (distance < closestDistance)
				{
					closestDistance = distance;
					closestEnemy = enemy;
				}
			}
			_closestEnemy = closestEnemy;
		}
	}
	
	private void OnAttackTimerTimeout()
	{
		TargetClosestEnemy();
		if (_closestEnemy != null)
		{ // @ BUG After moving from false to true, the first shot will shoot two projectiles
			// Could be played off as "Quickdraw"
			SpawnProjectile(GlobalPosition.DirectionTo(_closestEnemy.GlobalPosition));
			GetNode<Timer>("AttackTimer").WaitTime = 0.7;  // TODO Set this variable in code and have other weapons operate similarly
		} else {
			GetNode<Timer>("AttackTimer").WaitTime = 0.02;	// Equivalent to PhysicsProcess
		}
	}
}
