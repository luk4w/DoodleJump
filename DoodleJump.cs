using Godot;
using System;

public partial class DoodleJump : Node2D
{
	private Node platformContainer;
	private float initialPositionY;
	private Label scoreLabel;
	private bool lastPlatformIsHazard = false;
	private int score = 0;

	// Array de plataformas
	[Export]
	PackedScene[] packedScenes;

	public override void _Ready()
	{
		platformContainer = GetNode<Node>("PlatformContainer");
		initialPositionY = platformContainer.GetNode<Node2D>("Platform").Position.Y;
		scoreLabel = GetNode<Label>("Camera/ScoreLabel");

		Camera2D cam = GetNode<Camera2D>("Camera");
		cam.Connect("ScoreUpdate", new Callable(this, nameof(ScoreUpdate)));

		GD.Randomize();
		LevelGenerator(10);
	}

	public void LevelGenerator(int amount)
	{
		for (int i = 0; i < amount; i++)
		{
			var index = GD.Randi() % packedScenes.Length;

			initialPositionY -= GD.RandRange(36, 54);
			// StaticBody2D platform = packedScene.Instantiate() as StaticBody2D;
			StaticBody2D platform = null;
			switch (index)
			{
				case 0: // Platform
					platform = packedScenes[0].Instantiate() as StaticBody2D;
					break;
				case 1: // SpringPlatform
					platform = packedScenes[1].Instantiate() as StaticBody2D;
					break;
				case 2: // CloudPlatform
				case 3: // Enemy
					if (!lastPlatformIsHazard)
					{
						platform = packedScenes[index].Instantiate() as StaticBody2D;
						// Conecta o sinal "DeleteObject" de Platform ao método DeleteObject
						platform.Connect("DeleteObject", new Callable(this, nameof(DeleteObject)));
						lastPlatformIsHazard = true;
					}
					else
					{
						platform = packedScenes[0].Instantiate() as StaticBody2D; // Platform
						lastPlatformIsHazard = false;
					}
					break;
				default:
					GD.PrintErr("Platform not implemented");
					break;
			}

			if (platform != null)
			{
				platform.Position = new Vector2(GD.RandRange(20, 160), initialPositionY);
				platformContainer.AddChild(platform);
			}
			else
			{
				GD.PrintErr("Platform is null");
			}
		}
	}

	public void DeleteObject(Node obj)
	{
		// Remove o objeto da cena
		obj.QueueFree();
		// Adiciona um delay para chamar o método LevelGenerator
		CallDeferred(nameof(LevelGenerator), 1);
	}

	public void OnPlatformCleanerBodyEntered(Node body)
	{
		// if (body is Platform)
		// {
		// 	DeleteObject(body);
		// }
		// else if (body is Player)
		// {
		// 	GD.Print("Player morreu");
		// }

		if (body.IsInGroup("Platform"))
		{
			DeleteObject(body);
		}
		else if (body is Player player)
		{
			player.Die();
		}
	}

	public void ScoreUpdate()
	{
		score++;
		scoreLabel.Text = score.ToString();
	}

}
