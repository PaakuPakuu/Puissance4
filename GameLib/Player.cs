namespace Puissance4
{
    public class Player
    {
        private readonly Game _game;

        public int TokensPlayed { get; private set; }
        public string Name { get; private set; }
        public int PlayerID { get; private set; }

        public Player(Game game, string name, int playerID)
        {
            _game = game;
            PlayerID = playerID;

            TokensPlayed = 0;
            Name = name;
        }

        public bool Play(int columnIndex)
        {
            bool success = _game.Board.InsertToken(new Token(this), columnIndex);

            if (success)
            {
                TokensPlayed++;
            }

            return success;
        }
    }
}
