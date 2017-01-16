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

    abstract class Enemy : PhysicalObject
    {

        //Kod som kommer att tillåta programet att röra på fiendens textur.
        protected int Rows { get; set; }
        protected int Columns { get; set; }
        protected Texture2D EnemyTexture;
        protected int CurrentFrame;
        protected int TotalFrame;




        //Kordinat variabler som ska bestämma hur fienderna rör på sig samt var de befinner sig på skärmen.
        protected Vector2 EnemyCoord;
        protected Vector2 EnemySpeed;


        //Variabler som används för att sakta ned bildens hastighet, väntar 50 millisekunder innan den byter bild.
        protected int TimeSinceLastFrame = 0;
        protected int MillisecondsPerFrame = 290;


        //Konstruktor som skapar ett nytt fiendeobjekt.
        public Enemy(Texture2D enemyTexture, int row, int columns, float eX, float eY, float EspeedX, float EspeedY) : base(enemyTexture, eX, eY, EspeedX, EspeedY)
        {
            //Bestämmer hur framesen ska fungera.
            EnemyTexture = enemyTexture;
            Rows = row;
            Columns = columns;
            CurrentFrame = 0;
            TotalFrame = Rows * Columns;

            this.EnemyTexture = enemyTexture;

        }

        //metod för att sakta ned antalet frames/sec
        public virtual void Update(GameWindow window, GameTime gameTime)
        {
        }
        //Metod för att rita ut fienden i spelet.
        public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            int width = EnemyTexture.Width / Columns;
            int heigth = EnemyTexture.Height / Rows;
            int row = (int)((float)CurrentFrame / Columns);
            int column = CurrentFrame % Columns;

            //Bestämmer vilken del av bilden som kommer att visas.
            Rectangle sourceRectangle = new Rectangle(width * column, heigth * row, width, heigth);

            //bestämmer vart på skärmen som objektet kommer att placeras.            
            Rectangle destinationRectangle = new Rectangle((int)ObjectCoordinates.X, (int)ObjectCoordinates.Y, width, heigth);



            //programmet startar processen att rita ut objekt.
            spriteBatch.Draw(EnemyTexture, destinationRectangle, sourceRectangle, Color.White);
        }

        //Enenmy klassens egenskaper.
        public bool IsAlive
        {
            get { return isAlive; }
            set { isAlive = value; }
        }

    }

    class Robbe : Enemy
    {
        public Robbe(Texture2D RobbeTexture, int row, int columns, float rX, float rY, float RSpeedX, float RSpeedY) : base(RobbeTexture, row, columns, rX, rY, -2, -1)
        {

        }

        public override void Update(GameWindow window, GameTime gameTime)
        {
            //Kod som hantarar hur varje frame ska fungera, dvs när programmet ska byta till nästa frame och hur länge varje frame ska visas.
            TimeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (TimeSinceLastFrame > MillisecondsPerFrame)
            {
                TimeSinceLastFrame -= MillisecondsPerFrame;

                //byter till nästa frame.
                CurrentFrame++;
                TimeSinceLastFrame = 0;

                //Startar om processen.
                if (CurrentFrame == TotalFrame)
                {
                    CurrentFrame = 0;
                }
            }
            //Får fienden att flytta på sig.
            ObjectCoordinates.X += ObjectSpeed.X;


            if (ObjectCoordinates.X < 1800)
            {
                ObjectCoordinates.Y += ObjectSpeed.Y;

                if (ObjectCoordinates.X < 20)
                    ObjectSpeed.Y = 0;

                if (ObjectCoordinates.Y < 0)
                {
                    ObjectSpeed.Y *= -1;
                }
            }
            //"Dödar" fiender ifall de åker utanför vänstra sidan på fönstret.
            if (ObjectCoordinates.X < -50)
                isAlive = false;

        }
        

    
    }

    class Allan : Enemy
    {
        public Allan(Texture2D RobbeTexture, int row, int columns, float rX, float rY, float RSpeedX, float RSpeedY) : base(RobbeTexture, row, columns, rX, rY, -3, 0)
        {
        }

        public override void Update(GameWindow window, GameTime gameTime)
        {
            //Kod som hantarar hur varje frame ska fungera, dvs när programmet ska byta till nästa frame och hur länge varje frame ska visas.
            TimeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (TimeSinceLastFrame > MillisecondsPerFrame)
            {
                TimeSinceLastFrame -= MillisecondsPerFrame;

                //byter till nästa frame.
                CurrentFrame++;
                TimeSinceLastFrame = 0;

                //Startar om processen.
                if (CurrentFrame == TotalFrame)
                {
                    CurrentFrame = 0;
                }
            }
            //Får fienden att flytta på sig.
            ObjectCoordinates.X += ObjectSpeed.X;

            //"Dödar" fiender ifall de åker utanför vänstra sidan på fönstret.
            if (ObjectCoordinates.X < -50)
                isAlive = false;
        }

     }
}