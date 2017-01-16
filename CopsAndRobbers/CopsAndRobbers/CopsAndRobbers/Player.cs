using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CopsAndRobbers
{
    class Player : PhysicalObject
    {
        //Medlemsvariabler som gör att karaktären kan "röra" på sig.
        private int Rows { get; set; }
        private int Columns { get; set; }
        public Texture2D PlayerTexture;
        private int CurrentFrame;
        private int TotalFrame;
        List<Bullet> bullets;
        public List<Bullet> Bullets { get { return bullets; } }
        Texture2D bulletTexture;
        double timeSinceLastBullet = 0;


        private Vector2 playerCoord;
        private Vector2 playerSpeed;


        //Skapar en egenskap för spelaren att få poäng.
        int points = 0;
        public int Points { get { return points; } set { points = value; } }

        //Variabler som används för att sakta ned bildens hastighet, väntar 50 millisekunder innan den byter bild
        private int TimeSinceLastFrame = 0;
        private int MillisecondsPerFrame = 290;

        //Konstruktor som skapar spelaren och kulor, skickar data till GameObject för att få objektet att röra på sig.
        public Player(Texture2D playerTexture, int row, int columns, float pX, float pY, float PspeedX, float PspeedY, Texture2D BulletTexture) :
               base(playerTexture, pX, pY, PspeedX, PspeedY)
        {
            //Bestämmer hur framesen ska fungera.
            PlayerTexture = playerTexture;
            this.Rows = row;
            this.Columns = columns;
            CurrentFrame = 0;
            TotalFrame = Rows * Columns;

            this.PlayerTexture = playerTexture;
            this.playerCoord.X = pX;
            this.playerCoord.Y = pY;
            this.playerSpeed.X = PspeedX;
            this.playerSpeed.Y = PspeedY;


            //Skapar en lista så att spelaren kan skjuta flera skott.
            bullets = new List<Bullet>();
            this.bulletTexture = BulletTexture;
        }

        //metod för att sakta ned antalet frames/sec
        public void Update(GameWindow window, GameTime gameTime)
        {

            //Kod som hantarar hur varje frame ska fungera
            TimeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (TimeSinceLastFrame > MillisecondsPerFrame)
            {
                TimeSinceLastFrame -= MillisecondsPerFrame;

                //byter till nästa frame
                CurrentFrame++;
                TimeSinceLastFrame = 0;

                //Startar om processen
                if (CurrentFrame == TotalFrame)
                {
                    CurrentFrame = 0;
                }

            }

            //Kod för att få spelarobjektet att röra på sig.
            // Hämtar info från spelarens knapptryckningar. 
            KeyboardState keyboardState = Keyboard.GetState();

            //Kod som säger att karaktären ska förflytta sig om spelaren trycker ned en knapp, sålänge inte den är på väg ut från kanten.
            if (ObjectCoordinates.X <= window.ClientBounds.Width - (ObjectTexture.Width / 6) - 15 || ObjectCoordinates.X >= 0)
            {
                if (keyboardState.IsKeyDown(Keys.Right))
                    ObjectCoordinates.X += ObjectSpeed.X;
                if (keyboardState.IsKeyDown(Keys.Left))
                    ObjectCoordinates.X -= ObjectSpeed.X;
            }
            //Kod som bestämmer att karaktären ska röra sig upp eller ned när spelaren trycker ned en knapp, så länge karaktären inte är på väg ut från kanten.
            if (ObjectCoordinates.Y <= window.ClientBounds.Height - ObjectTexture.Height || ObjectCoordinates.Y >= 0)
            {
                if (keyboardState.IsKeyDown(Keys.Down))
                    ObjectCoordinates.Y += ObjectSpeed.Y;
                if (keyboardState.IsKeyDown(Keys.Up))
                    ObjectCoordinates.Y -= ObjectSpeed.Y;
            }

            //Gör så att karaktären inte kan åka utanför skärmen.
            if (ObjectCoordinates.X < 0)
                ObjectCoordinates.X = 0;
            if (ObjectCoordinates.X > window.ClientBounds.Width - ObjectTexture.Width / 6)
                ObjectCoordinates.X = window.ClientBounds.Width - ObjectTexture.Width / 6;
            if (ObjectCoordinates.Y < 0)
                ObjectCoordinates.Y = 0;
            if (ObjectCoordinates.Y > window.ClientBounds.Height - ObjectTexture.Height)
                ObjectCoordinates.Y = window.ClientBounds.Height - ObjectTexture.Height;

            //Gör så att spelaren kan skjuta när hen trycker på mellanslagstangenten.
            if (keyboardState.IsKeyDown(Keys.Space))
            {
                //Bestämmer att spelaren inte kan skjuta mer 5 kulor per sekund.
                if (gameTime.TotalGameTime.TotalMilliseconds > timeSinceLastBullet + 200)
                {
                    //Skapar skotten.
                    Bullet temp = new Bullet(bulletTexture, ObjectCoordinates.X + ObjectTexture.Width / 6, ObjectCoordinates.Y + 60);
                    bullets.Add(temp);

                    timeSinceLastBullet = gameTime.TotalGameTime.TotalMilliseconds;
                }
            }
            //Loop för att flytta på skotten samt ta bort dem om de hamnar utanför skärmen.
            foreach (Bullet b in bullets.ToList())
            {
                //Flyttar på skotten.
                b.Update();
                //Kontrollerar om skottet är dött och om det är det tar programmet bort skottet från listan.
                if (!b.IsAlive)
                    bullets.Remove(b);

            }
            if (keyboardState.IsKeyDown(Keys.Escape))
                isAlive = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            int width = PlayerTexture.Width / Columns;
            int heigth = PlayerTexture.Height / Rows;
            int row = (int)(CurrentFrame / Columns);
            int column = CurrentFrame % Columns;

            //Bestämmer vilken del av bilden som kommer att visas.
            Rectangle sourceRectangle = new Rectangle(width * column - 16, heigth * row, width - 1, heigth);

            //bestämmer vart på skärmen som objektet kommer att placeras.
            Rectangle destinationRectangle = new Rectangle((int)ObjectCoordinates.X, (int)ObjectCoordinates.Y, width, heigth);

            //Ritar ut kulorna.
            foreach (Bullet b in bullets)
                b.DrawObject(spriteBatch);



            //programmet startar processen att rita ut objekt.            
            spriteBatch.Draw(PlayerTexture, destinationRectangle, sourceRectangle, Color.White);

        }

        //Metod som låter användare starta spelet flera gånger genom att återställa spelar-objektet.
        public void Reset(float X, float Y, float speedX, float speedY)
        {

            isAlive = true;
            ObjectCoordinates.X = X;
            ObjectCoordinates.Y = Y;
            ObjectCoordinates.X = speedX;
            ObjectCoordinates.Y = speedY;

            //Tömmer alla kulor och poäng från dess listor.
            bullets.Clear();
            timeSinceLastBullet = 0;
            points = 0;

        }

    }

    class Bullet : PhysicalObject
    {


        //Konstruktoer för att skapa en ny kula.
        public Bullet(Texture2D BulletTexture, float BX, float BY) : base(BulletTexture, BX, BY, 8f, 0f)
        {
        }

        //Metod soom ska uppdatera kulans position och ta bort den om den åker utanför rutan.

        public void Update()
        {
            ObjectCoordinates.X += ObjectSpeed.X;
            if (ObjectCoordinates.X > 1900)
                isAlive = false;

            

        }


    }
}
