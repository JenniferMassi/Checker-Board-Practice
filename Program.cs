using System.Numerics;
using System;
using System.Media;
using System.IO;


namespace Checkerbot.Checkers {
    public static class Sound {
        static Random random = new Random();

        //// define capture sounds in array
        static string[] captureSounds = new string[] {
            ("capture1.wav"),
            ("welldone.wav"),
            ("alright.wav"),

        };//end captureSounds array

        static string[] pieceSounds = new string[] {
            ("piece1.wav"),
            ("piece2.wav"),
            ("piece3.wav"),
            ("piece4.wav"),
            ("piece5.wav"),
        };//end pieceSounds array

        //sound for everytime a point is added to scoreboard
        static string scoreSound = ("scoreboardChime.wav");

        // and 1 sound for the victory (if any player wins)
        static string victorySound = ("victory.wav");

        //intro music
        static string introSound = ("intro.wav");

        //game over sound
        static string gameOverSound = ("gameover.wav");

        static SoundPlayer music = new SoundPlayer(introSound);

        public static void PlayVictorySound() {
            SoundPlayer player = new SoundPlayer(victorySound);

            player.Play(); // play the sounds

        }//end PlayVictorySound Function
        public static void PlayGameOverSound() {
            SoundPlayer player = new SoundPlayer(gameOverSound);

            player.Play();
        }//end PlayGameOverSound function
        public static void PlayScoreboardSound() {
            SoundPlayer player = new SoundPlayer(scoreSound);

            player.Play();

        }//end PlayScoreboardSound function

        public static void PlayCaptureSound() {

            //SoundPlayer ohYeah = new SoundPlayer("capture1.wav");
            // ohYeah.Play();
            string sound = captureSounds[random.Next(captureSounds.Length)];
            SoundPlayer player = new SoundPlayer(sound);
            player.Play();

        }//end PlayCaptureSound Function

        public static void PlayPieceMove() {

            string sound = pieceSounds[random.Next(pieceSounds.Length)];
            SoundPlayer player = new SoundPlayer(sound);
            player.Play();

        }//end PlayPieceMove function
        public static void PlayIntroMusic() {
            SoundPlayer player = new SoundPlayer(introSound);

            player.PlayLooping();

        }//end PlayIntroMusic function

        public static void StopIntroMusic() {
            SoundPlayer player = new SoundPlayer(introSound);
            player.Stop();

        }//end StopIntroMusic

        static bool introMusicPlaying = true;
        public static void ToggleIntroMusic() {
            SoundPlayer player = new SoundPlayer(introSound);
            if (introMusicPlaying)//if the music is playing then
                player.Stop();
            else
                player.Play();
            // toggle intro music by inverting boolian value
            introMusicPlaying = !introMusicPlaying;

        }//stop intro music function
    }//end Sound Class

    public class Program {

        //8 rows & 8 columns
        const int boardRows = 8;
        const int boardColumns = 8;

        const int squareWidth = 8;
        const int squareHeight = 4;

        const int totalBoardWidth = boardColumns * squareWidth;
        const int totalBoardHeight = boardRows * squareHeight;

        //determines there's no winner. every loop updates the winner variable and if it returns -1 that means there is no winner yet
        const int playerNone = -1;
        /// <summary>
        /// player one is even pieces (black). use to calculate whose turn it is
        /// </summary>
        const int playerOne = 0;
        /// <summary>
        /// player two is odd pieces (red). use to calculate whose turn it is
        /// </summary>
        const int playerTwo = 1;

        /// <summary>
        /// turnCount increments every turn by one. by using modulus 2, we can calculate whose turn it is. 
        /// </summary>
        static int turnCount = 0;

        static int playerOneScore = 0;
        static int playerTwoScore = 0;

        /// <summary>
        /// currentPlayer is the WhoseTurn function as an inline "getter" function. this it acts like a variable and is easier to "plug in". if turn is even it's player 1 turns. if turn is odd player 2 turn. 
        /// </summary>
        static int currentPlayer => WhoseTurn();

        //define the colors for the squares and checker pieces
        static ConsoleColor evenSquareColorRed = ConsoleColor.Red;//red tiles are even
        static ConsoleColor oddSquareColorWhite = ConsoleColor.White;//white tiles are odd. pretty self explanatory 
        static ConsoleColor player1BlackCheckers = ConsoleColor.Black;//player 1(black) even
        static ConsoleColor player2RedCheckers = ConsoleColor.Red;//player 2 (red) odd

