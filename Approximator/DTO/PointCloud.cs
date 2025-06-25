namespace Approximator.DTO;

public class PointCloud
{
	public int Dimension { get; } = 0;
	public string Error { get; } = string.Empty;
	public bool HasError => this.Error != string.Empty;
	private Point[] Points { get; } = [];
	public static PointCloud Empty { get; } = new PointCloud();

	private PointCloud()
	{
		this.Error = $"Empty cloud!";
	}

	public PointCloud(string filepath)
	{
		if (!File.Exists(filepath))
		{
			this.Error = $"File {filepath} does not exist!";
			return;
		}
		
		var points = new List<Point>();
		var lineIndex = 1;
		try
		{
			var reader = new StreamReader(filepath);
			while (reader.ReadLine() is { } line)
			{
				points.Add(Point.FromString(line));
				lineIndex++;
			}
		}
		catch (Exception exception)
		{
			this.Error = $"Error reading file {filepath} at line {lineIndex}:\n{exception.Message}";
			return;
		}

		if (points.Count < 10)
		{
			this.Error = $"File {filepath} must contain at least 10 points!";
			return;
		}

		var dimension = points[0].Dimension;
		for (var index = 1; index < points.Count; index++)
		{
			var point = points[index];
			if (point.Dimension != dimension) continue;
			this.Error = $"Mismatch in dimension between first point and point at line {index + 1}!";
			return;
		}
		
		this.Dimension = dimension;
		this.Points = points.ToArray();
	}

	public IEnumerator<Point> GetEnumerator()
	{
		return (IEnumerator<Point>) this.Points.GetEnumerator();
	}
}