public enum UIShape
{
	circle, square
}

abstract public class BaseUIElement
{
	public Vector2 Position { get; protected set; }
	// public Vector2 size{get; protected set;}
	public Rectangle Bounds { get; protected set; }
	public Action? OnClick { get; set; }
	public bool IsClicked()
	{
		Vector2 mousePos = GetMousePosition();
		return IsMouseButtonPressed(MouseButton.Left) &&
			   CheckCollisionPointRec(mousePos, Bounds);
	}

	abstract public void Clicked();

	public bool IsHovered() => CheckCollisionPointRec(GetMousePosition(), Bounds);

	protected BaseUIElement()
	{
		UI.UIElements.Add(this);
	}

	abstract public void Draw();
}

public class TextUIToggle : UIToggled
{
	readonly string Text;
	readonly int FontSize;
	readonly Color TextColor;
	public TextUIToggle(Func<bool> ToggleFunc, Vector2 position, float padding, string text, Color color, int fontSize = 30) : base(ToggleFunc, position, padding)
	{
		Position = position + UI.Offset;
		FontSize = fontSize;
		Text = text;
		TextColor = color;
		Bounds = new(Position, MeasureText(text, fontSize) + padding, fontSize + padding);
	}

	public override void Draw()
	{
		UpdateToggled();
		DrawText(Text, (int)(Bounds.Center.X - (MeasureText(Text, FontSize) / 2f)), (int)(Bounds.Center.Y - (FontSize / 2f)), FontSize, Toggled ? TextColor : Fade(TextColor, .5f));
	}

	public override void Clicked()
	{
	}
}
public class TextUIButton : UIButton
{
	readonly string Text;
	readonly int FontSize;
	public TextUIButton(Vector2 position, float padding, string text, int fontSize = 30) : base(position, padding)
	{
		Position = position + UI.Offset;
		FontSize = fontSize;
		Text = text;

		Bounds = new(Position, MeasureText(text, fontSize), fontSize);
		Bounds.Grow(padding);
	}

	public override void Draw()
	{
		DrawText(Text, (int)Position.X, (int)Position.Y, FontSize, IsClicked() ? Color.White : Color.Red);
	}

	public override void Clicked()
	{
	}
}

public class ShapedUIToggled : UIToggled
{
	readonly UIShape Shape;
	readonly int Size;
	readonly float GrowSize;
	Color Color;
	public ShapedUIToggled(Func<bool> toggleFunc, Vector2 position, float padding, UIShape shape, Color color, int size = 15, float growSize = 1.5f) : base(toggleFunc, position, padding)
	{
		Bounds = new(position, new(size + padding));
		Shape = shape;
		Size = size;
		Color = color;
		GrowSize = growSize;
	}
	public override void Draw()
	{
		UpdateToggled();
		if (Shape == UIShape.circle)
		{
			DrawCircleV(Bounds.Center, Toggled ? Size * GrowSize : Size, Color);
		}
		if (Shape == UIShape.square)
		{
			DrawRectangleV(Position, Toggled ? new(Size * GrowSize) : new(Size), Color);
		}
	}

	public override void Clicked() { }
}
public abstract class UIButton : BaseUIElement
{
	protected UIButton(Vector2 position, float padding) : base()
	{
		Position = position /*+ UI.Offset*/;
		Bounds = new(Position, Vector2.Zero);
		Bounds.Grow(padding);
	}

	public override void Draw()
	{
	}
}

public abstract class UIToggled : BaseUIElement
{
	public bool Toggled { get; set; }
	readonly Func<bool> ToggleFunc;

	protected UIToggled(Func<bool> toggleFunc, Vector2 position, float padding) : base()
	{
		ToggleFunc = toggleFunc;
		Position = position + UI.Offset;
		Bounds = new(Position, Vector2.Zero);
		Bounds.Grow(padding);
	}

	public void UpdateToggled()
	{
		Toggled = ToggleFunc();
	}
}