        //board set to global but the actual 2D array of the board is under the ResetBoard function. putting it here would be unneccessary and redundant
        static int[,] boardPieces;

        //                    HELPER BOARD

        //    0 | 0  1 | 0  2 | 0  3 | 0  4 | 0  5 | 0  6 | 0  7 | 0
        //    0 | 1  1 | 1  2 | 1  3 | 1  4 | 1  5 | 1  6 | 1  7 | 1
        //    0 | 2  1 | 2  2 | 2  3 | 2  4 | 2  5 | 2  6 | 2  7 | 2
        //    0 | 3  1 | 3  2 | 3  3 | 3  4 | 3  5 | 3  6 | 3  7 | 3
        //    0 | 4  1 | 4  2 | 4  3 | 4  4 | 4  5 | 4  6 | 4  7 | 4
        //    0 | 5  1 | 5  2 | 5  3 | 5  4 | 5  5 | 5  6 | 5  7 | 5
        //    0 | 6  1 | 6  2 | 6  3 | 6  4 | 6  5 | 6  6 | 6  7 | 6
        //    0 | 7  1 | 7  2 | 7  3 | 7  4 | 7  5 | 7  6 | 7  7 | 7


        static void Main(string[] args) {
            // project is graphic heavy so the bufferheight needs to be increased otherwise progam will crash.              only needs to be called once. can be called anywhere.
            Console.BufferHeight = 1000;
            Console.BufferWidth = 1000;

            MainMenu();

        }//END MAIN
        static void MainMenu() {
            Console.BackgroundColor = ConsoleColor.DarkMagenta;
            Sound.PlayIntroMusic();

            Write($"  \\☻/ LET'S PLAY CHECKERS! \\☻/  ", 31, 5);
            while (true) {
                Console.WriteLine();
                Console.WriteLine();

                Write($" ♦ Press Enter to Play ♦ ", 34, 7);
                Write($"     1. Game Rules       ", 34, 9);
                Write($"     2. Sound On/Off     ", 34, 11);
                Write($"     3. Exit             ", 34, 13);
                ConsoleKeyInfo pressEnter = Console.ReadKey();
                Console.Clear();
                switch (pressEnter.Key) {
                    case ConsoleKey.Enter:
                        Sound.StopIntroMusic();
                        PlayGame();
                        break;
                    case ConsoleKey.D1:
                        ResetConsole();
                        GameRules();
                        break;
                    case ConsoleKey.D2:
                        Write($"\\☻/ LET'S PLAY CHECKERS! \\☻/ ", 33, 5);
                        Sound.ToggleIntroMusic();
                        break;
                    case ConsoleKey.D3:
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.DarkMagenta;
                        Write($"'Check' you later ☻ ", 30, 5);
                        Sound.PlayGameOverSound();
                        Thread.Sleep(3000); // delay for 3 seconds
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;

                        Console.ReadKey();

                }//end switch
            }//end while

        }//end MainMenu function

        static void MiniMenu(string c) {//two option menu that only goes in the PlayGame Function

            if (c == "M") {//if c == M then
                Console.Clear();//clear the screen
                MainMenu();//and go to the MainMenu function
            } else if (c == "X") {//otherwise exit the game
                Write($"'Check' you later ☻ ", 75, 7);
                Sound.PlayGameOverSound();
                Thread.Sleep(3000); // delay for 3 seconds
                Environment.Exit(0);
            }//end if

        }//end MiniMenu Function

