using Black_Magic_Backend.Models;

public class ClientSession {
    public Character Character { get; set; } = new Character();
    public DateTime ConnectedAt { get; set; } = DateTime.UtcNow;
}
