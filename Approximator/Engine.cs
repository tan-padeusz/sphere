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

	private void Approximate(Approximator.DTO.Approximator approximator)
	{
		new Thread(() =>
		{
			var population = new Population(approximator);
			this.GlobalBestIndividual = population.BestIndividual;
			this.LastImprovement = 0;
			this.PopulationsCreated = 1;
			while (this.IsRunning)
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
	
	public bool Start(Approximator.DTO.Approximator approximator)
	{
		if (this.IsRunning) return false;
		this.IsRunning = true;
		this.IsPaused = false;
		this.Approximate(approximator);
		this.Stopwatch.Restart();
		return true;
	}

	public bool Stop()
	{
		if (!this.IsRunning) return false;
		this.IsRunning = false;
		this.IsPaused = false;
		this.Stopwatch.Stop();
		return true;
	}

	public bool Pause()
	{
		if (!this.IsRunning || this.IsPaused) return false;
		this.IsPaused = true;
		this.Stopwatch.Stop();
		return true;
	}

	public bool Resume()
	{
		if (!this.IsRunning || !this.IsPaused) return false;
		this.IsPaused = false;
		this.Stopwatch.Start();
		return true;
	}
}