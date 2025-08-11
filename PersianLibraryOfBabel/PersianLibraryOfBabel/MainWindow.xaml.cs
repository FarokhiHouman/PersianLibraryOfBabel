using System.ComponentModel;
using System.Windows;

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

	private void GenerateButton_Click(object sender, RoutedEventArgs e) {
		try {
			// Validate inputs
			if (string.IsNullOrWhiteSpace(_viewModel.HexId) ||
				_viewModel.Wall   < 1                       ||
				_viewModel.Wall   > 4                       ||
				_viewModel.Shelf  < 1                       ||
				_viewModel.Shelf  > 5                       ||
				_viewModel.Volume < 1                       ||
				_viewModel.Volume > 32                      ||
				_viewModel.Page   < 1                       ||
				_viewModel.Page   > 410) {
				Log.Warning("Invalid input at {Time}", DateTime.Now);
				MessageBox.Show("ورودی‌ها نامعتبرند! لطفاً مقادیر درست وارد کنید.",
								"خطا",
								MessageBoxButton.OK,
								MessageBoxImage.Error);
				return;
			}

			// Create position and generate content
			LibraryPosition position = new LibraryPosition(_viewModel.HexId,
														   _viewModel.Wall,
														   _viewModel.Shelf,
														   _viewModel.Volume,
														   _viewModel.Page);
			string content = ContentGenerator.GeneratePageContent(position);

			// Display content
			PageContent.Text = content;
			Log.Information("Content generated for {Position} at {Time}", position, DateTime.Now);
		} catch (Exception ex) {
			Log.Error(ex, "Error generating content at {Time}", DateTime.Now);
			MessageBox.Show("خطایی رخ داد! لاگ‌ها را بررسی کنید.", "خطا", MessageBoxButton.OK, MessageBoxImage.Error);
		}
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
			PageContent.Text  = searchText;
			Log.Information("Search completed for text at {Time}", DateTime.Now);
		} catch (Exception ex) {
			Log.Error(ex, "Error during search at {Time}", DateTime.Now);
			MessageBox.Show("خطایی در جستجو رخ داد!", "خطا", MessageBoxButton.OK, MessageBoxImage.Error);
		}
	}

	private void ClearButton_Click(object sender, RoutedEventArgs e) {
		_viewModel.HexId     = string.Empty;
		_viewModel.Wall      = 1;
		_viewModel.Shelf     = 1;
		_viewModel.Volume    = 1;
		_viewModel.Page      = 1;
		SearchTextInput.Text = string.Empty;
		PageContent.Text     = string.Empty;
		Log.Information("Inputs cleared at {Time}", DateTime.Now);
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
	private int    _wall   = 1;
	private int    _shelf  = 1;
	private int    _volume = 1;
	private int    _page   = 1;
	private string _hexId  = string.Empty;

	private void OnPropertyChanged(string propertyName) {
		PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
	}

	public event PropertyChangedEventHandler PropertyChanged;
}