using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sudoku
{
    public partial class SudokuForm : Form
    {
        private SudokuData sudokuData;
        private BackgroundWorker backgroundSolver;
        public SudokuForm()
        {
            sudokuData = new SudokuData();
            backgroundSolver = new BackgroundWorker();
            backgroundSolver.DoWork += new DoWorkEventHandler(SolveInBackground);
            backgroundSolver.RunWorkerCompleted += new RunWorkerCompletedEventHandler(HandleSolve);

            InitializeComponent();
            FillTableLayout();
            DataToGrid();
        }

        public void DataToGrid()
        {
            for (int i = 0; i < sudokuTable.RowCount; i++)
            {
                for (int j = 0; j < sudokuTable.RowCount; j++)
                {
                    if(sudokuData.NumberMatrix[i, j] == 0)
                    {
                        sudokuTable.GetControlFromPosition(j, i).Text = "";
                    }
                    else
                    {
                        sudokuTable.GetControlFromPosition(j, i).Text = sudokuData.NumberMatrix[i, j].ToString();
                    }
                }
            }
        }

        public void GridToData()
        {
            for (int i = 0; i < sudokuTable.RowCount; i++)
            {
                for (int j = 0; j < sudokuTable.RowCount; j++)
                {
                    if (sudokuTable.GetControlFromPosition(j, i).Text == "")
                    {
                        sudokuData.NumberMatrix[i, j] = 0;
                    }
                    else
                    {
                        sudokuData.NumberMatrix[i, j] = Int32.Parse(sudokuTable.GetControlFromPosition(j, i).Text);
                    }
                }
            }
        }

        private void NumFilterKeyPress(object sender, KeyPressEventArgs e)
        {
            //Filter all except numbers, control chars and '.'
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            //Only one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
            
            //No zeroes
            if ((e.KeyChar == '0'))
            {
                e.Handled = true;
            }
        }

        public void FillTableLayout()
        {
            for (int i = 0; i < sudokuTable.RowCount; i++)
            {
                for (int j = 0; j < sudokuTable.RowCount; j++)
                {
                    var textBox = new TextBox()
                    {
                        Dock = DockStyle.Fill,
                        TextAlign = HorizontalAlignment.Center,
                        Font = new System.Drawing.Font("ArialBlack", 20, FontStyle.Bold),
                        AutoSize = false,
                        MaxLength = 1,
                        ShortcutsEnabled = false,
                        BackColor = Color.FromArgb(255-8*i, 255-8*j, 255)
                    };

                    textBox.KeyPress += NumFilterKeyPress;

                    sudokuTable.Controls.Add(textBox, i, j);
                }
            }
        }

        private void BtnLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                FileName = "Select a text file",
                Filter = "Text files (*.txt)|*.txt",
                Title = "Open text file"
            };


            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    sudokuData.LoadFromFile(openFileDialog.FileName);
                    DataToGrid();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error message: " + ex.Message);
                }
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text files(*.txt)| *.txt";
            saveFileDialog.Title = "Save a Text File";
            saveFileDialog.ShowDialog();

            // If the file name is not an empty string open it for saving.
            if (saveFileDialog.FileName != "")
            {
                try
                {
                    GridToData();
                    sudokuData.SaveToFile(saveFileDialog.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error message: " + ex.Message);
                }
            }
        }

        private void BtnSolve_Click(object sender, EventArgs e)
        {
            GridToData();

            if (sudokuData.CheckSudokuDataValidity())
            {
                buttonLayout.Enabled = false;
                sudokuTable.Enabled = false;
                backgroundSolver.RunWorkerAsync();

                //MessageBox.Show("Solved");
            }
            else
            {
                MessageBox.Show("Invalid input data. Check the input table.");
            }
        }

        private void SolveInBackground(object sender, DoWorkEventArgs e)
        {
            Solver solver = new Solver();
            sudokuData.NumberMatrix = solver.RunSolve(sudokuData.NumberMatrix);
        }

        private void HandleSolve(object sender, RunWorkerCompletedEventArgs e)
        {
            DataToGrid();
            buttonLayout.Enabled = true;
            sudokuTable.Enabled = true;
        }

        private void BtnValidate_Click(object sender, EventArgs e)
        {
            GridToData();

            if (sudokuData.CheckSudokuDataValidity())
            {
                MessageBox.Show("Input is valid sudoku");
            }
            else
            {
                MessageBox.Show("Input is invalid");
            }
        }
    }
}
