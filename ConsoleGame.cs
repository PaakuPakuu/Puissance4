using System;
using System.Text;

namespace Puissance4
{
    public enum Menu
    {
        Main,
        Game,
        Options
    }

    public class ConsoleGame
    {
        private const string MENU_CHOICES = "noq";
        private const string OPTION_CHOICES = "lcjq";
        private const string TOKENS = "XO#@~0¤%$ABCDEFGHIJKLMPQRSTUVWYZ";

        private Game _game;
        private Menu _menu;
        private bool _closeConsole;
        //private SceneManager _sceneManager;

        public void LaunchConsoleGame()
        {
            _menu = Menu.Main;
            _game = new Game();
            _closeConsole = false;

            //_sceneManager = new SceneManager();

            while (!_closeConsole)
            {
                //Old version
                switch (_menu)
                {
                    case Menu.Main:
                        StartingMenu();
                        break;
                    case Menu.Game:
                        OnGameMenu();
                        break;
                    case Menu.Options:
                        OptionsMenu();
                        break;
                    default:
                        break;
                }

                //_sceneManager.ShowActiveScene();

                Console.Clear();
            }

            Console.WriteLine("Copyright : Jordan Hereng");
            Console.ReadKey();
        }

        #region Starting menu

        private void StartingMenu()
        {
            char option;

            Console.WriteLine("    ===== Jeu du Puissance 4 =====\n");
            Console.WriteLine($"        n - Nouvelle partie");
            Console.WriteLine($"        o - Options");
            Console.WriteLine($"        q - Quitter\n");

            Console.Write($"Que souhaitez-vous faire ? [{String.Join('/', MENU_CHOICES)}] > ");
            option = GetOptionFromInput(MENU_CHOICES);

            switch (option)
            {
                case 'n':
                    _menu = Menu.Game;
                    break;
                case 'o':
                    _menu = Menu.Options;
                    break;
                case 'q':
                    _closeConsole = true;
                    break;
                default:
                    break;
            }
        }

        /*
        private bool AskToGoToOptionsMenu()
        {
            string output = Console.ReadLine().ToLower();

            while (output != "oui" && output != "non")
            {
                if (output == "")
                {
                    return false;
                }

                Console.Write("Répondez \"oui\" ou \"non\" : ");
                output = Console.ReadLine().ToLower();
            }

            return output == "oui";
        }
        */

        #endregion

        #region On game menu

        private void OnGameMenu()
        {
            Console.WriteLine("Combien de joueurs êtes vous ?");

            int players = GetNumberFromInput(_game.GameOptions.MinPlayers, _game.GameOptions.MaxPlayers);

            Console.WriteLine();

            string[] names = GetPlayersName(players);
            int columnPlayed = 0;
            bool canPlay;
            bool hasAligned = false;
            char display;

            _game.StartGame(players, names);

            // Boucle de jeu
            while (_game.IsPlaying)
            {
                Console.Clear();
                ShowBoard();

                display = TOKENS[_game.CurrentPlayer.PlayerID];
                Console.WriteLine($"{display} Tour de {_game.CurrentPlayer.Name} {display}\n");
                Console.Write("Jouez votre coup : ");

                canPlay = false;

                while (!canPlay)
                {
                    columnPlayed = GetNumberFromInput(1, _game.Board.Columns) - 1;
                    canPlay = _game.CurrentPlayer.Play(columnPlayed);

                    if (!canPlay)
                    {
                        Console.WriteLine("Vous ne pouvez pas jouer ce coup.");
                    }
                }

                hasAligned = _game.Board.HasAlignedTokens(columnPlayed, _game.GameOptions.TokensToWin);

                if (hasAligned || _game.IsBoardFull)
                {
                    _game.EndGame();
                } else
                {
                    _game.ChangePlayer();
                }
            }

            Console.Clear();
            ShowBoard();

            if (hasAligned)
            {
                Console.WriteLine($"{_game.CurrentPlayer.Name} a remporté la partie en {_game.CurrentPlayer.TokensPlayed} coups sur {_game.Turn} !");
            } else
            {
                Console.WriteLine($"Match nul.");
            }

            Console.ReadKey();

            _menu = Menu.Main;
        }

