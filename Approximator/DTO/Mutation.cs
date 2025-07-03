namespace Approximator.DTO;

public struct Mutation(int probability, int orderOfMagnitude, int populationId)
{
	public int OrderOfMagnitude = orderOfMagnitude;
	public int Probability = probability;
	public int PopulationId = populationId;
}