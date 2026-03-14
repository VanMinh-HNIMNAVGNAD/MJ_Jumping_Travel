using Godot;
using System;

public partial class SystemScore : Node
{
	public static SystemScore Instance { get; private set; }
	public int CurrentScore { get; private set; } = 0;

	[Signal] public delegate void ScoreChangedEventHandler(int newScore);
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Instance = this;
	}

	public void AddScore(int amount)
	{
		CurrentScore += amount;
		EmitSignal(SignalName.ScoreChanged, CurrentScore);
	}

	public void ResetScore()
	{
		CurrentScore = 0;
		EmitSignal(SignalName.ScoreChanged, CurrentScore);
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
