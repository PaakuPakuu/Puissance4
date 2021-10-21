using System;
using System.Collections.Generic;
using System.Text;

namespace Puissance4
{
    public class Token
    {
        public Player Owner;

        public Token(Player owner)
        {
            Owner = owner;
        }
    }
}
