using System.Numerics;
using Raylib_cs;

namespace src.settings;

public class PlayerBuild
{
    public Rectangle CharacterBody { get; set; }
    public Vector2 PlayerMapLocation { get; set; } = Vector2.Zero;
    public Camera2D MainCamera { get; set; }

    public float BodyWidth { get; set; }
    public float BodyHeight { get; set; }

    const float Gravity = 800f;
    const float PlayerSpeed = 200f;
    const float PlayerJump = -310f;
    const int AnimationSpeed = 7;

    float VelocityY = 0f;
    int DoubleJumpChecker = 0;
    public bool IsGround = false;

    public SpriteAnimation IdleSpriteAnimation { get; set; } = null!;
    public SpriteAnimation WalkSpriteAnimation { get; set; } = null!;
    public SpriteAnimation JumpSpriteAnimation { get; set; } = null!;

    public SpriteAnimation CurrentAnimation { get; set; } = null!;

    public string[] IdleTexturesTable = ["images/characterImages/1.png", "images/characterImages/2.png", "images/characterImages/1.png","images/characterImages/3.png"];
    public string[] WalkTexturesTable = ["images/characterImages/5.png", "images/characterImages/1.png", "images/characterImages/4.png"];
    public string[] JumpTextureTable = ["images/characterImages/5.png", "images/characterImages/5.png", "images/characterImages/1.png" ,"images/characterImages/4.png", "images/characterImages/4.png"];

    public PlayerBuild(Vector2 Location, float Width = 50, float Height = 50)
    {
        PlayerMapLocation = Location;
        BodyWidth = Width;
        BodyHeight = Height;

        CharacterBody = new(PlayerMapLocation, Width, Height);
        MainCamera = new()
        {
            Target = new Vector2(CharacterBody.X + 20.0f, CharacterBody.Y + 20.0f),
            Offset = new Vector2(Raylib.GetRenderWidth() / 2.0f, Raylib.GetRenderHeight() / 2.0f),
            Zoom = 2.1f
        };
        IdleSpriteAnimation = new(CharacterBody, Width, Height, AnimationSpeed, IdleTexturesTable);
        WalkSpriteAnimation = new(CharacterBody, Width, Height, AnimationSpeed, WalkTexturesTable);
        JumpSpriteAnimation = new(CharacterBody, Width, Height, AnimationSpeed, JumpTextureTable);

        CurrentAnimation = IdleSpriteAnimation;
    }

    public void Draw()
    {
        CurrentAnimation.ObjectRectangle = CharacterBody;
        CurrentAnimation.DrawTexture();
    }

    public void Update(params Rectangle[] TouchablePlatformsList)
    {
        if (TouchablePlatformsList.Length == 0) return;

        if (Raylib.IsKeyPressed(KeyboardKey.Space))
        {
            if (DoubleJumpChecker < 2)
            {
                VelocityY = PlayerJump;
                DoubleJumpChecker++;
                IsGround = false;
            }
        }

        float moveX = 0f;
        if (Raylib.IsKeyDown(KeyboardKey.D) || Raylib.IsKeyDown(KeyboardKey.Right))
        {
            moveX += PlayerSpeed * Raylib.GetFrameTime();
            IdleSpriteAnimation.FlipH = false;
            WalkSpriteAnimation.FlipH = false;
            JumpSpriteAnimation.FlipH = false;
        }
        if (Raylib.IsKeyDown(KeyboardKey.A) || Raylib.IsKeyDown(KeyboardKey.Left))
        {
            moveX -= PlayerSpeed * Raylib.GetFrameTime();
            IdleSpriteAnimation.FlipH = true;
            WalkSpriteAnimation.FlipH = true;
            JumpSpriteAnimation.FlipH = true;
        }

        CharacterBody = new(CharacterBody.X + moveX, CharacterBody.Y, CharacterBody.Width, CharacterBody.Height);

        VelocityY += Gravity * Raylib.GetFrameTime();

        float moveY = VelocityY * Raylib.GetFrameTime();
        float previousY = CharacterBody.Y;

        CharacterBody = new(CharacterBody.X, CharacterBody.Y + moveY, CharacterBody.Width, CharacterBody.Height);

        IsGround = false;

        foreach (var platform in TouchablePlatformsList)
        {
            if (Raylib.CheckCollisionRecs(CharacterBody, platform) && VelocityY > 0)
            {
                if ((previousY + CharacterBody.Height) <= platform.Y)
                {
                    VelocityY = 0;
                    float landedY = platform.Y - CharacterBody.Height;
                    CharacterBody = new(CharacterBody.X, landedY, CharacterBody.Width, CharacterBody.Height);
                    IsGround = true;
                    DoubleJumpChecker = 0;
                    break;
                }
            }
        }

        if (!IsGround)
        {
            CurrentAnimation = JumpSpriteAnimation;
        }
        else
        {
            CurrentAnimation = (moveX != 0) ? WalkSpriteAnimation : IdleSpriteAnimation;
        }

        IdleSpriteAnimation.ObjectRectangle = CharacterBody;
        WalkSpriteAnimation.ObjectRectangle = CharacterBody;
        JumpSpriteAnimation.ObjectRectangle = CharacterBody;

        IdleSpriteAnimation.Update();
        WalkSpriteAnimation.Update();
        JumpSpriteAnimation.Update();

        var camera = MainCamera;
        camera.Target = new Vector2(CharacterBody.X + 20, CharacterBody.Y + 20);
        MainCamera = camera;
    }
    public bool CheckSingleTouch(Rectangle Object)
    {
        return Raylib.CheckCollisionRecs(CharacterBody, Object);
    }
    public void UnloadTexture()
    {
        IdleSpriteAnimation.UnLoadTextures();
        WalkSpriteAnimation.UnLoadTextures();
        JumpSpriteAnimation.UnLoadTextures();
    }
}