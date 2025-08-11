using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

using PersianLibraryOfBabel.Models;
using PersianLibraryOfBabel.Services;

using Serilog;


// Simple, SOLID-compliant UI logic for content display
namespace PersianLibraryOfBabel;
public partial class MainWindow : Window {
	// ViewModel for binding validation
	private readonly InputViewModel _viewModel;

	public MainWindow() {
		InitializeComponent();
		_viewModel  = new InputViewModel();
		DataContext = _viewModel;
		Log.Information("MainWindow initialized at {Time}", DateTime.Now);
	}

	private void SearchButton_Click(object sender, RoutedEventArgs e) {
		try {
			string           searchText = SearchTextInput.Text;
			LibraryPosition? position   = ContentGenerator.FindPositionForText(searchText);
			if (position == null) {
				MessageBox.Show("موقعیت برای متن یافت نشد!", "جستجو", MessageBoxButton.OK, MessageBoxImage.Information);
				Log.Information("No position found for text at {Time}", DateTime.Now);
				return;
			}

			// Update inputs with found position
			_viewModel.HexId  = position.HexId;
			_viewModel.Wall   = position.Wall;
			_viewModel.Shelf  = position.Shelf;
			_viewModel.Volume = position.Volume;
			_viewModel.Page   = position.Page;

			// Generate and display full page content with search text bolded
			string content = ContentGenerator.GeneratePageContent(position);
			SetRichTextBoxContent(content, searchText);
			Log.Information("Search completed for text, content generated for {Position} at {Time}",
							position,
							DateTime.Now);
		} catch (Exception ex) {
			Log.Error(ex, "Error during search at {Time}", DateTime.Now);
			MessageBox.Show("خطایی در جستجو رخ داد!", "خطا", MessageBoxButton.OK, MessageBoxImage.Error);
		}
	}

	private void ClearButton_Click(object sender, RoutedEventArgs e) {
		_viewModel.HexId     = string.Empty;
		_viewModel.Wall      = 0; // Use 0 to indicate unassigned
		_viewModel.Shelf     = 0;
		_viewModel.Volume    = 0;
		_viewModel.Page      = 0;
		SearchTextInput.Text = string.Empty;
		PageContent.Document.Blocks.Clear();
		Log.Information("Inputs and content cleared at {Time}", DateTime.Now);
	}

	private void SearchTextInput_TextChanged(object sender, TextChangedEventArgs e) {
		if (sender is TextBox textBox) {
			string originalText = textBox.Text;
			string filteredText = new string(originalText.Where(c => CharacterSet.PersianChars.Contains(c)).ToArray());
			if (originalText != filteredText) {
				textBox.Text       = filteredText;
				textBox.CaretIndex = filteredText.Length; // Keep cursor at the end
				Log.Information("Filtered invalid characters from search input at {Time}", DateTime.Now);
			}
		}
	}

	// Helper method to set RichTextBox content with optional bolding of search text
	private void SetRichTextBoxContent(string content, string? searchText) {
		// Clear existing content
		PageContent.Document.Blocks.Clear();
		if (string.IsNullOrEmpty(content))
			return;

		// Clean content to remove control characters
		content = content.Replace("\n", "").Replace("\r", "");
		// Validate content characters
		if (!content.All(c => CharacterSet.PersianChars.Contains(c))) {
			Log.Warning("Content contains invalid characters at {Time}", DateTime.Now);
			content = new string(content.Where(c => CharacterSet.PersianChars.Contains(c)).ToArray());
		}
		Log.Information("Cleaned content length: {Length} at {Time}", content.Length, DateTime.Now);

		// Create a single paragraph for content
		Paragraph paragraph = new Paragraph {
												TextAlignment = TextAlignment.Right // Align text for Persian
											};

		// If no search text, display content as plain text
		if (string.IsNullOrEmpty(searchText)) {
			paragraph.Inlines.Add(new Run(content));
			PageContent.Document.Blocks.Add(paragraph);
			Log.Information("Displayed plain content in RichTextBox at {Time}", DateTime.Now);
			return;
		}

		// Find and bold all occurrences of searchText
		int index = 0;
		while (index < content.Length) {
			int foundIndex = content.IndexOf(searchText, index, StringComparison.Ordinal);
			if (foundIndex < 0) {
				// Add remaining text as plain
				paragraph.Inlines.Add(new Run(content.Substring(index)));
				break;
			}

			// Add text before the match as plain
			if (foundIndex > index)
				paragraph.Inlines.Add(new Run(content.Substring(index, foundIndex - index)));

			// Add matched text as bold
			Run  boldRun = new Run(content.Substring(foundIndex, searchText.Length));
			Bold bold    = new Bold(boldRun);
			paragraph.Inlines.Add(bold);
			index = foundIndex + searchText.Length;
		}
		PageContent.Document.Blocks.Add(paragraph);
		Log.Information("Displayed content with bolded search text in RichTextBox at {Time}", DateTime.Now);
	}
}

// Simple view model for input binding and validation
public class InputViewModel : INotifyPropertyChanged {
	public string HexId {
		get => _hexId;
		set {
			_hexId = value;
			OnPropertyChanged(nameof(HexId));
		}
	}
	public int Wall {
		get => _wall;
		set {
			_wall = value;
			OnPropertyChanged(nameof(Wall));
		}
	}
	public int Shelf {
		get => _shelf;
		set {
			_shelf = value;
			OnPropertyChanged(nameof(Shelf));
		}
	}
	public int Volume {
		get => _volume;
		set {
			_volume = value;
			OnPropertyChanged(nameof(Volume));
		}
	}
	public int Page {
		get => _page;
		set {
			_page = value;
			OnPropertyChanged(nameof(Page));
		}
	}
	private int    _wall;
	private int    _shelf;
	private int    _volume;
	private int    _page;
	private string _hexId = string.Empty;

	private void OnPropertyChanged(string propertyName) {
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

	public event PropertyChangedEventHandler PropertyChanged;
}