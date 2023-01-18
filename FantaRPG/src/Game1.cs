using FantaRPG.src.Movement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FantaRPG.src
{
    internal class Game1 : Game
    {
        private static Game1 _instance = null;
        public static Game1 Instance { get { return _instance ??= new Game1(); } }
        public GraphicsDeviceManager _graphics;
        private SpriteBatch spriteBatch;
        public Room CurrentRoom;
        public Camera cam;
        public SpriteFont debugFont;
        public Texture2D pixel;
        public float Ratio;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            IsFixedTimeStep = false;
            base.Initialize();
        }
        Player player;
        protected override void LoadContent()
        {
            debugFont = Content.Load<SpriteFont>("DebugFont");
            pixel = Content.Load<Texture2D>("pixel");
            spriteBatch = new SpriteBatch(GraphicsDevice);
            List<BackgroundLayer> backgrounds = new()
            {
                new BackgroundLayer(Content.Load<Texture2D>("bg5"), 5),
                new BackgroundLayer(Content.Load<Texture2D>("bg4"), 7),
                new BackgroundLayer(Content.Load<Texture2D>("bg3"), 9),
                new BackgroundLayer(Content.Load<Texture2D>("bg2"), 11),
                new BackgroundLayer(Content.Load<Texture2D>("bg1"), 13)
            };
            List<Entity> entities = new();
            /*for (int i = 0; i < 200; i++)
            {
                entities.Add(new Entity(pixel, r.Next(-20000, 20001), 20, 20, 20));
            }*/

            cam = new Camera();
            Dictionary<string, Keys> input = new()
            {
                { "Up", Keys.W },
                { "Down", Keys.S },
                { "Left", Keys.A },
                { "Right", Keys.D },
                { "Jump", Keys.Space }
            };
            player = new Player(pixel, input, -400, -400, 20, 20);

            CurrentRoom = new Room(backgrounds.OrderByDescending(x => x.LayerID).ToList(), new List<Platform>(), entities, player,new Rectangle(0,0,1920,1080));
            CurrentRoom.AddPlatform(new Platform(pixel, -200, -1000, 400, 800));
            CurrentRoom.AddPlatform(new Platform(pixel, -20000, 0, 40000, 20));
            SetResolution(1600, 900);
        }
        public void SetResolution(int x, int y)
        {
            _graphics.PreferredBackBufferWidth = x;
            _graphics.PreferredBackBufferHeight = y;
            _graphics.ApplyChanges();
            Ratio = (float)_graphics.PreferredBackBufferHeight / CurrentRoom.Backgrounds.First().Texture.Height;
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            CurrentRoom.Update(gameTime);
            cam.Follow(player);
            base.Update(gameTime);
            MovementInput.Update();
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            //Background
            CurrentRoom.DrawBackground(spriteBatch, cam.Transform);
            CurrentRoom.DrawPlatforms(spriteBatch, cam.Transform);
            CurrentRoom.DrawEntities(spriteBatch, cam.Transform);
            spriteBatch.Begin();
            //spriteBatch.Draw(pixel, new Rectangle(Mouse.GetState().Position.X - 10 + (int)cam.Offset.X, Mouse.GetState().Position.Y - 10 + (int)cam.Offset.Y, 20, 20), Color.Red);
            //spriteBatch.DrawString(Instance.debugFont, "{" + Mouse.GetState().Position.X+cam.Center.X.ToString("0.0") + ";" + Mouse.GetState().Position.Y+cam.Center.Y.ToString("0.0") + "}", Mouse.GetState().Position.ToVector2(), Color.Black);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}