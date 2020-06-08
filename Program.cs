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
            Console.WriteLine("Hello World!");
            window = new RenderWindow(new SFML.Window.VideoMode(600,480), "Dox v1.0");

            
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
                Vector2i position = Mouse.GetPosition();
                
                Console.WriteLine(string.Format("X: {0}, Y: {1}", position.X, position.Y));
                isMouseClicked = true;
            }
            else
            {
                isMouseClicked = false;
            }
        }

        static void Reset()
        {
        }
    }
}
