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
                DrawX(1, 1);

                window.Display();
                window.DispatchEvents();
            }
        }

        static void DrawGrid()
        {
            float lineThickness = 15f;
            float lineLength = 480f;

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
            CircleShape top = new CircleShape(40, 3);
            CircleShape bottom = new CircleShape(40, 3);

            top.Position = new Vector2f(((row * 10) + 80) + 40, (col * 10) + 80);
            bottom.Position = new Vector2f((row * 10) + 40, (col * 10) + 70);

            top.Rotation = 180f;

            window.Draw(top);
            window.Draw(bottom);
        }

        static void DrawO(Vector2f position)
        {

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
