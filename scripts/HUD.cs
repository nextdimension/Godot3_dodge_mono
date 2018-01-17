using Godot;
using System;

public class HUD : CanvasLayer
{
    Label ScoreLabel;
    Label MessageLabel;
    Button StartButton;
    Timer MessageTimer;

    public override void _Ready()
    {
        AddUserSignal("start_game");
        ScoreLabel = (Label)GetNode("ScoreLabel");
        MessageLabel = (Label)GetNode("MessageLabel");
        StartButton = (Button)GetNode("StartButton");
        MessageTimer = (Timer)GetNode("MessageTimer");

        StartButton.Connect("pressed", this, "OnStartButtonPressed");
        MessageTimer.Connect("timeout",this, "OnMessageTimerTimeout");
    }
    public void show_message(String text){
        MessageLabel.Text = text;
        MessageLabel.Show();
        MessageTimer.Start();
    }
    public async void ShowGameOver(){
        show_message("Game Over");
        await ToSignal(MessageTimer, "timeout");
        StartButton.Show();
        MessageLabel.Text = "Dodge the\nCreeps!";
        MessageLabel.Show();
    }
    public void update_score(int score){
        ScoreLabel.Text = score.ToString();
    }
    public void OnStartButtonPressed(){
        StartButton.Hide();
        MessageLabel.Hide();
        EmitSignal("start_game");    
    }
    public void OnMessageTimerTimeout(){
        MessageLabel.Hide();
    }
}
