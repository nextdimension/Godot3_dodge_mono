using Godot;
using System;

public class Main : Node
{
    public PackedScene Mob;
    private int Score;
    private Player Player;
    private Timer StartTimer;   
    private Timer ScoreTimer;
    private Timer MobTimer;
    private Position2D StartPosition;
    private Path2D Path;
    private PathFollow2D PathFollow;
    private HUD Hud;
    private AudioStreamPlayer Music;
    private AudioStreamPlayer DeathSound;
    public override void _Ready()
    {
        Hud = (HUD)GetNode("HUD");
        Player = (Player)GetNode("Player");
        Mob = (PackedScene)ResourceLoader.Load("res://scenes/Mob.tscn");
        StartTimer = (Timer)GetNode("StartTimer");
        ScoreTimer = (Timer)GetNode("ScoreTimer");
        MobTimer = (Timer)GetNode("MobTimer");
        StartPosition = (Position2D)GetNode("StartPosition");
        Path = (Path2D)GetNode("MobPath");
        PathFollow  =   (PathFollow2D)Path.GetNode("MobSpawnLocation");
        Music = (AudioStreamPlayer)GetNode("Music");
        DeathSound = (AudioStreamPlayer)GetNode("DeathSound");

        Player.Connect("hit", this, "GameOver");
        StartTimer.Connect("timeout", this, "OnStartTimerTimeout");
        ScoreTimer.Connect("timeout", this, "OnScoreTimerTimeout");
        MobTimer.Connect("timeout", this, "OnMobTimerTimeout");
        Hud.Connect("start_game", this, "NewGame");
        Hud.update_score(Score);
        Hud.show_message("Get Ready");
    }
    private void OnMobTimerTimeout(){
        Random r = new Random();
        PathFollow.SetOffset(NextFloat(r));
        var mob =  (Mob)Mob.Instance();
        AddChild(mob);
        float direction = PathFollow.Rotation + Mathf.PI/2;
        mob.Position = PathFollow.Position;
        //direction += RandomFloat(-Mathf.PI/4, Mathf.PI/4, r);
        mob.Rotation = direction;
        mob.SetLinearVelocity(new Vector2(r.Next(mob.MIN_SPEED,mob.MAX_SPEED),0).Rotated(direction));
        
    }
    private void OnScoreTimerTimeout(){
        Score +=1;
        Hud.update_score(Score);
    }
    private void OnStartTimerTimeout(){
        MobTimer.Start();
        ScoreTimer.Start();
    }

    private void GameOver(){
        ScoreTimer.Stop();
        MobTimer.Stop();
        Hud.ShowGameOver();
        DeathSound.Play();
        Music.Stop();
    }

    private void NewGame(){
        Score = 0;
        Player.start(StartPosition.Position);
        StartTimer.Start();
        Music.Play();
    }

    static float NextFloat(Random random)
    {
        double mantissa = (random.NextDouble() * 2.0) - 1.0;
        double exponent = Math.Pow(2.0, random.Next(-126, 128));
        return (float)(mantissa * exponent);
    }

    static float RandomFloat(float a, float b, Random rand) {
        float random = NextFloat(rand);
        float diff = b - a;
        float r = random * diff;
        return a + r;
    }
    
}