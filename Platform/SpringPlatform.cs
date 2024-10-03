using Godot;
using System;

public partial class SpringPlatform : Platform
{
    private AnimatedSprite2D anim; // Referência para obter o AnimatedSprite2D

    public override void _Ready()
    {
        ZIndex = 1;
        anim = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
    }

    public void Response()
    {
        anim.Play("default");
    }

    public void OnAnimatedSprite2dAnimationFinished()
    {
        // anim.Frame = 0; // Define o frame que a animação deve parar (por padrão é 0)
        anim.Stop();
    }
}
