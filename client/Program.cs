#define DOX_DEBUG

using System;
using SFML.Graphics;

namespace Dox
{
    public partial class Program
    {
        enum GameType {
            MultiPlayerLocal = 1,
            MultiPlayerOnline = 2
        }

        static void Reset()
        {
            playsLeft = 9;
            hasWon = false;
            currentPlayer = 1;
            state = emptyState.Clone() as int[,];
            multiplayerClientId = 0;
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

            currentGameType = GameType.MultiPlayerLocal;

            Reset();

            while (window.IsOpen)
            {
                window.Clear();

                if (globalGameState == -3) 
                {
                    DrawIntro();
                }
                else if (globalGameState == -2)
                {
                    DrawSelectGameType();
                }
                else if (globalGameState == -1)
                {
                    DrawSelectGameRoom();
                }
                else if (globalGameState == 0)
                {
                    DrawGrid();
                    DrawPlays();

                    if (currentGameType == GameType.MultiPlayerOnline)
                    {
                        if (isMultiplayerPlayLocked)
                        {
                            DrawNetworkPlayLocked();
                        }

                        if (currentPlayer != 0)
                        {
                            DrawCurrentPlayerSymbol();
                        }
                    }
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
        
    }
}
