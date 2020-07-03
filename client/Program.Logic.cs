using System;
using System.Threading;
using System.Threading.Tasks;

namespace Dox
{
    public partial class Program
    {
        
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

        static void PollNetworkPlay()
        {
            Task.Run(()=>{
                bool keepPolling = true;
                while(keepPolling)
                {
                    Thread.Sleep(1000);
                    QueryLastNetworkPlay((receivedValidNetworkPlay) => {
                        if (receivedValidNetworkPlay)
                        {
                            keepPolling = false;
                            isMultiplayerPlayLocked = false;
                        }
                    });
                }
                CheckWin();
            });
        }

        static void PollInitialNetworkPlay()
        {
            Task.Run(()=>{
                bool keepPolling = true;
                while(keepPolling)
                {
                    Thread.Sleep(1000);
                    QueryLastNetworkPlay((receivedValidNetworkPlay) => {
                        if (receivedValidNetworkPlay)
                        {
                            keepPolling = false;
                            isInitialPlaySynced = true;
                        }
                    });
                }
            });
        }

        static bool ProcessLastNetworkPlay(string lastNetworkPlayData)
        {
            bool result = false;
            string[] parts = lastNetworkPlayData.Split(';');

            if (parts.Length < 2)
                return false;

            int clientID = int.Parse(parts[2]);
            
            if (clientID != multiplayerClientId)
            {
                int row = int.Parse(parts[3]);
                int col = int.Parse(parts[4]);
                int opponent = int.Parse(parts[6]);

                if (state[col, row] == 0)
                {
                    state[col, row] = opponent;

                    result = true;

                    #if DOX_DEBUG
                        Console.WriteLine("Last network play applied locally");
                    #endif
                }
            }
            else
            {
                #if DOX_DEBUG
                    Console.WriteLine("Opponent has not played yet");
                #endif
            }

            return result;
        }
    }
}