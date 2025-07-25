namespace Approximator;

public partial class ApproximatorForm
{
	#region controls
	
	#region input controls
	
	private TextBox FileInput { get; } = new TextBox();
	private Label FileOutput { get; } = new Label();
	private Button FileButton { get; } = new Button();
	
	private Label MutationProbabilityLabel { get; } = new Label();
	private NumericUpDown MutationProbabilityInput { get; } = new NumericUpDown();
	
	private Label PatriarchyLevelLabel { get; } = new Label();
	private NumericUpDown PatriarchyLevelInput { get; } = new NumericUpDown();
	
	private Label PopulationSizeLabel { get; } = new Label();
	private NumericUpDown PopulationSizeInput { get; } = new NumericUpDown();
	
	private Label ThreadCountLabel { get; } = new Label();
	private NumericUpDown ThreadCountInput { get; } = new NumericUpDown();
	
	private Label TournamentSizeLabel { get; } = new Label();
	private NumericUpDown TournamentSizeInput { get; } = new NumericUpDown();
	
	#endregion
	
	#region output controls
	
	private Label ElapsedTimeLabel { get; } = new Label();
	private Label ElapsedTimeOutput { get; } = new Label();
	
	private Label ErrorLabel { get; } = new Label();
	private Label ErrorOutput { get; } = new Label();
	
	private Label LastImprovementLabel { get; } = new Label();
	private Label LastImprovementOutput { get; } = new Label();
	
	private Label PopulationsCreatedLabel { get; } = new Label();
	private Label PopulationsCreatedOutput { get; } = new Label();
	
	private Label OutcomeLabel { get; } = new Label();
	private Label OutcomeOutput { get; } = new Label();
	
	#endregion

	#region engine controls

	private Label EngineStatusLabel { get; } = new Label();
	private Button StartEngineButton { get; } = new Button();
	private Button StopEngineButton { get; } = new Button();
	private Button PauseEngineButton { get; } = new Button();
	private Button ResumeEngineButton { get; } = new Button();

	#endregion

	#region decorations

	private Label Divider { get; } = new Label();

	#endregion
	
	#endregion
	
	private Font BaseFont { get; } = new Font(FontFamily.GenericMonospace, 20);
	private Font InputFont { get; } = new Font(FontFamily.GenericMonospace, 14);
	
