using AutoSzerelo.Shared;
using Microsoft.AspNetCore.Mvc;

namespace AutoSzerelo.Controllers;

[ApiController]
[Route("munka")]
public class MunkaController : ControllerBase
{
    private IMunkaService _munkaService;

    public MunkaController(IMunkaService munkaService)
    {
        _munkaService = munkaService;
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] Munka munka)
    {
        var existingMunka = await _munkaService.GetAsync(munka.Id);

        if (existingMunka is not null)
        {
            return Conflict();
        }

        await _munkaService.AddAsync(munka);

        return Ok();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var munka = await _munkaService.GetAsync(id);

        if (munka is null)
        {
            return NotFound();
        }

        await _munkaService.DeleteAsync(id);

        return Ok();
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Munka>> Get(Guid id)
    {
        var munka = await _munkaService.GetAsync(id);

        if (munka is null)
        {
            return NotFound();
        }

        return Ok(munka);
    }

    [HttpGet]
    public async Task<ActionResult<List<Munka>>> Get()
    {
        return Ok(await _munkaService.GetAllAsync());
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] Munka newMunka)
    {
        if (id != newMunka.Id)
        {
            return BadRequest();
        }

        var existingMunka = await _munkaService.GetAsync(id);

        if (existingMunka is null)
        {
            return NotFound();
        }

        await _munkaService.UpdateAsync(newMunka);

        return Ok();
    }
}