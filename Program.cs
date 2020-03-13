using System;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace ConsoleConwayLife
{
    class Program
    {
        public static cell[] oldBoard = new cell[324];
        public static cell[] newBoard = new cell[324];

        static void Main(string[] args)
        {
            
            cell[] board2 = new cell[324];

            for (int i = 0; i < board2.Length; ++i)
            {
                board2[i] = new cell();
                board2[i].life = 0;
                board2[i].color = "#000000";
            }
            /*
            foreach (cell i in board2)
            {
                Console.WriteLine(i.life);
                Console.WriteLine(i.color);
            }
            */

            board2[245].life = 1;

            string jsonString;
            jsonString = JsonSerializer.Serialize(board2);

            

            SendMessage("brad", jsonString);

            //Console.WriteLine(jsonString);

            //cell[] board = new cell[256];

            //for (int i = 0; i < board.Length; ++i)
            //{
            //    board[i] = new cell();
            //}

            //int count = 0;

            //using (JsonDocument document = JsonDocument.Parse(jsonString))
            //{
            //    JsonElement root = document.RootElement;
            //    //JsonElement studentsElement = root.GetProperty("Students");
            //    foreach (JsonElement cell in root.EnumerateArray())
            //    {
            //        cell.TryGetProperty("life", out JsonElement lifeElement);
            //        board[count].life = lifeElement.GetInt32();
            //        //Console.WriteLine(lifeElement.ToString());
            //        cell.TryGetProperty("color", out JsonElement colorElement);
            //        board[count].color = colorElement.ToString();
            //        //Console.WriteLine(colorElement.ToString());
            //        count++;
            //    }
            //}


            //foreach (cell i in board)
            //{
            //    Console.WriteLine(i.life);
            //    Console.WriteLine(i.color);
            //}


        }



        public static void SendMessage(string user, string jsonString)
        {
            //populate oldBoard

            for (int i = 0; i < oldBoard.Length; ++i)
            {
                oldBoard[i] = new cell();
            }

            
            //populate newBoard blank

            for (int i = 0; i < newBoard.Length; ++i)
            {
                newBoard[i] = new cell();
                newBoard[i].life = 0;
                newBoard[i].color = "#000000";
            }

            

            //add in cell data for the gameboard from the jsonString to oldBoard

            int count = 0;

            using (JsonDocument document = JsonDocument.Parse(jsonString))
            {
                JsonElement root = document.RootElement;
                //JsonElement studentsElement = root.GetProperty("Students");
                foreach (JsonElement cell in root.EnumerateArray())
                {
                    cell.TryGetProperty("life", out JsonElement lifeElement);
                    oldBoard[count].life = lifeElement.GetInt32();
                    //Console.WriteLine(lifeElement.ToString());
                    cell.TryGetProperty("color", out JsonElement colorElement);
                    oldBoard[count].color = colorElement.ToString();
                    //Console.WriteLine(colorElement.ToString());
                    count++;
                }
            }

            oldBoard[38].life = 1;
            oldBoard[39].life = 1;
            oldBoard[40].life = 1;

            oldBoard[41].life = 1;
            oldBoard[60].life = 1;
            oldBoard[79].life = 1;



            int x = 19;
            while (x < (18 * 17))
            {
                if (x % 18 == 0)
                {
                    x++;
                }
                else if (x % 18 == 17)
                {
                    Console.Write("\n");
                    x++;
                }
                else
                {
                    Console.Write(oldBoard[x].life);
                    //Console.Write(x);
                    ///check cell
                    x++;
                }
            }



            checkBoard(oldBoard);


            Console.WriteLine();

            //print board:

                
             /////////////////
                x = 19;
                while (x < (18 * 17))
                {
                    if (x % 18 == 0)
                    {
                        x++;
                    } else if (x % 18 == 17)
                    {
                        Console.Write("\n");
                        x++;
                    }
                    else
                    {
                        Console.Write(newBoard[x].life);
                        //Console.Write(x);
                        ///check cell
                        x++;
                    }
                }
              /////////////////
            

            ///receive board in json
            ///deserialize the board from json
            ///do board calculations on a new board
            ///serialize the new board back into json
            ///send to all clients

            //await Clients.All.SendAsync("ReceiveMessage", user, newBoard);
        }


        ///receive board, and index of cell return count of horizontal live cells
        public static int horCheck(cell[] board, int i)
        {

            int count = 0;
            if (board[i + 1].life == 1)
            {
                count++;
            }
            if (board[i - 1].life == 1)
            {
                count++;
            }
            return count;
        }

        ///return a count of how many vertical cells are live
        public static int vertCheck(cell[] board, int i)
        {
            int count = 0;
            if (board[i + 18].life == 1)
            {
                count++;
            }
            if (board[i - 18].life == 1)
            {
                count++;
            }
            return count;
        }

        ///check if two cells are diagnoally adjacent

        public static int diagCheck(cell[] board, int i)
        {
            int count = 0;
            if (board[i + 19].life == 1)
            {
                count++;
            }
            if (board[i + 17].life == 1)
            {
                count++;
            }
            if (board[i - 19].life == 1)
            {
                count++;
            }
            if (board[i - 17].life == 1)
            {
                count++;
            }
            return count;
        }

        ///game logic:
        //loop through all relevent cells:

        public static void checkBoard(cell[] board)
        {

            ///board is 16 by 16, with one extra row and column around the board
            ///makeing the board 18 by 18

            //start at this cell, the first relevent cell
            int x = 19;

            //go until the second to last row
            while (x < 18 * 16)
            {
                if (x % 18 == 0 || x % 18 == 17)
                {
                    x++;
                }
                else
                {
                    //valid cells, check:
                    //1 Any live cell with two or three neighbors survives.
                    //2 Any dead cell with three live neighbors becomes a live cell.
                    //3 All other live cells die in the next generation.Similarly, all other dead cells stay dead

                    int liveNeighbors = horCheck(board, x) + vertCheck(board, x) + diagCheck(board, x);
                    if (board[x].life == 1)
                    {
                        //if live cell

                        if (liveNeighbors == 2 || liveNeighbors == 3)
                        {
                            //live cell stays alive
                            newBoard[x].life = 1;
                        }
                        else
                        {
                            //live cell dies
                            newBoard[x].life = 0;
                        }
                    }
                    else
                    {
                        //currently dead cell
                        if (liveNeighbors == 3)
                        {
                            //becomes alive
                            newBoard[x].life = 1;
                        }
                        else
                        {
                            newBoard[x].life = 0;
                        }
                    }
                    x++;
                }
            }
            ///example/////////////////
            //int x = 19;
            //while (x < 18 * 17)
            //{
            //    if (x % 18 == 0 || x % 18 == 17)
            //    {
            //        x++;
            //    }
            //    else
            //    {
            //        ///check cell

            //    }
            //}
            ///example/////////////////
        }
   
    }
}
