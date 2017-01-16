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


    
    // HighScore, innehåller en lista med hsItems samt metoder för att manipulera listan.
    class HighScore
    {
        // Objekt för hur många spelare som får vara i listan.
        int maxInList = 5;
        
        //Lista för alla spelarna som kan vara med i HS listan. 
        List<HSItem> highscore = new List<HSItem>();
        
        //Objekt för spelarnas namn.
        string name; 

        // Objekt som ska användas för att kunna skriva ut vilket tecken spelaren har valt just nu.
        string currentChar;

        //Objekt för att mata in spelarens namn.
        int key_index = 0;

        // Objekt som ska användas för att kontrollera när tangenter trycktes in:
        double lastChange = 0;
        Keys previousKey;

        
        // Klassens konstruktor
        public HighScore(int maxInList)
        {
            this.maxInList = maxInList;
        }


        // Metod som ska sortera HS-listan. Metoden anropas av metoden Add() när en ny Spelare ska läggas till i listan.Metoden använder algoritmen bubblesort.
        void Sort()
        {
            int max = highscore.Count - 1;

            // Den yttre loopen, går igenom hela listan            
            for (int i = 0; i < max; i++)
            {
                // Den inre, går igenom element för element för att se hur många som redan gåtts igenom.
                int nrLeft = max - i;
                for (int j = 0; j < nrLeft; j++)
                {
                    // Jämför elementen.
                    if (highscore[j].playerPoints < highscore[j + 1].playerPoints) 
                    {
                        // Byter plats på elementen.
                        HSItem temp = highscore[j];
                        highscore[j] = highscore[j + 1];
                        highscore[j + 1] = temp;
                    }
                }
            }
        }

        // Metod som lägger till en ny spelare i HS-listan.
        void Add(int points)
        {
            // Skapar en tillfällig variabel och lägger till den i HS-listan.
            HSItem temp = new HSItem(name, points);
            highscore.Add(temp);

            // Listan ska sorteras efter att en spelare har lagt till.
            Sort(); 

            // Om det finns för många spelare i listan ska den spelare som ligger sist tas bort. 
            if (highscore.Count > maxInList)
            {
                //I och med att den spelare med lägst poäng har sorerats till slutet av listan i kan den sista spelaren bara tas bort.
                highscore.RemoveAt(maxInList);
            }
        }

        // Metod som kontrollerar om användaren  har tryckt ned en viss tangent och om det gått lagom lång tid det tagit sedan denne har tryckt på samma tangent.
        bool CheckKey(Keys key, GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(key))
            {
                //Kontrollerar om det gått tillräckligt lång tid sedan senaste tryck, eller om en ny tangent tryckts ned.
                if (lastChange + 130 < gameTime.TotalGameTime.TotalMilliseconds
                    || previousKey != key)
                {
                    // Återställer variablerna inför nästa varv i spelloopen:
                    previousKey = key;
                    lastChange = gameTime.TotalGameTime.TotalMilliseconds;
                    return true;
                }
            }
            // Om tangenten (key) inte trycktes ned, eller om samma tangent trycktes ned alldeles nyligen.
            return false;
        }

        // Metod för att skriva ut listan.
        public void PrintDraw(SpriteBatch spriteBatch, SpriteFont font)
        {
            
            string text = "HIGHSCORE\n";
            foreach (HSItem h in highscore)
                text += h.playerName + " " + h.playerPoints + "\n";

            spriteBatch.DrawString(font, text, new Vector2(700,500), Color.Black);
        }

        
        // Metod för användaren matar in sitt användarnamn där bokstäverna A-Z är tillåtna.
        // är Update-delen i spel-loopen för inmatning av highscore-namn. Metoden
        // ska fortsätta anropas av Update() så länge true returneras.
        public bool EnterUpdate(GameTime gameTime, int points)
        {
            // Tillgängliga tecken.
            char[] key = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K',
                       'L', 'M', 'N', 'O', 'P',  'Q', 'R', 'S', 'T', 'U',
                       'V', 'X', 'Y', 'Z'};


            // Användaren trycker knappen nedåt, stega framlänges i key-vektorn:
            if (CheckKey(Keys.Down, gameTime))
            {
                key_index++;
                if (key_index >= key.Length)
                    key_index = 0;
            }

            // Användaren trycker knappen uppåt, stega baklänges i key-vektorn:
            if (CheckKey(Keys.Up, gameTime))
            {
                key_index--;
                if (key_index <= 0)
                    key_index = key.Length - 1;
            }

            // Användaren trycker höger piltangent, lägg till det valda tecknet i strängen name.
            if (CheckKey(Keys.Right, gameTime))
            {
                name += key[key_index].ToString();
                if (name.Length == 5)
                {
                    // Återställ namnet och allt så att man kan lägga till namnet på en ny spelare:
                    Add(points);
                    name = "";
                    currentChar = "";
                    key_index = 0;


                    //Anger att återställningen är klar.
                    return true; 
                }
            }
            // Lagrar det tecken som nu är valt, så att det kan skrivas ut i EnterDraw()-metoden.
            currentChar = key[key_index].ToString();
            // Anger att användaren inte är färdig med sitt namn än, fortsätter anropa denna metod via Update():
            return false;
        }

        // Metod som skriver ut de tecken spelaren har matat in av sitt namn (om något) samt det tecken (av tre) som just nu är valt.
        public void EnterDraw(SpriteBatch spriteBatch, SpriteFont font)
        {
            string text = "ENTER NAME:" + name + currentChar;
            spriteBatch.DrawString(font, text, new Vector2(700,500), Color.Black);
        }

        // Metod för att spara HS till en fil.
        public void SaveToFile(string filename)
        {
            using (StreamWriter sw = new StreamWriter(filename))
            {
                foreach (HSItem item in highscore)

                {
                    //Sparar spelarens namn och poäng i en textfil.
                    string NameAndPoints = item.playerName + ":" + item.playerPoints;
                    sw.WriteLine(NameAndPoints);

                }
                sw.Close();
            }
        
        }

        // Metod för att ladda in HS från en fil.
        public void LoadFromFile(string filename)
        {
            StreamReader sr = new StreamReader("HS-Save.txt");

            string row;
            while ((row = sr.ReadLine()) != null)
            {
                // skapa en vektor som innehåller namn och poäng,
                // words[0] blir namnet och words[1] är poängen:
                string[] words = row.Split(':');
                int points = Convert.ToInt32(words[1]);
                // Lägg till i listan:
                HSItem temp = new HSItem(words[0], points);
                highscore.Add(temp);
            }

            sr.Close(); // Stäng filen

        }
    }

    // HsItem, en behållare-klass som innehåller info om en person i highscorelistan.
    class HSItem
    {
        // Variabler och egenskaper för spelarens namn och dennes poäng.
        string PlayerName;
        int PlayerPoints;

        public string playerName { get { return PlayerName; } set { PlayerName = value; } }
        public int playerPoints { get { return PlayerPoints; } set { PlayerPoints = value; } }


        // Klassens konstruktor
        public HSItem(string Pname, int Ppoints)
        {
            this.PlayerName = Pname;
            this.PlayerPoints = Ppoints;
        }
    }
}

