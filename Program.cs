#region basic window setup
const int screenWidth = 800;
const int screenHeight = 450;

InitWindow(screenWidth, screenHeight, "network project");

SetTargetFPS(60);
SetWindowIcon(LoadImage("icon.png"));
#endregion

Device? SelectedDevice = null;
Device? clickedDevice = null;
Device? HovredDevice = null;
Message? message = null;
Vector2 clickedDevicePosition = new(0, 0);
List<Vector2> messagePathTrace = [];
string logs = "x";
Device movingDevice = null;
Vector2 mouseDistance = new();
EndDevice.Reset();

while (!WindowShouldClose())
{
	BeginDrawing();

	ClearBackground(Color.Gray);
	SimulationTools.devices.ForEach((device) => device.Draw());

	#region  interaction

	DrawRectangleV(UI.bgStart, UI.bgEnd, Color.Black);

	Vector2 MP = GetMousePosition();

	if (IsMouseButtonPressed(MouseButton.Left) && IsVector2InField(MP, UI.bgStart, UI.bgEnd))
	{
		// SumulationTools.devices.Clear();
		// EndDevice.Reset();
	}
	#endregion
	else
	{

		#region mode and color picker
		switch (GetKeyPressed())
		{
			case (int)KeyboardKey.Q:
				SimulationTools.SelectedColor = (SimulationTools.SelectedColor + 1) % 3;
				break;

			case (int)KeyboardKey.W:
				SimulationTools.ToolMode = (Modes)((int)(SimulationTools.ToolMode + 1) % 3);
				SelectedDevice = null;
				break;

			case (int)KeyboardKey.R:
				SimulationTools.devices.Clear();
				EndDevice.Reset();
				break;
			case (int)KeyboardKey.E:
				SimulationTools.PlacementMode = (Placement)((int)(SimulationTools.PlacementMode + 1) % 3);
				SelectedDevice = null;
				break;
			default:
				break;
		}
		#endregion

		#region select device
		HovredDevice = null;
		SimulationTools.devices.ForEach((device) =>
		{

			if (device.Clicked())
			{
				clickedDevice = device;
				if (SelectedDevice == null) SelectedDevice = clickedDevice;
				else if (SelectedDevice == device) SelectedDevice = null;
			}

			if (device.IsHovered()) HovredDevice = device;
		});
		#endregion

		#region generating

		#region device mode
		if (SimulationTools.ToolMode == Modes.DevicesMode)
		{
			if (SimulationTools.PlacementMode == Placement.Create)
			{

				if (IsMouseButtonPressed(MouseButton.Left))
				{
					EndDevice device = new(MP, SimulationTools.colors[SimulationTools.SelectedColor]);
					SimulationTools.devices.Add(device);
				}
				if (IsMouseButtonPressed(MouseButton.Right))
				{
					NetworkSwitch @switch = new(MP, SimulationTools.colors[SimulationTools.SelectedColor]);
					SimulationTools.devices.Add(@switch);
				}
			}
			else if (SimulationTools.PlacementMode == Placement.Delete)
			{
				if (IsMouseButtonPressed(MouseButton.Left))
				{
					SimulationTools.devices.ForEach((device) =>
					{
						device.Connections.Remove(clickedDevice);
					});
					SimulationTools.devices.Remove(clickedDevice);
					SelectedDevice = null;
				}
			}
			else
			{
				if (SelectedDevice != null)
				{
					SelectedDevice.Position = GetMousePosition();
				}
			}
		}
		#endregion

		#region conncetion mode

		else if (SimulationTools.ToolMode == Modes.ConnectionMode)
		{
			if (SelectedDevice != null && HovredDevice != null)
			{
				DrawLineV(SelectedDevice.Position, HovredDevice.Position, Color.Black);
			}
			if (IsMouseButtonPressed(MouseButton.Left))
			{
				if (SelectedDevice == null && clickedDevice != null)
				{
					SelectedDevice = clickedDevice;
				}
				else if (SelectedDevice != null
						 && clickedDevice != null
						 && SelectedDevice != clickedDevice)
				{
					if (SimulationTools.PlacementMode == Placement.Create)
					{
						if (!SelectedDevice.Connections.Contains(clickedDevice))
						{
							SelectedDevice.Connections.Add(clickedDevice);
							clickedDevice.Connections.Add(SelectedDevice);
						}
					}
					else
					{
						SelectedDevice.Connections.Remove(clickedDevice);
						clickedDevice.Connections.Remove(SelectedDevice);
					}

					SelectedDevice = null;
					clickedDevice = null;
				}
			}
		}
		#endregion

		#region message mode

		else if (SimulationTools.ToolMode == Modes.MessageMode)
		{
			if (IsMouseButtonPressed(MouseButton.Left) && message == null)
			{
				// first click
				if (SelectedDevice == null && clickedDevice is EndDevice)
				{
					SelectedDevice = clickedDevice;
				}
				// second click
				else if (SelectedDevice != null && clickedDevice is EndDevice target && SelectedDevice != clickedDevice)
				{
					var path = (SelectedDevice as EndDevice).Search(target.ID);
					if (path.Count > 0)
					{
						message = new(
							(SelectedDevice as EndDevice).ID,
							target.ID,
							path,
							SelectedDevice.Position
						);
						logs = "successful";
					}
					else { logs = "failed"; }
					SelectedDevice = null;
					clickedDevice = null;
				}
			}
		}
		#endregion
		#endregion

	}
	#region drawing elements
	SimulationTools.devices.ForEach((device) => device.Draw());

	//selected device indecator
	if (SelectedDevice != null && (SimulationTools.PlacementMode != Placement.Moving || SimulationTools.ToolMode == Modes.MessageMode))
	{
		DrawCircleLinesV(SelectedDevice.Position, 35, Color.Green);
	}
	#endregion


	#region drawing message
	if (message != null && message.Path.Count > 0)
	{
		Vector2 targetPos = message.Path.Peek().Position;
		message.Position = Vector2MoveTowards(message.Position, targetPos, 5);

		Vector2 direction = Vector2Normalize(Vector2Subtract(message.Path.Peek().Position, message.Position));

		Vector2[] tri = new Vector2[3];
		float size = 15;
		tri[0] = Vector2Add(message.Position, Vector2Rotate(direction, 0) * size);

		tri[1] = Vector2Add(message.Position, Vector2Rotate(direction, DEG2RAD * -150) * size);

		tri[2] = Vector2Add(message.Position, Vector2Rotate(direction, DEG2RAD * 150) * size);
		messagePathTrace.Add(message.Position);
		for (int i = 0; i < messagePathTrace.Count - 1; i++)
		{
			DrawLineEx(messagePathTrace[i], messagePathTrace[i + 1], 3, Color.White);
		}
		DrawTriangle(tri[0], tri[1], tri[2], Color.White);
		if (Vector2Distance(message.Position, targetPos) < 1.0f)
		{
			message.Path.Dequeue();
		}

		if (message.Path.Count == 0)
		{
			message = null;
			messagePathTrace.Clear();
		}
	}

	#endregion

	#region UI drawing 
	UI.Update();
	#endregion

	// logs = devicePlaceholder?.ToString();
	// logs = $"";
	DrawText(logs, 0 + (int)UI.Offset.X, screenHeight - 60 + ((int)UI.Offset.Y), 30, logs == "successful" ? Color.Green : Color.Red);

	EndDrawing();
}

CloseWindow();

static bool IsVector2InField(Vector2 v, Vector2 point1, Vector2 scale)
{
	if (v.X >= point1.X && v.X <= point1.X + scale.X && v.Y >= point1.Y && v.Y <= point1.Y + scale.Y)
		return true;
	return false;
}
