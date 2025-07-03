using Approximator.DTO;
using Timer = System.Windows.Forms.Timer;

namespace Approximator;

public partial class ApproximatorForm : Form
{
	private Engine Engine { get; } = new Engine();
	private PointCloud? PointCloud { get; set; } = null;
	private Timer Timer { get; } = new Timer();
	
	public ApproximatorForm()
	{
		this.InitializeComponent();
		this.ConfigureTimer();
	}

	private void ConfigureTimer()
	{
		this.Timer.Interval = 20;
		this.Timer.Tick += (sender, args) =>
		{
			var result = this.Engine.Result;
			this.ElapsedTimeOutput.Text = result.ElapsedTime;
			this.ErrorOutput.Text = result.Error;
			this.PopulationsCreatedOutput.Text = result.PopulationsCreated;
			this.LastImprovementOutput.Text = result.LastImprovement;
			this.OutcomeOutput.Text = result.Outcome;
			this.EngineStatusLabel.Text = $"ENGINE STATUS: {this.Engine.Status}";
		};
		this.Timer.Start();
	}

	private void FileButtonClick(object? sender, EventArgs args)
	{
		var ofd = new OpenFileDialog();
		ofd.DefaultExt = ".csv";
		ofd.Filter = "CSV Files (*.csv)|*.csv";
		ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

		if (ofd.ShowDialog() != DialogResult.OK) return;
		
		var filepath = ofd.FileName;
		this.FileInput.Text = filepath;
			
		var cloud = new PointCloud(filepath);
		if (cloud.HasError)
		{
			this.FileOutput.BackColor = Color.LightCoral;
			this.FileOutput.Text = cloud.Error;
			cloud = null;
		}
		else
		{
			this.FileOutput.BackColor = Color.DarkSeaGreen;
			this.FileOutput.Text = this.FileInput.Text;
		}
			
		this.ResizeFontToFit(this.FileOutput);
		this.PointCloud = cloud;
	}

	private void StartEngineButtonClick(object? sender, EventArgs args)
	{
		if (this.PointCloud == null)  return;
		
		var approximator = new Approximator.DTO.Approximator.Builder()
			.SetMutationProbability(this.MutationProbabilityInput.Value)
			.SetPatriarchyLevel(this.PatriarchyLevelInput.Value)
			.SetPointCloud(this.PointCloud)
			.SetPopulationSize(this.PopulationSizeInput.Value)
			.SetThreadCount(this.ThreadCountInput.Value)
			.SetTournamentSize(this.TournamentSizeInput.Value)
			.Build();
		
		this.Engine.Start(approximator);
	}

	private void StopEngineButtonClick(object? sender, EventArgs args)
	{
		this.Engine.Stop();
	}

	private void PauseEngineButtonClick(object? sender, EventArgs args)
	{
		this.Engine.Pause();
	}

	private void ResumeEngineButtonClick(object? sender, EventArgs args)
	{
		this.Engine.Resume();
	}

	private void PopulationSizeInputValueChanged(object? sender, EventArgs args)
	{
		var value = this.PopulationSizeInput.Value;
		var halfValue = Math.Floor(value / 2);
		this.TournamentSizeInput.Maximum = halfValue;
		this.TournamentSizeInput.Value = Math.Min(this.TournamentSizeInput.Value, halfValue);
	}
}