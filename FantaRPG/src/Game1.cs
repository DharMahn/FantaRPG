using FantaRPG.src.Animations;
using FantaRPG.src.Interfaces;
using FantaRPG.src.Items;
using FantaRPG.src.Movement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Tweening;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public Room Room1, Room2;
        private static FastRandom _random = null;
        public static FastRandom Random { get { return _random ??= new FastRandom(); } }
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            //for (int i = 0; i < 100; i++)
            //{
            //    Modifier modifier = Modifier.GenerateModifier(Random.Next(50, 500));
            //    Debug.WriteLine(modifier.ToString());
            //}
        }

        protected override void Initialize()
        {
            IsFixedTimeStep = true;
            base.Initialize();
        }
        Player player;
        bool isChangingRoom = false;
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
            player = new Player(input, 100, -400, EntityConstants.PlayerSize);
            
            Room1 = new Room(backgrounds.OrderByDescending(x => x.LayerID).ToList(), 
                             new List<Entity>(), 
                             new List<Entity>(), 
                             player, 
                             new Point(3840, 2560));
            Room1.AddObject(new Platform(200, -1000, new Vector2(400, 800)));
            Room1.AddObject(new Platform(-20000, 0, new Vector2(40000, 20))); 
            Room1.AddObject(new Platform(-20, -2540, new Vector2(40, 2540)));
            Room1.AddObject(new Portal(1400, 500, EntityConstants.PortalSize));
            Room1.AddEntity(new Enemies.WalkerEnemy(200, -20, EntityConstants.WalkerSize));
            Room1.AddEntity(new Enemies.WalkerEnemy(200, -200, EntityConstants.WalkerSize));
            Room1.AddEntity(new Enemies.WalkerEnemy(400, -100, EntityConstants.WalkerSize));
            Room2 = new Room(backgrounds.OrderByDescending(x => x.LayerID).ToList(), new List<Entity>(), new List<Entity>(), player, new Point(1920, 1080));
            Room2.AddObject(new Platform(200, 400, new Vector2(50, 50)));
            Room2.AddObject(new Platform(-20000, 0, new Vector2(40000, 50)));
            Room2.AddObject(new Portal(300, 500, EntityConstants.PortalSize));

            ((Portal)Room1.Objects.Last()).SetAsPortal(Room2);
            ((Portal)Room2.Objects.Last()).SetAsPortal(Room1);

            ChangeRoom(Room1);
            SetResolution(1600, 900);
        }
        public void SetResolution(int x, int y)
        {
            _graphics.PreferredBackBufferWidth = x;
            _graphics.PreferredBackBufferHeight = y;
            _graphics.ApplyChanges();
            Ratio = (float)_graphics.PreferredBackBufferHeight / CurrentRoom.Backgrounds.First().Texture.Height;
        }
        FadeToBlack fadeToBlack;
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (isChangingRoom)
            {
                if (!fadeToBlack.IsFinished)
                {
                    fadeToBlack.Update(gameTime);
                }
                else
                {
                    if (!fadeToBlack.IsReverse)
                    {
                        fadeToBlack = new FadeToBlack(true);
                        CurrentRoom = nextRoom;
                        foreach (Portal item in CurrentRoom.Objects.Where(x => x is Portal))
                        {
                            item.Reset();
                        }
                        nextRoom = null;
                    }
                    else
                    {
                        isChangingRoom = false;
                    }
                }
            }

            CurrentRoom.Update(gameTime);
            cam.Follow(player);
            base.Update(gameTime);
            MovementInput.Update();
        }
        private Room nextRoom;
        private void ChangeRoom(Room targetRoom)
        {
            CurrentRoom = targetRoom;
        }
        internal void TransitionToRoom(Room targetRoom)
        {
            isChangingRoom = true;
            nextRoom = targetRoom;
            fadeToBlack = new FadeToBlack();
        }
        static Color bgColor = new Color(16, 0, 32);
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(bgColor);
            CurrentRoom.DrawBackground(spriteBatch, cam.Transform);

            CurrentRoom.DrawPlatforms(spriteBatch, cam.Transform);
            CurrentRoom.DrawEntities(spriteBatch, cam.Transform);
            if (isChangingRoom)
            {
                fadeToBlack.Draw(spriteBatch, cam);
            }
            //spriteBatch.Begin();
            //spriteBatch.Draw(pixel, new Rectangle(Mouse.GetState().Position.X - 10 + (int)cam.Offset.X, Mouse.GetState().Position.Y - 10 + (int)cam.Offset.Y, 20, 20), Color.Red);
            //spriteBatch.DrawString(Instance.debugFont, "{" + Mouse.GetState().Position.X+cam.Center.X.ToString("0.0") + ";" + Mouse.GetState().Position.Y+cam.Center.Y.ToString("0.0") + "}", Mouse.GetState().Position.ToVector2(), Color.Black);

            //spriteBatch.End();
            base.Draw(gameTime);
        }

    }
}