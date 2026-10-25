using Raylib_cs;
using src.scenes;
using System.Numerics;

namespace src.settings;

public class PauseMenuBuild
{
    public Constructor CurrentScene { get; set; } = null!;

    public Rectangle ContainerMenuBackground { get; set; }
    public Vector2 MousePoint { get; private set; } = new Vector2(0.0f, 0.0f);

    public ButtonBuild ResumeButton { get; set; } = null!;
    public ButtonBuild ResetSceneButton { get; set; } = null!;
    public ButtonBuild BackToMenu { get; set; } = null!;

    public bool IsGameplayPaused { get; set; } = false;

    const int ContainerWidth = 200;
    const int ContainerHeight = 150;

    const int ButtonWidth = 200;
    const int ButtonHeight = 50;

    public PauseMenuBuild(Constructor Scene)
    {
        if (Scene == null) return;
        CurrentScene = Scene;

        ContainerMenuBackground = new((1280 / 2f) - (ContainerWidth / 2f), (720 / 2f) - (ContainerHeight / 2f), ContainerWidth, ContainerHeight);
        ResumeButton = new(ContainerMenuBackground.X + ((ContainerWidth - ButtonWidth) / 2f), ContainerMenuBackground.Y + ContainerHeight - 150, ButtonWidth, ButtonHeight, 30, "RESUME", Color.Black);
        ResetSceneButton = new(ContainerMenuBackground.X + ((ContainerWidth - ButtonWidth) / 2f), ContainerMenuBackground.Y + ContainerHeight - 100, ButtonWidth, ButtonHeight, 30, "RESET", Color.Black);
        BackToMenu = new(ContainerMenuBackground.X + ((ContainerWidth - ButtonWidth) / 2f), ContainerMenuBackground.Y + ContainerHeight - 50, ButtonWidth, ButtonHeight, 30, "BACK", Color.Black);
    }
    public void Draw()
    {
        if (IsGameplayPaused)
        {
            Raylib.DrawRectangleRec(ContainerMenuBackground, Color.White);

            ResumeButton.Draw();
            ResetSceneButton.Draw();
            BackToMenu.Draw();
        }
    }
    public void Update()
    {
        MousePoint = Raylib.GetMousePosition();

        if (Raylib.IsKeyPressed(KeyboardKey.Escape)) IsGameplayPaused = !IsGameplayPaused;
        if (IsGameplayPaused)
        {
            ResumeButton.OnButtonClick(MousePoint, () => IsGameplayPaused = false);
            ResetSceneButton.OnButtonClick(MousePoint, () => CurrentScene.Init());
            BackToMenu.OnButtonClick(MousePoint, () =>
            {
                Program.CurrentScene = new Menu();
                Program.CurrentScene.Init();
            });
        }
    }
}