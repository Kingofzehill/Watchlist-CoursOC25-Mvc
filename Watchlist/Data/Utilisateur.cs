namespace Watchlist.Data
{
    // SMO: Utlisateur étend la classe Microsoft.AspNetCore.Identity.IdentityUser (table AspNetUseers)
    // Elle dispose d'un constructeur qui appelle le constructeur de l'objet hérité.
    public class Utilisateur : Microsoft.AspNetCore.Identity.IdentityUser
    {
        public Utilisateur() : base() //Constructeur
        {
            // Instanciation collection de films favoris de l'utilisateur.
            this.ListeFilms = new HashSet<FilmUtilisateur>(); 
        }

        //Attribut Prenom ajouté à la table AspNetUsers
        public string Prenom { get; set; }
        
        // Navigation
        // Liste de films favoris de l'utilisateur
        public virtual ICollection<FilmUtilisateur> ListeFilms { get; set; } 
    }
}
