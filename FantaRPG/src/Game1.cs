using FantaRPG.src.Animations;
using FantaRPG.src.Interfaces;
using FantaRPG.src.Movement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
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
        CollisionComponent collisionComponent;
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
        IAnimation fade;
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
            List<IEntity> entities = new();
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
            player = new Player(pixel, input, new RectangleF(-400, -100, 20, 20));
            CurrentRoom = new Room(backgrounds.OrderByDescending(x => x.LayerID).ToList(), entities, player, new Rectangle(0, 0, 1920, 1080));
            CurrentRoom.AddEntity(new Platform(pixel, new RectangleF(-200, -1000, 400, 800)));
            CurrentRoom.AddEntity(new Platform(pixel, new RectangleF(-20000, 0, 40000, 20)));
            ChangeRoom(CurrentRoom);
            SetResolution(1600, 900);
            fade = new FadeToBlack();
        }
        public void SetResolution(int x, int y)
        {
            _graphics.PreferredBackBufferWidth = x;
            _graphics.PreferredBackBufferHeight = y;
            _graphics.ApplyChanges();
            Ratio = (float)_graphics.PreferredBackBufferHeight / CurrentRoom.Backgrounds.First().Texture.Height;
        }
        public void ChangeRoom(Room room)
        {
            Instance.CurrentRoom = room;
            collisionComponent = new CollisionComponent(new Rectangle(-10000,-10000,20000,20000));
            foreach (var item in CurrentRoom.Entities)
            {
                collisionComponent.Insert(item);
            }
            collisionComponent.Insert(player);
            //collisionComponent.Insert();
        }
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            fade.Update(gameTime);
            CurrentRoom.Update(gameTime);
            cam.Follow(player);
            collisionComponent.Update(gameTime);
            base.Update(gameTime);
            MovementInput.Update();
        }
        static Color bgColor = new Color(16, 0, 32);
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(bgColor);
            //Background
            CurrentRoom.DrawBackground(spriteBatch, cam);
            //foreground stuff
            CurrentRoom.DrawEntities(spriteBatch, cam);
            //fade.Draw(spriteBatch, cam);
            base.Draw(gameTime);
        }
    }
}