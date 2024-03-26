using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Watchlist.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Watchlist.Data;
using Watchlist.Models;

namespace Watchlist.Models
{
    public class FilmsController : Controller
    {
        private readonly ApplicationDbContext _context;
        //UPD010: Permet l'accès au UserManager pour accéder à l'utilsateur connecté.
        private readonly UserManager<Utilisateur> _gestionnaire;

        public FilmsController(ApplicationDbContext context, UserManager<Utilisateur> gestionnaire)
        {
            _context = context;
            //UPD010:  variable permettant d'accéder au UserManager.
            _gestionnaire = gestionnaire; 
        }

        //UPD010: récupère l'id de l'utilisateur connecté
        [HttpGet]
        public async Task<string> RecupererIdUtilisateurCourant()
        {
            Utilisateur utilisateur = await GetCurrentUserAsync();
            return utilisateur?.Id;
        }

        //UPD014(BUG010): récupère l'utilisateur connecté
        public async Task<Utilisateur> RecupererUtilisateurCourant()
        {
            Utilisateur utilisateur = await GetCurrentUserAsync();
            return utilisateur;
        }

        //UPD010: GetCurrentUserAsync appelle la méthode GetUserAsync du userManager contextuel de l'app
        private Task<Utilisateur> GetCurrentUserAsync() =>
        _gestionnaire.GetUserAsync(HttpContext.User);

