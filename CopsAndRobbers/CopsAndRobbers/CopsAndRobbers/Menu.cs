using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace CopsAndRobbers
{
    class Menu
    {
        //Skapar en lista för menyvalen.
        List<MenuItem> menu;

        //Objekt för första valet i listan och som är valt som "standard".
        int selected = 0;

        //Objekt för att rita att kunna rita ut menyvalen (från menuItems) på olika höjd. 
        float currentHeight = 0;

        //Objekt för att sakta ned tangentryckningarna så att det inte går för fort när man bläddrar mellan menyvalen.
        double lastChange = 0;

        //Objekt som representerar själva menyns state.
        int defaultMenuState;


        //Konstruktor som skapar en listan med menuItems, dvs de olika menyvalen.
        public Menu(int defaultMenuState)
        {
            menu = new List<MenuItem>();

            //GameState för menyn.
            this.defaultMenuState = defaultMenuState;


        }

        //Metod som lägger till menyval i listan med MenuItem. Menyn ska både ha en bikd och en state.
        public void AddItem(Texture2D itemTexture, int state)
        {
            //Koordinater för vart på spelrutan som item ska ritas ut på. 
            float X = 700;
            float Y = 300+ currentHeight;

            //Ändrar currentHeight efter items koordinater och lite extra mellanrum melan varje val.
            currentHeight += itemTexture.Height + 20;

            //Skapar ett tillfälligt objekt och lägger den i listan.
            MenuItem temp = new MenuItem(itemTexture, new Vector2(X, Y), state);
            menu.Add(temp);
        }

        //Meto som kontrollerar ifall användaren har tryckt på en tangent som ska göra att menyn hoppar till nästa val, eller att användaren väljer menyvalet.
        public int Update(GameTime gameTime)
        {
            //Läser in tangentryckningar.
            KeyboardState keyboardState = Keyboard.GetState();

            //If-sats som ska låta användaren byta mellan olika menyval. För att det inte ska gå för fort att bläddra måste programmet sakta ned menyvalen.
            if (lastChange + 130 < gameTime.TotalGameTime.TotalMilliseconds)
            {
                //Om användaren tycker på piltangenten nedåt ska valet gå ned ett steg i menyn.
                if (keyboardState.IsKeyDown(Keys.Down))
                {
                    selected++;

                    //Om använaren kommer längs ned i menyn ska denne kunna gå utanför och börja på toppen i menyn igen.
                    if (selected > menu.Count - 1)
                        selected = 0;
                }

                //Om användaren tycker på piltangenten uppåt ska valet gå ned ett steg i menyn.
                if (keyboardState.IsKeyDown(Keys.Up))
                {
                    selected--;

                    //Om använaren kommer längs nupp i menyn ska denne kunna gå utanför och börja på sistamenyvalet.
                    if (selected < 0)
                        selected = menu.Count - 1;
                }

                //Ställer in lastChange till det exakta ögonblicket.
                lastChange = gameTime.TotalGameTime.TotalMilliseconds;

            }

            //Om användaren vill välja ett menyval ska denne kunna trycka på enter.
            if (keyboardState.IsKeyDown(Keys.Enter))
                return menu[selected].menuState;
            //Returnerar menyvalets state.
            return defaultMenuState;
        }

        //Metod som ritar ut menyn.
        public void Draw(SpriteBatch spriteBatch)
        {
            //Ritar ut det aktiva menyvalet med en speciell färg.
            for (int i = 0; i < menu.Count; i++)
            {
                if (i == selected)
                    spriteBatch.Draw(menu[i].menuTexture, menu[i].menuPosition, Color.Red);
                else
                    spriteBatch.Draw(menu[i].menuTexture, menu[i].menuPosition, Color.White);
            }
        }
    }
    class MenuItem
    {
        //Variabler för menyns bildval samt menyvalets position och state.
        Texture2D MenuTexture;
        Vector2 MenuPosition;
        int currentState;

        //Konstruktor som ska sätta värden för de olika menyvalen.
        public MenuItem(Texture2D menuTexture, Vector2 menuPosition, int currentState)
        {
            this.MenuTexture = menuTexture;
            this.MenuPosition = menuPosition;
            this.currentState = currentState;
        }

        //Klassens egenskaper.
        public Texture2D menuTexture { get { return MenuTexture; } }
        public Vector2 menuPosition { get { return MenuPosition; } }
        public int menuState { get { return currentState; } }
    }
}
