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

        var (isError, message) = this.SavePointsToFile(sphere);
        if (isError)
        {
            GeneratorForm.ShowError(message);
            return;
        }
        
        GeneratorForm.ShowMessage(message);
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

    private (bool, string) SavePointsToFile(double[] sphere)
    {
        var sfd = new SaveFileDialog();
        sfd.DefaultExt = ".csv";
        sfd.Filter = "CSV Files (*.csv)|*.csv";
        sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        sfd.RestoreDirectory = true;

        try
        {
            if (sfd.ShowDialog() != DialogResult.OK) throw new IOException("Saving cancelled!");

            var random = Random.Shared;
            var count = (int) this.CountInput.Value;
            var noise = (int) this.NoiseInput.Value;
            var deviation = (2 * random.NextDouble() - 1) * (noise / 1000.0);
            var noiseFactor = 1 + deviation;
            
            using var writer = new StreamWriter(sfd.FileName);

            for (var _ = 0; _ < count; _++)
            {
                var point = GeneratorForm.GeneratePoint(sphere, noiseFactor);
                writer.WriteLine(string.Join(';', point));
            }
            
            writer.Close();
            
            return (false, $"Points successfully saved to {sfd.FileName}!");
        }
        catch (IOException exception)
        {
            return (true, $"Error while saving file:\n{exception.Message}");
        }
    }

    private static double[] GeneratePoint(double[] sphere, double noiseFactor)
    {
        var dimensions = sphere.Length;
        var radius = sphere.Last();
        var random = Random.Shared;
        
        var point = new double[dimensions];
        
        for (var dimension = 0; dimension < dimensions; dimension++)
        {
            // Box-Muller transform
            var u1 = 1 - random.NextDouble();
            var u2 = random.NextDouble();
            var value = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Cos(2.0 * Math.PI * u2);
            point[dimension] = value;
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
        
        return point;
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

    private static void ShowMessage(string message)
    {
        MessageBox.Show(message, "SUCCESS", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private static void ShowError(string error)
    {
        MessageBox.Show(error, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}