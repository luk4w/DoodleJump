using Godot;
using System;

public partial class Platform : StaticBody2D
{
	[Export]
	public float VelocityMultiplier { get; private set; } = 0.0f;

	[Signal]
	public delegate void DeleteObjectEventHandler();
	

}
