# Persian Library of Babel

## Overview
The Persian Library of Babel is a C# WPF application inspired by Jorge Luis Borges' short story "The Library of Babel" and the online implementation at [libraryofbabel.info](https://libraryofbabel.info). This project simulates a virtual library containing all possible pages of text in Persian, using a character set of 33 Persian letters, space, comma, and period. Each page is uniquely addressable by a hexadecimal ID, wall, shelf, volume, and page number, with content generated deterministically from these coordinates.

The application supports the following operations:
- **Generate Content**: Input a specific library location (Hex ID, Wall, Shelf, Volume, Page) to retrieve a 3200-character page.
- **Search Text**: Input a text string (up to 3200 characters) to find its precise location in the library.
- **Clear Inputs**: Reset the interface for a new request.

Built with simplicity and SOLID principles, the project uses **MaterialDesignInXaml** for a modern UI and **Serilog** for clean, precise logging. It avoids heavy frameworks like MVVM, prioritizing readable and maintainable code.

## The Library of Babel Concept
Jorge Luis Borges' *Library of Babel* envisions a theoretical library containing all possible books writable with a given alphabet. Each book is 410 pages, with each page containing 40 lines of 80 characters. Using a finite character set, the library includes every possible combination of characters, encompassing all stories, poems, or even meaningless strings.

The [libraryofbabel.info](https://libraryofbabel.info) project digitizes this concept, generating content algorithmically based on library coordinates. This project adapts the idea to Persian, using a 35-character set (33 Persian letters, space, comma, period) to create a deterministic, infinite library of Persian text.

## Features
- **Interactive UI**: Built with WPF and MaterialDesignInXaml for a clean, responsive Persian interface.
- **Content Generation**: Generates a unique 3200-character page based on input coordinates (Hex ID, Wall, Shelf, Volume, Page).
- **Text Search**: Locates the exact position of a given text within the library, if it exists in the character set.
- **Validation**: Ensures valid inputs through range checks (e.g., Wall: 1-4, Shelf: 1-5, Volume: 1-32, Page: 1-410).
- **Logging**: Utilizes Serilog for minimal, high-insight logging to aid debugging without altering logic.
- **Persian Character Set**: Supports 33 Persian letters, space, comma, and period for text generation.

## Prerequisites
- **.NET 8.0** or later
- **Visual Studio 2022** (or a compatible IDE)
- **MaterialDesignInXaml** NuGet package
- **Serilog** NuGet packages (Serilog.AspNetCore, Serilog.Sinks.File)

## Installation
1. Clone the repository:
   ```
   git clone https://github.com/FarokhiHouman/PersianLibraryOfBabel.git
   ```
2. Open the solution (`PersianLibraryOfBabel.sln`) in Visual Studio.
3. Restore NuGet packages:
   - Right-click the solution in Solution Explorer and select "Restore NuGet Packages."
4. Build and run the project.

## Usage
### Generate Content
- Enter a valid Hex ID (e.g., `hex123`), Wall (1-4), Shelf (1-5), Volume (1-32), and Page (1-410).
- Click the "Generate Content" button to display the corresponding 3200-character page.

### Search Text
- Enter a Persian text string (up to 3200 characters) using the supported character set.
- Click the "Search" button to find the library position where the text exists.

### Clear Inputs
- Click the "Clear" button to reset all inputs and displayed content.

## Project Structure
- **MainWindow.xaml/.cs**: Defines the WPF UI and interaction logic.
- **LibraryStructure.cs**: Contains the `LibraryPosition` model and Persian character set.
- **ContentGenerator.cs**: Implements content generation and search logic.
- **RangeValidationRule.cs**: Validates numeric inputs for Wall, Shelf, Volume, and Page.
- **App.xaml/.cs**: Configures the application and Serilog logging.

## Logging
The project uses **Serilog** to log significant events (e.g., content generation, search, errors) to a file (`logs/app-.log`). Logs are minimal yet thorough, designed for debugging while adhering to SOLID principles.

## Contributing
Contributions are welcome! To contribute:
1. Fork the repository.
2. Create a feature branch:
   ```
   git checkout -b feature/YourFeature
   ```
3. Commit your changes:
   ```
   git commit -m "Add YourFeature"
   ```
4. Push to the branch:
   ```
   git push origin feature/YourFeature
   ```
5. Open a pull request.

## License
This project is licensed under the MIT License. See the `LICENSE` file for details.

## Acknowledgments
- Inspired by Jorge Luis Borges' *"The Library of Babel."*
- Based on the digital implementation at [libraryofbabel.info](https://libraryofbabel.info).
- Built using **MaterialDesignInXaml** for modern UI elements.
- Utilizes **Serilog** for logging.
