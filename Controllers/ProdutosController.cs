using APICatalogo.Filters;
using APICatalogo.Models;
using APICatalogo.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers;

[Route("[controller]")]
[ApiController]
public class ProdutosController : ControllerBase
{
    private readonly IUnitOfWork _uow;
    public ProdutosController(IUnitOfWork uow)
    {
        _uow = uow;
    }

    [HttpGet]
    [ServiceFilter(typeof(ApiLoggingFilter))]
    public ActionResult<IEnumerable<Produto>> Get()
    {
        try
        {
            var produtos = _uow.ProdutoRepository.Get().ToList();

            if (produtos is null)
                return NotFound("Produtos não encontrados...");

            return produtos;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [HttpGet("menorpreco")]
    public ActionResult<IEnumerable<Produto>> GetProdutosPrecos()
    {
        try
        {
            var produtos = _uow.ProdutoRepository.GetProdutosPorPreco().ToList();

            if (produtos is null)
                return NotFound("Produto não encontrado...");

            return produtos;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [HttpGet("{id:int:min(1)}", Name = "ObterProduto")]
    public ActionResult<Produto> Get(int id)
    {
        try
        {
            var produto = _uow.ProdutoRepository.GetById(p => p.ProdutoId == id);
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

            _uow.ProdutoRepository.Add(produto);
            _uow.Commit();

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

            _uow.ProdutoRepository.Update(produto);
            _uow.Commit();

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
            var produto = _uow.ProdutoRepository.GetById(p => p.ProdutoId == id);
            if (produto is null)
                return NotFound($"Produto com ID {id} não encontrado...");

            _uow.ProdutoRepository.Delete(produto);
            _uow.Commit();

            return Ok(produto);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}
