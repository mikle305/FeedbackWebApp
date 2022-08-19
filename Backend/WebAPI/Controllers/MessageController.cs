using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.DB;
using WebAPI.Models;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MessageController: ControllerBase
{
    private readonly ApplicationContext _db;

    public MessageController(ApplicationContext db)
    {
        _db = db;
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create(string name, string email, string phoneNumber, 
        int topicId, string body)
    {
        var (success, error) = await ValidateMessageInfo(topicId, body);
        if (!success)
            return Ok(new
            {
                Success = false,
                Error = error
            });
        (success, error) = ValidateContactInfo(name, email, phoneNumber);
        if (!success)
            return Ok(new
            {
                Success = false,
                Error = error
            });
        int contactId = await CreateContactIfNotExists(name, email, phoneNumber);
        var message = new Message { TopicId = topicId, ContactId = contactId, 
            Body = body, IsActive = true, SentAt = DateTime.Now };
        await _db.AddAsync(message);
        await _db.SaveChangesAsync();
        return Ok(new 
        {
            Success = true,
            MessageId = message.Id
        });
    }

    [HttpGet("GetInfoById")]
    public async Task<IActionResult> GetInfoById(int id)
    {
        var message = (await _db.Messages.ToListAsync()).FirstOrDefault(m => m.Id == id);
        if (message == null)
            return Ok(new
            {
                Success = false,
                Error = "Message with current id doesn't exist"
            });
        return Ok(new 
        {
            Success = true,
            Message = new
            {
                message.Id,
                message.TopicId,
                message.ContactId,
                message.Body,
                message.IsActive,
                message.SentAt
            }
        });
    }

    private async Task<int> CreateContactIfNotExists(string name, string email, string phoneNumber)
    {
        var contact = (await _db.Contacts.ToListAsync())
            .FirstOrDefault(c => c.Email == email && c.PhoneNumber == phoneNumber);
        if (contact != null) return contact.Id;
        contact = new Contact { Name = name, Email = email, PhoneNumber = phoneNumber };
        await _db.AddAsync(contact);
        await _db.SaveChangesAsync();
        return contact.Id;
    }

    private Tuple<bool, string> ValidateContactInfo(string name, string email, string phoneNumber)
    {
        string emailPattern = "[.\\-_a-z0-9]+@([a-z0-9][\\-a-z0-9]+\\.)+[a-z]{2,6}";
        string phonePattern = @"^(\+[0-9].{5,20})$";
        string error = string.Empty;
        var isValidName = !string.IsNullOrEmpty(name);
        var isValidEmail = Regex.Match(email, emailPattern, RegexOptions.IgnoreCase).Success;
        var isValidPhone = Regex.Match(phoneNumber, phonePattern).Success;
        for (int i = 0; i < phoneNumber.Length; i++)
        {
            if (i > 0 && !char.IsDigit(phoneNumber[i]))
            {
                isValidPhone = false;
                break;
            }
        }
        
        if (!isValidName)
            error += "Name";
        
        if (!isValidEmail)
        {
            if (error.Length != 0)
                error += ", e";
            else
                error += "E";
            error += "mail";
        }

        if (!isValidPhone)
        {
            if (error.Length != 0)
                error += ", p";
            else
                error += "P";
            error += "hone number";
        }
        
        if (error.Length != 0)
            return Tuple.Create<bool, string>(false, $"{error} is (are) not valid");
        return Tuple.Create(true, string.Empty);
    }

    private async Task<Tuple<bool, string>> ValidateMessageInfo(int topicId, string body)
    {
        if (string.IsNullOrEmpty(body))
            return Tuple.Create(false, "Message body must be filled");
        var topic = (await _db.Topics.ToListAsync())
            .FirstOrDefault( t => t.Id == topicId);
        if (topic == null)
            return Tuple.Create(false, "Topic with current id doesn't exist");
        return Tuple.Create(true, string.Empty);
    }
}
