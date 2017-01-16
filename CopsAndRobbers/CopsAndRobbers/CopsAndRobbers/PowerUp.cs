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
    class PowerUp
    {
    }

    class Coins : PhysicalObject
    {
        //Variabel som bestämmer hur länge ett mynt lever.
        double TimeToDie;

        //Kontruktor för att skapa mynten.
        public Coins (Texture2D cTexture, float cX, float cY, GameTime gameTime) : base (cTexture, cX,cY, 0, 0)
        {
            //Bestämmer att myntet ska finnas i 5 sekunder.
            TimeToDie = gameTime.TotalGameTime.TotalMilliseconds + 5000; 
        }

        public void Update(GameTime gameTime)
        {
            if(TimeToDie < gameTime.TotalGameTime.TotalMilliseconds)
                isAlive = false;
        }
    }
}
