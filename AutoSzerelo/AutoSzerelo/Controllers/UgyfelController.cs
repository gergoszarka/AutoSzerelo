using AutoSzerelo.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace AutoSzerelo.Controllers;

public class UgyfelController : ControllerBase
{
    private IUgyfelService _ugyfelService;

    public UgyfelController(IUgyfelService ugyfelService)
    {
        _ugyfelService = ugyfelService;
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] Ugyfel ugyfel)
    {
        var existingUgyfel = await _ugyfelService.GetAsync(ugyfel.Id);

        if (existingUgyfel is not null)
        {
            return Conflict();
        }

        await _ugyfelService.AddAsync(ugyfel);

        return Ok();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var person = await _ugyfelService.GetAsync(id);

        if (person is null)
        {
            return NotFound();
        }

        await _ugyfelService.DeleteAsync(id);

        return Ok();
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Ugyfel>> Get(Guid id)
    {
        var ugyfel = await _ugyfelService.GetAsync(id);

        if (ugyfel is null)
        {
            return NotFound();
        }

        return Ok(ugyfel);
    }

    [HttpGet]
    public async Task<ActionResult<List<Ugyfel>>> Get()
    {
        return Ok(await _ugyfelService.GetAllAsync());
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] Ugyfel newUgyfel)
    {
        if (id != newUgyfel.Id)
        {
            return BadRequest();
        }

        var existingUgyfel = await _ugyfelService.GetAsync(id);

        if (existingUgyfel is null)
        {
            return NotFound();
        }

        await _ugyfelService.UpdateAsync(newUgyfel);

        return Ok();
    }
}