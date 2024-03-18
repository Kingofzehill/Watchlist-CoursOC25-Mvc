﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Watchlist.Data;

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
            }
            return View(modele);
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
