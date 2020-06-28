using System.Collections.Generic;
using SFML.Graphics;


namespace Dox
{
    public partial class Program
    {
        static int globalGameState = -3;
        static readonly uint screenHeight = 480;
        static readonly uint screenWidth = 600;
        static Font font;
        static readonly string fontName = "Ubuntu-Bold.ttf";
        static RenderWindow window;
        static bool isMouseClicked;
        static bool isMultiplayerPlayLocked;

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
        static GameType currentGameType;
        static int multiplayerClientId = 0;
        static int multiplayerRoomId = 0;
        static List<string> gameRooms = new List<string>();
    }
}