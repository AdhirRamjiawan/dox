#define DOX_DEBUG

using System;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Dox
{
    class Program
    {

        static int globalGameState = -1;
        static readonly uint screenHeight = 480;
        static readonly uint screenWidth = 600;
        static Font font;
        static readonly string fontName = "Ubuntu-Bold.ttf";
        static RenderWindow window;
        static bool isMouseClicked;

        static float lineThickness = 15f;
        static float lineLength = 480f;

        static readonly int[,] emptyState = new int[3,3] {
            {0,0,0},
            {0,0,0},
            {0,0,0}
        };

        static int[,] state = emptyState.Clone() as int[,];

        static int currentPlayer = 1;
        static bool hasWon = false;
        static bool hasDrawn = false;
        static int playsLeft = 9;

        static void Reset()
        {
            playsLeft = 9;
            hasWon = false;
            currentPlayer = 1;
            state = emptyState.Clone() as int[,];
        }

        static void Main(string[] args)
        {
            const string version = "Dox v1.0";
            Console.WriteLine(version);
            window = new RenderWindow(new SFML.Window.VideoMode(screenWidth,screenHeight), version);

            window.Closed += onClose;
            window.KeyPressed += onKeyPressed;
            window.MouseButtonPressed += onMouseButtonPressed;

            font = new Font(fontName);

            Reset();

            while (window.IsOpen)
            {
                window.Clear();

                if (globalGameState == -1) 
                {
                    DrawIntro();
                }
                else if (globalGameState == 0)
                {
                    DrawGrid();
                    DrawPlays();
                }
                else if (globalGameState == 1)
                {
                    DrawWinText();
                }
                else if (globalGameState == 2)
                {
                    DrawDrawText();
                }
                
                window.Display();
                window.DispatchEvents();
            }
        }

        #region Event Handling
        static void onClose(object sender, EventArgs e)
        {
            RenderWindow window = sender as RenderWindow;
            window.Close();
        }

        static void onKeyPressed(object sender, KeyEventArgs e)
        {
            if (e.Code == Keyboard.Key.R)
                Reset();
        }

        static void onMouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            HandleMouseInput();
        }
        #endregion

        #region Draw Calls

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
        #endregion

        #region Input Handling
        static void HandleKeyboardInput()
        {
            if (Keyboard.IsKeyPressed(Keyboard.Key.Right))
            {

            }
        }

        static void HandleMouseInput()
        {
            Vector2i position = Mouse.GetPosition(window);
            
            #if DOX_DEBUG
            Console.WriteLine(string.Format("X: {0}, Y: {1}", position.X, position.Y));
            #endif

            if(globalGameState == -1)
            {
                globalGameState = 0;
                Reset();
                return;
            }
            else if (globalGameState == 1)
            {
                globalGameState = 0;
                Reset();
                return;
            }
            else if (globalGameState == 2)
            {
                globalGameState = 0;
                Reset();
                return;
            }


            var gp = GetGridPosition(position.X, position.Y);

            if (gp != null)
            {
                //DrawO(gp.Item1, gp.Item2);

                if (state[gp.Item2, gp.Item1] == 0)
                {
                    state[gp.Item2, gp.Item1] = currentPlayer;
                    SwitchPlayer();
                }
                else
                {
                    // stub sound
                }

                #if DOX_DEBUG
                Console.WriteLine(string.Format("Grid Position {0}, {1}", gp.Item1, gp.Item2));
                #endif
            }
            

            CheckWin();
            CheckDraw();

            if (hasWon)
            {
                globalGameState = 1;
                //Reset();
                Console.WriteLine($"We have a winner: {currentPlayer}!");
            }

            if (hasDrawn)
            {
                globalGameState = 2;
            }
        }
    #endregion
        
        #region Logic Functions

        static void CheckDraw() 
        {
            hasDrawn = playsLeft == 0 && !hasWon;
        }
        static void CheckWin()
        {
            // row checks
            for (int i = 0; i < 3; i++)
            {
                if (state[0,i] == 0 || state[1,i] == 0 || state[2,i] == 0)
                    continue;
                
                if (state[0,i] == state[1,i] && state[1,i] == state[2,i])
                {
                    hasWon = true;
                    return;
                }
            }

            // column checks
            for (int i = 1; i < 3; i++)
            {
                if (state[i, 0] == 0 || state[i,1] == 0 || state[i,2] == 0)
                    continue;

                if (state[i,0] == state[i,1] && state[i,1] == state[i,2])
                {
                    hasWon = true;
                    return;
                }
            }

            // diagonal checks
            if (state[0,0] != 0 && state[0,0] == state[1,1] && state[1,1] == state[2,2])
            {
                hasWon = true;
                return;
            }

            if (state[0,2] != 0 && state[0,2] == state[1,1] && state[1,1] == state[2,0])
            {
                hasWon = true;
                return;
            }
        }

        static Tuple<int, int> GetGridPosition(int x, int y)
        {
            int space = 160;

            // THIS CAN BE SIMPLIFIED

            // ROW 1 CHECKS
            if (x > 0 && x < space && y > 0 && y < space)
                return new Tuple<int, int>(0,0);

            if (x > space && x < (2 * space) && y > 0 && y < space)
                return new Tuple<int, int>(0,1);

            if (x > (2 * space) && x < (3 * space) && y > 0 && y < space)
                return new Tuple<int, int>(0,2);

            // ROW 2 CHECKS
            if (x > 0 && x < space && y > space && y < (2 *space))
                return new Tuple<int, int>(1,0);

            if (x > space && x < (2 * space) && y > space && y < (2 *space))
                return new Tuple<int, int>(1,1);

            if (x > (2 * space) && x < (3 * space) && y > space && y < (2 *space))
                return new Tuple<int, int>(1,2);

            // ROW 3 CHECKS
            if (x > 0 && x < space && y > (2 * space) && y < (3 * space))
                return new Tuple<int, int>(2,0);

            if (x > space && x < (2 * space) && y > (2 * space) && y < (3 * space))
                return new Tuple<int, int>(2,1);

            if (x > (2 * space) && x < (3 * space) && y > (2 * space) && y < (3 * space))
                return new Tuple<int, int>(2,2);

            return null;
        }

        static void SwitchPlayer() 
        {
            currentPlayer = (currentPlayer == 1) ? 2:1;
            playsLeft--;
        }

        #endregion
    }
}
