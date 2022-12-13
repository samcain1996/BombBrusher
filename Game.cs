using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using static System.Reflection.Metadata.BlobBuilder;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace BombBrusher
{
    public partial class Game : Form
    {
        private static readonly Random random = new();

        // Difficulty name and percentage of bombs
        private readonly Dictionary<string, short> Difficulty = new() { 
            { "Easy", 10 }, { "Medium", 20 }, {"Hard", 40 }
        };

        // Board length and width options
        private readonly Dictionary<string, short> SizeChoices = new() {
            {"Small", 7 }, {"Medium",10 }, {"Large", 12 } 
        };

        private int n = 0;
        private double bombPercentage = 0.0d;
        private List<Cell> cells = new();

        public Game() => InitializeComponent();

        // Number of bombs on board
        private int BombCount { get; set; }
        // Number of flagged cells
        private int FlaggedCount { get { return cells.Count(c => c.Flagged); } }

        private void InitializeGame()
        {
            cells = new();

            // Make an n by n grid of cells.
            for (int col = 0; col < n; col++)
            {
                for (int row = 0; row < n; row++)
                {
                    var cell = new Cell(UI.padding, UI.padding, row, col, OnClick);
                    cells.Add(cell);
                    Controls.Add(cell);
                }
            }

            BombCount = (int)Math.Ceiling(n * n * bombPercentage);
            UI.Message = "Bombs: " + BombCount;
        }
        public IEnumerable<Cell> Neighbors(Cell cell)
        {
            return cells.Where(neighbor => cell.IsNeighbor(neighbor) && neighbor.Enabled);
        }
        public void InitializeBombs(in Cell exludeCell)
        {
            int bombsLeft = BombCount;
            // Place bombs until enough bombs are placed
            while (bombsLeft > 0)
            {
                Cell randomCell = cells.ElementAt(random.Next(0, cells.Count));

                // Skip if cell is the cell that was clicked, otherwise immediate game over
                if (randomCell.IsBomb || randomCell == exludeCell) { continue; }
                else
                {
                    randomCell.IsBomb = true;

                    // Set the number of neighbors with bombs for each cell
                    foreach (Cell neighbor in Neighbors(randomCell))
                    {
                        neighbor.NeighborsWithBombs++;
                    }
                }

                bombsLeft--;
            }
        }
        private void Cascade(in Cell startingCell)
        {
            // Reveal all neighboring cells except the ones that have already been looked at
            var cellsToReveal = Neighbors(startingCell).Where(c => !c.Revealed);

            // Reveal cells and conditionally reveal its neighbors as well
            foreach (Cell cell in cellsToReveal)
            {
                cell.Reveal();
                if (cell.NeighborsWithBombs == 0) { Cascade(cell); }
            }

        }
        private void StartGame(in short size, in short difficulty)
        {
            n = size;
            bombPercentage = difficulty / 100.0d;

            InitializeGame();
            Size = UI.InitializeUI(n);
            UI.ShowMenu = false;
        }

        private void Flag(in Cell cell)
        {
            if (cell.Revealed) { return; }
            
            if(!cell.Flagged && FlaggedCount == BombCount) { return; }

            cell.Flag();

            UI.Message = "Bombs: " + (BombCount - FlaggedCount);


        }

        public void GameOver(bool win = false)
        {
            cells.ForEach(c => c.Reveal());
            UI.Message = win ? "You Win!" : "Game Over!";
            UI.ShowMenu = true;
        }

        private void Reset()
        {
            cells.ForEach(c => Controls.Remove(c));
            cells.Clear();
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            Reset();

            StartGame(SizeChoices[UI.GetSize], Difficulty[UI.GetDifficulty]);
            
        }

        private void OnClick(object? sender, MouseEventArgs mouseEventArgs)
        {
            if (sender is not Cell cell) { return; }

            if (!cells.Any(c => c.IsBomb)) { InitializeBombs(cell); }
            if (mouseEventArgs.Button == MouseButtons.Left)
            {

                cell.Reveal();
                if (cell.IsBomb) { GameOver(); }
                if (cell.NeighborsWithBombs == 0) { Cascade(cell); }

            }
            else { Flag(cell); }
            if (cells.Count(c => c.IsBomb) == cells.Count(c => c.Flagged) &&
                cells.Count - cells.Count(c => c.Flagged) == cells.Count(c => c.Revealed)) { GameOver(true); }
        }
    }
}