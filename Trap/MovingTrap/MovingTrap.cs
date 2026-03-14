using Godot;
using System;

public partial class MovingTrap : Trap
{
	[Export] public float Speed = 100.0f;
	[Export] public Vector2 MoveDirection = Vector2.Right;
	[Export] public float EdgePadding = 0.0f;
	[Export] public float DetectAhead = 2.0f;

	private int direction = 1;
	private RayCast2D wallDetector;
	private AnimatedSprite2D anima2D;
	private Vector2 moveAxis = Vector2.Right;
	private float colliderExtent = 12.0f;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		wallDetector = GetNode<RayCast2D>("WallDetector");
		moveAxis = MoveDirection == Vector2.Zero ? Vector2.Right : MoveDirection.Normalized();
		colliderExtent = EstimateColliderExtent();

		UpdateDetectorDirection();

		if (HasNode("AnimatedSprite2D"))
		{
			anima2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
			anima2D.Play("on");
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		Position += moveAxis * Speed * direction * (float)delta;
		if (wallDetector.IsColliding())
		{
			direction *= -1;
			UpdateDetectorDirection();

			if (anima2D != null)
			{
				if (Mathf.Abs(moveAxis.X) >= Mathf.Abs(moveAxis.Y))
				{
					anima2D.FlipH = direction < 0;
				}
				else
				{
					anima2D.FlipV = direction < 0;
				}
			}
		}
	}

	private void UpdateDetectorDirection()
	{
		float startOffset = colliderExtent + EdgePadding;
		float castDistance = Mathf.Max(DetectAhead, 0.5f);

		wallDetector.Position = moveAxis * startOffset * direction;
		wallDetector.TargetPosition = moveAxis * castDistance * direction;
	}

	private float EstimateColliderExtent()
	{
		CollisionShape2D collisionShape = GetNodeOrNull<CollisionShape2D>("CollisionShape2D");
		if (collisionShape == null || collisionShape.Shape == null)
		{
			return 12.0f;
		}

		float extent;
		if (collisionShape.Shape is CircleShape2D circle)
		{
			extent = circle.Radius;
		}
		else if (collisionShape.Shape is RectangleShape2D rectangle)
		{
			extent = Mathf.Abs(moveAxis.X) >= Mathf.Abs(moveAxis.Y)
				? rectangle.Size.X * 0.5f
				: rectangle.Size.Y * 0.5f;
		}
		else if (collisionShape.Shape is CapsuleShape2D capsule)
		{
			extent = Mathf.Abs(moveAxis.X) >= Mathf.Abs(moveAxis.Y)
				? capsule.Radius
				: capsule.Height * 0.5f;
		}
		else
		{
			extent = 12.0f;
		}

		float axisScale = Mathf.Abs(moveAxis.X) >= Mathf.Abs(moveAxis.Y)
			? Mathf.Abs(collisionShape.Scale.X)
			: Mathf.Abs(collisionShape.Scale.Y);

		if (axisScale <= 0.0001f)
		{
			axisScale = 1.0f;
		}

		return extent * axisScale;
	}
}
