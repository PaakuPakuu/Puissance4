namespace Puissance4
{
    public class Board
    {
        private readonly int[] _tokensInColumn;

        public int Lines;
        public int Columns;
        public Token[,] Tokens;

        public Board(int lines, int columns)
        {
            Lines = lines;
            Columns = columns;

            _tokensInColumn = new int[columns];

            for (int i = 0; i < columns; i++)
            {
                _tokensInColumn[i] = 0;
            }
        }

        public bool InsertToken(Token token, int columnIndex)
        {
            if (columnIndex < 0 || columnIndex >= Columns || _tokensInColumn[columnIndex] >= Columns - 2)
            {
                return false;
            }

            Tokens[_tokensInColumn[columnIndex]++, columnIndex] = token;

            return true;
        }

        public Token GetToken(int line, int column)
        {
            // TODO : Exception ?
            return Tokens[line % Lines, column % Columns];
        }

        public bool HasAlignedTokens(int column, int tokensToWin)
        {
            // vecteurs : (0, 1) (1, 0) (1, 1) (1, -1)
            int[][] vectors = new int[4][];

            vectors[0] = new int[2] { 0, 1 };
            vectors[1] = new int[2] { 1, 0 };
            vectors[2] = new int[2] { 1, 1 };
            vectors[3] = new int[2] { 1, -1 };

            // Vérifie dans les 4 directions, chacune dans les 2 sens
            foreach (int[] vector in vectors)
            {
                if (CheckFromVector(vector, _tokensInColumn[column] - 1, column, tokensToWin))
                {
                    return true;
                }
            }

            return false;
        }

        private bool CheckFromVector(int[] vector, int line, int column, int tokensToWin)
        {
            int tokensCounter = 1;
            short direction = 1;
            int cursor = 1;

            int iChecker;
            int jChecker;
            Token token = Tokens[line, column];

            bool changeVector = false;

            while (!changeVector)
            {
                iChecker = line + cursor * vector[0] * direction;
                jChecker = column + cursor * vector[1] * direction;

                if (!IsValid(iChecker, jChecker) || Tokens[iChecker, jChecker].Owner != token.Owner)
                {
                    if (direction == 1)
                    {
                        direction = -1;
                    }
                    else // == -1
                    {
                        changeVector = true;
                    }

                    cursor = 1;
                }
                else
                {
                    cursor++;
                    tokensCounter++;
                }
            }

            return tokensCounter == tokensToWin;
        }

        private bool IsValid(int line, int column)
        {
            return (line >= 0 && line < Lines) && (column >= 0 && column < Columns);
        }
    }
}
