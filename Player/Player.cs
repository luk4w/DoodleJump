using Godot;
using System;

public partial class Player : CharacterBody2D
{
	public const float Speed = 300.0f;
	public const float JumpVelocity = -500.0f;

	private AnimatedSprite2D anim; // Referência para obter o AnimatedSprite2D
	private Vector2 screenSize; // Referência para obter o tamanho da tela

	public override void _Ready()
	{
		anim = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		screenSize = GetViewport().GetVisibleRect().Size;
		ZIndex = 10; // Define a ordem de renderização do personagem
	}

	public override void _PhysicsProcess(double delta)
	{
		Move(delta);

		// Mathf.Wrap garante que a posição X do personagem permaneça dentro dos limites da tela.
		// Se o personagem ultrapassar a borda direita (800), ele reaparece na esquerda (0), e vice-versa.
		SetPosition(new Vector2(Mathf.Wrap(Position.X, 0, screenSize.X), Position.Y));
		// Vector2 pos = Position;
		// pos.X = Mathf.Wrap(pos.X, 0, screenSize.X);
		// Position = pos;
	}


	public void Move(double delta)
	{
		// Velocidade atual do corpo
		Vector2 velocity = Velocity;

		// Adicionar a gravidade
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		// Comandos de movimentação
		float abscissa = Input.GetAxis("ui_left", "ui_right");
		if (abscissa != 0)
		{
			// Inverte a escala do sprite para a esquerda
			anim.FlipH = abscissa > 0;

			// Mathf.Lerp suaviza a transição entre a velocidade atual e a velocidade desejada.
			// O primeiro parâmetro é o valor atual, o segundo é o valor desejado e o terceiro é a rapidez da interpolação.
			velocity.X = Mathf.Lerp(velocity.X, abscissa * Speed, 0.05f);

			// velocity.X = abscissa * Speed;
		}
		else
		{
			// Análogo ao Mathf.Lerp, mas altera a velocidade a uma taxa constante.
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
		}


		// Atualiza a animação
		if (velocity.Y > 0)
		{
			anim.Play("fall");
		}
		else
		{
			anim.Play("idle");
		}

		KinematicCollision2D collision = MoveAndCollide(Velocity * (float)delta);
		if (collision != null)
		{
			// Verifica se o collider é uma instancia de Platform
			if (collision.GetCollider() is Platform platform)
			{
				velocity.Y = JumpVelocity * platform.VelocityMultiplier;

				// Verifica se a plataforma possui o método Response
				platform.GetType().GetMethod("Response")?.Invoke(platform, null);
			}
		}

		// Atualiza a velocidade do corpo
		Velocity = velocity;
	}
}