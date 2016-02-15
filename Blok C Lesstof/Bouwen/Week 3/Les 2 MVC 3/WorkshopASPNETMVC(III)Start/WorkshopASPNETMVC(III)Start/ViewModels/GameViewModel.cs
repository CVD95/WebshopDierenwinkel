using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using WorkshopASPNETMVC_III_Start.Models;


namespace WorkshopASPNETMVC_III_Start.ViewModels
{
    public class GameViewModel
    {
        public Game Game { get; set; }
        public SelectList Genres { get; set; }
        
        
        [Range(1, Double.MaxValue, ErrorMessage="Het is verplicht om een genre te selecteren")]
        public int SelectedGenreID { get; set; }
        
    }
}