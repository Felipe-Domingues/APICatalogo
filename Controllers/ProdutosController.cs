using APICatalogo.DTOs;
using APICatalogo.Filters;
using APICatalogo.Models;
using APICatalogo.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers;

[Route("[controller]")]
[ApiController]
public class ProdutosController : ControllerBase
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;
    public ProdutosController(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    [HttpGet]
    [ServiceFilter(typeof(ApiLoggingFilter))]
    public ActionResult<IEnumerable<ProdutoDTO>> Get()
    {
        try
        {
            var produtos = _uow.ProdutoRepository.Get().ToList();

            if (produtos is null)
                return NotFound("Produtos não encontrados...");

            var produtosDto = _mapper.Map<List<ProdutoDTO>>(produtos);
            return produtosDto;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [HttpGet("menorpreco")]
    public ActionResult<IEnumerable<ProdutoDTO>> GetProdutosPrecos()
    {
        try
        {
            var produtos = _uow.ProdutoRepository.GetProdutosPorPreco().ToList();

            if (produtos is null)
                return NotFound("Produto não encontrado...");

            var produtosDto = _mapper.Map<List<ProdutoDTO>>(produtos);
            return produtosDto;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [HttpGet("{id:int:min(1)}", Name = "ObterProduto")]
    public ActionResult<ProdutoDTO> Get(int id)
    {
        try
        {
            var produto = _uow.ProdutoRepository.GetById(p => p.ProdutoId == id);
            if (produto is null)
                return NotFound($"Produto com ID {id} não encontrado...");

            var produtoDto = _mapper.Map<ProdutoDTO>(produto);
            return produtoDto;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [HttpPost]
    public ActionResult Post(ProdutoDTO produtoDto)
    {
        try
        {
            var produto = _mapper.Map<Produto>(produtoDto);
            if (produto is null)
                return BadRequest("Dados inválidos");

            _uow.ProdutoRepository.Add(produto);
            _uow.Commit();

            var produtoDTO = _mapper.Map<ProdutoDTO>(produto);

            return new CreatedAtRouteResult("ObterProduto", new { id = produto.ProdutoId }, produtoDTO);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [HttpPut("{id:int:min(1)}")]
    public ActionResult Put(int id, ProdutoDTO produtoDto)
    {
        try
        {
            if (id != produtoDto.ProdutoId)
                return BadRequest("Dados inválidos");
            
            var produto = _mapper.Map<Produto>(produtoDto);

            _uow.ProdutoRepository.Update(produto);
            _uow.Commit();
            
            var produtoDTO = _mapper.Map<ProdutoDTO>(produto);

            return Ok(produtoDTO);
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

            var produtoDto = _mapper.Map<ProdutoDTO>(produto);
            
            return Ok(produtoDto);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}
