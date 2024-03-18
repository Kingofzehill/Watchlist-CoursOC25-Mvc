using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Watchlist.Data;
using Watchlist.Models;
using Microsoft.AspNetCore.Authorization;

namespace Watchlist.Controllers
{
    //UPD010 : sécruise la classe qui ne peut être utilisé si l'utilisateur n'est pas authentifié
    [Authorize]
    public class ListeFilmsController : Controller
    {
        //UPD002 contexte de la base de données.
        private readonly ApplicationDbContext _contexte;
        //UPD003 UserManager.
        private readonly UserManager<Utilisateur> _gestionnaire;

        //Injection de dépendances Base de données (DbContext) et UserManager
        //dans le controleur.
        public ListeFilmsController(ApplicationDbContext contexte,
      UserManager<Utilisateur> gestionnaire)
        {
            //contextes BDD et Gestionnaire utilisateurs 
            _contexte = contexte;
            _gestionnaire = gestionnaire;
        }

        //UPD004 : utilisateur connecté
        private Task<Utilisateur> GetCurrentUserAsync() =>
   _gestionnaire.GetUserAsync(HttpContext.User);

        //UPD004 : recupère l'utilisateur connecté via une requête HTTP GET
        [HttpGet]
        public async Task<string> RecupererIdUtilisateurCourant()
        {
            Utilisateur utilisateur = await GetCurrentUserAsync();
            return utilisateur?.Id;
        }

        //UPD004 : Id de l'utilisateur connecté
        // la méthode passe async (nécessaire pour appeler les méthodes du service UserManager)
        public async Task<IActionResult> Index()
        {
            var id = await RecupererIdUtilisateurCourant();

            //UPD005 select de la liste des films de l'utilisateur (LINQ) et
            //la retourne dans la Vue.
            // LINQ : films de l'utilisateur.
            var filmsUtilisateur = _contexte.FilmsUtilisateur.Where(x => x.IdUtilisateur == id);
            // Liste des films selon le modèle défini.
            var modele = filmsUtilisateur.Select(x => new ModeleVueFilm
            {
                IdFilm = x.IdFilm,
                Titre = x.Film.Titre,
                Annee = x.Film.Annee,
                Vu = x.Vu,
                PresentDansListe = true,
                Note = x.Note
            }).ToList();

            // Liste des films retournée dans la vue.
            return View(modele);
        }
    }
}
