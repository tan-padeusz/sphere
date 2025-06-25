using Approximator.Genetics;

namespace Approximator.DTO;

public struct Parents
{
	public Individual Father { get; }
	public Individual Mother { get; }
	public int PopulationId { get; }

	public Parents(Individual father, Individual mother)
	{
		if (father.IsWorseThan(mother)) (father, mother) = (mother, father);
		this.Father = father;
		this.Mother = mother;
		this.PopulationId = father.PopulationId;
	}
}