        private void ShowBoard()
        {
            StringBuilder boardSB = new StringBuilder();
            char display;
            Token token;

            // Show column indexes
            for (int i = 0; i < _game.Board.Columns; i++)
            {
                boardSB.Append($"{(i >= 10 ? " " : "  ")}{i + 1} ");
            }

            boardSB.AppendLine();

            // Show the board
            for (int i = 0; i < _game.Board.Lines; i++)
            {
                boardSB.Append('-', _game.Board.Columns * 4 + 1).AppendLine();
                boardSB.Append("|");

                for (int j = 0; j < _game.Board.Columns; j++)
                {
                    token = _game.Board.Tokens[_game.Board.Lines - i - 1, j];
                    display = (token.Owner == null ? ' ' : TOKENS[token.Owner.PlayerID]);
                    boardSB.Append($" {display} |");
                }

                boardSB.AppendLine();
            }

            boardSB.Append('-', _game.Board.Columns * 4 + 1).AppendLine();

            Console.WriteLine(boardSB.ToString());
        }

        private string[] GetPlayersName(int players)
        {
            string[] names = new string[players];
            string input;

            for (int i = 0; i < players; i++)
            {
                Console.Write($"Entrez le nom du {i + 1}{(i == 0 ? "er" : "ème")} joueur : ");
                input = Console.ReadLine();
                names[i] = (input.Contains(' ') || input == string.Empty ? $"Joueur {i + 1}" : input);
            }

            return names;
        }

        #endregion

        #region Option menu

        private void OptionsMenu()
        {
            bool stopSetting = false;
            char option;

            while (!stopSetting)
            {
                Console.WriteLine("Menu des options de jeu :\n");
                Console.WriteLine($"l - Nombre de lignes du plateau : {_game.GameOptions.BoardLines}");
                Console.WriteLine($"c - Nombre de colonnes du plateau : {_game.GameOptions.BoardColumns}");
                Console.WriteLine($"j - Nombre de jetons à aligner : {_game.GameOptions.TokensToWin}\n");
                Console.WriteLine($"q - Quitter le menu des options\n");

                Console.Write($"Que souhaitez-vous modifier ? [{String.Join('/', OPTION_CHOICES)}] > ");
                option = GetOptionFromInput(OPTION_CHOICES);

                stopSetting = !AskToChangeOption(option);

                Console.Clear();
            }

            _menu = Menu.Main;
        }

        private bool AskToChangeOption(char option)
        {
            int input;

            switch (option)
            {
                case 'l':
                    Console.WriteLine("Indiquez le nouveau nombre de lignes");
                    input = GetNumberFromInput(_game.GameOptions.MinLines, _game.GameOptions.MaxLines);
                    _game.GameOptions.BoardLines = input;
                    break;
                case 'c':
                    Console.WriteLine("Indiquez le nouveau nombre de colonnes");
                    input = GetNumberFromInput(_game.GameOptions.MinColumns, _game.GameOptions.MaxColumns);
                    _game.GameOptions.BoardColumns = input;
                    break;
                case 'j':
                    Console.WriteLine("Indiquez le nouveau nombre de jetons à aligner");
                    input = GetNumberFromInput(_game.GameOptions.MinTokensToWin, _game.GameOptions.MaxTokensToWin);
                    _game.GameOptions.TokensToWin = input;
                    break;
                case 'q':
                    return false;
                default:
                    break;
            }

            return true;
        }

        #endregion

        private int GetNumberFromInput(int minRange, int maxRange)
        {
            bool stopInput = false;
            bool validInput = true;
            int result = 0;

            while (!stopInput)
            {
                if (!validInput)
                {
                    Console.WriteLine($"La valeur doit être comprise entre {minRange} et {maxRange}.");
                }

                Console.Write("> ");

                validInput = int.TryParse(Console.ReadLine(), out result) && result >= minRange && result <= maxRange;
                stopInput = validInput;
            }

            return result;
        }

        private char GetOptionFromInput(string options)
        {
            bool stopInput = false;
            bool validInput = true;
            string input = "";

            while (!stopInput)
            {
                if (!validInput)
                {
                    Console.Write("Cette option n'existe pas...\n> ");
                }

                input = Console.ReadLine().ToLower();

                validInput = input.Length == 1 && options.Contains(input[0]);
                stopInput = validInput;
            }

            return input[0];
        }
    }
}