        public static void GameRules() {
            Console.BackgroundColor = ConsoleColor.Magenta;
            Console.ForegroundColor = ConsoleColor.White;
            Write($" ♦  RULES  ♦  ", 75, 4);

            Write($"1)The game is played between two players, with one player using pieces that are black, and the other player using pieces that are red  ", 34, 7);
            Write($"2)The goal of the game is to capture all of the opponent's pieces or block them so they cannot make any more moves.", 34, 9);
            Write($"3)The game starts with the player with the black pieces making the first move. ", 34, 11);
            Write($"4)Pieces can only move diagonally and can only move forward. ", 34, 13);
            Write($"5)When a piece reaches the opposite end of the board, it becomes a king and can move both forwards and backwards. ", 34, 15);
            Write($"6)If a piece lands on a square occupied by an opponent's piece, the opponent's piece is captured and removed from the board. ", 34, 17);
            Write($"7)A move that captures an opponent's piece is called a \"jump,\" and a player must make a jump if one is available. ", 34, 19);
            Write($"8)If a player is able to make multiple jumps in a single turn, they must continue to do so. ", 34, 21);
            Write($"9)The game is over when one player captures all of the opponent's pieces, or when the opponent is unable to make any more moves. ", 34, 23);
            // Console.ForegroundColor = ConsoleColor.White;
            Write($" ♦ Press Enter to Play ♦ ", 70, 30);
            Write($"     1. Main Menu       ", 70, 31);
            Write($"     2. Mute Music       ", 70, 32);
            Write($"     3. Exit             ", 70, 33);
            ConsoleKeyInfo pressEnter = Console.ReadKey();
            Console.Clear();
            //  Console.BackgroundColor = ConsoleColor.Black;
            switch (pressEnter.Key) {
                case ConsoleKey.Enter:
                    Console.WriteLine("Starting the game...");
                    //Sound.ToggleIntroMusic();
                    PlayGame();
                    //Sound.ToggleIntroMusic();
                    break;
                case ConsoleKey.D1:

                    MainMenu();
                    break;
                case ConsoleKey.D2:
                    Write($"\\☻/ LET'S PLAY CHECKERS! \\☻/ ", 30, 5);
                    Sound.ToggleIntroMusic();
                    GameRules();
                    break;
                case ConsoleKey.D3:
                    Write($"'Check' you later ☻ ", 30, 5);
                    Sound.PlayGameOverSound();
                    Thread.Sleep(3000); // delay for 3 seconds
                    Environment.Exit(0);
                    break;
                default:
                    Write("Invalid option. Please try again.");
                    break;
                    Console.ReadKey();
                    ResetConsole();
            }//end while statement

        }//end Game Rules function
        static void PlayGame() {


            int winner = playerNone;
            ResetBoard();
            while (winner == playerNone) {

                DrawBoard();
                ResetConsole();

                //displays whether it's the black checkers (player1) or red checkers(player 2) turn. for readability 
                string checker;
                if (currentPlayer % 2 == 0) {//if the turn is even display team black

                    checker = "Black Checkers";
                } else {//otherwise display team red
                        // Console.BackgroundColor = ConsoleColor.DarkRed;
                    checker = "Red Checkers";
                }//end else

                string coords = Input($"  {checker} (Player {currentPlayer + 1}) \n       Please enter your move in this format: XY-XY or XYXY: ", 19, totalBoardHeight + 3);
                int length = coords.Length;
                if (coords[0] != 'M' && coords[0] != 'X') {
                    if (length != 4 && length != 5) {//then
                        Console.ForegroundColor = ConsoleColor.DarkRed;

                        Write("You entered in an incorrect format. Please use XY-XY or XYXY, i.e. 12-23 or 0514", 3, totalBoardHeight + 4);
                        Console.ReadKey();
                        continue;
                    }//end if
                }//end if
                if (length == 5)
                    coords = coords.Remove(2, 1);//removes the position 2 (really position 3 bc it goes 0,1,2 so 2 is really 3) which would be the dash. the 1 signifies you only want to remove 1 character(the dash)

                else if (coords[0] == 'M' || coords[0] == 'X') {
                    MiniMenu(coords);
                }//end if

                int fromX = int.Parse(coords[0].ToString());
                int fromY = int.Parse(coords[1].ToString());
                int toX = int.Parse(coords[2].ToString());
                int toY = int.Parse(coords[3].ToString());

                if (!MoveChecker(fromX, fromY, toX, toY, false)) {//if coordinates user entered do not follow the rules outlined in the MoveChecker function then it's an illegal move and ask user to re-enter answer
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Write("         This move is illegal. Press enter and try again.        ", 3, totalBoardHeight + 4);
                    Console.ReadKey();
                    continue;

                } else {
                    turnCount++;
                }//end else


                winner = GetWinner();
            }//end while

            Sound.PlayVictorySound();
            ResetConsole();
            Console.Clear();
            Write($"Player {winner + 1} wins! \\☺/", 5, 5);

            Console.ReadKey();//"debugger" added to stop programming from crashing
        }//end PlayGame Function

