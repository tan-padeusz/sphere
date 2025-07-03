using System.Diagnostics;
using Approximator.DTO;
using Approximator.Genetics;

namespace Approximator;

public class Engine
{
	private bool IsRunning { get; set; }
	private bool IsPaused { get; set; }
	
	private Stopwatch Stopwatch { get; } = new Stopwatch();
	
	private Individual? GlobalBestIndividual { get; set; }
	private long LastImprovement { get; set; }
	private long PopulationsCreated { get; set; }
	
	public Result Result { get; private set; } = Result.Default;
	public string Status { get; private set; } = "STOPPED";

	private void Approximate(Approximator.DTO.Approximator approximator)
	{
		new Thread(() =>
		{
			var population = new Population(approximator);
			this.GlobalBestIndividual = population.BestIndividual;
			this.LastImprovement = 0;
			this.PopulationsCreated = 1;
			while (this.IsRunning && this.GlobalBestIndividual.DisplayError > 0)
			{
				while (this.IsPaused)
				{
					Thread.Sleep(50);
				}
				
				population = new Population(approximator, population);
				if (population.BestIndividual.IsBetterThan(this.GlobalBestIndividual))
				{
					this.GlobalBestIndividual = population.BestIndividual;
					this.LastImprovement = population.Id;
				}
				this.PopulationsCreated++;
				
				this.UpdateResult();
			}
		}).Start();
	}
	
	private void UpdateResult()
	{
		this.Result = new Result.Builder()
			.SetElapsedTime(this.Stopwatch.ElapsedMilliseconds)
			.SetError(this.GlobalBestIndividual!.DisplayError)
			.SetLastImprovement(this.LastImprovement)
			.SetPopulationsCreated(this.PopulationsCreated)
			.SetOutcome(this.GlobalBestIndividual!.Outcome)
			.Build();
	}
	
	public void Start(Approximator.DTO.Approximator approximator)
	{
		if (this.IsRunning) return;
		this.IsRunning = true;
		this.IsPaused = false;
		this.Approximate(approximator);
		this.Stopwatch.Restart();
		this.Status = "RUNNING";
	}

	public void Stop()
	{
		if (!this.IsRunning) return;
		this.IsRunning = false;
		this.IsPaused = false;
		this.Stopwatch.Stop();
		this.Status = "STOPPED";
	}

	public void Pause()
	{
		if (!this.IsRunning || this.IsPaused) return;
		this.IsPaused = true;
		this.Stopwatch.Stop();
		this.Status = "PAUSED";
	}

	public void Resume()
	{
		if (!this.IsRunning || !this.IsPaused) return;
		this.IsPaused = false;
		this.Stopwatch.Start();
		this.Status = "RUNNING";
	}
}