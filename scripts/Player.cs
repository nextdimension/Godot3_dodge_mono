using Godot;
using System;

public class Player : Area2D
{
	[Export] 
	public int SPEED;
	private Vector2 Velocity;
	private Vector2 screenSize;
    private AnimatedSprite Animations;
	private CollisionShape2D Collision;
    
    public override void _Ready()
    {
        AddUserSignal("hit");
        screenSize = GetViewportRect().Size;
        Animations = (AnimatedSprite)GetNode("AnimatedSprite");
		Collision = (CollisionShape2D)GetNode("CollisionShape2D");
        this.Connect("body_entered", this, "OnPlayerBodyEntered");
        Hide(); 
    }
    public override void _Process(float delta)
    {
        Velocity = new Vector2();
        if (Input.IsActionPressed("ui_right")){
            Velocity.x += 1;
        }
        if (Input.IsActionPressed("ui_left"))
        {
            Velocity.x -= 1;
        }
        if (Input.IsActionPressed("ui_up"))
        {
            Velocity.y -= 1;
        }
        if (Input.IsActionPressed("ui_down"))
        {
            Velocity.y += 1;
        }
        if (Velocity.Length() > 0)
        {
            Velocity = Velocity.Normalized() * SPEED;
            Animations.Play();
        }
        else {
           Animations.Stop();
        }

        Position += Velocity * delta;
        Position = new Vector2(Mathf.Clamp(Position.x, 0, screenSize.x), Mathf.Clamp(Position.y, 0, screenSize.y));

        if(Velocity.x != 0) {
            Animations.Animation = "right";
            Animations.FlipV = false;
            Animations.FlipH = Velocity.x < 0;
        } else if(Velocity.y != 0) {
            Animations.Animation = "up";
             Animations.FlipV = Velocity.y > 0;
        }
    }

    private void OnPlayerBodyEntered(CollisionShape2D body) {
        Hide();
        EmitSignal("hit");
        Collision.Disabled = true;
    }

    public void start(Vector2 pos){
        Position = pos;
        Collision.Disabled = false;
        Show();
    }
    
}



