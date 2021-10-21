using System.Collections.Generic;

namespace Puissance4
{
    public class Game
    {
        private int _currentPlayerIndex;
        private List<Player> _players;

        public GameOptions GameOptions;
        public Board Board { get; private set; }
        public bool IsPlaying { get; private set; }
        public int Turn { get; private set; }
        public bool IsBoardFull { get { return Turn == Board.Lines * Board.Columns; } }
        public Player CurrentPlayer
        {
            get
            {
                return _players[_currentPlayerIndex];
            }
        }

        public Game(int boardLines = 6, int boardColumns = 8, int tokensToWin = 4)
        {
            IsPlaying = false;
            GameOptions = new GameOptions(boardLines, boardColumns, tokensToWin);
        }

        public void StartGame(int player, string[] names)
        {
            InitializeBoard();
            InitializePlayers(player, names);

            Turn = 1;
            IsPlaying = true;
            _currentPlayerIndex = 0;
        }

        public Player ChangePlayer()
        {
            _currentPlayerIndex++;
            _currentPlayerIndex %= _players.Capacity;

            Turn++;

            return _players[_currentPlayerIndex];
        }

        public void EndGame()
        {
            IsPlaying = false;
        }

        private void InitializeBoard()
        {
            Board = new Board(GameOptions.BoardLines, GameOptions.BoardColumns);
            Board.Tokens = new Token[Board.Lines, Board.Columns];

            for (int i = 0; i < Board.Lines; i++)
            {
                for (int j = 0; j < Board.Columns; j++)
                {
                    Board.Tokens[i, j] = new Token(null);
                }
            }
        }

        private void InitializePlayers(int player, string[] names)
        {
            _players = new List<Player>(player);

            for (int i = 0; i < player; i++)
            {
                _players.Add(new Player(this, names[i], i));
            }
        }
    }
}
