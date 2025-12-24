class UI
{
    public static List<BaseUIElement> UIElements = [];
    static readonly float BoundsWidth = 3;
    public static Vector2 bgStart = new(0, 0f);
    public static Vector2 bgEnd = new(200, 450);
    public static Vector2 Offset = new(5, 5);

    static UI()
    {
        // --- Color Selection ---
        new ShapedUIToggled(() => SimulationTools.SelectedColor == 0, new(0, 0), 30, UIShape.circle, Color.Red, 15)
            .OnClick += () => { SimulationTools.SelectedColor = 0; };

        new ShapedUIToggled(() => SimulationTools.SelectedColor == 1, new(40, 0), 30, UIShape.circle, Color.Blue, 15)
            .OnClick += () => { SimulationTools.SelectedColor = 1; };

        new ShapedUIToggled(() => SimulationTools.SelectedColor == 2, new(80, 0), 30, UIShape.circle, Color.Green, 15)
            .OnClick += () => SimulationTools.SelectedColor = 2;


        // --- Tool Modes (Devices, Connections, Messages) ---
        new TextUIToggle(() => SimulationTools.ToolMode == Modes.DevicesMode, new(0, 60), 10, "device", Color.White)
            .OnClick += () => SimulationTools.ToolMode = Modes.DevicesMode;

        new TextUIToggle(() => SimulationTools.ToolMode == Modes.ConnectionMode, new(0, 100), 10, "connction", Color.White)
            .OnClick += () => SimulationTools.ToolMode = Modes.ConnectionMode;

        new TextUIToggle(() => SimulationTools.ToolMode == Modes.MessageMode, new(0, 140), 10, "message", Color.White)
            .OnClick += () => SimulationTools.ToolMode = Modes.MessageMode;


        // --- Placement Actions (Create, Delete, Move) ---
        new TextUIToggle(() => SimulationTools.PlacementMode == Placement.Create, new(0, 180), 10, "+", Color.Green)
            .OnClick += () => SimulationTools.PlacementMode = Placement.Create;

        new TextUIToggle(() => SimulationTools.PlacementMode == Placement.Delete, new(40, 180), 10, "-", Color.Red)
            .OnClick += () => SimulationTools.PlacementMode = Placement.Delete;

        new TextUIToggle(() => SimulationTools.PlacementMode == Placement.Moving, new(80, 180), 10, "@", Color.Blue)
            .OnClick += () => SimulationTools.PlacementMode = Placement.Moving;


        // --- Utilities (Clear) ---
        new TextUIButton(new(0, 210), 10, "clear")
            .OnClick += () =>
            {
                SimulationTools.devices.Clear();
                EndDevice.Reset();
            };
    }

    public static void Update()
    {
        // Draw the sidebar background
        DrawRectangleV(bgStart, bgEnd, Color.Black);

        foreach (var element in UIElements)
        {
            // Handle Clicking
            if (element.IsClicked())
            {
                element.OnClick?.Invoke();
                element.Clicked();
            }

            // Render
            element.Draw();

            // Hover effect
            if (element.IsHovered())
            {
                DrawRectangleLinesEx(element.Bounds, BoundsWidth, Color.White);
            }
        }
    }
}
