using Raylib_cs;
using System.Numerics;

namespace src.settings;

public class SpriteAnimation
{
    public Rectangle ObjectRectangle { get; set; }

    public int AnimationSpeed { get; set; }
    public float ObjectWidthSize { get; set; }
    public float ObjectHeightSize { get; set; }

    public bool FlipH { get; set; } = false;
    public Texture2D[] TexturesTable { get; set; }

    int CurrentFrameIndex = 0;
    float FrameTimer = 0f;

    public SpriteAnimation(Rectangle Object, float Width = 15f, float Height = 15f, int SpeedValue = 7, params string[] ListTable)
    {
        ObjectRectangle = Object;
        ObjectWidthSize = Width;
        ObjectHeightSize = Height;
        AnimationSpeed = SpeedValue;

        TexturesTable = new Texture2D[ListTable.Length];
        for(int i=0; i<ListTable.Length; i++)
        {
            string relativePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ListTable[i]);
            TexturesTable[i] = Raylib.LoadTexture(relativePath);
        }
    }
    public void DrawTexture()
    {
        if (TexturesTable.Length == 0) return;
        Texture2D currentTexture = TexturesTable[CurrentFrameIndex];

        float sourceWidth = FlipH ? -currentTexture.Width : currentTexture.Width;

        Rectangle sourceRec = new(0, 0, sourceWidth, currentTexture.Height);
        Rectangle destRec = new(ObjectRectangle.X, ObjectRectangle.Y, ObjectWidthSize, ObjectHeightSize);

        Raylib.DrawTexturePro(currentTexture, sourceRec, destRec, Vector2.Zero, 0f, Color.White);
    }
    public void Update()
    {
        if (TexturesTable.Length == 0) return;
        FrameTimer += Raylib.GetFrameTime();

        float frameDuration = 1.0f / AnimationSpeed;

        if (FrameTimer >= frameDuration)
        {
            FrameTimer = 0f;
            CurrentFrameIndex = (CurrentFrameIndex + 1) % TexturesTable.Length;
        }
    }
    public void UnLoadTextures()
    {
        foreach(var textures in TexturesTable)
        {
            Raylib.UnloadTexture(textures);
        }
    }
}