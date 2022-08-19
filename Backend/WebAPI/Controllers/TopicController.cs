using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.DB;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TopicController: ControllerBase
{
    private readonly ApplicationContext _db;

    public TopicController(ApplicationContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var topics = await _db.Topics.ToListAsync();
        return Ok(new
        {
            Success = true,
            Topics = topics
        });
    }
}
