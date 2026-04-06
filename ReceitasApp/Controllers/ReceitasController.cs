using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReceitasApp.Data;
using ReceitasApp.Models;
using ReceitasApp.ViewModels;


namespace ReceitasApp.Controllers
{
    public class ReceitasController : Controller
    {
        private readonly AppDbContext _context;

        public ReceitasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Receitas
        public IActionResult Index()
        {
            return RedirectToAction(nameof(Gerir));
        }

        // GET: Receitas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var receita = await _context.Receitas
                .Include(r => r.Ingredientes) 
                    .ThenInclude(ir => ir.Ingrediente) 
                .FirstOrDefaultAsync(m => m.Id == id);
            if (receita == null)
            {
                return NotFound();
            }

            return View(receita);
        }

        // GET: Receitas/Create
        public IActionResult Create()
        {
            var viewModel = new ReceitaCreateViewModel();

                    viewModel.Dificuldades = new List<SelectListItem>
            {
                new SelectListItem { Value = "Facil", Text = "Fácil" },
                new SelectListItem { Value = "Medio", Text = "Médio" },
                new SelectListItem { Value = "Dificil", Text = "Difícil" }
            };

                    viewModel.Categorias = new List<SelectListItem>
            {
                new SelectListItem { Value = "Entrada", Text = "Entrada" },
                new SelectListItem { Value = "PratoPrincipal", Text = "Prato Principal" },
                new SelectListItem { Value = "Sobremesa", Text = "Sobremesa" },
                new SelectListItem { Value = "Bebida", Text = "Bebida" }
            };

                    viewModel.Medidas = new List<SelectListItem>
            {
                new SelectListItem { Value = "Unidade", Text = "Unidade" },
                new SelectListItem { Value = "Grama", Text = "Grama" },
                new SelectListItem { Value = "Kg", Text = "Kg" },
                new SelectListItem { Value = "Ml", Text = "Ml" },
                new SelectListItem { Value = "Litro", Text = "Litro" }
            };

            return View(viewModel);
        }

        // POST: Receitas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReceitaCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var receita = new Receita
            {
                Nome = model.Nome,
                TempoPreparacao = model.TempoPreparacao,
                NumeroPessoas = model.NumeroPessoas,
                Dificuldade = model.Dificuldade,
                Categoria = model.Categoria,
                Preparacao = model.Preparacao
            };

            _context.Receitas.Add(receita);
            await _context.SaveChangesAsync();

            // salvar ingredientes
            foreach (var item in model.Ingredientes)
            {
                if (string.IsNullOrWhiteSpace(item.Nome))
                    continue;

                var ingrediente = new Ingrediente
                {
                    Nome = item.Nome
                };

                _context.Ingredientes.Add(ingrediente);
                await _context.SaveChangesAsync();

                var ingredienteReceita = new IngredienteDaReceita
                {
                    ReceitaId = receita.Id,
                    IngredienteId = ingrediente.Id,
                    Quantidade = item.Quantidade,
                    Medida = item.Medida
                };

                _context.IngredientesDaReceita.Add(ingredienteReceita);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Receitas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var receita = await _context.Receitas.FindAsync(id);
            if (receita == null)
            {
                return NotFound();
            }
            return View(receita);
        }

        // POST: Receitas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,TempoPreparacao,NumeroPessoas,Dificuldade,Categoria,Preparacao")] Receita receita)
        {
            if (id != receita.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(receita);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReceitaExists(receita.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Gerir));
            }
            return View(receita);
        }

        // GET: Receitas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var receita = await _context.Receitas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (receita == null)
            {
                return NotFound();
            }

            return View(receita);
        }

        // POST: Receitas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var receita = await _context.Receitas.FindAsync(id);
            if (receita != null)
            {
                _context.Receitas.Remove(receita);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Gerir));
        }

        private bool ReceitaExists(int id)
        {
            return _context.Receitas.Any(e => e.Id == id);
        }

        public async Task<IActionResult> Consulta(ReceitaFiltroViewModel filtro)
        {
            var query = _context.Receitas.AsQueryable();

            if (!string.IsNullOrEmpty(filtro.Nome))
            {
                query = query.Where(r => r.Nome.Contains(filtro.Nome));
            }

            if (filtro.TempoPreparacao.HasValue)
            {
                query = query.Where(r => r.TempoPreparacao == filtro.TempoPreparacao);
            }

            if (filtro.NumeroPessoas.HasValue)
            {
                query = query.Where(r => r.NumeroPessoas == filtro.NumeroPessoas);
            }

            if (!string.IsNullOrEmpty(filtro.Dificuldade))
            {
                query = query.Where(r => r.Dificuldade == filtro.Dificuldade);
            }

            if (!string.IsNullOrEmpty(filtro.Categoria))
            {
                query = query.Where(r => r.Categoria == filtro.Categoria);
            }

            var viewModel = new ReceitaFiltroViewModel
            {
                Nome = filtro.Nome,
                TempoPreparacao = filtro.TempoPreparacao,
                NumeroPessoas = filtro.NumeroPessoas,
                Dificuldade = filtro.Dificuldade,
                Categoria = filtro.Categoria,

                Dificuldades = new List<SelectListItem>
        {
            new SelectListItem { Value = "Facil", Text = "Fácil" },
            new SelectListItem { Value = "Medio", Text = "Médio" },
            new SelectListItem { Value = "Dificil", Text = "Difícil" }
        },

                Categorias = new List<SelectListItem>
        {
            new SelectListItem { Value = "Entrada", Text = "Entrada" },
            new SelectListItem { Value = "PratoPrincipal", Text = "Prato Principal" },
            new SelectListItem { Value = "Sobremesa", Text = "Sobremesa" },
            new SelectListItem { Value = "Bebida", Text = "Bebida" }
        },

                Receitas = await query.ToListAsync()
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Gerir()
        {
            var receitas = await _context.Receitas.ToListAsync();
            return View(receitas);
        }
    }
}
