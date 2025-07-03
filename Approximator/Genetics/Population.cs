using Approximator.Utils;

namespace Approximator.Genetics;

using DTO;

public class Population
{
	public Individual BestIndividual { get; }
	public long Id { get; }
	private Individual[] Individuals { get; }
	private static Func<Approximator, Population?, Individual> CreateIndividual { get; set; } = Population.CreateRandomIndividual;

	public Population(Approximator approximator)
	{
		Population.CreateIndividual = Population.CreateRandomIndividual;
		this.Id = 0;
		this.Individuals = Population.CreatePopulation(approximator, null);
		this.BestIndividual = this.FindBestIndividual();
		Population.CreateIndividual = Population.CreateOffspringIndividual;
	}

	public Population(Approximator approximator, Population previousPopulation)
	{
		this.Id = previousPopulation.Id + 1;
		this.Individuals = Population.CreatePopulation(approximator, previousPopulation);
		this.BestIndividual = this.FindBestIndividual();
	}
	
	private static Individual CreateOffspringIndividual(Approximator approximator, Population? previousPopulation = null)
	{
        ArgumentNullException.ThrowIfNull(previousPopulation);
        var parents = RandomUtils.RandomParents(previousPopulation.Individuals, approximator.TournamentSize);
        return new Individual(approximator, parents);
	}

	private static Individual[] CreatePopulation(Approximator approximator, Population? previousPopulation)
	{
		var individuals = new Individual[approximator.PopulationSize];
		var chunkSize = (int) Math.Ceiling(approximator.PopulationSize / (double) approximator.ThreadCount);
		var threads = new Thread[approximator.ThreadCount];

		for (var threadIndex = 0; threadIndex < approximator.ThreadCount; threadIndex++)
		{
			var start = threadIndex * chunkSize;
			var end = Math.Min(start + chunkSize, approximator.PopulationSize);

			threads[threadIndex] = new Thread(() =>
			{
				for (var individualIndex = start; individualIndex < end; individualIndex++)
					individuals[individualIndex] = Population.CreateIndividual(approximator, previousPopulation);
			});
			threads[threadIndex].Start();
		}
		
		foreach (var thread in threads)
			thread.Join();
		return individuals;
	}

	private static Individual CreateRandomIndividual(Approximator approximator, Population? previousPopulation = null)
	{
		return new Individual(approximator);
	}

	private Individual FindBestIndividual()
	{
		var bestIndividual = this.Individuals[0];
		foreach (var individual in this.Individuals)
		{
			if (individual.IsBetterThan(bestIndividual))
				bestIndividual = individual;
		}
		return bestIndividual;
	}
}