        static void ResetBoard() {

            boardPieces = new int[,] {
            //    A  B  C  D  E  F  G  H
                { 0, 1, 0, 1, 0, 0, 0, 1 }, // 1
                { 1, 0, 1, 0, 1, 0, 1, 0 }, // 2
                { 0, 1, 0, 2, 0, 1, 0, 1 }, // 3
                { 0, 0, 0, 0, 2, 0, 0, 0 }, // 4
                { 0, 0, 0, 0, 0, 0, 0, 0 }, // 5
                { 0, 0, 1, 0, 2, 0, 2, 0 }, // 6
                { 0, 2, 0, 2, 0, 2, 0, 2 }, // 7
                { 0, 0, 0, 0, 2, 0, 2, 0 }  // 8
            };



            //boardPieces = new int[,] {
            ////    A  B  C  D  E  F  G  H
            //    { 0, 1, 0, 1, 0, 0, 0, 1 }, // 1
            //    { 1, 0, 1, 0, 1, 0, 1, 0 }, // 2
            //    { 0, 1, 0, 2, 0, 1, 0, 1 }, // 3
            //    { 0, 0, 0, 0, 2, 0, 0, 0 }, // 4
            //    { 0, 0, 0, 0, 0, 0, 0, 0 }, // 5
            //    { 2, 0, 2, 0, 2, 0, 2, 0 }, // 6
            //    { 0, 2, 0, 2, 0, 2, 0, 2 }, // 7
            //    { 2, 0, 2, 0, 2, 0, 2, 0 }  // 8
            //};



        }//end ResetBoard Function

        static void DrawSquare(int xPos, int yPos, ConsoleColor color) {
            //loop through rows and columns to create board
            for (int x = 0; x < squareWidth; x++) {
                for (int y = 0; y < squareHeight; y++) {
                    SetBlock(
                        (xPos * squareWidth) + x, //this is the x coordinate of the current block and consequently the entire board bc the for loop iterated through every square to form the entire board. it calculates the x coordinate of the current block by multiplying the x coordinate of the square by the width of the square, and then adding the current x value from the for loop.
                        (yPos * squareHeight) + y,//same as above except for y coordinate
                         color
                    );
                }//end 2nd for loop
            }//end first for loop
        }//end DrawSquare function

        static void DrawChecker(int xPos, int yPos) {
            int piece = boardPieces[yPos, xPos];
            if (piece == 0)//check if the element in the specific position of the array is equal to 0. if so:   
                return; //do not draw
            if (piece > 0) { //then
                bool isKing = (piece > 2); //if array elements are greater than 2 (meaning 3 or 4)
                int checkerHeight = squareHeight - 2; //****MAKE THIS GLOBAL IN CLASS
                if (isKing) {
                    checkerHeight = checkerHeight + 1; // increase the height bc it's a king and kings will be bigger than pawns
                }//end if
                ConsoleColor color;
                if (piece % 2 == playerOne) {
                    color = player1BlackCheckers;
                } else {
                    color = player2RedCheckers;
                }//end if
                 //loops through width & height of checkers
                for (int x = 1; x < squareWidth - 1; x++) {
                    for (int y = 1; y < checkerHeight; y++) {
                        SetBlock(
                            (xPos * squareWidth) + x,
                            (yPos * squareHeight) + y,
                             color
                        );
                    }//end y for loop
                }//end x for loop
            }//end if statement
        }//end function

        static void DrawBoard() {
            ResetConsole();
            Console.Clear(); //redraw board to clear inputted coordinates
            //TEXT INPUT FOR SCOREBOARD
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Write($" Score Player 1: {playerOneScore} ", 13, totalBoardHeight + 6);
            Write($" Score Player 2: {playerTwoScore} ", 34, totalBoardHeight + 6);
            Console.ResetColor();
            //TEXT INPUT FOR MINI MENU
            Write($"Press 'M' to go back to the Menu", 16, totalBoardHeight + 8);
            Write($"Press 'X' to Exit the Game", 16, totalBoardHeight + 9);
            //DRAW CHECKER BOARD
            for (int y = 0; y < boardColumns; y++) {
                for (int x = 0; x < boardRows; x++) {
                    ConsoleColor color = evenSquareColorRed;
                    if ((x + y) % 2 == 1) {//if checker square is odd then: 
                        color = oddSquareColorWhite;
                    }//end if statement
                    DrawSquare(x, y, color);//loops through the width and height of each square and calls the SetBlock function to set the color of each block in the square
                    Console.ForegroundColor = ConsoleColor.Black;
                    //write coordinates on the board 
                    Write(//writes coordinates to board on each square. coordinates are determined by the values of variables x and y, and the position on the board is determined by the squareWidth and squareHeight variables.
                        x.ToString().ToUpper() + "|" + y.ToString().ToUpper(),
                        2 + (x * squareWidth),//x position. ***POSSIBLY CHANGE 2 TO 4 BC IT CENTERS THE NUMBERS MORE*** COME BACK TO THIS
                        2 + (y * squareHeight)); //y position
                                                 //DISPLAY ALL CHECKERS
                    DrawChecker(x, y);//checks the value of the piece at a specific position in the boardPieces array and draws piece
                }//end 2nd for loop
            }//end 1st for loop
        }//end DrawBoard Function

