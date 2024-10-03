using Godot;
using System;

public partial class Enemy : Platform
{
	private Vector2 direction = Vector2.Right;
	private Vector2 velocity = Vector2.Zero;

	[Export]
	private float speed = 100;
	private float screenWidth;

	private AnimatedSprite2D anim; // Declare o AnimatedSprite2D

	public override void _Ready()
	{
		screenWidth = GetViewport().GetVisibleRect().Size.X;
		anim = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		anim.Play("default");
		anim.FlipH = true;
	}

	public override void _Process(double delta)
	{
		Movement(delta);
	}

	public void Movement(double delta)
	{
		velocity = direction * speed;
		Position += velocity * (float)delta;

		if (Position.X >= screenWidth || Position.X <= 0)
		{
			direction *= -1;
			anim.FlipH = !anim.FlipH;
		}
	}

	public void OnHitboxBodyEntered(Node body)
	{
		if (body is Player player)
		{
			player.Die();
		}
	}



	public void Response()
	{
		EmitSignal("DeleteObject", this);
	}
}
