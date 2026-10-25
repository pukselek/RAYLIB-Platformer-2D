using System.Numerics;
using Raylib_cs;

namespace src.settings;

public class ButtonBuild
{
    public Rectangle Background { get; set; }
    public Color BackgroundColor { get; set; } = Color.White;
    public int ButtonWidth { get; set; }
    public int ButtonHeight { get; set; }
    public int ButtonFontSize { get; set; }
    public string? ButtonText { get; set; } = default;
    public Color ButtonTextColor { get; set; }
    public bool IsActionActivated { get; set; } = false;
    public bool IsHover { get; set; } = false;
    public float XPos { get; set; }
    public float YPos { get; set; }
    public ButtonBuild(float X = 0, float Y = 0, int WIDTH = 200, int HEIGHT = 150, int TEXT_SIZE = 16, string TEXT = "Hello World!", Color? TEXT_COLOR = null)
    {
        IsActionActivated = false;
        ButtonWidth = WIDTH;
        ButtonHeight = HEIGHT;
        ButtonText = TEXT;
        ButtonTextColor = TEXT_COLOR ?? Color.Black;
        ButtonFontSize = TEXT_SIZE;

        XPos = X;
        YPos = Y;

        Background = new Rectangle(XPos, YPos, ButtonWidth, ButtonHeight);
    }
    public bool OnButtonClick(Vector2 MousePoint, Action action = null!)
    {
        if (Raylib.CheckCollisionPointRec(MousePoint, Background))
        {
            IsHover = true;
            BackgroundColor = Color.LightGray;
            if (Raylib.IsMouseButtonPressed(MouseButton.Left))
            {
                action?.Invoke();
                return true;
            }
        }
        else
        {
            IsHover = false;
            BackgroundColor = Color.White;
        }
        return false;
    }
    public void Draw()
    {
        Raylib.DrawRectangle((int)XPos, (int)YPos, ButtonWidth, ButtonHeight, BackgroundColor);
        if (!string.IsNullOrEmpty(ButtonText))
        {
            int TextWidth = Raylib.MeasureText(ButtonText, ButtonFontSize);
            int TextHeight = ButtonFontSize;

            float TextX = XPos + ((ButtonWidth - TextWidth) / 2f);
            float TextY = YPos + ((ButtonHeight - TextHeight) / 2f);

            Raylib.DrawText(ButtonText, (int)TextX, (int)TextY, ButtonFontSize, ButtonTextColor);
        }
    }
}