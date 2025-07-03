namespace Approximator.DTO;

public class PointCloud
{
	public int Dimension { get; }
	public string Error { get; } = string.Empty;
	public bool HasError => this.Error != string.Empty;
	public int OrderOfMagnitude { get; }
	private Point[] Points { get; } = [];
	public int Size => this.Points.Length;
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
		var orderOfMagnitude = int.MinValue;
		for (var index = 1; index < points.Count; index++)
		{
			var point = points[index];
			foreach (var coordinate in point)
			{
				var oom = PointCloud.EvaluateOrderOfMagnitude(coordinate);
				if (oom > orderOfMagnitude) orderOfMagnitude = oom;
			}
			if (point.Dimension == dimension) continue;
			this.Error = $"Mismatch in dimension between first point and point at line {index + 1}!";
			return;
		}
		
		this.Dimension = dimension;
		this.OrderOfMagnitude = orderOfMagnitude;
		this.Points = points.ToArray();
	}

	private static int EvaluateOrderOfMagnitude(double value)
	{
		value = Math.Abs(value);
		if (value < 0.00001)
			return 0;
		return (int) Math.Floor(Math.Log10(value));
	}
	
	public IEnumerator<Point> GetEnumerator()
	{
		return ((IEnumerable<Point>) this.Points).GetEnumerator();
	}
}