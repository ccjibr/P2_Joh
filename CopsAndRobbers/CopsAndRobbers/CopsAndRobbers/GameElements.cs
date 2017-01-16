using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.IO;

namespace CopsAndRobbers
{
    static class GameElements
    {

        //Anropar splere.
        static Player player;

        //Skapar en lista så att fler fiender kan skapas samt ska fienderna få egna texture2D sprites senare i koden.
        static List<Enemy> enemies;
        static Texture2D EnemySpriteRobbe;
        static Texture2D EnemySpriteAllan;

        //Lista som kan skapa fler mynt samt ska mynten ha en Texture2D sprite senare i koden.
        static List<Coins> coins;
        static Texture2D CoinSprite;

        //Skapar en font som gör att jag kan skriva ut text på skärmen.
        static SpriteFont myFont;

        static int playerPoints; 

        //Objekt för att kunna skapa menyn.
        static Menu menu;

        static Texture2D arrows;
        static Texture2D spaecebar;


        static HighScore highScore; 

        //Skapar olika gamestates.
        public enum State { Menu, Run, HighScore, HighScoreView, Quit };
        public static State currentState;
        public static State highscoreState;

        //Metod som ska anropas av Game1.Initialize() när spelet startas. I denna metod ska all kod som initiera objekten finnas och skapa dem.  
        public static void Initialize()
        {
            highScore = new HighScore(5);
            coins = new List<Coins>();
        }

        // Metod som ska anropas av Game1.LoadContent när spelet startas. I denna metod ska all kod som ska ladda in olika filer finnas.
        public static void LoadContent(ContentManager content, GameWindow window)
        {
            //Anropar meny-objektets konstruktor och lägger till menybilderna till dem.
            menu = new Menu((int)State.Menu);

            //Lägger till bild och vilket state den motsvarar
            menu.AddItem(content.Load<Texture2D>("Images/start"), (int)State.Run);
            menu.AddItem(content.Load<Texture2D>("Images/highscore"), (int)State.HighScore);
            menu.AddItem(content.Load<Texture2D>("Images/exit"), (int)State.Quit);
            

            //Laddar in bilden och skickar bilden till Player klassen samt bstämmer hastigheten på spelaren och storleken på fönstret.
            player = new Player(content.Load<Texture2D>("Images/Police (1)"), 1, 6, 380, 400, 7, 7, content.Load<Texture2D>("Images/bullet"));

            //Laddar in mynten och fienden.
            CoinSprite = content.Load<Texture2D>("Images/coin");
            EnemySpriteRobbe = content.Load<Texture2D>("Images/Robbe");
            EnemySpriteAllan = content.Load<Texture2D>("Images/maskman");

            //Ska sladda in spelarnas HS från en text-fil.
            highScore.LoadFromFile("HS-Save.txt");

            //Laddar in bilder till menyn.
            arrows = content.Load<Texture2D>("Images/arrowKeys");
            spaecebar = content.Load<Texture2D>("Images/spacebar");

            //(menu.Enterpressed = highScore.Enterpressed)

            //Skapar en lista för att kunna spawna in fler fiender samt skapar en random för att kunna placera ut dem slumpmässigt samt bestämmer vilket intervall som fineden ska skapas i.
            enemies = new List<Enemy>();
            Random random = new Random();


            //När spelet starar ska 5 av varje sort fiender skapas. 
            for (int i = 0; i < 5; i++)
            {
                //Skapar slumpmässiga koordninater för vart fienden ska "spawna" nånstans, dock ska det ske utanför bildskärmen på X-ledet men innanför spelrutan i Y-ledet.
                int rndX = random.Next(2000, 2500);
                int rndY = random.Next(0, window.ClientBounds.Height - 100);
                Robbe temp = new Robbe(EnemySpriteRobbe, 1, 6, rndX, rndY, 0, 0);
                Allan temp2 = new Allan(EnemySpriteAllan, 1, 6, rndX, rndY, 0, 0);
                enemies.Add(temp);
                enemies.Add(temp2);
            }

            //Laddar in en font som sedan ska skrivas ut på spelskärmen.
            myFont = content.Load<SpriteFont>("myFont");

        }

