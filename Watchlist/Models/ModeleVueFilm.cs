// add a reference to System.ComponentModel.DataAnnotations DLL
using System.ComponentModel.DataAnnotations;
//using MessagePack;

namespace Watchlist.Models
{
    public class ModeleVueFilm
    {
        // BUG002 : specifies key identifier (IdFilm).
        // Resolve the bug which prevent to create a
        // Razor View of this model in Listefilms dir.
        // https://learn.microsoft.com/en-us/aspnet/mvc/overview/getting-started/getting-started-with-ef-using-mvc/creating-a-more-complex-data-model-for-an-asp-net-mvc-application
        //[System.ComponentModel.DataAnnotations.Key]
        [Key()]
        public int IdFilm { get; set; }
        public string Titre { get; set; }
        public int Annee { get; set; }
        public bool PresentDansListe { get; set; }
        public bool Vu { get; set; }
        public int? Note { get; set; }
    }
}
