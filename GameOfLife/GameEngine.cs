using System;
using static System.Net.Mime.MediaTypeNames;
using System.Drawing;
using System.Windows.Forms;

namespace GameOfLife
{
    public class GameEngine
    {
        public uint CurrentGeneration { get; private set; }

        private bool[,] field; // know cell position and whether it is alive
        private readonly int rows;
        private readonly int cols;

        public GameEngine(int rows, int cols, int density)
        {
            this.rows = rows;
            this.cols = cols;

            Random random = new Random();

            field = new bool[cols, rows];
            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    field[x, y] = random.Next(density) == 0;
                }
            }
        }

        public void GenerateNextGeneration()
        {
            bool[,] newField = new bool[cols, rows];

            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    int neighborsCount = CountNeighbors(x, y);
                    bool hasLife = field[x, y];

                    if (!hasLife && neighborsCount == 3) // new life is born
                    {
                        newField[x, y] = true;
                    }
                    else if (hasLife && (neighborsCount < 2 || neighborsCount > 3)) // cell dies
                    {
                        newField[x, y] = false;
                    }
                    else
                    {
                        newField[x, y] = hasLife; // field[x,y] // leave as it is
                    }
                }
            }

            field = newField;
            CurrentGeneration++;
        }

        public bool[,] GetCurrentGeneration()
        {
            // Because array is referency type, we dont want to change the source array somewhere outside this class
            bool[,] fieldCopy = new bool[cols, rows];
            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    fieldCopy[x, y] = field[x, y];
                }
            }

            return fieldCopy;
        }

        public void AddCell(int x, int y)
        {
            field[x, y] = true;
        }

        public void RemoveCell(int x, int y)
        {
            field[x, y] = false;
        }

        private void UpdateCellState(int x, int y, bool state)
        {
            if(ValidateCellPosition(x, y))
                field[x, y] = state;
        }

        private int CountNeighbors(int x, int y)
        {
            int count = 0;

            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    int col = (x + i + cols) % cols; // like earth globe (from the left side comes right side)
                    int row = (y + j + rows) % rows;

                    bool isSelfChecking = (col == x && row == y);
                    bool hasLife = field[col, row];

                    if (hasLife == true && isSelfChecking == false)
                    {
                        count++;
                    }
                }
            }

            return count;
        }
        private bool ValidateCellPosition(int x, int y)
        {
            return x >= 0 && y >= 0 && x < cols && y < rows;
        }

    }
}
