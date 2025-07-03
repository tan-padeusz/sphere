using Approximator.Utils;

namespace Approximator.Genetics;

using DTO;

public class Individual
{
	public double DisplayError { get; }
	private double Error { get; }
	private double[] Genes { get; }
	public string Outcome => string.Join(';', this.Genes);
	public int PopulationId { get; }
	private (Point, double) Sphere => (Point.FromArray(this.Genes.Take(this.Genes.Length - 1).ToArray()), this.Genes.Last());

	public Individual(Approximator approximator)
	{
		this.PopulationId = 0;
		this.Genes = RandomUtils.RandomArray(approximator.ChromosomeSize);
		(this.Error, this.DisplayError) = this.EvaluateError(approximator);
	}

	public Individual(Approximator approximator, Parents parents)
	{
		this.PopulationId = parents.PopulationId + 1;
		this.Genes = Individual.Crossover(parents, approximator);
		this.Mutate(approximator, this.PopulationId);
		(this.Error, this.DisplayError) = this.EvaluateError(approximator);
	}

	private (double, double) EvaluateError(Approximator approximator)
	{
		var cloud = approximator.PointCloud;
		var (center, radius) = this.Sphere;
		var error = 0.0;
		foreach (var point in cloud)
		{
			error += Math.Abs(point.DistanceTo(center) - radius);
		}

		return (error, Math.Round(error / cloud.Size, 6));
	}

	public bool IsBetterThan(Individual other)
	{
		return this.Error < other.Error;
	}
	
	public bool IsWorseThan(Individual other)
	{
		return this.Error > other.Error;
	}

	private void Mutate(Approximator approximator, int populationId)
	{
		for (var index = 0; index < this.Genes.Length; index++)
		{
			var gene = this.Genes[index];
			var mutation = new Mutation(approximator.MutationProbability, approximator.OrderOfMagnitude, populationId);
			this.Genes[index] = RandomUtils.AttemptMutation(gene, mutation);
		}
	}

	private static double[] Crossover(Parents parents, Approximator approximator)
	{
		var genes = new double[approximator.ChromosomeSize];
		for (var index = 0; index < genes.Length; index++)
		{
			var fatherGene = parents.Father.Genes[index];
			var motherGene = parents.Mother.Genes[index];
			genes[index] = RandomUtils.SelectWithProbability(fatherGene, motherGene, approximator.PatriarchyLevel);
		}
		return genes;
	}
}