        public static void UnloadContent()
        {
            
            highScore.SaveToFile("HS-Save.txt"); 
        }

        //Metod som ska kontrollera vad användare väljer för menyval.
        public static State MenuUpdate(GameTime gameTime)
        {

            //Returnerar menyns state med gameTime med hjälp av menuUpdate.
            return (State)menu.Update(gameTime);
        }

        //Metod som ska rita ut menyn.
        public static void MenuDraw(SpriteBatch spriteBatch)
        {
            //Ritar ut bilder för instruktioner.
            spriteBatch.Draw(arrows, new Vector2(100, 300));
            spriteBatch.Draw(spaecebar, new Vector2(100, 550));

            //Ritar ut texten med instruktioner.
            spriteBatch.DrawString(myFont,"Flytta med piltangenterna!", new Vector2(110, 500), Color.Black);
            spriteBatch.DrawString(myFont,"Skjut med mellanslag!" ,new Vector2(110, 700), Color.Black);
            spriteBatch.DrawString(myFont, "Spara ditt highscore efter spelet med E-knappen!", new Vector2(1100, 400), Color.Black);
            menu.Draw(spriteBatch);
        }

        //Update-metod som ska uppdatera själva spelet när det körs.
        public static State RunUpdate(ContentManager content, GameWindow window, GameTime gameTime)
        {
            //Anropar player till spelet.
            player.Update(window, gameTime);
            Random random = new Random();
            


            //Det ska successivt komma fler fiender under spelets gång.
            int ERandom = random.Next(1, 60);
            if (ERandom == 1)
            {
                int rndX = random.Next(2000, 2500);
                int rndY = random.Next(0, window.ClientBounds.Height - 100);
                Robbe temp = new Robbe(EnemySpriteRobbe, 1, 6, rndX, rndY, 0, 0);
                enemies.Add(temp);
            }

            //Spawnar in successivt fler fineder, dock färre Allan än Robbe.
            int EARandom = random.Next(1, 100);
            if (EARandom == 1)
            {
                int rndX = random.Next(2000, 2500);
                int rndY = random.Next(0, window.ClientBounds.Height - 100);
                Allan temp = new Allan(EnemySpriteAllan, 1, 6, rndX, rndY, 0, 0);
                enemies.Add(temp);
            }


            // Kontrollera ifall kulorna kolliserar med fienderna, ifall de gör det, döda fienderna.
            foreach (Enemy e in enemies.ToList())
            {
                //Så länge fienden lever ska de uppdateras utifrån villkoren i Enemy klassen och.
                if (e.IsAlive)
                    e.Update(window, gameTime);

                foreach (Bullet b in player.Bullets)
                {
                    //Om kulan träffar en fiende ska den dö och och spelaren ska få ett poäng samtidigt som kulan som träffat fienden ska försvinna.
                    if (e.CheckCollision(b))
                    {
                        e.IsAlive = false;
                        player.Points++;
                        b.IsAlive = false;
                    }
                }

                //Kontrollerar om fienden lever och om de har gjort det och kolliderat med spelaren ska spelet avslutas och gå till HS-menyn.
                if (e.IsAlive)
                {
                    if (e.CheckCollision(player))
                        player.IsAlive = false;
                    e.Update(window, gameTime);
                }

                else
                    enemies.Remove(e);
            }

            //Mynten ska spawna in slumpmässigt, en chans på 200.
            int newCoin = random.Next(1, 200);

            //Om ett nytt mynt har skapats ska det spawna innanför spelgränserna.
            if (newCoin == 1)
            {
                int rndX = random.Next(0, window.ClientBounds.Width - CoinSprite.Width);
                int rndY = random.Next(0, window.ClientBounds.Height - CoinSprite.Height);

                //Lägger till myntet i listan.
                coins.Add(new Coins(CoinSprite, rndX, rndY, gameTime));
            }

            //Går igenom listan för att kontrollera om mynten lever eller om de ska tas bort eller om de har kolliderat med spelaren.
            foreach (Coins c in coins.ToList())
            {

                //Kontrollerar hur gammalt myntet är och om det ska tas bort.
                if (c.IsAlive)
                {
                    c.Update(gameTime);
                    //Kontrollerar ifall spelaren har kontrollerat med myntet.
                    if (c.CheckCollision(player))
                    {
                        //Tar bort myntet ifall det har kolliderat med spelaren.
                        coins.Remove(c);
                        player.Points++;
                        
                    }
                }
                // Om det inte lever tas myntet bort.
                else
                    coins.Remove(c);
            }

            playerPoints = player.Points;

            // Om spelaren är död ska programmet återgå till menyn samtidigt som alla objekt ska återställas inför nästa start a spelet.

            if (!player.IsAlive)
            {
                Reset(window, content);
                
                return State.HighScore;
            }
            //Annars ska programmet stanna kvar i Run läget.
            return State.Run;



        }

