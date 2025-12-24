

static class SimulationTools
{
    static public readonly Color[] colors = [Color.Red, Color.Blue, Color.Green];

    static public int SelectedColor
    {
        get;
        set
        {
            if (value >= 0 && value < colors.Length)
            {
                field = value;
            }
            else
            {
                Console.WriteLine("value out of index bound for colors");
            }
        }
    }
    static public Modes ToolMode { get; set; } = Modes.DevicesMode;
    static public Placement PlacementMode { get; set; } = Placement.Create;
    public static int counter = 0;

    // devices.Clear();
    // EndDevice.Reset();

    static public List<Device> devices = [];
}

enum Modes
{
    DevicesMode, ConnectionMode, MessageMode
}

enum Placement
{
    Create, Delete, Moving
}