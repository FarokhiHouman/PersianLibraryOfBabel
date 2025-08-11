namespace PersianLibraryOfBabel.Models;
// Simple, SOLID-compliant model for Persian Library of Babel
public record LibraryPosition(string HexId, int Wall, int Shelf, int Volume, int Page);
public static class CharacterSet {
	public static int CharCount => PersianChars.Length; // 35
	// Persian character set for content generation
	public static readonly char[] PersianChars = new[] {
														   'ا',
														   'ب',
														   'پ',
														   'ت',
														   'ث',
														   'ج',
														   'چ',
														   'ح',
														   'خ',
														   'د',
														   'ذ',
														   'ر',
														   'ز',
														   'ژ',
														   'س',
														   'ش',
														   'ص',
														   'ض',
														   'ط',
														   'ظ',
														   'ع',
														   'غ',
														   'ف',
														   'ق',
														   'ک',
														   'گ',
														   'ل',
														   'م',
														   'ن',
														   'و',
														   'ه',
														   'ی',
														   ' ',
														   '،',
														   '۔'
													   };
}