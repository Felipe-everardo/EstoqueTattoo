using EstoqueLiaTattoo.Data;
using EstoqueLiaTattoo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EstoqueLiaTattoo.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriasController : ControllerBase
{
    private readonly EstoqueLiaTattooContext _context;

    public CategoriasController(EstoqueLiaTattooContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Categoria>>> Get()
    {
        return Ok(await _context.Categoria.OrderBy(c => c.Nome).ToListAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Categoria>> Get(int id)
    {
        var categoria = await _context.Categoria.FindAsync(id);
        return categoria == null ? NotFound() : Ok(categoria);
    }
}
