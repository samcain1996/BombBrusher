using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombBrusher
{
    internal partial class Cell : Button
    {

        public static HashSet<Cell> AllCells = new();
        // Map the number of neighbors with bombs to cell font color
        public static readonly Dictionary<int, Color> NumberColorMap = new(){

            { 0, Color.DarkGray },
            { 1, Color.Blue },
            { 2, Color.Green },
            { 3, Color.Red },
            { 4, Color.DarkBlue },
            { 5, Color.IndianRed },
            { 6, Color.Turquoise },
            { 7, Color.Olive },
            { 8, Color.Black },

        };

        // Background colors
        private static readonly Color DefaultColor = Color.White;
        private static readonly Color RevealedColor = Color.DarkGray;
        private static readonly Color BombBackColor = Color.Orange;
        private static readonly Color BombForeColor = Color.Black;
        private static readonly Color BombGuessColor = Color.Red;

        public static readonly Dictionary<CellType, ReferenceCell> GetCellType = new(){
            { CellType.Bomb, new ReferenceCell(BombForeColor, BombBackColor, "X") },
            { CellType.BombGuess, new ReferenceCell(BombGuessColor, DefaultColor, "B") },
            { CellType.Revealed, new ReferenceCell(RevealedColor, RevealedColor, String.Empty) },
            { CellType.Default, new ReferenceCell(DefaultColor, DefaultColor, String.Empty) }
        };

        private CellType type = CellType.Default;

        public CellType Type
        {
            get { return type; }
            private set
            {
                ReferenceCell exampleType = GetCellType[value];
                type = value;

                ForeColor = exampleType.forecolor;
                BackColor = exampleType.backcolor;
                Text = exampleType.text;
                Font = exampleType.font;
            }
        }

        public static int MarkedBombs
        {
            get
            {
                string bombText = GetCellType[CellType.Bomb].text;
                return AllCells.Count(cell => cell.Text == bombText);
            }
        }

        private static readonly Font CellFont =
            new("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);

        public override bool Equals(object? obj)
        {
            // Two cells are equal if they have the same row and col
            if (obj is not Cell other) { return false; }

            return other.row == row && other.col == col;
        }

        private void OnMouseDown(object? sender, MouseEventArgs mouseEventArgs)
        {
            if (Parent is not GameBoard board) { return; }

            if (mouseEventArgs.Button == MouseButtons.Right) { MarkAsBomb(board.BombCount); }
            board.bombCountLabel.Text = "Bombs: " + (board.BombCount - MarkedBombs).ToString();
        }

        public override int GetHashCode() { return base.GetHashCode(); }
    }
}
