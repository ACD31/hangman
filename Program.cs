using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Welcome to Hangman!");
        // Prompt for player name
        Console.Write("Enter your name: ");
        string playerName = Console.ReadLine();

        Console.Clear();

        //list of movies
        List<string> movies = new List<string>()
        {
            "Home Alone",
            "Jaws",
            "test1",
            "test2",
            "test3"
        };

        List<PlayerGame> history = new List<PlayerGame>();

        bool running = true;
        while (running)
        {
            Console.WriteLine("\n--- Hangman Menu ---");
            Console.WriteLine("1. Play again");
            Console.WriteLine("2. View statistics");
            Console.WriteLine("3. Quit");
            Console.Write("Choose an option: ");

            string choiceText = Console.ReadLine();
            int choice = 0;
            int.TryParse(choiceText, out choice);

            switch (choice)
            {
                case 1:
                    PlayGame(movies, history, playerName);   
                    break;
                case 2:
                    ShowStatistics(history);    
                    break;
                case 3:
                    running = false;
                    break;
                default:
                    Console.WriteLine("Invalid option, please try again.");
                    break;
            }
        }

        Console.WriteLine("Goodbye!");
    }

    static void PlayGame(List<string> films, List<PlayerGame> history, string playerName)
    {
        Random rand = new Random();
        int index = rand.Next(0, films.Count);
        string film = films[index].ToUpper();

        // Build hidden version
        char[] display = new char[film.Length];
        for (int i = 0; i < film.Length; i++)
        {
            display[i] = (film[i] == ' ') ? ' ' : '_';
        }

        List<char> guessedLetters = new List<char>();
        int incorrect = 0;
        int maxIncorrect = 8;
        bool finished = false;

        while (!finished)
        {
            Console.Clear();

            DrawHangman(incorrect);

            Console.WriteLine("\nFilm: " + new string(display));
            Console.WriteLine("Guessed letters: " + string.Join(", ", guessedLetters));
            Console.WriteLine("Incorrect guesses: " + incorrect + "/" + maxIncorrect);

            Console.Write("Guess a letter: ");
            string input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input) || input.Length != 1)
            {
                Console.WriteLine("Please enter a single letter.");
                continue;
            }

            char guess = Char.ToUpper(input[0]);

            if (guessedLetters.Contains(guess))
            {
                Console.WriteLine("You already guessed that letter.");
                continue;
            }

            guessedLetters.Add(guess);

            bool found = false;
            for (int i = 0; i < film.Length; i++)
            {
                if (film[i] == guess)
                {
                    display[i] = guess;
                    found = true;
                }
            }

            if (!found)
            {
                incorrect++;
                Console.WriteLine("Incorrect guess.");
            }
            else
            {
                Console.WriteLine("Correct!");
            }

            if (new string(display) == film)
            {
                Console.WriteLine("\nYou guessed it! The film was: " + film);
                finished = true;

                history.Add(new PlayerGame()
                {
                    PlayerName = playerName,
                    FilmTitle = film,
                    Won = true,
                    IncorrectGuesses = incorrect
                });
            }
            else if (incorrect >= maxIncorrect)
            {
                Console.WriteLine("\nYou lost! The film was: " + film);
                finished = true;

                history.Add(new PlayerGame()
                {
                    PlayerName = playerName,
                    FilmTitle = film,
                    Won = false,
                    IncorrectGuesses = incorrect
                });
            }
        }
    }
    static void ShowStatistics(List<PlayerGame> history)
    {
        if (history.Count == 0)
        {
            Console.WriteLine("No games played yet.");
            return;
        }

        int total = history.Count;
        int won = 0;
        int lost = 0;

        Console.WriteLine("\n--- Game History ---");
        foreach (PlayerGame game in history)
        {
            Console.WriteLine($"Player: {game.PlayerName}, Film: {game.FilmTitle}, Result: {(game.Won ? "Won" : "Lost")}, Incorrect guesses: {game.IncorrectGuesses}");
            if (game.Won) won++;
            else lost++;
        }

        Console.WriteLine("\n--- Summary ---");
        Console.WriteLine("Total games: " + total);
        Console.WriteLine("Games won: " + won);
        Console.WriteLine("Games lost: " + lost);
    }

    static void DrawHangman(int incorrect)
    {
        Console.WriteLine("\n +----+");
        Console.WriteLine(" |    |");

        Console.WriteLine(incorrect >= 1 ? " O    |" : "      |");

        if (incorrect >= 3) Console.WriteLine("/|\\   |");
        else if (incorrect == 2) Console.WriteLine("/|    |");
        else if (incorrect == 1) Console.WriteLine(" |    |");
        else Console.WriteLine("      |");

        if (incorrect >= 5) Console.WriteLine("/ \\   |");
        else if (incorrect == 4) Console.WriteLine("/     |");
        else Console.WriteLine("      |");

        Console.WriteLine("      |");
        Console.WriteLine(" =======");
    }
}

class PlayerGame
{
    public string PlayerName = string.Empty;
    public string FilmTitle = string.Empty;
    public bool Won;
    public int IncorrectGuesses;
}
