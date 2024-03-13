namespace Watchlist.Data
{
    public class FilmUtilisateur
    {
        public string IdUtilisateur { get; set; }
        public int IdFilm { get; set; }
        public bool Vu { get; set; }
        public int Note { get; set; }
        // SMO: ces propriétés correspondent aux relations
        // avec les classes Film et Utilisateur.
        public virtual Utilisateur User { get; set; }
        public virtual Film Film { get; set; }
    }
}
