using SFML.Graphics;
using SFML.System;
using SFML.Window;


namespace Dox 
{
    public partial class Program
    {
        
        static void DrawClickToContinue()
        {
            Text subText = new Text("Click to Continue", font);
            subText.CharacterSize = 20;
            subText.FillColor = Color.Blue;
            subText.Position = new Vector2f(220, 300);

            window.Draw(subText);
        }

        static void DrawIntro()
        {
            Text text = new Text("~-=|DoX|=-~", font);
            text.CharacterSize = 50;
            text.FillColor = Color.Blue;
            text.Position = new Vector2f(150, 170);

            window.Clear(Color.White);
            window.Draw(text);
            DrawClickToContinue();
        }

        static void DrawSelectGameType()
        {
            Text title = CreateText("Select game", 30, Color.Blue, 150, 70);
            Text option1 = CreateText("2P Local", 20, Color.Blue, 150, 150);
            Text option2 = CreateText("2P Online", 20, Color.Blue, 150, 250);

            Vector2i mousePosition =  Mouse.GetPosition(window);

            if (mousePosition.X > 150 && mousePosition.X < 400)
            {
                if (mousePosition.Y > 150 && mousePosition.Y < 200)
                    option1.FillColor = Color.Magenta;

                if (mousePosition.Y > 250 && mousePosition.Y < 300)
                    option2.FillColor = Color.Magenta;
            }

            window.Clear(Color.White);
            window.Draw(title);
            window.Draw(option1);
            window.Draw(option2);
        }

        static void DrawSelectGameRoom()
        {
            Text title = CreateText("Select room", 30, Color.Blue, 150, 70);
            Vector2i mousePosition =  Mouse.GetPosition(window);

            window.Clear(Color.White);
            window.Draw(title);
            int roomIndex = 1;

            foreach (var room in gameRooms)
            {
                Text option1 = CreateText($"Room {room}", 20, Color.Blue, 150, 150 + (50 * roomIndex));
                
                if (mousePosition.X > 150 && mousePosition.X < 400)
                {
                    if (mousePosition.Y > (150 + (50 * roomIndex)) && mousePosition.Y < (200 + (50 * roomIndex)))
                        option1.FillColor = Color.Magenta;
                }

                window.Draw(option1);
                roomIndex++;
            }
        }

        static void DrawWinText()
        {
            if (!hasWon)
                return;

            string strCurrentPlayer =   (currentPlayer == 1) ? "O" : 
                                        (currentPlayer == 2) ? "X" :"";
            string winMessage = $"{strCurrentPlayer} has won!";
            Text text = new Text(winMessage, font);
            text.CharacterSize = 50;
            text.FillColor = Color.Blue;
            text.Position = new Vector2f(170, 170);

            window.Clear(Color.White);
            window.Draw(text);
            DrawClickToContinue();
        }

        static void DrawDrawText()
        {
            Text text = new Text("Match ends in draw!", font);
            text.CharacterSize = 50;
            text.FillColor = Color.Red;
            text.Position = new Vector2f(70, 170);

            window.Clear(Color.White);
            window.Draw(text);
            DrawClickToContinue();
        }

        static Text CreateText(string message, uint size, Color color, float x, float y)
        {
            Text text = new Text(message, font);
            text.CharacterSize = size;
            text.FillColor = color;
            text.Position = new Vector2f(x,y);
            return text;
        }

        static void DrawGrid()
        {
            // draw horizontal lines
            for (int i = 0; i < 4; i++)
            {
                RectangleShape line = new RectangleShape(new Vector2f(lineLength, lineThickness));
                line.Position = new Vector2f(0, 155 * i);
                window.Draw(line);  
            }

            // draw vertical lines
            for (int i = 0; i < 4; i++)
            {
                RectangleShape line = new RectangleShape(new Vector2f(lineLength, lineThickness));
                line.Position = new Vector2f(160 * i + lineThickness, 0);
                line.Rotation = 90;
                window.Draw(line);  
            }
        }

        static void DrawNetworkPlayLocked()
        {
            Text text = CreateText("Waiting\nfor\nOpponent...", 15, Color.Yellow, 510, 50);
            window.Draw(text);
        }

        static void DrawCurrentPlayerSymbol()
        {
            Text text = CreateText($"Your\nsymbol is\n{currentPlayer}...", 15, Color.White, 510, 150);
            window.Draw(text);
        }

        static void DrawX(int col, int row)
        {
            row++;
            col++;

            RectangleShape l = new RectangleShape();
            l.Size = new Vector2f(100, lineThickness);
            l.Position = new Vector2f((row * 160) - 110, (col * 160) - 130);
            l.Rotation = 45;
            window.Draw(l);
        }

        static void DrawO(int col, int row)
        {
            row++;
            col++;

            CircleShape c = new CircleShape(40);
            c.Position = new Vector2f((row * 160) - 115, (col * 160) - 115);
            window.Draw(c);
        }

        static void DrawPlays()
        {
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    if (state[row, col] == 1)
                        DrawX(col, row);
                    else if (state[row, col] == 2)
                        DrawO(col, row);
                }
            }
        }
    }
}