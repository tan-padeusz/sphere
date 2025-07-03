namespace Approximator.DTO;

public class Approximator
{
	public int BaseThreshold { get; private set; } = 5;
	public int ChromosomeSize => this.PointCloud.Dimension + 1;
	public int MutationProbability { get; private set; } = 200;
	public int OrderOfMagnitude => this.PointCloud.OrderOfMagnitude;
	public int PatriarchyLevel { get; private set; } = 600;
	public PointCloud PointCloud { get; private set; } = PointCloud.Empty;
	public int PopulationSize { get; private set; } = 400;
	public int ThreadCount { get; private set; } = 4;
	public int TournamentSize { get; private set; } = 20;
	
	internal class Builder
	{
		private Approximator Approximator { get; } = new Approximator();

		public Builder SetBaseThreshold(decimal baseThreshold)
		{
			this.Approximator.BaseThreshold = (int) baseThreshold;
			return this;
		}

		public Builder SetMutationProbability(decimal mutationProbability)
		{
			this.Approximator.MutationProbability = (int) mutationProbability;
			return this;
		}

		public Builder SetPatriarchyLevel(decimal patriarchyLevel)
		{
			this.Approximator.PatriarchyLevel = (int) patriarchyLevel;
			return this;
		}

		public Builder SetPointCloud(PointCloud pointCloud)
		{
			this.Approximator.PointCloud = pointCloud;
			return this;
		}

		public Builder SetPopulationSize(decimal populationSize)
		{
			this.Approximator.PopulationSize = (int) populationSize;
			return this;
		}

		public Builder SetThreadCount(decimal threadCount)
		{
			this.Approximator.ThreadCount = (int) threadCount;
			return this;
		}

		public Builder SetTournamentSize(decimal tournamentSize)
		{
			this.Approximator.TournamentSize = (int) tournamentSize;
			return this;
		}

		public Approximator Build()
		{
			return this.Approximator;
		}
	}
}