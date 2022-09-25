using System.Collections.Generic;
using System.Net.Http.Headers;
using static System.Reflection.Metadata.BlobBuilder;

namespace BombBrusher
{
    public partial class GameBoard : Form
    {
        private readonly Dictionary<string, short> Difficulty = new() { 
            { "Easy", 10 }, { "Medium", 20 }, {"Hard", 40 }
        };
        private readonly Dictionary<string, short> SizeChoices = new() {
            {"Small", 7 }, {"Medium",10 }, {"Large", 12 } 
        };

        private readonly Random random = new();
        private List<Cell> cells;
        
        private bool bombsPlaced = false;

        public int n = 0;
        public double bombPercentage = 0.0d;
        public GameBoard()
        {
            InitializeComponent();
            InitializeGame();
        }

        private void InitializeBombs(in Cell exludeCell)
        { 
            int bombsLeft = (int)Math.Ceiling(n * n * bombPercentage);
            
            // Place bombs until enough bombs are placed
            while (bombsLeft > 0)
            {
                Cell randomCell = cells[random.Next(0, cells.Count)];

                // Skip if cell is the cell that was clicked, otherwise immediate game over
                if (randomCell.IsBomb || randomCell == exludeCell) { continue; }
                else {
                    randomCell.IsBomb = true;

                    // Set the number of neighbors with bombs for each cell
                    foreach (Cell neighbor in Neighbors(randomCell))
                    {
                        neighbor.neighborsWithBombs++;
                    }
                }

                bombsLeft--;
            }
            bombsPlaced = true;
        }

        
        private void InitializeGame()
        {
            cells = new();
            bombsPlaced = false;
            
            int cellSize = 100;

            // Make an n by n grid of cells.
            for (int row = 0; row < n; row++)
            {
                for (int col = 0; col < n; col++)
                {
                    var cell = new Cell(col, row, cellSize, new System.EventHandler(onClick));
                    cells.Add(cell);
                    Controls.Add(cell);
                }
            }

        }

        private void onClick(object sender, EventArgs e)
        {
            var cell = (Cell)sender;

            // Pick bomb locations if this is the first click.
            if (!bombsPlaced) { InitializeBombs(cell); }
            
            if (cell.IsBomb)
            { 
                GameOver();
            }
            else
            {
                // Reveal cell, if none of its neighbors have bombs, reveal them recursively
                if (cell.neighborsWithBombs == 0) { Cascade(cell); } 

            }
        }

        private void Cascade(Cell startingCell)
        {
            HashSet<Cell> visited = new();
            Cascade(startingCell, ref visited);
        }

        private void Cascade(Cell startingCell, ref HashSet<Cell> visited)
        {
            // Reveal all neighboring cells except the ones that have already been looked at
            var cellsToReveal = Neighbors(startingCell).Except(visited);
            visited = visited.Union(cellsToReveal).ToHashSet();  // Add all cells to visited that are about to be revealed

            // Reveal cells and conditionally reveal its neighbors as well
            foreach (Cell cell in cellsToReveal)
            {
                cell.Reveal();
                if (cell.neighborsWithBombs == 0) { Cascade(cell, ref visited); }
            }
        }
        
        private IEnumerable<Cell> Neighbors(Cell cell)
        {
            return cells.Where(neighbor => cell.IsNeighbor(neighbor) && neighbor.Enabled == true);
        }

        private void StartGame(in short size, in short difficulty)
        {
            n = size;
            bombPercentage = difficulty / 100.0d;

            ToggleMenu(false);

            InitializeGame();
        }

        private void ToggleMenu(bool on = false)
        {
            if (on)
            {

                startButton.Show();
                sizeCBox.Show();
                DifficultyCbox.Show();

            }
            else
            {
                startButton.Hide();
                sizeCBox.Hide();
                DifficultyCbox.Hide();
            }
        }
        
        private void GameOver()
        {
            cells.ForEach(c => c.Reveal());

            retryButton.Show();
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            StartGame(SizeChoices[(string)sizeCBox.SelectedItem], 
                Difficulty[(string)DifficultyCbox.SelectedItem]);
        }

        private void retryButton_Click(object sender, EventArgs e)
        {
            cells.ForEach(cell => Controls.Remove(cell));
            cells.Clear();
            
            ToggleMenu(true);
        }
    }
}