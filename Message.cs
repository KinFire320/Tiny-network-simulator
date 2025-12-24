// Replace the entire content of Message.cs with this:
public class Message(int senderID, int reciverID, Queue<Device> path, Vector2 position)
{
    public Vector2 Position = position;
    public int SenderID { get; private set; } = senderID;
    public int ReeciverID { get; private set; } = reciverID;
    // Changed from Stack to Queue for easier forward-navigation
    public Queue<Device> Path = path;
}
