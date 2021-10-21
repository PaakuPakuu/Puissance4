using System;

namespace Puissance4
{
    public class GameOptions
    {
        private const int _minLines = 6;
        private const int _maxLines = 20;
        private const int _minColumns = 8;
        private const int _maxColumns = 30;
        private const int _minTokensToWin = 3;
        private const int _minPlayers = 2;
        private const int _maxPlayers = 32;

        private int _boardLines;
        private int _boardColumns;
        private int _tokensToWin;

        public int MinLines => _minLines;
        public int MaxLines => _maxLines;
        public int MinColumns => _minColumns;
        public int MaxColumns => _maxColumns;
        public int MinTokensToWin => _minTokensToWin;
        public int MaxTokensToWin => Math.Min(_boardLines, _boardColumns);
        public int MinPlayers => _minPlayers;
        public int MaxPlayers => _maxPlayers;

        public int BoardLines
        {
            get { return _boardLines; }
            set
            {
                if (value < _minLines || value > _maxLines)
                {
                    _boardLines = _minLines;
                }
                else
                {
                    _boardLines = value;
                }
            }
        }

        public int BoardColumns
        {
            get { return _boardColumns; }
            set
            {
                if (value < _minColumns || value > _maxColumns)
                {
                    _boardColumns = _minColumns;
                }
                else
                {
                    _boardColumns = value;
                }
            }
        }

        public int TokensToWin
        {
            get { return _tokensToWin; }
            set
            {
                if (value < _minTokensToWin)
                {
                    _tokensToWin = _minTokensToWin;
                }
                else if (value > MaxTokensToWin)
                {
                    _tokensToWin = MaxTokensToWin;
                }
                else
                {
                    _tokensToWin = value;
                }
            }
        }

        public GameOptions(int boardLines, int boardColumns, int tokensToWin)
        {
            _boardLines = boardLines;
            _boardColumns = boardColumns;
            _tokensToWin = tokensToWin;
        }
    }
}
