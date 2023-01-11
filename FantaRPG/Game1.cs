using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace FantaRPG
{
    public class Game1 : Game
    {
        private static Game1 _instance = null;
        public static Game1 Instance { get { return _instance ?? (_instance = new Game1()); } }
        private GraphicsDeviceManager _graphics;
        private SpriteBatch spriteBatch;
        private List<BackgroundLayer> backgrounds;
        private Camera cam;
        public SpriteFont debugFont;
        private Texture2D pixel;
        Random r = new Random();
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }
        Player player;
        List<Entity> entities;
        protected override void LoadContent()
        {
            debugFont = Content.Load<SpriteFont>("DebugFont");
            pixel = Content.Load<Texture2D>("pixel");
            spriteBatch = new SpriteBatch(GraphicsDevice);
            backgrounds = new List<BackgroundLayer>();
            entities = new List<Entity>();
            for (int i = 0; i < 200; i++)
            {
                entities.Add(new Entity(pixel,r.Next(-20000,20001),0));
            }
            cam = new Camera(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
            transform = Matrix.CreateTranslation(cam.Position.X, cam.Position.Y, 0);
            Dictionary<string, Keys> input = new Dictionary<string, Keys>();
            input.Add("Up", Keys.W);
            input.Add("Down", Keys.S);
            input.Add("Left", Keys.A);
            input.Add("Right", Keys.D);
            input.Add("Jump", Keys.Space);
            player = new Player(pixel, input);
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            
            // TODO: Add your update logic here
            player.Update(gameTime);
            cam.SetCenter(player.Position.X, player.Position.Y);
            transform = Matrix.CreateTranslation(-cam.Position.X, -cam.Position.Y, 0);
            base.Update(gameTime);
            MovementInput.Update();
        }
        static Matrix transform;
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            //Background
            spriteBatch.Begin(SpriteSortMode.Deferred,BlendState.AlphaBlend,SamplerState.LinearWrap);
            foreach (var item in backgrounds)
            {
                item.Draw(spriteBatch,cam);
            }
            spriteBatch.End();
            spriteBatch.Begin(transformMatrix:transform);

            foreach (var item in entities)
            {
                item.Draw(spriteBatch);
            }
            player.Draw(spriteBatch);
            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}