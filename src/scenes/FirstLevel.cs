using src.settings;
using Raylib_cs;
using System.Numerics;
namespace src.scenes;

public class FirstLevel : Constructor
{
    public PlayerBuild Player { get; set; } = null!;
    public Rectangle Platform { get; set; }
    public Rectangle ResetPlatform { get; set; }
    public Rectangle CompleteLevel { get; set; }

    public Rectangle CompletedLevelInfoBG { get; set; }

    public Vector2 MousePoint { get; private set; } = new Vector2(0.0f, 0.0f);

    public PauseMenuBuild PauseMenu { get; set; } = null!;

    public ButtonBuild ReloadLevelButton { get; set; } = null!;

    public Vector2 MapPlayerLocation { get; set; } = new Vector2(80, -90);
    public Vector2 PlatformLocation { get; set; } = Vector2.Zero;

    public Rectangle[] TouchablePlatformsList { get; set; } = null!;
    public Rectangle[] DrawAirPlatformsList { get; set; } = null!;

    public bool IsLevelCompleted = false;

    public override void Init()
    {
        IsLevelCompleted = false;

        Player = new(MapPlayerLocation, 38f, 50f);
        Platform = new(PlatformLocation, 500f, 20f);

        ResetPlatform = new(0, Platform.Y + 90, 2000f, 20f);
        CompleteLevel = new(1800, -130, 50, 100);

        PauseMenu = new(this);
        CompletedLevelInfoBG = new((1280 / 2f) - (350 / 2f), (720 / 2f) - (250f / 2f), 350, 250);
        ReloadLevelButton = new(CompletedLevelInfoBG.X + ((350 - 200) / 2f), CompletedLevelInfoBG.Y + 250 - 70, 200, 50, 20, "RELOAD LEVEL", Color.Black);

        DrawAirPlatformsList = [
            new Rectangle(270f, -40f, 50f, 5f),
            new Rectangle(360f, -80f, 50f, 5f),
            new Rectangle(500f, -160f, 50f, 5),
            new Rectangle(700f, -160f, 50f, 5),
            new Rectangle(900f, -160f, 50f, 5f),
            new Rectangle(1200f, -160f, 50f, 5f),
            new Rectangle(1500, -20f, 250f, 20f)
        ];
        TouchablePlatformsList = [Platform, ..DrawAirPlatformsList];
    }
    public override void InterfaceUI()
    {
        Raylib.DrawText("[1] FIRST LEVEL TEST", 110, 20, 20, Color.White);
        if (!IsLevelCompleted)
        {
            Raylib.BeginMode2D(Player.MainCamera);
            foreach (var airPlatform in TouchablePlatformsList)
            {
                Raylib.DrawRectangleRec(airPlatform, Color.White);
            }
            Player.Draw();
                Raylib.DrawRectangleRec(ResetPlatform, Color.Red);
                Raylib.DrawRectangleRec(CompleteLevel, Color.Blue);
            Raylib.EndMode2D();

            PauseMenu.Draw();
        }
        else
        {
            int textWidth = Raylib.MeasureText("Level 1 Completed!", 30);

            float textX = CompletedLevelInfoBG.X + ((CompletedLevelInfoBG.Width - textWidth) / 2f);
            float textY = CompletedLevelInfoBG.Y + ((CompletedLevelInfoBG.Height - 30) / 2f);

            Raylib.DrawRectangleRec(CompletedLevelInfoBG, Color.White);
            Raylib.DrawText("Level 1 Completed!", (int)textX, (int)textY, 30, Color.Red);
            ReloadLevelButton.Draw();
        }
    }
    public override void Update()
    {
        MousePoint = Raylib.GetMousePosition();
        if (!IsLevelCompleted)
        {
            PauseMenu.Update();
            if (!PauseMenu.IsGameplayPaused)
            {
                if (Player.CheckSingleTouch(ResetPlatform)) Player = new(MapPlayerLocation, 38f, 50f);
                if (Player.CheckSingleTouch(CompleteLevel)) IsLevelCompleted = true;
                Player.Update(TouchablePlatformsList);
            }
        }
        else
        {
            ReloadLevelButton.OnButtonClick(MousePoint, () => Init());
        }
    }
    public override void UnLoadTextures()
    {
        Player.UnloadTexture();
    }
}