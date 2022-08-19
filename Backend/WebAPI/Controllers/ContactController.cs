using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.DB;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContactController: ControllerBase
{
    private readonly ApplicationContext _db;

    public ContactController(ApplicationContext db)
    {
        _db = db;
    }
    
    [HttpGet("GetInfoById")]
    private async Task<IActionResult> GetInfoById(int id)
    {
        var contact = (await _db.Contacts.ToListAsync()).FirstOrDefault(c => c.Id == id);
        if (contact == null)
            return Ok(new
            {
                Success = false,
                Error = "Topic with current id doesn't exist"
            });
        return Ok(new 
        {
            Success = true,
            Contact = new
            {
                contact.Id,
                contact.Name,
                contact.Email,
                contact.PhoneNumber
            }
        });
    }
}