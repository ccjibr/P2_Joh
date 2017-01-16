using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;
using System.Collections.Generic;

namespace CopsAndRobbers
{
    public class Game1 : Game
    {
        //Används för grafiken.
        GraphicsDeviceManager graphics;
        //Används för att rita bilder(endast tillgänglig i Game1() klassen).
        SpriteBatch spriteBatch;

        //Objekt för att kunna ruta ut en fast bakgrundsbild.
        Texture2D background_Texture;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";



            graphics.PreferredBackBufferHeight = 1000;
            graphics.PreferredBackBufferWidth = 1800;
        }
        
        //Metoden anropas när spelet startas, hämtar sin state från GameElements.
        protected override void Initialize()
        {
            GameElements.currentState = GameElements.State.Menu;
            GameElements.Initialize();
            base.Initialize();
        }

        //Metoden anropas när spelet startas, hämtar sin state från GameElements.
        protected override void LoadContent()
        {
            //Laddar in en bakgrundsbild.
            background_Texture = Content.Load<Texture2D>("Images/Background");
            spriteBatch = new SpriteBatch(GraphicsDevice);
            GameElements.LoadContent(Content, Window);
           
        }

        protected override void UnloadContent()
        {
            GameElements.UnloadContent();
        }


        protected override void Update(GameTime gameTime)
        {
            //Stänger av spelet om man trycker på Back-Knappen på gamepaden.
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            switch (GameElements.currentState)
            {
                case GameElements.State.Run:
                    GameElements.currentState =
                    GameElements.RunUpdate(Content, Window, gameTime);
                    break;
                case GameElements.State.HighScore:
                    GameElements.currentState =
                    GameElements.HighScoreUpdate(gameTime);
                    break;
                case GameElements.State.Quit:
                    this.Exit();
                    break;
                default:
                    GameElements.currentState = GameElements.MenuUpdate(gameTime);
                    break;

            }


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.CornflowerBlue);

            //programmet startar processen att rita ut objekt.
            spriteBatch.Begin();

            //Ritar ut bakgrundsbild.
            spriteBatch.Draw(background_Texture, Vector2.Zero, Color.White);

            switch (GameElements.currentState)
            {
                case GameElements.State.Run:
                    GameElements.RunDraw(spriteBatch, gameTime);
                    break;
                case GameElements.State.HighScore:
                    GameElements.HighScoreDraw(spriteBatch);
                    break;
                case GameElements.State.HighScoreView:
                    GameElements.HighScoreDraw(spriteBatch);
                    break;
                case GameElements.State.Quit:
                    this.Exit();
                    break;
                default:
                    GameElements.MenuDraw(spriteBatch);
                    break;
            }


            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
