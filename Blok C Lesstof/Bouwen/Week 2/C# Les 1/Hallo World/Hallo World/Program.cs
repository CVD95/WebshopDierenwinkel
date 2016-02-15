using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Hallo_World
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World");
            GameGenre game1 = new GameGenre { ID = 1, Verslavend = true, Naam = "World of Warcraft" };
            GameGenre game2 = new GameGenre { ID = 2, Verslavend = false, Naam = "Dungeons of Awesome" };
            GameGenre game3 = new GameGenre { ID = 3, Verslavend = true, Naam = "Dwarf Fortress" };
            //Console.WriteLine(game1.ToString());
            Game gameWarcraft = new Game ( 33, "Warcrafter", game1);
            Game dungeons = new Game(22, "Awesome", game2);
            Game drop = new Game(22, "Fortress Mode", game3);
            Console.WriteLine("-----------------");
            //Console.WriteLine(gameWarcraft.ToString());

            List<Game> games = new List<Game>();

            games.Add(gameWarcraft);
            games.Add(dungeons);
            games.Add(drop);

            isGameVerslavend(games);

            Console.ReadKey();
            

        }


        public static void isGameVerslavend(List<Game> games)
        {

            foreach(Game game in games)
            {
                if(game.GameGenre.Verslavend == true)
                {
                    Console.WriteLine(game);
                }
            }

        }


    }
}
