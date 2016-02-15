using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hallo_World
{
    class Game : GameGenre
    {
        public int _ID { get; set; }
        public string _naam { get; set; }
        public GameGenre GameGenre { get; private set; }


        public Game(int ID, string naam, GameGenre gameGenre)
        {
            _ID = ID;
            _naam = naam;
            GameGenre = gameGenre;
        }

        public override string ToString()
        {
            return String.Format("Het ID is : {0} De naam is :  {1}  GenreInfo: {2} ",
                                        _ID, _naam, GameGenre);
        }
        
    }
}

