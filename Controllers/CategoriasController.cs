using APICatalogo.DTOs;
using APICatalogo.Models;
using APICatalogo.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers;

[Route("[controller]")]
[ApiController]
public class CategoriasController : ControllerBase
{
    private readonly IUnitOfWork _uow;
    private readonly ILogger _logger;
    private readonly IMapper _mapper;

    public CategoriasController(IUnitOfWork uow, ILogger<CategoriasController> logger, IMapper mapper)
    {
        _uow = uow;
        _logger = logger;
        _mapper = mapper;
    }

    [HttpGet("produtos")]
    public ActionResult<IEnumerable<CategoriaDTO>> GetCategoriasProdutos()
    {
        try
        {
            _logger.LogInformation("==================== GET /categorias/produtos ====================");
            var categorias = _uow.CategoriaRepository.GetCategoriasProdutos().ToList();

            if (categorias is null)
                return NotFound("Categorias não encontradas...");

            var categoriasDto = _mapper.Map<List<CategoriaDTO>>(categorias);
            
            return categoriasDto;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [HttpGet]
    public ActionResult<IEnumerable<CategoriaDTO>> Get()
    {
        try
        {
            var categorias = _uow.CategoriaRepository.Get().ToList();

            if (categorias is null)
                return NotFound("Categorias não encontradas...");

            var categoriasDto = _mapper.Map<List<CategoriaDTO>>(categorias);
            
            return categoriasDto;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [HttpGet("{id:int:min(1)}", Name = "ObterCategoria")]
    public ActionResult<CategoriaDTO> Get(int id)
    {
        try
        {
            var categoria = _uow.CategoriaRepository.GetById(c => c.CategoriaId == id);

            if (categoria is null)
                return NotFound($"Categoria com ID {id} não encontrada...");

            var categoriaDto = _mapper.Map<CategoriaDTO>(categoria);
            
            return categoriaDto;
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar a solicitação.");
        }
    }

    [HttpPost]
    public ActionResult Post(CategoriaDTO categoriaDto)
    {
        try
        {
            var categoria = _mapper.Map<Categoria>(categoriaDto);
            if (categoria is null)
                return BadRequest("Dados inválidos");

            _uow.CategoriaRepository.Add(categoria);
            _uow.Commit();

            var categoriaDTO = _mapper.Map<CategoriaDTO>(categoria);

            return new CreatedAtRouteResult("ObterCategoria", new { id = categoria.CategoriaId }, categoriaDTO);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [HttpPut("{id:int:min(1)}")]
    public ActionResult Put(int id, Categoria categoriaDto)
    {
        try
        {
            if (id != categoriaDto.CategoriaId)
                return BadRequest("Dados inválidos");

            var categoria = _mapper.Map<Categoria>(categoriaDto);
            _uow.CategoriaRepository.Update(categoria);
            _uow.Commit();

            var categoriaDTO = _mapper.Map<CategoriaDTO>(categoria);
            return Ok(categoriaDTO);
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
            var categoria = _uow.CategoriaRepository.GetById(c => c.CategoriaId == id);
            if (categoria is null)
                return NotFound($"Categoria com ID {id} não encontrada...");

            _uow.CategoriaRepository.Delete(categoria);
            _uow.Commit();

            var categoriaDto = _mapper.Map<CategoriaDTO>(categoria);
            return Ok(categoriaDto);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}