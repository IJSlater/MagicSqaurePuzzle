using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MagicSquare
{
    public partial class UserInterface : Form
    {
        private Square _currentpuzzle = new Square();
        private Square _solution= null;
        private TextBox[,] _textBoxGrid;
        private Label[] _labelRows;
        private Label[] _labelColumns;

        /// <summary>
        /// constructor for the userInterface class
        /// </summary>
        public UserInterface()
        {
            InitializeComponent();
            _textBoxGrid = new TextBox[4, 4] { { uxBox11, uxBox12, uxBox13, uxBox14 }, { uxBox21, uxBox22, uxBox23, uxBox24 }, { uxBox31, uxBox32, uxBox33, uxBox34 }, { uxBox41, uxBox42, uxBox43, uxBox44 } };

            _labelRows = new Label[4] { uxRow1, uxRow2, uxRow3, uxRow4 };
            _labelColumns = new Label[4] { uxCol1, uxCol2, uxCol3, uxCol4 };
            Square _currentPuzzle = new Square();
            Square _solution = new Square();

            for (int i = 0; i < Square.SIZE; i++)
            {
                for (int j = 0; j < Square.SIZE; j++)
                {

                        _textBoxGrid[i, j].Leave += new EventHandler(HandleUserInput);
                    
        
                }
            }
            
        }

        /// <summary>
        /// solve the current puzzle and returns the solutino
        /// </summary>
        /// <returns></returns>
        private Square MagicStack()
        {

            int next;
            Square checking;
            Square holder;

            Stack<Square> stack = new Stack<Square>();
            stack.Push(_currentpuzzle);
            while (stack.Count > 0)
            {
                checking = stack.Pop();

                if (checking.Complete())
                {
                    return checking;
                }
                else
                {
                    next = checking.NextAvailable();
                    if (next == 666)
                    {
                        continue;
                    }
                    for (int i = 0; i < Square.SIZE; i++)
                    {
                        for (int j = 0; j < Square.SIZE; j++)
                        {
                            holder = checking.Duplicate();
                            bool mov = holder.Move(i, j, next);
                            if (mov)
                            {
                                bool poss = holder.Possible();
                                if(poss)
                                    {
                                    stack.Push(holder);
                                }
                    
                            }
                        }
                    }
                }
            }
            return new Square();
        }

        /// <summary>
        /// Disables all un empty text boxes
        /// </summary>
        private void Disabel()
        {
            int count = 0;
            for (int i = 0; i < Square.SIZE; i++)
            {
                for (int j = 0; j < Square.SIZE; j++)
                {
                    if (_textBoxGrid[i, j].Text != "")
                    {

                        _textBoxGrid[i, j].Enabled = false;
                    }
                }
            }
        }

        /// <summary>
        /// Syncs the text box grid and the squares integer grid, as well as updates the labes 
        /// </summary>
        private void Sync()
        {
           
            for (int k = 0; k < Square.SIZE; k++)
            {

                _labelRows[k].Text = _currentpuzzle.SumRow(k).ToString();
                _labelColumns[k].Text = _currentpuzzle.SumColumn(k).ToString();

            }
            uxDiagtb.Text = _currentpuzzle.SumTopBottom().ToString();
            uxDiagbt.Text = _currentpuzzle.SumBottomTop().ToString();
        }

        /// <summary>
        /// is called when the .leave event is triggered
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleUserInput(object sender, EventArgs e)
        {
            TextBox holder = (TextBox)sender;
            for (int i = 0; i < Square.SIZE; i++)
            {
                for (int j = 0; j < Square.SIZE; j++)
                {

                    if (_textBoxGrid[i,j] == holder)
                    {
                        int input = 0;
                        if (holder.Text == "")
                        {
                            _currentpuzzle[i, j] = 0;
                            Sync();
                            return;
                        }
                        try
                        {
                            input = Convert.ToInt32(holder.Text);
                         }
                        catch (FormatException)
                         {
                            MessageBox.Show("Invalid!\nInput must be a whole number between 1 and 16");
                            holder.Text = "";
                         }
                        if (input > 0 && input < 17)
                        {
                            _currentpuzzle[i, j] = input;
                            Sync();
                        }
                        else
                        {
                            MessageBox.Show("Invalid!\nInput must be a whole number between 1 and 16");
                            holder.Text = "";
                        }

                    }

                }
            }
            
        }   
        private void Label3_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// handles the fucntionality of the check button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UxCheck_Click(object sender, EventArgs e)
        {



                    if (_currentpuzzle.Conflict())
                    {
                        if (_currentpuzzle.Complete())
                        {
                            MessageBox.Show("That is a Solved box!");
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Impossible!\nCan not have Duplicates");
                        return;

                    }

        }

        /// <summary>
        /// handles the fucntionality of the hint button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UxHint_Click(object sender, EventArgs e)
        {

            if (_currentpuzzle.Possible())
            {
                 Disabel();
                if (_solution == null)
                {
                    _solution = MagicStack();
                }
                for (int i = 0; i < Square.SIZE; i++)
                {
                    for (int j = 0; j < Square.SIZE; j++)
                    {
                        if (_textBoxGrid[i, j].Enabled == true)
                        {
                            _textBoxGrid[i, j].Text = _solution[i, j].ToString();
                            _textBoxGrid[i, j].Focus();
                            _textBoxGrid[i, j].Enabled = false;
                            return;
                        }
                    }
                }
            }
            else
            {
               MessageBox.Show("Impossible!\nThis square can not be solved in current configuration.");
                return;
            }

        }

        /// <summary>
        /// handles the fucntionality of the reset button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        /// <summary>
        /// handles the fucntionality of the solve button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UxSolve_Click(object sender, EventArgs e)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            if (_currentpuzzle.Possible())
            {
                Disabel();
                if (_solution == null)
                {
                    _solution = MagicStack();
                }
                for (int i = 0; i < Square.SIZE; i++)
                {
                    for (int j = 0; j < Square.SIZE; j++)
                    {
                        if (_textBoxGrid[i, j].Enabled == true)
                        {
                            _textBoxGrid[i, j].Text = _solution[i, j].ToString();
                            _textBoxGrid[i, j].Focus();
                            _textBoxGrid[i, j].Enabled = false;

                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Impossible!\nThis square can not be solved in current configuration.");
                return;
            }
        }
    }



}


    