        static void SetBlock(int x, int y, ConsoleColor color) {
            Console.BackgroundColor = color;
            Console.SetCursorPosition(x + 2, y + 2); // pos of x + 2 to the right, pos of y + 4 down
            Console.Write(' '); //***Tried this with string. that works too. going to leave it as a char though
        }//end SetBlock Function

        // base function without providing a position
        static void Write(string text) {
            Console.Write(text);
        }//end Write Function

        // overload function in case you want to also declare the position of the text to be written
        static void Write(string text, int x, int y) {
            Console.SetCursorPosition(x, y);
            Write(text);
        }//end Write overload function

        static string Input(string text) {
            Console.Write(text + "");
            return Console.ReadLine();
        }//end Input Function

        static string Input(string text, int x, int y) {
            Console.SetCursorPosition(x, y);
            Console.Write(text + "");
            return Console.ReadLine();
        }//end Input Function

        public static bool MoveChecker(int xStart, int yStart, int xEnd, int yEnd, bool legalMoveTest) {

            // check if move start is inside the board. if it's not it's an illegal move then return false
            if (xStart < 0 || xStart > 7 || yStart < 0 || yStart > 7)
                return false;

            // check if move end is inside the board. return false if it's not
            if (xEnd < 0 || xEnd > 7 || yEnd < 0 || yEnd > 7)
                return false;

                                                                                     // idk why the board has the y axis where the x should be and the x axis where y should be but bc of this invert x and y
            int piece = boardPieces[yStart, xStart];//starting position of piece
            if (piece == 0)                                                         //if its element in the array is 0 then return false bc that would mean there's no checker piece there
                return false;

            //turnCount & checker piece need to match (both odd or both even) bc if they don't players can move opponents checker piece
            int owner = piece % 2;
            if (owner != currentPlayer && !legalMoveTest)//
                return false;

            //checker pieces can only go on odd squares/tiles
            int startSum = xStart + yStart; // take the sum of the coordinates from the starting position
            int endSum = xEnd + yEnd; // take the sum of the coordinates from the ending position
            bool isStartEven = isEven(startSum);
            bool isEndEven = isEven(endSum);
            // neither of start or end positions must be even
            if (isStartEven || isEndEven)
                return false;

            // get the difference of xEnd - xStart (delta values) bc pieces can only move 1 space
            int deltaX = xEnd - xStart;
            if (deltaX > 1 || deltaX < -1) //difference between start and end must be either 1 or -1 on the x axis
                return false;

            // check if the direction of piece. piece either moves up (player1) or down (player2)
            // if it's a king check if the absolute of deltaY is 1 bc king can move both up and down
            bool isKing = piece > 2;
            int deltaY = yEnd - yStart;
            if (isKing) {
                if (Math.Abs(deltaY) < 1)//should allow the king to move more than 1 space
                    return false;
            } else {
                if (currentPlayer == playerOne) {
                    if (deltaY != -1)
                        return false;
                } else {
                    if (deltaY != 1)
                        return false;
                }//end else
            }//end else

            // you can not move on top of your own pieces
            int target = boardPieces[yEnd, xEnd];//"target" represents the piece that is located at the ending position (xEnd, yEnd) of a move.
            if (target == 0) {//if element in the array is empty then move is legal
                if (legalMoveTest) // if move is legal then return true and move the piece
                    return true;

                Move(xStart, yStart, xEnd, yEnd);
            } else if (target % 2 == currentPlayer) {
                //cant capture our own pieces
                return false;
            } else {
                int jumpX = xEnd + deltaX;//jumpX = endingx coordinate plus (endingx - startingx) 
                int jumpY = yEnd + deltaY;
                if (jumpX < 0 || jumpX > 7 || jumpY < 0 || jumpY > 7)
                    return false;

                if (boardPieces[jumpY, jumpX] == 0) {
                    if (legalMoveTest) 
                        return true;
                    Move(xStart, yStart, jumpX, jumpY);
                    Capture(xEnd, yEnd);

                } else {
                    return false;
                }//end else
            }//end if      
            return true;
        }//end MoveChecker Function

        static bool isOdd(int number) {
            return number % 2 == 1;
        }//end isOdd function

        static bool isEven(int number) {
            return number % 2 == 0;
        }//end isEven function

