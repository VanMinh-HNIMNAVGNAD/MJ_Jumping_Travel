using System;
using Godot;

public partial class Char : CharacterBody2D
{
    [Export]
    public float Speed = 300.0f;

    [Export]
    public float JumpVelocity = -400.0f;

    [Export]
    public float AbyssY = 800.0f;

    [Export]
    public float ReloadY = 780.0f;

    [Export]
    public float DeathFallGravityScale = 1.35f;

    private float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
    private AnimatedSprite2D animatedSprite2D;
    private CollisionShape2D collisionShape2D;
    private bool isDead = false;

    public int JumpCount = 0;

    public override void _Ready()
    {
        animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        collisionShape2D = GetNode<CollisionShape2D>("CollisionShape2D");
    }

    public override void _PhysicsProcess(double delta)
    {

        if (!isDead && GlobalPosition.Y >= AbyssY)
        {
            Die(new Vector2(0, JumpVelocity * 0.45f));
        }

        if (isDead)
        {
            Vector2 deadVelocity = Velocity;
            deadVelocity.Y += gravity * DeathFallGravityScale * (float)delta;
            Velocity = deadVelocity;
            MoveAndSlide();

            if (GlobalPosition.Y >= ReloadY)
            {
                GetTree().ReloadCurrentScene();
            }

            return;
        }

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

    public void Die(Vector2? knockback = null)
    {
        if (isDead)
        {
            return;
        }

        isDead = true;
        JumpCount = 0;

        // Drop collision with ground so the death state can fall out of the screen.
        SetCollisionMaskValue(1, false);
        collisionShape2D.SetDeferred("disabled", true);

        Vector2 defaultKnockback = new Vector2(animatedSprite2D.FlipH ? 110.0f : -110.0f, JumpVelocity * 0.55f);
        Velocity = knockback ?? defaultKnockback;

        if (animatedSprite2D.SpriteFrames.HasAnimation("be_attacked"))
        {
            animatedSprite2D.Play("be_attacked");
        }
    }

    public void UpdateAnimation(float direction)
    {
        if (isDead)
        {
            return;
        }

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
