using Raylib_cs;
using src.scenes;
using src.settings;
namespace src;

static class Program
{
    private const int WindowWidth = 1280;
    private const int WindowHeight = 720;

    public static string Title = "My Application";
    public static Constructor CurrentScene { get; set; } = null!;

    public static void Main()
    {
        Raylib.SetConfigFlags(ConfigFlags.UnfocusedWindow);

        Raylib.InitWindow(WindowWidth, WindowHeight, Title);
        Raylib.SetTargetFPS(60);

        Raylib.SetExitKey(KeyboardKey.Null);

        CurrentScene = new Menu();
        CurrentScene?.Init();

        while (!Raylib.WindowShouldClose())
        {
            CurrentScene?.Update();
            Raylib.BeginDrawing();
                Raylib.DrawFPS(20, 20);
                Raylib.ClearBackground(Color.DarkGray);
                CurrentScene?.InterfaceUI();
            Raylib.EndDrawing();
        }
        CurrentScene?.UnLoadTextures();
        Raylib.CloseWindow();
    }
}
