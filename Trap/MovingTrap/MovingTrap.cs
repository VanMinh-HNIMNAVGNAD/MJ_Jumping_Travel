using Godot;
using System;

public partial class MovingTrap : Trap
{
		[Export] public float Speed = 100.0f;
		private int direction = 1;
		private RayCast2D wallDetector;
		private AnimatedSprite2D anima2D;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		wallDetector = GetNode<RayCast2D>("WallDetector");
		if (HasNode("AnimatedSprite2D"))
		{
			anima2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
			anima2D.Play("on");
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		Position += new Vector2(Speed * direction * (float)delta, 0);
		if (wallDetector.IsColliding())
		{
			direction *= -1;
			wallDetector.TargetPosition = new Vector2(wallDetector.TargetPosition.X * -1, 0);
			if (anima2D != null)
			{
				anima2D.FlipH = direction < 0;
			}
		}
	}
}
