using MessagePack;
using System.ComponentModel.DataAnnotations;

namespace Watchlist.Models
{
    public class ModeleVueFilm
    {
        // BUG002 : specifies key identifier (IdFilm).
        // Resolve the bug which prevent to create a
        // Razor View of this model in Listefilms dir.
        [System.ComponentModel.DataAnnotations.Key]
        public int IdFilm { get; set; }
        public string Titre { get; set; }
        public int Annee { get; set; }
        public bool PresentDansListe { get; set; }
        public bool Vu { get; set; }
        public int? Note { get; set; }
    }
}
