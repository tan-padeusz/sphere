namespace Approximator.DTO;

public readonly struct Point
{
	private double[] Coordinates { get; }
	private double this[int index] => this.Coordinates[index];
	public int Dimension => this.Coordinates.Length;

	public Point()
	{
		throw new NotImplementedException("Parameterless constructor is not allowed!");
	}
	
	private Point(double[] coordinates)
	{
		this.Coordinates = coordinates;
	}

	public double DistanceTo(Point other)
	{
		if (this.Dimension != other.Dimension)
			throw new ArgumentException($"Dimensions mismatch! THIS={this.Dimension}|OTHER={other.Dimension}!");
		
		var sum = 0.0;
		for (var index = 0; index < this.Dimension; index++)
		{
			var difference = this[index] - other[index];
			sum += difference * difference;
		}
		return Math.Sqrt(sum);
	}

	public static Point FromArray(double[] coordinates)
	{
		return new Point(coordinates);
	}

	public static Point FromString(string coordinates)
	{
		var doubleCoordinates = coordinates.Split(';').Select(double.Parse).ToArray();
		return new Point(doubleCoordinates);
	}
	
	public IEnumerator<double> GetEnumerator()
	{
		return (IEnumerator<double>) this.Coordinates.GetEnumerator();
	}
}