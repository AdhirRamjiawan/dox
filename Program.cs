using System;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Dox
{
    class Program
    {

        static RenderWindow window;
        static bool isMouseClicked;

        static float lineThickness = 15f;
        static float lineLength = 480f;

        static int[,] state = new int[3,3] {
            {0,0,0},
            {0,0,0},
            {0,0,0}
        };

        static string currentPlayer = "x";
        static bool hasWon = false;

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

        static void Main(string[] args)
        {
            const string version = "Dox v1.0";
            Console.WriteLine(version);
            window = new RenderWindow(new SFML.Window.VideoMode(600,480), version);

            window.Closed += onClose;
            window.KeyPressed += onKeyPressed;

            Reset();

            while (window.IsOpen)
            {
                window.Clear();

                HandleKeyboardInput();
                HandleMouseInput();

                DrawGrid();
                /*DrawX(1, 1);
                DrawX(2, 1);
                DrawX(3, 1);

                DrawX(1, 2);
                DrawX(2, 2);
                DrawX(3, 2);

                DrawX(1, 3);
                DrawX(2, 3);
                DrawX(3, 3);*/

                DrawO(1, 1);
                DrawO(2, 1);
                DrawO(3, 1);

                DrawO(1, 2);
                DrawO(2, 2);
                DrawO(3, 2);

                DrawO(1, 3);
                DrawO(2, 3);
                DrawO(3, 3);

                window.Display();
                window.DispatchEvents();
            }
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

        static void DrawX(int row, int col)
        {
            RectangleShape l = new RectangleShape();
            l.Size = new Vector2f(100, lineThickness);
            l.Position = new Vector2f((row * 160) - 110, (col * 160) - 130);
            l.Rotation = 45;
            window.Draw(l);
        }

        static void DrawO(int row, int col)
        {
            CircleShape c = new CircleShape(40);
            c.Position = new Vector2f((row * 160) - 115, (col * 160) - 115);
            window.Draw(c);
        }

        static void HandleKeyboardInput()
        {
            if (Keyboard.IsKeyPressed(Keyboard.Key.Right))
            {

            }
        }

        static void HandleMouseInput()
        {
            if (!isMouseClicked && Mouse.IsButtonPressed(Mouse.Button.Left))
            {
                Vector2i position = Mouse.GetPosition(window);
                
                Console.WriteLine(string.Format("X: {0}, Y: {1}", position.X, position.Y));
                isMouseClicked = true;

                var gp = GetGridPosition(position.X, position.Y);

                if (gp != null)
                {
                    Console.WriteLine(string.Format("Grid Position {0}, {1}", gp.Item1, gp.Item2));
                }
                

                CheckWin();
            }
            else
            {
                isMouseClicked = false;
            }
        }

        static void Reset()
        {
            hasWon = false;
            state = new int[3,3] {
                {0,0,0},
                {0,0,0},
                {0,0,0}
            };
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
    }
}
