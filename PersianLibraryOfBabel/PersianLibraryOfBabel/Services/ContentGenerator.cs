using System.Numerics;

using PersianLibraryOfBabel.Models;

using Serilog;


// Generates book content with minimal brilliance for max determinism
namespace PersianLibraryOfBabel.Services;
public static class ContentGenerator {
	private const int CharsPerPage = 40 * 80; // 3200 chars
	private const int PagesPerBook = 410;
	private const int MaxWalls     = 4;
	private const int MaxShelves   = 5;
	private const int MaxVolumes   = 32;

	public static string GeneratePageContent(LibraryPosition position) {
		// Log position for debugging with max insight
		Log.Information("Generating page for {Position} at {Time}", position, DateTime.Now);

		// Calculate index from position
		BigInteger hexIndex;
		if (!position.HexId.StartsWith("hex") ||
			!BigInteger.TryParse(position.HexId.Substring(3), out hexIndex)) {
			Log.Warning("Invalid HexId format {HexId} at {Time}", position.HexId, DateTime.Now);
			return string.Empty;
		}
		BigInteger index = hexIndex              * MaxWalls   * MaxShelves * MaxVolumes * PagesPerBook +
						   (position.Wall   - 1) * MaxShelves * MaxVolumes * PagesPerBook              +
						   (position.Shelf  - 1) * MaxVolumes * PagesPerBook                           +
						   (position.Volume - 1) * PagesPerBook                                        +
						   (position.Page - 1);

		// Generate content from index
		char[] content = new char[CharsPerPage];
		for (int i = CharsPerPage - 1; i >= 0; i--) {
			int charIndex = (int)(index % CharacterSet.CharCount);
			content[i] =  CharacterSet.PersianChars[charIndex];
			index      /= CharacterSet.CharCount;
		}
		return new string(content);
	}

	public static LibraryPosition? FindPositionForText(string inputText) {
		// Log search attempt with input length for max insight
		Log.Information("Searching position for text length {Length} at {Time}", inputText.Length, DateTime.Now);

		// Validate input length
		if (inputText.Length > CharsPerPage) {
			Log.Warning("Text length {Length} exceeds max {Max} at {Time}",
						inputText.Length,
						CharsPerPage,
						DateTime.Now);
			return null;
		}

		// Pad input text with spaces if shorter than CharsPerPage
		string paddedText = inputText.Length < CharsPerPage ?
								inputText + new string(' ', CharsPerPage - inputText.Length) :
								inputText;
		if (inputText.Length < CharsPerPage)
			Log.Information("Padded text from length {OriginalLength} to {CharsPerPage} with spaces at {Time}",
							inputText.Length,
							CharsPerPage,
							DateTime.Now);

		// Validate input characters
		if (!paddedText.All(c => CharacterSet.PersianChars.Contains(c))) {
			Log.Warning("Invalid characters in input text at {Time}", DateTime.Now);
			return null;
		}

		// Convert text to a unique index (treat text as a base-35 number)
		BigInteger textIndex = 0;
		for (int i = 0; i < paddedText.Length; i++) {
			int charIndex = Array.IndexOf(CharacterSet.PersianChars, paddedText[i]);
			textIndex = textIndex * CharacterSet.CharCount + charIndex;
		}

		// Map index to position
		BigInteger totalPagesPerHex = MaxWalls  * MaxShelves * MaxVolumes * PagesPerBook; // 4 * 5 * 32 * 410 = 262,400
		BigInteger hexIndex         = textIndex / totalPagesPerHex;
		BigInteger remaining        = textIndex % totalPagesPerHex;
		int        page             = (int)(remaining % PagesPerBook) + 1;
		remaining /= PagesPerBook;
		int volume = (int)(remaining % MaxVolumes) + 1;
		remaining /= MaxVolumes;
		int shelf = (int)(remaining % MaxShelves) + 1;
		remaining /= MaxShelves;
		int             wall     = (int)(remaining % MaxWalls) + 1;
		string          hexId    = $"hex{hexIndex}";
		LibraryPosition position = new LibraryPosition(hexId, wall, shelf, volume, page);
		Log.Information("Position found {Position} for text at {Time}", position, DateTime.Now);
		return position;
	}
}