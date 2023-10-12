using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Reflection.Metadata;
using Vector2 = Godot.Vector2;

public partial class main : Node2D
{
	[Export]
	public Rect2 Bounds = new Rect2(50, 50, 500, 500);
	public float ParticleRadius = 5.0f;
	public float CollisionDampening = 0.1f;
	
	// an array of positions for the particles
	
	public List<Godot.Vector2> Positions = new List<Godot.Vector2>();
	public List<Godot.Vector2> Velocities = new List<Godot.Vector2>();
	
	private float _gravity = 98f;
	private Vector2 _halfBoundsSize = Vector2.Zero;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GD.Print("screen size: " + ScreenBoundsinWorldUnits());
		// create a single particle
		CreateParticle(Bounds.Position + Bounds.Size / 2, Vector2.Zero);
	}
	// draw function
	public override void _Draw()
	{
		DrawRect(Bounds, Colors.Yellow, false, 2.0f);
		DrawParticles();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		_halfBoundsSize = Bounds.Size / 2 - Vector2.One * ParticleRadius + Bounds.GetCenter();
		UpdateParticles((float) delta);
		QueueRedraw();
	}

	private Vector2 ScreenBoundsinWorldUnits()
	{
		// get the screen size
		var screenSize = GetViewportRect().Size;
		return screenSize;
	}
	
	private void CreateParticle(Vector2 position, Vector2 velocity)
	{
		Positions.Add(position);
		Velocities.Add(velocity);
	}

	private void UpdateParticles(float delta)
	{
		for(int i=0; i<Positions.Count; i++)
		{
			UpdateParticle(i, delta);
		}
	}
	
	private void UpdateParticle(int index, float delta)
	{
		 Velocities[index] += Vector2.Down * _gravity * delta;
		 Positions[index] += Velocities[index] * delta;

		 if(Math.Abs(Positions[index].X) > _halfBoundsSize.X)
		 {
			 float sign = Math.Sign(Positions[index].X);
			 Positions[index] = new Vector2(_halfBoundsSize.X * sign, Positions[index].Y);
			 Velocities[index] = new Vector2(Velocities[index].X * -1 * (1.0f - CollisionDampening), Velocities[index].Y);
		 }
		 if (Math.Abs(Positions[index].Y) > _halfBoundsSize.Y)
		 {
			 float sign = Math.Sign(Positions[index].Y);
			 Positions[index] = new Vector2(Positions[index].X, (_halfBoundsSize.Y * sign));
			 Velocities[index] = new Vector2(Velocities[index].X, Velocities[index].Y * -1.0f * (1.0f - CollisionDampening));
			 
		 }
	}
	
	private void DrawParticles()
	{
		for (int i = 0; i < Positions.Count; i++)
		{
			DrawCircle(Positions[i], ParticleRadius, Colors.LightBlue);
		}
	}
}
