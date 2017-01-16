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
    class GameObject
    {
        // Objektens textur och koordinat, skrivs med protected för att klassobjekt ska kunna ärva dess egenskaper.
        protected Texture2D ObjectTexture;
        protected Vector2 ObjectCoordinates;
        //Konstruktor för att skapa objekten och för att animera karaktärerna.
        public GameObject(Texture2D ObTexture, float ObjectCoordX, float ObjectCoordY)
        {
            //Vad objektets koordinater är.
            this.ObjectTexture = ObTexture;
            this.ObjectCoordinates.X = ObjectCoordX;
            this.ObjectCoordinates.Y = ObjectCoordY;

        }

        //Metod som ska rita ut objekten på skärmen

        public void DrawObject(SpriteBatch ObjectSpriteBatch)
        {
            ObjectSpriteBatch.Draw(ObjectTexture, ObjectCoordinates, Color.White);
        }

        //Egenskper för klassen(GameObject)
        public float XCoord { get { return ObjectCoordinates.X; } }
        public float YCoord { get { return ObjectCoordinates.Y; } }
        public float ObjectWidth { get { return ObjectTexture.Width; } }
        public float ObjectHight { get { return ObjectTexture.Height; } }
    }

    //Klass för objekt som ska röra på sig, MovingObject som ska ärva objekts egenskaper från GameObject
    class MovingObject : GameObject
    {
        //Skrivs som protected för att andra klasser ska kunna ärva av klassobjektet men inte ändra på det.
        // ObjectSpeed ska reglera hastigheten på objektet.
        protected Vector2 ObjectSpeed;

        //Konstruktor som skapar nya objekt som kan röra på sig genom att ärva från GameObject
        public MovingObject(Texture2D MovingObjectTexture, float MovingObjectX, float MovingObjectY, float objectSpeedX, float objectSpeedY) :
            base(MovingObjectTexture, MovingObjectX, MovingObjectY)
        {
            this.ObjectSpeed.X = objectSpeedX;
            this.ObjectSpeed.Y = objectSpeedY;
        }
    }

    abstract class PhysicalObject : MovingObject
    {
        //Gör en bool för att avgöra om ett objekt lever eller inte.
        protected bool isAlive = true;

        //Konstruktor för att skapa objekt som kan kollidera med andra objekt och röra på sig, ärver från MovingObject.
        public PhysicalObject(Texture2D physTexture, float physX, float physY, float physSpeedX, float physSpeedY) :
            base(physTexture, physX, physY, physSpeedX, physSpeedY)
        {
        }

        //Metod som skapar rektanglar (som inte syns) runt objekten. Om dessa rektanglar korsas "kolliderar" objekten med varandra.
        public bool CheckCollision(PhysicalObject other)
        {
            Rectangle myRect = new Rectangle(Convert.ToInt32(XCoord), Convert.ToInt32(YCoord),
                               Convert.ToInt32((ObjectWidth/6)-20), Convert.ToInt32(ObjectHight/2 + 50));
            Rectangle otherRect = new Rectangle(Convert.ToInt32(other.XCoord), Convert.ToInt32(other.YCoord),
                                  Convert.ToInt32((other.ObjectWidth / 6)), Convert.ToInt32(other.ObjectHight));
            return myRect.Intersects(otherRect);
        }

        //egenskpaer hos klassen, hämtar data från andra klasser för att avgöra om ett objekt lever eller inte.
        public bool IsAlive
        {
            get { return isAlive; }
            set { isAlive = value; }
        }
    }
}


