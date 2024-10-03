using Godot;
using System;

public partial class Camera : Camera2D
{

	[Export]
	private CharacterBody2D player;
	// Called when the node enters the scene tree for the first time.

	[Signal]
	public delegate void ScoreUpdateEventHandler();

	private float previousPosition;

	public override void _Ready()
	{
		previousPosition = Position.Y;
		ZIndex = 2;
	}

	public override void _Process(double delta)
	{
		if (player != null)
		{

			float currentPosition = Position.Y;  // Posição atual da câmera

			// Verifica se a posição mudou
			if (currentPosition <= previousPosition-80)
			{
				EmitSignal("ScoreUpdate");  // Emite o sinal ScoreUpdate
				previousPosition = currentPosition;  // Atualiza a posição anterior
			}

			if (player.Position.Y < Position.Y)
			{
				Position = new Vector2(Position.X, player.Position.Y);
			}
		}
		else
		{
			throw new Exception("Player Object not found");
		}
	}

}
