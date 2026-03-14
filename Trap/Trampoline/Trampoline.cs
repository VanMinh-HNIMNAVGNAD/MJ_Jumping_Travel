using Godot;
using System;

public partial class Trampoline : Area2D
{
	[Export] public float JumpBoost = -500.0f;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
		this.BodyEntered += OnBodyEntered;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnBodyEntered(Node2D body)
	{
		if (body is Vm vm)
		{
			vm.Velocity = new Vector2(vm.Velocity.X, JumpBoost);
			GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("jump");
		}
	}
}
