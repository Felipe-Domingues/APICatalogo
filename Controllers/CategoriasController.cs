using System;
using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers;

[Route("[controller]")]
[ApiController]
public class CategoriasController : ControllerBase
{
    private readonly AppDbContext _context;

    public CategoriasController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("produtos")]
    public async Task<ActionResult<IEnumerable<Categoria>>> GetCategoriasProdutosAsync()
    {
        try
        {
            var categorias = await _context.Categorias.Include(p => p.Produtos).AsNoTracking().Take(10).ToListAsync();

            if (categorias is null)
                return NotFound("Categorias não encontradas...");

            return categorias;
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar a solicitação.");
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Categoria>>> GetAsync()
    {
        try
        {
            var categorias = await _context.Categorias.AsNoTracking().Take(10).ToListAsync();

            if (categorias is null)
                return NotFound("Categorias não encontradas...");

            return categorias;
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar a solicitação.");
        }
    }

    [HttpGet("{id:int:min(1)}", Name = "ObterCategoria")]
    public async Task<ActionResult<Categoria>> GetAsync(int id)
    {
        try
        {
            var categoria = await _context.Categorias.AsNoTracking().Take(10).FirstOrDefaultAsync(c => c.CategoriaId == id);

            if (categoria is null)
                return NotFound($"Categoria com ID {id} não encontrada...");

            return categoria;
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar a solicitação.");
        }
    }

    [HttpPost]
    public ActionResult Post(Categoria categoria)
    {
        try
        {
            if (categoria is null)
                return BadRequest("Dados inválidos");

            _context.Categorias.Add(categoria);
            _context.SaveChanges();

            return new CreatedAtRouteResult("ObterCategoria", new { id = categoria.CategoriaId }, categoria);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar a solicitação.");
        }
    }

    [HttpPut("{id:int:min(1)}")]
    public ActionResult Put(int id, Categoria categoria)
    {
        try
        {
            if (id != categoria.CategoriaId)
                return BadRequest("Dados inválidos");

            _context.Entry(categoria).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(categoria);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar a solicitação.");
        }
    }

    [HttpDelete("{id:int:min(1)}")]
    public ActionResult Delete(int id)
    {
        try
        {
            var categoria = _context.Categorias.FirstOrDefault(c => c.CategoriaId == id);
            if (categoria is null)
                return NotFound($"Categoria com ID {id} não encontrada...");

            _context.Categorias.Remove(categoria);
            _context.SaveChanges();

            return Ok(categoria);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar a solicitação.");
        }
    }
}