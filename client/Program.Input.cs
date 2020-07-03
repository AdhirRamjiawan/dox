using System;
using System.Threading;
using System.Threading.Tasks;
using SFML.System;
using SFML.Window;


namespace Dox
{
    public partial class Program
    {
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

            if(globalGameState == -3)
            {
                globalGameState = -2;
                Reset();
                return;
            }
            else if(globalGameState == -2)
            {
                Vector2i mousePosition =  Mouse.GetPosition(window);

                if (mousePosition.X > 150 && mousePosition.X < 400)
                {
                    if (mousePosition.Y > 150 && mousePosition.Y < 200)
                    {
                        currentGameType = GameType.MultiPlayerLocal;
                        globalGameState = 0;
                    }
                    else if (mousePosition.Y > 250 && mousePosition.Y < 300)
                    {
                        GetMultiplayerClientIdForGame();
                        GetAvailableRooms();
                        globalGameState = -1;
                        currentGameType = GameType.MultiPlayerOnline; 
                    }
                    else
                        return;
                }
                else
                {
                    return;
                }
                
                return;
            }
            else if (globalGameState == -1)
            {
                // test code
                multiplayerRoomId = 0;
                AllocateRoomId(multiplayerRoomId, multiplayerClientId);
                globalGameState = 0;
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

                if (state[gp.Item2, gp.Item1] == 0)
                {
                    if (currentGameType == GameType.MultiPlayerOnline)
                    {
                        if (isMultiplayerPlayLocked)
                            return;

                        SendNetworkPlay(gp.Item1, gp.Item2);
                        isMultiplayerPlayLocked = true;
                        playsLeft--;

                        //#if DOX_DEBUG
                            Console.WriteLine($"After network play currentPlayer {currentPlayer}");
                        //#endif

                        state[gp.Item2, gp.Item1] = currentPlayer;

                        PollNetworkPlay();
                    }
                    else
                    {
                        state[gp.Item2, gp.Item1] = currentPlayer;
                        SwitchPlayer();
                    }
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
                
                if (currentGameType == GameType.MultiPlayerLocal)
                    SwitchPlayer();

                Console.WriteLine($"We have a winner: {currentPlayer}!");
            }

            if (hasDrawn)
            {
                globalGameState = 2;
            }
        }
    }
}