	private void InitializeComponent()
	{
		// FORM
		this.ClientSize = new Size(860, 370);
		this.FormBorderStyle = FormBorderStyle.FixedSingle;
		this.StartPosition = FormStartPosition.CenterScreen;
		this.Text = "SPHERE APPROXIMATOR";
		
		// INPUT CONTROLS
		this.FileInput.Location = new Point(10, 10);
		this.FileInput.Size = new Size(200, 30);
		this.Controls.Add(this.FileInput);
		
		this.ConfigureLabel(this.FileOutput, new Point(10, 50), "FILE NOT LOADED");
		this.FileOutput.BackColor = Color.LightCoral;
		this.ConfigureButton(this.FileButton, new Point(220, 10), "LOAD FILE");
		this.FileButton.Click += this.FileButtonClick;
		
		this.ConfigureLabel(this.MutationProbabilityLabel, new Point(10, 90), "MUTATION PROBABILITY");
		this.ConfigureInput(this.MutationProbabilityInput, new Point(220, 90), 0, 1000, 300, 20);
		
		this.ConfigureLabel(this.PatriarchyLevelLabel, new Point(10, 130), "PATRIARCHY LEVEL");
		this.ConfigureInput(this.PatriarchyLevelInput, new Point(220, 130), 0, 1000, 600, 20);
		
		this.ConfigureLabel(this.PopulationSizeLabel, new Point(10, 170), "POPULATION SIZE");
		this.ConfigureInput(this.PopulationSizeInput, new Point(220, 170), 0, 1000, 100, 50);
		this.PopulationSizeInput.ValueChanged += this.PopulationSizeInputValueChanged;
		
		this.ConfigureLabel(this.ThreadCountLabel, new Point(10, 210), "THREAD COUNT");
		this.ConfigureInput(this.ThreadCountInput, new Point(220, 210), 1, Environment.ProcessorCount, 1, 1);
		
		this.ConfigureLabel(this.TournamentSizeLabel, new Point(10, 250), "TOURNAMENT SIZE");
		this.ConfigureInput(this.TournamentSizeInput, new Point(220, 250), 10, 200, 20, 10);
		
		// OUTPUT CONTROLS
		this.ConfigureLabel(this.ElapsedTimeLabel, new Point(440, 10), "ELAPSED TIME");
		this.ConfigureLabel(this.ElapsedTimeOutput, new Point(650, 10), "N/A");
		
		this.ConfigureLabel(this.ErrorLabel, new Point(440, 50), "ERROR");
		this.ConfigureLabel(this.ErrorOutput, new Point(650, 50), "N/A");
		
		this.ConfigureLabel(this.LastImprovementLabel, new Point(440, 90), "LAST IMPROVEMENT");
		this.ConfigureLabel(this.LastImprovementOutput, new Point(650, 90), "N/A");
		
		this.ConfigureLabel(this.PopulationsCreatedLabel, new Point(440, 130), "POPULATIONS CREATED");
		this.ConfigureLabel(this.PopulationsCreatedOutput, new Point(650, 130), "N/A");
		
		this.ConfigureLabel(this.OutcomeLabel, new Point(440, 170), "OUTCOME");
		this.ConfigureLabel(this.OutcomeOutput, new Point(650, 170), "N/A");
		
		// ENGINE CONTROLS
		this.EngineStatusLabel.Font = new Font(FontFamily.GenericMonospace, 18);
		this.EngineStatusLabel.Location = new Point(10, 290);
		this.EngineStatusLabel.Size = new Size(410, 70);
		this.EngineStatusLabel.Text = "ENGINE STATUS: STOPPED";
		this.EngineStatusLabel.TextAlign = ContentAlignment.MiddleCenter;
		this.Controls.Add(this.EngineStatusLabel);
		
		this.ConfigureButton(this.StartEngineButton, new Point(440, 210), "START");
		this.StartEngineButton.Click += this.StartEngineButtonClick;
		this.ConfigureButton(this.StopEngineButton, new Point(650, 210), "STOP");
		this.StopEngineButton.Click += this.StopEngineButtonClick;
		this.ConfigureButton(this.PauseEngineButton, new Point(440, 290), "PAUSE");
		this.PauseEngineButton.Click += this.PauseEngineButtonClick;
		this.ConfigureButton(this.ResumeEngineButton, new Point(650, 290), "RESUME");
		this.ResumeEngineButton.Click += this.ResumeEngineButtonClick;
		
		// DECORATIONS
		this.Divider.BackColor = Color.LightGray;
		this.Divider.Location = new Point(428, 10);
		this.Divider.Size = new Size(5, 350);
		this.Controls.Add(this.Divider);
	}

	private void ConfigureButton(Button button, Point location, string text)
	{
		this.Font = this.BaseFont;
		button.Location = location;
		button.Size = new Size(200, 70);
		button.Text = text;
		this.Controls.Add(button);
	}

	private void ConfigureLabel(Label label, Point location, string text)
	{
		label.Font = this.BaseFont;
		label.Location = location;
		label.Size = new Size(200, 30);
		label.Text = text;
		label.TextAlign = ContentAlignment.MiddleCenter;
		this.Controls.Add(label);
		this.ResizeFontToFit(label);
	}

	private void ConfigureInput(NumericUpDown input, Point location, int min, int max, int def, int inc)
	{
		input.DecimalPlaces = 0;
		input.Font = this.InputFont;
		input.Increment = inc;
		input.Location = location;
		input.Maximum = max;
		input.Minimum = min;
		input.Size = new Size(200, 30);
		input.TextAlign = HorizontalAlignment.Left;
		input.Value = def;
		this.Controls.Add(input);
	}
	
	

	private void ResizeFontToFit(Label label)
	{
		if (string.IsNullOrEmpty(label.Text)) return;
		var font = this.BaseFont;
		var fontSize = font.Size;

		var textSize = TextRenderer.MeasureText(label.Text, font);
		while (textSize.Width > label.Width && fontSize > 6F)
		{
			fontSize -= 0.5F;
			font = new Font(FontFamily.GenericMonospace, fontSize);
			textSize = TextRenderer.MeasureText(label.Text, font);
		}
		
		label.Font = font;
	}
}