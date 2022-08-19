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

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll()
    {
        var topics = await _db.Topics.ToListAsync();
        return Ok(new
        {
            Success = true,
            Topics = topics
        });
    }
    
    [HttpGet("GetInfoById")]
    public async Task<IActionResult> GetInfoById(int id)
    {
        var topic = (await _db.Topics.ToListAsync()).FirstOrDefault(t => t.Id == id);
        if (topic == null)
            return Ok(new
            {
                Success = false,
                Error = "Topic with current id doesn't exist"
            });
        return Ok(new 
        {
            Success = true,
            Topic = new
            {
                topic.Id,
                topic.Name
            }
        });
    }
}
