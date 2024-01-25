using APICatalogo.Context;
using APICatalogo.Filters;
using APICatalogo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers;

[Route("[controller]")]
[ApiController]
public class ProdutosController : ControllerBase
{
    private readonly AppDbContext _context;
    public ProdutosController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [ServiceFilter(typeof(ApiLoggingFilter))]
    public async Task<ActionResult<IEnumerable<Produto>>> GetAsync()
    {
        try
        {
            var produtos = await _context.Produtos.AsNoTracking().Take(10).ToListAsync();

            if (produtos is null)
                return NotFound("Produtos não encontrados...");

            return produtos;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [HttpGet("primeiro")]
    public async Task<ActionResult<Produto>> GetPrimeiroAsync()
    {
        try
        {
            var produto = await _context.Produtos.AsNoTracking().FirstOrDefaultAsync();

            if (produto is null)
                return NotFound("Produto não encontrado...");

            return produto;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [HttpGet("{id:int:min(1)}", Name = "ObterProduto")]
    public async Task<ActionResult<Produto>> GetAsync(int id)
    {
        try
        {
            var produto = await _context.Produtos.AsNoTracking().Take(10).FirstOrDefaultAsync(p => p.ProdutoId == id);
            if (produto is null)
                return NotFound($"Produto com ID {id} não encontrado...");

            return produto;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [HttpPost]
    public ActionResult Post(Produto produto)
    {
        try
        {
            if (produto is null)
                return BadRequest("Dados inválidos");

            _context.Produtos.Add(produto);
            _context.SaveChanges();

            return new CreatedAtRouteResult("ObterProduto", new { id = produto.ProdutoId }, produto);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [HttpPut("{id:int:min(1)}")]
    public ActionResult Put(int id, Produto produto)
    {
        try
        {
            if (id != produto.ProdutoId)
                return BadRequest("Dados inválidos");

            _context.Entry(produto).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(produto);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [HttpDelete("{id:int:min(1)}")]
    public ActionResult Delete(int id)
    {
        try
        {
            var produto = _context.Produtos.FirstOrDefault(p => p.ProdutoId == id);
            if (produto is null)
                return NotFound($"Produto com ID {id} não encontrado...");

            _context.Produtos.Remove(produto);
            _context.SaveChanges();

            return Ok(produto);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}
