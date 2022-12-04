using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using static System.Reflection.Metadata.BlobBuilder;

namespace BombBrusher
{
    public partial class GameBoard : Form
    {
        // Difficulty name and percentage of bombs
        private readonly Dictionary<string, short> Difficulty = new() { 
            { "Easy", 10 }, { "Medium", 20 }, {"Hard", 40 }
        };

        // Board length and width options
        private readonly Dictionary<string, short> SizeChoices = new() {
            {"Small", 7 }, {"Medium",10 }, {"Large", 12 } 
        };

        private readonly Random random = new();
        private List<Cell> cells = new();
        
        // Flag to check if the location of the bombs has been calculated
        private bool bombsPlaced = false;

        public int n = 0;  // Length and width of board
        public double bombPercentage = 0.0d;
        public GameBoard() => InitializeComponent();

        public int BombCount { get; private set; }

        // Place the bombs on the board
        private void InitializeBombs(in Cell exludeCell)
        {
            int bombsLeft = BombCount;

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
                    var cell = new Cell(col, row, cellSize, onClick);
                    cells.Add(cell);
                    Controls.Add(cell);
                }
            }

            BombCount = (int)Math.Ceiling(n * n * bombPercentage);

        }

        private void InitializeUI()
        {
            bombCountLabel = new();
            this.bombCountLabel.Name = "BombCountLabel";
            this.bombCountLabel.TabIndex = 3;
            int rightEdgeOfBoard = n * 100;
            int margin = 50;
            Size size = new(100, 25);
            bombCountLabel.Location = new(rightEdgeOfBoard + margin, 150);
            bombCountLabel.Size = size;
            
            bombCountLabel.Show();
            Controls.Add(bombCountLabel);
        }

        private void InitializeValues()
        {
            DifficultyCbox.SelectedItem = DifficultyCbox.Items[0];
            sizeCBox.SelectedItem = sizeCBox.Items[0];
        }

        private void onClick(object? sender, EventArgs e)
        {
            if (sender is not Cell cell) { return; }

            // Pick bomb locations if this is the first click.
            if (!bombsPlaced) { InitializeBombs(cell); }
            
            if (cell.IsBomb) { GameOver(); }
            else
            {
                // Reveal cell, if none of its neighbors have bombs, reveal them recursively
                if (cell.neighborsWithBombs == 0) { Cascade(cell); } 

            }
        }

        // Reveal all neighboring cells
        private void Cascade(Cell startingCell)
        {
            HashSet<Cell> visited = new();
            Cascade(startingCell, ref visited);
        }

        // Reveal all neighboring cells if the current cell has no neighboring bombs
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
            InitializeUI();
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
                retryButton.Hide();
                sizeCBox.Hide();
                DifficultyCbox.Hide();
            }
        }
        
        private void GameOver()
        {
            cells.ForEach(c => c.Reveal());

            retryButton.Show();
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            StartGame(SizeChoices[(string)sizeCBox.SelectedItem], 
                Difficulty[(string)DifficultyCbox.SelectedItem]);
        }

        private void RetryButton_Click(object sender, EventArgs e)
        {
            cells.ForEach(cell => Controls.Remove(cell));
            cells.Clear();
            
            ToggleMenu(true);
        }
    }
}