using System.Numerics;
using System.Security.Cryptography;
using System.Text;

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

		// Validate input characters
		if (!inputText.All(c => CharacterSet.PersianChars.Contains(c))) {
			Log.Warning("Invalid characters in input text at {Time}", DateTime.Now);
			return null;
		}

		// Generate deterministic seed from input text
		byte[] hash   = SHA256.HashData(Encoding.UTF8.GetBytes(inputText));
		int    seed   = BitConverter.ToInt32(hash, 0);
		Random random = new Random(seed);

		// Determine random but deterministic position for inputText in the page
		int maxStartIndex = CharsPerPage - inputText.Length;
		int startIndex    = random.Next(0, maxStartIndex + 1);
		Log.Information("Placing text at index {StartIndex} for input length {Length} at {Time}",
						startIndex,
						inputText.Length,
						DateTime.Now);

		// Generate deterministic random content for the entire page
		char[] pageContent = new char[CharsPerPage];
		for (int i = 0; i < CharsPerPage; i++) {
			if (i >= startIndex &&
				i < startIndex + inputText.Length) {
				// Place inputText at the chosen position
				pageContent[i] = inputText[i - startIndex];
			} else {
				// Fill with deterministic random Persian chars
				pageContent[i] = CharacterSet.PersianChars[random.Next(CharacterSet.CharCount)];
			}
		}

		// Convert page content to a unique index
		BigInteger textIndex = 0;
		for (int i = 0; i < pageContent.Length; i++) {
			int charIndex = Array.IndexOf(CharacterSet.PersianChars, pageContent[i]);
			textIndex = textIndex * CharacterSet.CharCount + charIndex;
		}

		// Map index to position
		BigInteger totalPagesPerHex = MaxWalls  * MaxShelves * MaxVolumes * PagesPerBook;
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