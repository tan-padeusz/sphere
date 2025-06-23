namespace Generator;

public partial class GeneratorForm
{
    // CONTROLS
    private Label SphereLabel { get; } = new Label();
    private TextBox SphereInput { get; } = new TextBox();
    private Label CountLabel { get; } = new Label();
    private NumericUpDown CountInput { get; } = new NumericUpDown();
    private Label NoiseLabel { get; } = new Label();
    private NumericUpDown NoiseInput { get; } = new NumericUpDown();
    private Button GenerateButton { get; } = new Button();
    
    // OTHER
    private Font ApplicationFont { get; } = new Font(FontFamily.GenericMonospace, 14);
    
    private void InitializeComponent()
    {
        // FORM
        this.ClientSize = new Size(445, 215);
        this.FormBorderStyle = FormBorderStyle.FixedSingle;
        this.MaximizeBox = false;
        this.StartPosition = FormStartPosition.CenterScreen;
        this.Text = "SPHERE GENERATOR";
        
        // CONTROLS
        this.ConfigureLabel(this.SphereLabel, new Point(15, 15), "SPHERE:");
        this.SphereInput.Font = this.ApplicationFont;
        this.SphereInput.Location = new Point(130, 15);
        this.SphereInput.Size = new Size(300, 30);
        this.SphereInput.TextAlign = HorizontalAlignment.Left;
        this.Controls.Add(this.SphereInput);
        
        this.ConfigureLabel(this.CountLabel, new Point(15, 60), "COUNT:");
        this.ConfigureInput(this.CountInput, new Point(130, 60), 10, 1000000, 100, 20);
        
        this.ConfigureLabel(this.NoiseLabel, new Point(15, 105), "NOISE:");
        this.ConfigureInput(this.NoiseInput, new Point(130, 105), 0, 1000, 200, 20);

        this.GenerateButton.Click += this.GenerateButtonClick;
        this.GenerateButton.Font = new Font(FontFamily.GenericMonospace, 20);
        this.GenerateButton.Location = new Point(15, 150);
        this.GenerateButton.Size = new Size(415, 50);
        this.GenerateButton.Text = "GENERATE";
        this.GenerateButton.TextAlign = ContentAlignment.MiddleCenter;
        this.Controls.Add(this.GenerateButton);
    }

    private void ConfigureLabel(Label label, Point location, string text)
    {
        label.Font = this.ApplicationFont;
        label.Location = location;
        label.Size = new Size(100, 30);
        label.Text = text;
        label.TextAlign = ContentAlignment.MiddleRight;
        this.Controls.Add(label);
    }

    private void ConfigureInput(NumericUpDown numericUpDown, Point location, int min, int max, int def, int inc)
    {
        numericUpDown.Font = this.ApplicationFont;
        numericUpDown.Increment = inc;
        numericUpDown.Location = location;
        numericUpDown.Maximum = max;
        numericUpDown.Minimum = min;
        numericUpDown.Size = new Size(100, 30);
        numericUpDown.TextAlign = HorizontalAlignment.Left;
        numericUpDown.Value = def;
        this.Controls.Add(numericUpDown);
    }
}