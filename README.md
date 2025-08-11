# Persian Library of Babel

## Overview
The **Persian Library of Babel** is a C# WPF application inspired by Jorge Luis Borges' short story *"The Library of Babel"* and the digital implementation at [libraryofbabel.info](https://libraryofbabel.info/). This project simulates a virtual library containing every possible page of text in Persian, using a defined character set (33 Persian letters, space, comma, and period). Each page is uniquely addressable by a hexadecimal ID, wall, shelf, volume, and page number, generating deterministic content based on these coordinates.

The application allows users to:
- **Generate Content**: Input a specific library position (Hex ID, Wall, Shelf, Volume, Page) to retrieve a page of 3200 characters.
- **Search Text**: Enter a text string (up to 3200 characters) to find its exact position in the library.
- **Clear Inputs**: Reset the interface for a new query.

Built with simplicity and adherence to SOLID principles, this project uses MaterialDesignInXaml for a modern UI and Serilog for minimal, precise logging. It avoids complex frameworks like MVVM, prioritizing readable and maintainable code.

## The Library of Babel Concept
The Library of Babel, as envisioned by Borges, is a theoretical library containing every possible book that can be written with a given alphabet. Each book has 410 pages, with each page containing 40 lines of 80 characters. By using a finite character set, the library encompasses every conceivable combination of these characters, including every possible story, poem, or random string.

The [libraryofbabel.info](https://libraryofbabel.info/) project digitizes this concept, generating content algorithmically based on a position in the library. Our project adapts this idea for the Persian language, using a 35-character set (33 Persian letters, space, comma, period) to create a deterministic, infinite library of Persian text.

## Features
- **Interactive UI**: Built with WPF and MaterialDesignInXaml for a clean, responsive interface in Persian.
- **Content Generation**: Generates a unique 3200-character page based on the input position (Hex ID, Wall, Shelf, Volume, Page).
- **Text Search**: Finds the exact position of a given text in the library, if it exists within the character set.
- **Validation**: Ensures valid inputs with range checks (e.g., Wall: 1-4, Shelf: 1-5, Volume: 1-32, Page: 1-410).
- **Logging**: Uses Serilog for minimal, high-insight logging to aid debugging without altering logic.
- **Persian Character Set**: Supports 33 Persian letters plus space, comma, and period for text generation.

## Prerequisites
- **.NET 8.0** or later
- **Visual Studio 2022** (or compatible IDE)
- **MaterialDesignInXaml** NuGet package
- **Serilog** NuGet packages (Serilog.AspNetCore, Serilog.Sinks.File)

## Installation
1. Clone the repository:
   ```bash
   git clone [https://github.com/your-username/PersianLibraryOfBabel](https://github.com/FarokhiHouman/PersianLibraryOfBabel/).git
