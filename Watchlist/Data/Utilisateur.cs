namespace Watchlist.Data
{
    // SMO: Utlisateur étend la classe Microsoft.AspNetCore.Identity.IdentityUser
    // Elle dispose d'un constructeur qui appelle le constructeur de l'objet hérité.
    public class Utilisateur : Microsoft.AspNetCore.Identity.IdentityUser
    {
        public Utilisateur() : base() //Constructeur
        {
            // Instanciation collection de films.
            this.ListeFilms = new HashSet<FilmUtilisateur>(); 
        }

        public string Prenom { get; set; }
        // Propriété correspondant à la liste de film.
        public virtual ICollection<FilmUtilisateur> ListeFilms { get; set; } 
    }
}