        //UPD011: affiche la liste des films de l'utilisateur connecté en utilisant le modèle : ModeleVueFilm
        // GET: Films
        public async Task<IActionResult> Index()
        {
            //Old code before UPD011
            /*
              return _context.Films != null ? 
                          View(await _context.Films.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Films'  is null.");
            */
            
            //UPD011: récupère l'id utilisateur connecté.
            var idUtilisateur = await RecupererIdUtilisateurCourant();
            //LinQ : select des films. 
            var modele = await _context.Films.Select(x =>
                    new ModeleVueFilm
                    {
                        IdFilm = x.Id,
                        Titre = x.Titre,
                        Annee = x.Annee
                    }).ToListAsync();

            //Parcours les films.
            foreach (var item in modele)
            {
                //Parcours le DbSet (jeu d'enregistrement) des films (FilmsUtlisateur, voir fichier dans le dossier Data de l'app dans Visual Studio)
                //pour l'utilisateur connecté et le film sélectionné
                var m = await _context.FilmsUtilisateur.FirstOrDefaultAsync(x =>
                           x.IdUtilisateur == idUtilisateur && x.IdFilm == item.IdFilm);
                if (m != null)
                {
                    item.PresentDansListe = true;
                    item.Note = m.Note;
                    item.Vu = m.Vu;
                }
                else
                {
                    //BUG008: les propriétés du film ne sont pas initialisées quand il ne fait pas partie de la liste de ses films.
                    item.PresentDansListe = false;
                    item.Note = 0;
                    item.Vu = false;
                }
            }
            //BUG009 : la vue est basée sur le modèle Film.cs et non ModeleVueFilm.cs donc logiquement exception ==> 
            /*
                    InvalidOperationException: The model item passed into the ViewDataDictionary is of type 
                    'System.Collections.Generic.List`1[Watchlist.Models.ModeleVueFilm]', but this ViewDataDictionary 
                    instance requires a model item of type 'System.Collections.Generic.IEnumerable`1[Watchlist.Data.Film]'.
             */
            //BUG009 : il faut reprendre la vue Index.cshtml du dossier Views/Films pour la baser sur le modèle ModeleVueFilm.cs
            return View(modele);
}

//UPD012: méthode ajouterSupprimer un film de sa liste
//BUG006: le code de OCR est incorrect, avec des oublis et une logique de code incohérente.
//ajout des paramètres en entrée id (film) et val (sélectionné dans sa liste 0(non) et 1(oui).
//NOTE003 : async methode comme elle contient des instructions async.
//permet d'éviter que le dbContext change avant le traitement
//des instructions async comme SaveChangesAsync.
[HttpGet]
public async Task<JsonResult> AjouterSupprimer(int id, int val)
{

    //on initialise la valeur de retour
    int valret = -1;  

    //on récupère l'utilisateur connecté
    var idUtilisateur = await RecupererIdUtilisateurCourant();

    //***** il apparait possible que id et val soient null.
    //Si par exemple l'utilisateur tripote l'url.
    // le code n'est pas très solide****** on renverrait -1 dans ce cas ?    

    //BUG007: valret est la valeur de retour... Grosse erreur de codage ici.
    //On veut vérifier si la case à cocher présentDansListe est false (0) ou true (1)
    //cette valeur est censée être en entrée de la méthode dans un paramètre Val.
    //Le code if (valret == 1) este modifié pour if (val == 1)
    if (val == 1)
    {
        // s'il existe un enregistrement dans FilmsUtilisateur qui contient à la fois l'identifiant de l'utilisateur
        // et celui du film, alors le film existe dans la liste de films et peut
        // être retiré de la liste de l'utilisateur.
        var film = _context.FilmsUtilisateur.FirstOrDefault(x =>
                x.IdFilm == id && x.IdUtilisateur == idUtilisateur);
        if (film != null)
        {
            _context.FilmsUtilisateur.Remove(film);
            valret = 0; //= non coché
        }

    }
    else
    {
        // le film n'est pas dans la liste de films, nous devons donc
        // créer un nouvel objet FilmUtilisateur et l'ajouter à la base de données.
                
        //BUG010: le film n'est pas ajouté à la liste des films de l'utilisateur
        /* old code (BUG010)
            * 
        _context.FilmsUtilisateur.Add(
        new FilmUtilisateur
        {
            IdUtilisateur = idUtilisateur,
            IdFilm = id,
            Vu = false,
            Note = 0               
        });
        */

        //UPD014(BUG010): on renseigne les clés étrangères Utilisateur et Film.
        FilmUtilisateur filmUtil = new FilmUtilisateur()
        {
            IdUtilisateur = idUtilisateur,
            IdFilm = id,
            Vu = false,
            Note = 0
        };
        //renseigne le film sélectionné
        Film selectFilm = _context.Films.FirstOrDefault(s => s.Id == id);                
        filmUtil.Film = selectFilm;                
        //renseigne l'utilisateur
        filmUtil.User = await RecupererUtilisateurCourant();
        //ajoute le film à la liste des favoris de l'utilisateur.
        //_context.Add(filmUtil);
        _context.FilmsUtilisateur.Add(filmUtil);

        /* test for BUG010 FIX
        //BUG010 fix3.1 : on vérifie le ContextId entre un emplacement où la sauvegarde ne fonctionne pas et un autre
        // vérifier le _context.ContextId; entre les emplacements ou à la sauvergarde fonctionne et là ou elle ne fonctionne pas.

        //BUG010 fix5: On force l'EntityState en modifié.
        _context.Entry(filmUtil).State = EntityState.Added;

        //BUG010 fix1: vérifier la chaine de connexion si ok avant sauvegarde.
        string connection = _context.Database.GetConnectionString();
        //BUG010 fix2: vérifier mapping, schéma et table.
        var mapping = _context.Model.FindEntityType(typeof(FilmUtilisateur));
        string schema = mapping.GetSchema();
        string table = mapping.GetTableName();

        //BUG010 fix4: on vérifie qu'il y a bien des changes identifiés et on vérifie le nombre de change effectué.
        _context.ChangeTracker.AutoDetectChangesEnabled = true;
        _context.ChangeTracker.DetectChanges();
        bool hasChanges = _context.ChangeTracker.HasChanges(); // should be 
        //await _context.SaveChangesAsync();                

        //BUG010 fix6: forcer une transaction pour pouvoir la commiter.
        var transaction = _context.Database.BeginTransaction();
        _context.Add(filmUtil);
        var hasTransaction = _context.Database.CurrentTransaction != null;
        transaction.Commit();
        //var updates = _context.SaveChanges();
        END test for BUG010 FIX*/ 
        
        valret = 1; // = coché
        //await _context.SaveChangesAsync();              
    }


    // nous pouvons maintenant enregistrer les changements dans la base de données
    await _context.SaveChangesAsync();

    // Renvoi si film supprimé ou ajouté à la liste (-1 (non trouvé), 0 ou 1) au script qui a appelé
    // cette méthode depuis la page Index
    return Json(valret);    
 }

// GET: Films/Details/5
public async Task<IActionResult> Details(int? id)
{
if (id == null || _context.Films == null)
{
    return NotFound();
}

var film = await _context.Films
    .FirstOrDefaultAsync(m => m.Id == id);
if (film == null)
{
    return NotFound();
}

return View(film);
}

// GET: Films/Create
public IActionResult Create()
{
return View();
}

// POST: Films/Create
// To protect from overposting attacks, enable the specific properties you want to bind to.
// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
[HttpPost]
[ValidateAntiForgeryToken]
// NOTE001 : La liste Bind définit les champs contrôlés 
public async Task<IActionResult> Create([Bind("Id,Titre,Annee")] Film film)
{
//NOTE001 : vérification de la validité du formulaire avant création de l'enregristrement
if (ModelState.IsValid)
{
    //Met à jour le DbContexte de l'app
    _context.Add(film);
    //BUG010 fix3.2 : on vérifie le ContextId entre un emplacement où la sauvegarde ne fonctionne pas et un autre
    // vérifier le _context.ContextId; entre les emplacements ou à la sauvergarde fonctionne et là ou elle ne fonctionne pas.                

    //Sauvegarde en BDD en asynchrone
    await _context.SaveChangesAsync();
    //Renvoi à l'index des films après création
    return RedirectToAction(nameof(Index));
}
return View(film);
}

// GET: Films/Edit/5
public async Task<IActionResult> Edit(int? id)
{
if (id == null || _context.Films == null)
{
    return NotFound();
}

var film = await _context.Films.FindAsync(id);
if (film == null)
{
    return NotFound();
}
return View(film);
}

// POST: Films/Edit/5
// To protect from overposting attacks, enable the specific properties you want to bind to.
// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Edit(int id, [Bind("Id,Titre,Annee")] Film film)
{
if (id != film.Id)
{
    return NotFound();
}

if (ModelState.IsValid)
{
    try
    {
        _context.Update(film);
        await _context.SaveChangesAsync();
    }
    catch (DbUpdateConcurrencyException)
    {
        if (!FilmExists(film.Id))
        {
            return NotFound();
        }
        else
        {
            throw;
        }
    }
    return RedirectToAction(nameof(Index));
}
return View(film);
}

// GET: Films/Delete/5
public async Task<IActionResult> Delete(int? id)
{
if (id == null || _context.Films == null)
{
    return NotFound();
}

var film = await _context.Films
    .FirstOrDefaultAsync(m => m.Id == id);
if (film == null)
{
    return NotFound();
}

return View(film);
}

// POST: Films/Delete/5
[HttpPost, ActionName("Delete")]
[ValidateAntiForgeryToken]
public async Task<IActionResult> DeleteConfirmed(int id)
{
if (_context.Films == null)
{
    return Problem("Entity set 'ApplicationDbContext.Films'  is null.");
}
var film = await _context.Films.FindAsync(id);
if (film != null)
{
    _context.Films.Remove(film);
}

await _context.SaveChangesAsync();
return RedirectToAction(nameof(Index));
}

private bool FilmExists(int id)
{
return (_context.Films?.Any(e => e.Id == id)).GetValueOrDefault();
}
}
}
