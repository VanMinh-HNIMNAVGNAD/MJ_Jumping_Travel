using System;
using Godot;

public partial class Vm : CharacterBody2D
{
    [Export]
    public float Speed = 300.0f;

    [Export]
    public float JumpVelocity = -400.0f;

    private float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
    private AnimatedSprite2D animatedSprite2D;

    public int JumpCount = 0;

    public override void _Ready()
    {
        animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
    }

    public override void _PhysicsProcess(double delta)
    {

        Vector2 velocity = Velocity;
        if (IsOnFloor())
        {
            JumpCount = 0;
        }
        if (!IsOnFloor())
        {
            velocity.Y += gravity * (float)delta;
        }
        if (Input.IsActionJustPressed("jump") && JumpCount < 2)
        {
            JumpCount++;
            velocity.Y = JumpVelocity;
        }
        float direction = Input.GetAxis("move_left", "move_right");
        if (direction != 0)
        {
            velocity.X = direction * Speed;
            animatedSprite2D.FlipH = direction < 0;
        }
        else
        {
            velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed * (float)delta);
        }
        Velocity = velocity;
        MoveAndSlide();
        UpdateAnimation(direction);
    }

    public void UpdateAnimation(float direction)
    {
        if (!IsOnFloor())
        {
            if (Velocity.Y < 0)
            {
                if (JumpCount == 1)
                {
                    animatedSprite2D.Play("jump");
                }
                else if (JumpCount > 1)
                {
                    animatedSprite2D.Play("double_jump");
                }
            }

                
            else
                animatedSprite2D.Play("fall");
        }
        else
        {
            if (direction != 0)
            {
                animatedSprite2D.Play("run");
            }
            else
            {
                animatedSprite2D.Play("idle");
            }
        }
    }
}
