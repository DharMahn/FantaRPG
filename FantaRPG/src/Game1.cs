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
using System.IO;
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
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            using StreamWriter sw = new StreamWriter("debugModifier.txt", false);
            for (int i = 0; i < 100; i++)
            {
                Modifier modifier = Modifier.GenerateModifier(RNG.Get(5, 500));
                float sum = 0;
                foreach (var item in modifier.Stats.GetAllStats())
                {
                    sum += item.Value * Stats.statValues[item.Key];
                }
                if (sum < modifier.Level - 5)
                {
                    throw new Exception("shits fucked yo");
                }
                sw.WriteLine(modifier.ToString());
                sw.WriteLine("sum: " + sum + Environment.NewLine);
            }
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
            //debugFont = Content.Load<SpriteFont>("DebugFont");
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
            player = new Player(input, 900, -400, EntityConstants.PlayerSize)
            {
                Center = new Vector2(850, -50)
            };

            Room1 = new Room(backgrounds.OrderByDescending(x => x.LayerID).ToList(),
                             new List<Platform>(),
                             new List<Entity>(),
                             new List<Portal>(),
                             player,
                             new Point(3840, 2560));
            Room1.AddPlatform(new Platform(200, -1000, new Vector2(400, 800)));
            Room1.AddPlatform(new Platform(-20000, 0, new Vector2(40000, 20)));
            Room1.AddPlatform(new Platform(-20, -2540, new Vector2(40, 2540)));
            Room1.AddPortal(new Portal(Room1, 1600, -200, EntityConstants.PortalSize));
            Room1.AddEntity(new Enemies.WalkerEnemy(200, -20, EntityConstants.WalkerSize));
            Room1.AddEntity(new Enemies.WalkerEnemy(200, -200, EntityConstants.WalkerSize));
            Room1.AddEntity(new Enemies.WalkerEnemy(400, -100, EntityConstants.WalkerSize));
            Room2 = new Room(backgrounds.OrderByDescending(x => x.LayerID).ToList(),
                             new List<Platform>(),
                             new List<Entity>(),
                             new List<Portal>(),
                             player,
                             new Point(1920, 1080));
            Room2.AddPlatform(new Platform(200, 400, new Vector2(50, 50)));
            Room2.AddPlatform(new Platform(-20000, 0, new Vector2(40000, 50)));
            Room2.AddPortal(new Portal(Room2, 800, -200, EntityConstants.PortalSize));

            Room1.Portals.Last().SetPortalTo(Room2);

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
                        fadeToBlack = new FadeToBlack(reverse: true);
                        CurrentRoom = nextRoom;
                        foreach (Portal item in CurrentRoom.Platforms.Where(x => x is Portal p && p != exitPortal))
                        {
                            item.Reset();
                        }
                        nextRoom = null;
                        if (exitPortal != null)
                        {
                            player.Center = exitPortal.Center - (exitPortal.TargetPortal.Center - player.Center);
                            exitPortal.TargetPortal.IsWorking = false;
                            exitPortal.TargetPortal.FadeOutNow();
                            exitPortal.IsWorking = false;
                            exitPortal.FadeOutNow();
                            exitPortal.FadeIn();
                        }
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
        static Color bgColor = new(16, 0, 32);
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(bgColor);
            CurrentRoom.DrawBackground(spriteBatch, cam.Transform);

            CurrentRoom.DrawPlatforms(spriteBatch, cam.Transform);
            CurrentRoom.DrawPortals(spriteBatch, cam.Transform);
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

        Portal exitPortal = null;
        internal void UsePortal(Portal portal)
        {
            TransitionToRoom(portal.TargetPortal.ContainingRoom);
            exitPortal = portal.TargetPortal;
        }
    }
}