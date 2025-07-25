using Approximator.DTO;
using Approximator.Genetics;

namespace Approximator.Utils;

public class RandomUtils
{
	private static Random Random { get; } = Random.Shared;
	private static int[] Signs { get; } = [-1, 1];

	public static double AttemptMutation(double value, Mutation mutation)
	{
		var randomValue = RandomUtils.Random.Next(1000);
		if (randomValue >= mutation.Probability) return value;
		// var divider = 1.0 / Math.Cos(0.25 * mutation.PopulationId);
		var multiplier = Math.Cos(0.25 * mutation.PopulationId);
		var maxThreshold = mutation.OrderOfMagnitude * multiplier;
		var minThreshold = -6 + 6 * multiplier;
		var delta = Math.Pow(10, RandomUtils.RandomInt(minThreshold, maxThreshold));
		var sign = RandomUtils.RandomSign();
		return value + delta * sign;
		
		// dynamiczne dostrajanie parametrów mutacji
	}
	
	private static Individual PerformTournament(Individual[] group)
	{
		var bestIndividual = group.First();
		foreach (var individual in group)
		{
			if (individual.IsBetterThan(bestIndividual))
				bestIndividual = individual;
		}
		return bestIndividual;
	}

	public static double[] RandomArray(int size)
	{
		var array = new double[size];
		for (var index = 0; index < size; index++) array[index] = RandomUtils.RandomDouble(-1, 1);
		return array;
	}

	private static double RandomDouble(double min, double max)
	{
		if (min > max) (min, max) = (max, min);
		return min + RandomUtils.Random.NextDouble() * (max - min + double.Epsilon);
	}

	private static int RandomInt(double min, double max)
	{
		return (int) Math.Round(RandomUtils.RandomDouble(min, max));
	}
	
	private static T[] RandomElements<T>(T[] source, int count)
	{
		var copy = (T[]) source.Clone();
		for (var index = 0; index < count; index++)
		{
			var randomIndex = RandomUtils.Random.Next(index, source.Length);
			(copy[index], copy[randomIndex]) = (copy[randomIndex], copy[index]);
		}
		
		var result = new T[count];
		Array.Copy(copy, result, count);
		return result;
	}
	
	public static Parents RandomParents(Individual[] population, int tournamentSize)
	{
		var picked = RandomUtils.RandomElements(population, 2 * tournamentSize);
		var fatherGroup = picked.Take(tournamentSize).ToArray();
		var motherGroup = picked.Skip(tournamentSize).ToArray();
		var father = RandomUtils.PerformTournament(fatherGroup);
		var mother = RandomUtils.PerformTournament(motherGroup);
		return new Parents(father, mother);
	}

	private static int RandomSign()
	{
		var picked = RandomUtils.RandomElements(RandomUtils.Signs, 1);
		return picked[0];
	}
	
	public static double SelectWithProbability(double better, double worse, int probability)
	{
		var randomValue = RandomUtils.Random.Next(1000);
		return randomValue < probability ? better : worse;
	}
}