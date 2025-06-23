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