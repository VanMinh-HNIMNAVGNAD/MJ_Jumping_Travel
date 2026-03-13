using Godot;
using System;

public partial class Trap : Area2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.BodyEntered += OnBodyEntered;
	}

	public void OnBodyEntered(Node2D body)
	{
		if (body is Vm)
		{
			GetTree().ReloadCurrentScene();
		}
	}
}
