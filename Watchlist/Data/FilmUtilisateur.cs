using Humanizer;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Watchlist.Data
{
    public class FilmUtilisateur
    {
        //clé primaire composite 1
        [Key, Column(Order = 0)]
        public int IdFilm { get; set; }
        //clé primaire composite 2
        [Key, Column(Order = 1)]
        public string IdUtilisateur { get; set; }
        public bool Vu { get; set; }
        public int Note { get; set; }
        // SMO: ces propriétés correspondent aux relations
        // avec les classes Film et Utilisateur (clés étrangères).
        //UPD13(BUG010) : on spécifie explicitement les clés étrangères
        //car elles sont part intégrante de la clé primaire composite
        [ForeignKey("IdUtilisateur")]
        public virtual Utilisateur User { get; set; }
        [ForeignKey("IdFilm")]
        public virtual Film Film { get; set; }
    }
}
