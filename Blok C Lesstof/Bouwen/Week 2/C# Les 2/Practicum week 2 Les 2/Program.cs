using System;
using System.Collections.Generic;
using System.Linq;
using WorkshopCSharp_II_Start.Models;
using WorkshopCSharp_II_Start.DatabaseControllers;

namespace WorkshopCSharp_II_Start
{
    class Program
    {
        static GenreController genreController = new GenreController();
        
       public static void Main(string[] args)
        {
            Console.WriteLine("Genres toegevoegd");
            Genre genreTest = new Genre {ID = 117 , Naam = "plutformer", Verslavend = true };
           Genre genreUpdate = new Genre { ID = 117 , Naam = "supertester" , Verslavend = false};
            genreController.GetGenres();

            Console.WriteLine("----------");
            genreController.InsertGenre(genreTest);

            Console.WriteLine(genreTest);

            genreController.UpdateGenre(genreUpdate);

            Console.WriteLine(genreUpdate);

            genreController.DeleteGenre(117);

           Console.WriteLine("Genre succesvol verwijderd");
            Console.WriteLine("----------");

            genreController.DeleteAllGenres();
            Console.WriteLine("alle genres zijn verwijderd!");
            Console.WriteLine("----------");

            Console.WriteLine("");

            genreController.InsertGenre(genreUpdate);

            Console.WriteLine(TestGenreController());

            Console.ReadKey();
           //wacht op input van de gebruiker.
        }

        static void clearDataBase()
        {
            genreController.DeleteAllGenres();          
        }

        static string TestGenreController()
        {
            
            PrintGenres();

            Genre genre1 = new Genre { Naam = "Platformer", Verslavend = false };
            Genre genre2 = new Genre { Naam = "Retro", Verslavend = true };
            
            genreController.InsertGenre(genre1);
            genreController.InsertGenre(genre2);

            PrintGenres();

            return "Testdata tgoegevoegd";

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
