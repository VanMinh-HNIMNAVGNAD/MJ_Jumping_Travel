using Godot;
using System;

public partial class FireTrap : Trap

{
	private AnimatedSprite2D animatedSprite2D;
	private Timer timer;
	private CollisionShape2D collisionShape2D;
	private bool isOn = false;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		collisionShape2D = GetNode<CollisionShape2D>("CollisionShape2D");
		animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		timer = GetNode<Timer>("Timer");
		timer.Timeout += ToggleFire;
	}

	private void ToggleFire()
	{
		isOn = !isOn;
		collisionShape2D.SetDeferred("disabled", !isOn);
		if (isOn)
		{
			animatedSprite2D.Play("on");
		}
		else
		{
			animatedSprite2D.Play("off");
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
