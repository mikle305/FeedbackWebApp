namespace WebAPI.Models;

public class Message
{
    public int Id { get; set; }

    public int ContactId { get; set; }    
    
    public int TopicId { get; set; }
    
    public string Body { get; set; }
    
    public DateTime SentAt { get; set; }
    
    
    public virtual Contact Contact { get; set; }
    
    public virtual Topic Topic { get; set; }
}