        static void Capture(int x, int y) {
            int piece = boardPieces[y, x];
            boardPieces[y, x] = 0;
            if (piece % 2 == playerOne) {//if piece is even then
                playerTwoScore++;
            } else {
                playerOneScore++;
            }
            Sound.PlayCaptureSound();
        }//end Capture function

        static void Move(int xFrom, int yFrom, int xTo, int yTo) {//this function serves two purposes. it moves the checker and also checks to see if it a piece reaches "kings row". if it does then the pawn is promoted 

            int piece = boardPieces[yFrom, xFrom];
            //pieces are promoted to kings if piece > 2;
            // make king if pieces reach opponents last row
            if (yTo == 7 && piece % 2 == playerTwo) {
                piece += 2;
                Sound.PlayScoreboardSound();
            } else if (yTo == 0 && piece % 2 == playerOne) {
                piece += 2;
                Sound.PlayScoreboardSound();
            } else {
                Sound.PlayPieceMove();
            }
            //moves a piece from one square on the board to another square
            boardPieces[yFrom, xFrom] = 0;//"array swap" saves the variable that needs to be replace in a temporary 'container'. this is now an empty square
            boardPieces[yTo, xTo] = piece;//updates the target position of the move  

        }//end Move Function

        static int WhoseTurn() {
            //set to -1 bc when turn is incremented it will become 0 (player 1s turn). when it increments again it will become 1 (player 2s turn)        
            int player = -1;
            if (turnCount % 2 == 0) {//if turn is even. 
                player = playerOne;
            } else {//otherwise it will be player 2s turn if modulus is odd. whenever modulus is equal to 1 aka has a remainder of 1 that means it's an odd number. if the remainder is 0 that's always even
                player = playerTwo;
            }//end if statement
            return player;
        }//end Turn Function

        static int GetWinner() {
            // the following conditions must be met for a player to win:
            // 1.) enemy is out of pieces
            // 2.) enemy has no legal moves left (is blocked)
            // variables will be used to keep track of the number of pieces and legal moves for each player.
            int playerOnePieces = 0;
            int playerOneLegalMoves = 0;
            int playerTwoPieces = 0;
            int playerTwoLegalMoves = 0;
            //loops through the rows and columns and for each piece on the board it checks if the piece is owned by player one or player two. 
            for (int y = 0; y < boardColumns; y++) {
                for (int x = 0; x < boardRows; x++) {
                    int piece = boardPieces[y, x];
                    if (piece == 0)//if piece is equal to 0 then continue executing the following code
                        continue;
                    //checks to see whose piece it is (player 1 or player 2)
                    if (isEven(piece)) {//***IF PIECE IS OWNED BY PLAYER ONE (TEAM BLACK) THEN INCREMENT PLAYERONEPIECES AND CONTINUE ON TO THE LEGAL MOVE CHECK
                        playerOnePieces++;
                        //check to see if there are any legal moves left for team black using 4 different parameters ( a piece on the board can only move 4 different ways)
                        if (MoveChecker(x, y, x + 1, y + 1, true))
                            playerOneLegalMoves++;
                        if (MoveChecker(x, y, x + 1, y - 1, true))
                            playerOneLegalMoves++;
                        if (MoveChecker(x, y, x - 1, y + 1, true))
                            playerOneLegalMoves++;
                        if (MoveChecker(x, y, x - 1, y - 1, true))
                            playerOneLegalMoves++;
                    } else {
                        playerTwoPieces++;
                        if (MoveChecker(x, y, x + 1, y + 1, true))
                            playerTwoLegalMoves++;
                        if (MoveChecker(x, y, x + 1, y - 1, true))
                            playerTwoLegalMoves++;
                        if (MoveChecker(x, y, x - 1, y + 1, true))
                            playerTwoLegalMoves++;
                        if (MoveChecker(x, y, x - 1, y - 1, true))
                            playerTwoLegalMoves++;
                    }//end else


                }//end second for loop
            }//end first for loop
             //if team black(player 1) is out of legal moves & out of pieces then team two wins and visa versa
            if (playerOneLegalMoves == 0 || playerOnePieces == 0)
                return playerTwo;//playerTwo (red) == odd
            else if (playerTwoLegalMoves == 0 || playerTwoPieces == 0)
                return playerOne;//playerOne(black)== even
            else
                return playerNone;//if neither of these conditions are met then the game continues
        }//end GetWinner Function

        static void ResetConsole() {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
        }//end ResetConsole


    }//END CLASS
}//END NAMESPACE