        //Metod som ska rita ut själva spelet när det körs.
        public static void RunDraw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            // Ritar ut spelaren.
            player.Draw(spriteBatch);

            //Ritar ut fienden.
            foreach (Enemy e in enemies.ToList())
                e.Draw(spriteBatch, gameTime);

            // Ritar ut mynten.
            foreach (Coins c in coins)
                c.DrawObject(spriteBatch);

            spriteBatch.DrawString(myFont, "Points: " + player.Points, new Vector2(200, 0), Color.Black);


        }

        //Metod för att uppdatera heighscorelistan.
        public static State HighScoreUpdate(GameTime gameTime)
        {
            
            KeyboardState keyboardState = Keyboard.GetState();

            

            switch (highscoreState)
            {
                case State.HighScore: // Skriv in oss i listan

                    // Fortsätt så länge HighScore.EnterUpdate() returnerar true:
                    if (highScore.EnterUpdate(gameTime, playerPoints))
                    {
                        highscoreState = State.HighScoreView;
                    }
                    break;
                //highscoreState = State.HighScore;    

                default: // Highscore-listan (tar emot en tangent)
                    if (keyboardState.IsKeyDown(Keys.E))
                        highscoreState = State.HighScore;      
                    break;

            }

            //Användaren ska återgå till menyn ifall hen trycker på escape.
            if (keyboardState.IsKeyDown(Keys.Escape))
                return State.Menu;
            //Annars ska användaren vara kvar i HighScore.
            return State.HighScore;

            
        }

        //Metod som ritar ut heighscorelistan.
        public static void HighScoreDraw(SpriteBatch spriteBatch)
        {
            switch (highscoreState)

            {
                //Ändrar spelet state så att användaren kan skriva in sitt namn.
                case State.HighScore:
                    highScore.EnterDraw(spriteBatch, myFont);
                    break;
                //Ritar ut HS-listan.
                default: 
                    highScore.PrintDraw(spriteBatch, myFont);
                    break;

            }
        }

        public static void Reset(GameWindow window, ContentManager content)
        {

            //"Resetar" spelaren så att man kan starta ett nytt spel.
            player.Reset(50, 400, 5, 7);
            enemies.Clear();
            
            Random random = new Random();
            Texture2D tmpSprite = content.Load<Texture2D>("Images/Robbe");

            for (int i = 0; i < 5; i++)
            {
                int rndX = random.Next(2000, 2500);
                int rndY = random.Next(0, window.ClientBounds.Height - 100);
                Robbe temp = new Robbe(EnemySpriteRobbe, 1, 6, rndX, rndY, 0, 0);
                enemies.Add(temp);
            }

            tmpSprite = content.Load<Texture2D>("Images/maskman");
            for (int i = 0; i < 5; i++)
            {
                int rndX = random.Next(2000, 2500);
                int rndY = random.Next(0, window.ClientBounds.Height - 100);
                Allan temp = new Allan(EnemySpriteAllan, 1, 6, rndX, rndY, 0, 0);
                enemies.Add(temp);
            }
        }
    }
    }
