using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicSquare
{
    public class Square
    {

        public const int SIZE = 4;
        public const int MAGIC = 34;
        public const int EMPTY = 0;
        private int[,] _grid = new int[SIZE, SIZE] { { EMPTY, EMPTY, EMPTY, EMPTY }, { EMPTY, EMPTY, EMPTY, EMPTY }, { EMPTY, EMPTY, EMPTY, EMPTY }, { EMPTY, EMPTY, EMPTY, EMPTY } };
        private bool[] _used = new bool[SIZE * SIZE + 1] { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false };
        public int this[int x, int y]
        {
            get => _grid[x, y];
            set
            {

                _grid[x, y] = value;
                _used[value] = true;
                if (value == 0)
                {
                    _used = new bool[SIZE * SIZE + 1] { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false };
                }
                for (int i = 0; i < SIZE; i++)
                {
                    for (int j = 0; j < SIZE; j++)
                    {
                        if (_grid[i, j] != 0)
                        {
                            _used[_grid[i, j]] = true;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// returns a new square with all the same values as the as the current grid and used array
        /// </summary>
        /// <returns></returns>
        public Square Duplicate()
        {
            Square s = new Square();
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = 0; j < SIZE; j++)
                {
                    s[i,j] = _grid[i, j];

                }
            }
            for (int k = 1; k < SIZE*SIZE+1; k++)
            {
                s._used[k] = _used[k];
            }
            return s;
        }
        /// <summary>
        /// if grid at point r, c isnt alread taken
        /// set that point equal to value
        /// </summary>
        /// <param name="r"></param>
        /// <param name="c"></param>
        /// <param name="val"></param>
        /// <returns></returns>

        public bool Move(int r, int c, int val)
        {
            if (_used[val] ||_grid[r, c] !=EMPTY)
            {
                return false;
            }
            else
            {
                _grid[r, c] = val;
                _used[val] = true;
                
                
            }
            return true;
        }


        /// <summary>
        /// Returns the sum of all the values in a row
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public int SumRow(int row)
        {
            int sum = 0;
            for (int i = 0; i < SIZE; i++)
            {
                sum += _grid[row, i];
            }
            return sum;
        }

        /// <summary>
        /// Returns the sum of all the values in a column
        /// </summary>
        /// <param name="col"></param>
        /// <returns></returns>
        public int SumColumn(int col)
        {
            int sum = 0;
            for (int i = 0; i < SIZE; i++)
            {
                sum += _grid[i, col];
            }
            return sum;
        }

        /// <summary>
        /// To string method for the square class
        /// </summary>
        /// <returns></returns>
        private string TS()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < Square.SIZE; i++)
            {
                for (int j = 0; j < Square.SIZE; j++)
                {
                    sb.Append(_grid[i,j].ToString());
                    sb.Append(" ");

                }
                sb.Append("\n");
            }
            return sb.ToString();
        }

        /// <summary>
        /// Sums all values in the diagonal from top to bottom    
        /// </summary>
        /// <returns></returns>
        public int SumTopBottom()
        {
            int sum = 0;
            for (int i = 0; i < SIZE; i++)
            {
                sum += _grid[i, i];
            }
            return sum;
        }
        /// <summary>
        /// Sums all values in the diagonal from bottom to top 
        /// </summary>
        /// <returns></returns>
        public int SumBottomTop()
        {
            int sum = 0;
            int j = 0;
            for (int i = 0; i < SIZE; i++)
            {
                j = (SIZE - 1) - i;
                sum += _grid[j, i];

            }
            return sum;
        }

        /// <summary>
        /// returns the lowest int between 1 and 16 that hasnt been used yet
        /// </summary>
        /// <returns></returns>
        public int NextAvailable()
        {
            for (int i =1; i <= 16; i++)
            {
                if (_used[i] == false)
                {
                    return i;
                    
                }

            }
            return 666;
        }

        /// <summary>
        /// returns true if there grid is a solution
        /// </summary>
        /// <returns></returns>
        public bool Complete()
        {
            for (int i = 0; i < SIZE; i++)
            {
                if (SumRow(i) != 34 || SumColumn(i) != 34)
                {
                    return false;
                }
            }
            if (SumBottomTop() != 34 || SumTopBottom() != 34)
            {
                return false;
            }
            for (int i = 0; i < SIZE - 1; i++)
            {
                for (int j = 0; j < SIZE - 1; j++)
                {
                    if (_grid[i, j] == 0)
                    {
                        return false;
                    }
                }
            }
                    return true;

        }

        /// <summary>
        /// Checks for repeats and impossible rows
        /// </summary>
        /// <returns></returns>
        public bool Conflict()
        {
            int[] repeats = new int[16];
            int count = 0;
            for (int i = 0; i < SIZE - 1; i++)
            {
                for (int j = 0; j < SIZE - 1; j++)
                {

                    if (_grid[j, i] != 0)

                    {
                        if (repeats.Contains(_grid[j, i]))
                        {

                            return false;

                        }
                        else
                        {

                            repeats[count] = _grid[j, i];
                            count++;

                        }
                    }
                }
            }


            return true;
        }
        /// <summary>
        /// returns the alrgest int 1-16 that hasnt been used
        /// </summary>
        /// <returns></returns>
        private int BiggestUnused()
        {
            for (int i = 16; i > 0; i--)
            {
                if (_used[i] == false)
                {
                    return i;
                }

            }
            return 666;
        }
        /// <summary>
        /// helper method for  Possible()
        /// </summary>
        /// <returns></returns>
        private bool CheckRows()
        {
            int count;
            int sum;
            for (int line = 0; line < SIZE; line++)
            {
                count = 0;
                sum = SumRow(line);
                for (int i = 0; i < SIZE; i++)
                {
                    if (_grid[line, i] != 0)
                    {
                        count++;
                    }
                }
                if (count ==4 && sum != MAGIC)
                {
                return false;
                }
                else if (count >= 3 && !(sum + BiggestUnused() >= MAGIC) && sum >= 6)
                {

                    return false;

                }

            }
            return true;
        }
        /// <summary>
        /// helper method for  Possible()
        /// </summary>
        /// <returns></returns>
        private bool CheckColumns()
        {
            int count;
            int sum;
            for (int line = 0; line < SIZE; line++)
            {
                count = 0;
                sum = SumColumn(line);
                for (int i = 0; i < SIZE; i++)
                {
                    if (_grid[i, line] != 0)
                    {
                        count++;
                    }
                }
                if (count == 4 && sum != MAGIC)
                {
                    return false;
                }
                else if (count == 3 && !(sum + BiggestUnused() >= MAGIC) && sum >= 6)
                {
                    return false;
                }

            }
            return true;
        }
        /// <summary>
        /// helper method for  Possible()
        /// </summary>
        /// <returns></returns>
        private bool CheckTB()
        {
            int count;
            int sum;
            count = 0;
            sum = SumTopBottom();
            for (int i = 0; i < SIZE; i++)
            {
                if (_grid[i, i] != 0)
                {
                    count++;
                }
            }
            if (count == 4 && sum != MAGIC)
            {
                return false;
            }
            else if (count == 3 && !(sum + BiggestUnused() >= MAGIC) && sum >= 6)
            {

                return false;
            }
            return true;
        }
        /// <summary>
        /// helper method for  Possible()
        /// </summary>
        /// <returns></returns>
        private bool CheckBT()
        {
            int count;
            int sum;
            count = 0;
            sum = SumBottomTop();
            for (int i = 0; i < SIZE; i++)
            {
                if (_grid[(SIZE - 1) - i, i] != 0)
                {
                    count++;
                }
            }
            if (count == 4 && sum != MAGIC)
            {
                return false;
            }
            else if (count == 3 && !(sum + BiggestUnused() >= MAGIC) && sum >= 6)
            {

                return false;

            }
            return true;
        }
        /// <summary>
        /// returns true if is there is a possible solution left
        /// </summary>
        /// <returns></returns>
        public bool Possible()
        {
            if (Conflict())
            {
                for (int i = 0; i < SIZE; i++)
                {
                    if (SumRow(i) > MAGIC  || SumColumn(i) > MAGIC )
                    {

                        return false;
                    }
                }
                if (SumBottomTop() > MAGIC  || SumTopBottom() > MAGIC )
                {

                    return false;
                }
                if (CheckRows() && CheckColumns() && CheckBT() && CheckTB())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
    }
}
