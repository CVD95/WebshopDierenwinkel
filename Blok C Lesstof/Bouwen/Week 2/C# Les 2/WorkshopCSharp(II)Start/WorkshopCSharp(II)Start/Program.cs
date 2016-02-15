using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WorkshopCSharp_II_Start.Models;
using WorkshopCSharp_II_Start.DatabaseControllers;

namespace WorkshopCSharp_II_Start
{
    class Program
    {
        static GenreController genreController = new GenreController();
        
        static void Main(string[] args)
        {
            clearDataBase();
            TestGenreController();
            clearDataBase();
            Console.ReadKey();

            Console.Write("----------");

        }

        static void clearDataBase()
        {
            genreController.DeleteAllGenres();          
        }

        static void TestGenreController()
        {
            
            PrintGenres();

            Genre genre1 = new Genre { Naam = "Platformer", Verslavend = false };
            Genre genre2 = new Genre { Naam = "Retro", Verslavend = true };
            
            genreController.InsertGenre(genre1);
            genreController.InsertGenre(genre2);

            

            PrintGenres();

        }

        static void PrintGenres()
        {
            Console.WriteLine("Bestaande genres in database:");
            List<Genre> genres = genreController.GetGenres();
            if (genres.Count == 0)
            {
                Console.WriteLine("Geen genres in database aanwezig");
                return;
            }
            else
            {
                foreach (Genre genre in genres)
                {
                    Console.WriteLine(genre);
                }
            }
            
        }
    }
}
