using System;
using SFML.Graphics;
using SFML.Window;

namespace Dox
{
    public partial class Program
    {
        
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
    }
}