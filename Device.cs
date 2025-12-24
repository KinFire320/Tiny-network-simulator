public abstract class Device(Vector2 position, Color color)
{

	public Vector2 Position { get; set; } = position;
	public Color Color { get; set; } = color;
	public List<Device> Connections { set; get; } = [];
	public Message? memory;

	public bool Clicked() => IsHovered() && IsMouseButtonPressed(MouseButton.Left);

	public bool IsHovered() => CheckCollisionPointCircle(GetMousePosition(), Position, 35);

	public abstract void Draw();
}



internal class EndDevice : Device
{
	private static int index = 0;
	public int ID { get; private set; }

	public EndDevice(Vector2 position, Color color) : base(position, color)
	{
		index++;
		ID = index;
	}

	public Queue<Device> Search(int targetID)
	{
		Queue<Device> queue = new();
		Dictionary<Device, Device?> parents = [];

		queue.Enqueue(this);
		parents[this] = null;

		Device? targetDevice = null;

		while (queue.Count > 0)
		{
			Device current = queue.Dequeue();

			if (current is EndDevice ed && ed.ID == targetID)
			{
				targetDevice = current;
				break;
			}

			foreach (var neighbor in current.Connections)
			{
				if (!parents.ContainsKey(neighbor))
				{
					parents[neighbor] = current;
					queue.Enqueue(neighbor);
				}
			}
		}

		if (targetDevice == null) return new Queue<Device>();

		// Reconstruct path
		List<Device> pathList = [];
		Device? curr = targetDevice;
		while (curr != null)
		{
			pathList.Add(curr);
			curr = parents[curr];
		}
		pathList.Reverse();
		return new Queue<Device>(pathList);
	}

	public static void Reset() => index = 0;

	public override void Draw()
	{
		if (IsHovered())
		{
			if (SimulationTools.ToolMode == Modes.DevicesMode)
			{
				if (SimulationTools.PlacementMode == Placement.Delete)
					DrawCircleLinesV(Position, 35, Color.Red);

				if (SimulationTools.PlacementMode == Placement.Moving)
					DrawCircleLinesV(Position, 35, Color.Blue);
			}
			if (SimulationTools.ToolMode == Modes.ConnectionMode)
			{
				if (SimulationTools.PlacementMode == Placement.Delete)
					DrawCircleLinesV(Position, 35, Color.Red);

				if (SimulationTools.PlacementMode == Placement.Create)
					DrawCircleLinesV(Position, 35, Color.White);
			}
			if (SimulationTools.ToolMode == Modes.MessageMode)
			{
				DrawCircleLinesV(Position, 35, Color.White);
			}
		}
		Connections.ForEach((device) => DrawLineEx(Position, Position - (Position - device.Position) / 2f, 5, Color));
		Rectangle ComputerBase = new(Position + new Vector2(-13.125f, 13.125f), new Vector2(26.25f, 7.5f));
		DrawRectangleRec(ComputerBase, Color);

		Rectangle ComputerScreen = new(Position - new Vector2(20, 15), new Vector2(40, 30));
		DrawRectangleRec(ComputerScreen, Color.Black);
		string text = $"PC-{ID}";
		DrawTextEx(new Font(), text, new Vector2(ComputerScreen.Center.X - MeasureText(text, 20) / 2, ComputerScreen.Center.Y - 35), 20, 2, Color.White);
	}
}

class NetworkSwitch(Vector2 position, Color color) : Device(position, color)
{
	public override void Draw()
	{
		if (IsHovered()) DrawCircleLinesV(Position, 35, Color.White);

		Connections.ForEach((device) => DrawLineEx(Position, Position - (Position - device.Position) / 2f, 5, Color));
		Rectangle rect = new(Position - new Vector2(40, 15), new(80, 30));
		DrawRectangleRec(rect, Color.Red);
		Rectangle rect1 = new(rect.Position + new Vector2(5, 5), new Vector2(20));
		DrawRectangleRec(rect1, Fade(Color.White, .15f));
		rect1.Position += new Vector2(25, 0);
		DrawRectangleRec(rect1, Fade(Color.White, .15f));
		DrawCircleV(rect1.Center + new Vector2(25, 0), 10, Fade(Color.White, .15f));
		DrawCircleV(rect1.Center + new Vector2(25, 0), 5, Fade(Color.White, .15f));

	}
}
