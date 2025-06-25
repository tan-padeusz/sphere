namespace Approximator.Utils;

public class RandomUtils
{
	private static Random Random { get; } = Random.Shared;
	private static int[] Signs { get; } = [-1, 1];

	public static double AttemptMutation(double value, int mutationProbability, double baseThreshold, int populationId)
	{
		var randomValue = RandomUtils.Random.Next(1000);
		if (randomValue >= mutationProbability) return value;
		var threshold = baseThreshold / Math.Sqrt(populationId + 1);
		var delta = RandomUtils.RandomDouble(-threshold, threshold);
		var sign = RandomUtils.RandomSign();
		return value + delta * sign;
	}
	
	private static void PerformTournament()
	{
		throw new NotImplementedException();
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
	
	public static void RandomParents()
	{
		throw new NotImplementedException();
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