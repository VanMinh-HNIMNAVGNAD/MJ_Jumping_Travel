using Godot;
using System;

public partial class Collectible : Area2D
{
	[Export] public int ScoreValue = 10;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	private void OnBodyEntered(Node2D body)
	{
		if (body.IsInGroup("Player"))
		{
			SystemScore.Instance.AddScore(ScoreValue);
			GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
			GetNode<AnimatedSprite2D>("AnimatedSprite2D").Hide();
			GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("Effect");
			QueueFree();
			// Optionally, you could also emit a signal here to notify other parts of the game about the collection event.
			// EmitSignal(SignalName.Collected, ScoreValue);
			// You could also add some visual or audio feedback here, such as playing a sound effect or spawning a particle effect to enhance the player's experience when collecting the item.
			
		}
		{
			
		}
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
	}
}
