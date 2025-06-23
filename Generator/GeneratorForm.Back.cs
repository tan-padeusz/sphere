using System.Globalization;

namespace Generator;

public partial class GeneratorForm : Form
{
    public GeneratorForm()
    {
        this.InitializeComponent();
    }

    private void GenerateButtonClick(object? sender, EventArgs e)
    {
        var (sphere, error) = this.ValidateSphereInput();
        if (!string.IsNullOrEmpty(error))
        {
            GeneratorForm.ShowError(error);
            return;
        }
    }

    private (double[], string) ValidateSphereInput()
    {
        string[] input = [this.SphereInput.Text];
        if (string.IsNullOrWhiteSpace(input[0]))
        {
            return ([], "Invalid sphere!\nInput is empty!");
        }

        input = input[0].Split(';');
        if (input.Length < 2)
        {
            return ([], "Invalid sphere!\nInput must contain at least two values!");
        }
        
        var sphere = new double[input.Length];
        if (input.Where((value, index) => !GeneratorForm.TryParse(value, out sphere[index])).Any())
        {
            return ([], "Invalid sphere!\nPlease input real values separated by a semicolon!");
        }
        
        return (sphere, string.Empty);
    }

    private double[][] GeneratePoints(double[] sphere)
    {
        var count = (int) this.CountInput.Value;
        var noise = (int) this.NoiseInput.Value;
        
        var dimensions = sphere.Length - 1;
        var radius = sphere.Last();
        
        var points = new double[count][];
        var random = Random.Shared;
        
        var deviation = (2 * random.NextDouble() - 1) * (noise / 1000.0);
        var noiseFactor = 1 + deviation;

        for (var index = 0; index < count; index++)
        {
            var point = new double[dimensions];
            for (var dimension = 0; dimension < dimensions; dimension++)
            {
                // Box-Muller transform
                var u1 = 1 - random.NextDouble();
                var u2 = random.NextDouble();
                var value = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Cos(2.0 * Math.PI * u2);
                point[index] = value;
            }
            
            var norm = Math.Sqrt(point.Sum(value => value * value));
            for (var dimension = 0; dimension < dimensions; dimension++)
            {
                point[dimension] /= norm;
            }

            for (var dimension = 0; dimension < dimensions; dimension++)
            {
                point[dimension] = sphere[dimension] + point[dimension] * radius * noiseFactor;
            }
            
            points[index] = point;
        }
        
        return points;
    }

    private static bool TryParse(string input, out double output)
    {
        try
        {
            var normalized = input.Replace(',', '.');
            output = double.Parse(normalized, CultureInfo.InvariantCulture);
            return true;
        }
        catch (Exception)
        {
            output = 0;
            return false;
        }
    }

    private static void ShowError(string error)
    {
        MessageBox.Show(error, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}