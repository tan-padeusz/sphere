namespace Approximator.DTO;

public class Result
{
	public string ElapsedTime { get; private set; } = "--:--";
	public string Error { get; private set; } = "N/A";
	public string LastImprovement { get; private set; } = "N/A";
	public string PopulationsCreated { get; private set; } = "N/A";
	public string Outcome { get; private set; } = "N/A";
	public static Result Default => new Result();
	
	internal class Builder
	{
		private Result Result { get; } = new Result();

		private static string FormatTime(long milliseconds)
		{
			var seconds = (milliseconds / 1000) % 60;
			var minutes = milliseconds / 60000;
            
			var minutesString = minutes < 10 ? $"0{minutes}" : minutes.ToString();
			var secondsString = seconds < 10 ? $"0{seconds}" : seconds.ToString();

			return $"{minutesString}:{secondsString}";
		}
		
		public Builder SetElapsedTime(long milliseconds)
		{
			this.Result.ElapsedTime = Builder.FormatTime(milliseconds);
			return this;
		}

		public Builder SetError(double error)
		{
			this.Result.Error = $"{error}";
			return this;
		}

		public Builder SetLastImprovement(long lastImprovement)
		{
			this.Result.LastImprovement = $"{lastImprovement}";
			return this;
		}

		public Builder SetPopulationsCreated(long populationsCreated)
		{
			this.Result.PopulationsCreated = $"{populationsCreated}";
			return this;
		}

		public Builder SetOutcome(string outcome)
		{
			this.Result.Outcome = outcome;
			return this;
		}

		public Result Build()
		{
			return this.Result;
		}
	}
}