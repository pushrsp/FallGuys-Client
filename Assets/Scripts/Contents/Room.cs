using Google.Protobuf.Protocol;

public class Room
{
    public int Idx { get; set; }
    public string Title { get; set; }
    public int PlayerCount { get; set; }
    public RoomState State { get; set; }
}