using Godot;
using System;
using System.Collections.Generic;

public class Mob : RigidBody2D
{
    [Export]
    public int MIN_SPEED;
    [Export]
    public int MAX_SPEED;
    List<string> mob_types = new List<string>(new string[] {"walk", "fly", "swim"});
    private AnimatedSprite Animations;
    private VisibilityNotifier2D Visibility;

    public override void _Ready()
    {
		 MIN_SPEED = 150;
		 MAX_SPEED = 250;
         Animations = (AnimatedSprite)GetNode("AnimatedSprite");
         Visibility = (VisibilityNotifier2D)GetNode("Visibility");
         Random r = new Random();
         Animations.Animation = mob_types[r.Next(mob_types.Count)];
         Visibility.Connect("screen_exited", this, "OnVisibilityScreenExited");

    }
    private void OnVisibilityScreenExited(){
        QueueFree();
    }
}
