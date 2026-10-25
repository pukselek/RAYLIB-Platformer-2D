using Raylib_cs;
using src.settings;
using System.Numerics;

namespace src.scenes;

public class Menu : Constructor
{
    public ButtonBuild PlayButton { get; set; } = null!;
    public ButtonBuild AboutGameButton { get; set; } = null!;
    public ButtonBuild ExitGameButton { get; set; } = null!;

    public ButtonBuild ReturnToMenuButton { get; set; } = null!;
    public Rectangle AboutContainerBackground { get; set; }
    public Vector2 MousePoint { get; private set; } = new Vector2(0.0f, 0.0f);

    const int ButtonWidth = 200;
    const int ButtonHeight = 50;
    const int ButtonFontSize = 30;

    const int ContainerWidth = 500;
    const int ContainerHeight = 230;

    public int Clicked { get; set; } = 0;
    public bool AboutContainerVisible { get; set; } = false;

    public override void Init()
    {
        PlayButton = new((1280 / 2f) - (ButtonWidth / 2f), (720 / 2f) - (ButtonHeight / 2f), ButtonWidth, ButtonHeight, ButtonFontSize, "NEW GAME", Color.Black);
        AboutGameButton = new((1280 / 2f) - (ButtonWidth / 2f), (720 / 2f) - (ButtonHeight / 2f) + 60, ButtonWidth, ButtonHeight, ButtonFontSize, "ABOUT", Color.Black);
        ExitGameButton = new((1280 / 2f) - (ButtonWidth / 2f), (720 / 2f) - (ButtonHeight / 2f) + 120, ButtonWidth, ButtonHeight, ButtonFontSize, "EXIT", Color.Black);

        AboutContainerBackground = new((1280 / 2f) - (ContainerWidth / 2f), (720 / 2f) - (ContainerHeight / 2f), ContainerWidth, ContainerHeight);
        ReturnToMenuButton = new(AboutContainerBackground.X + ((ContainerWidth - ButtonWidth) / 2f), AboutContainerBackground.Y + ContainerHeight - 70, ButtonWidth, ButtonHeight, ButtonFontSize, "BACK", Color.Black);
    }
    public override void InterfaceUI()
    {
        if (!AboutContainerVisible)
        {
            int TextWidth = Raylib.MeasureText("[TEST GAMEPLAY]", 50);
            const int TextHeight = 50;

            float TextX = (1270 - TextWidth) / 2f;
            const float TextY = (720 - TextHeight) / 2f;

            Raylib.DrawText("[TEST GAMEPLAY]", (int)TextX, (int)TextY - 130, 50, Color.White);

            PlayButton.Draw();
            AboutGameButton.Draw();
            ExitGameButton.Draw();
        }
        else
        {
            Raylib.DrawRectangleRec(AboutContainerBackground, Color.White);
            int TextWidth1 = Raylib.MeasureText("Made by Pukselek", 30);
            int TextWidth2 = Raylib.MeasureText("[https://github.com/pukselek]", 25);

            float textX1 = AboutContainerBackground.X + ((AboutContainerBackground.Width - TextWidth1) / 2f);
            float textY1 = AboutContainerBackground.Y + ((AboutContainerBackground.Height - 30) / 2f) - 40;

            float textX2 = AboutContainerBackground.X + ((AboutContainerBackground.Width - TextWidth2) / 2f);
            float textY2 = AboutContainerBackground.Y + ((AboutContainerBackground.Height - 25) / 2f);

            Raylib.DrawText("Made by Pukselek", (int)textX1, (int)textY1, 30, Color.Black);
            Raylib.DrawText("[https://github.com/pukselek]", (int)textX2, (int)textY2, 25, Color.Black);

            ReturnToMenuButton.Draw();
        }
    }
    public override void Update()
    {
        MousePoint = Raylib.GetMousePosition();
        if (!AboutContainerVisible)
        {
            PlayButton.OnButtonClick(MousePoint, () =>
            {
                Program.CurrentScene?.UnLoadTextures();
                Program.CurrentScene = new FirstLevel();
                Program.CurrentScene.Init();
            });
            AboutGameButton.OnButtonClick(MousePoint, () => AboutContainerVisible = true);
            ExitGameButton.OnButtonClick(MousePoint, () =>
            {
                Program.CurrentScene?.UnLoadTextures();
                Raylib.CloseWindow();
                Environment.Exit(0);
            });
        }
        else
        {
            ReturnToMenuButton.OnButtonClick(MousePoint, () => AboutContainerVisible = false);
        }
    }
    public override void UnLoadTextures()
    {
    }
}