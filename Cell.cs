using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Drawing;

namespace BombBrusher
{
    internal class Cell : Button
    {
        // Map the number of neighbors with bombs to cell font color
        public static readonly Dictionary<int, Color> NumberColorMap = new (){

            { 1, Color.Blue },
            { 2, Color.Green },
            { 3, Color.Red },
            { 4, Color.DarkBlue },
            { 5, Color.IndianRed },
            { 6, Color.Turquoise },
            { 7, Color.Olive },
            { 8, Color.Black }
        
        };

        // Background colors
        private static readonly Color RevealedColor = Color.DarkGray;
        private static readonly Color BombColor = Color.Orange;

        private static readonly Font CellFont = 
            new("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);



        public readonly int row;
        public readonly int col;
        public int neighborsWithBombs;

        public Cell(in int row, in int col, in int size,
               in EventHandler onClick)
        {
            this.row = row;
            this.col = col;

            this.Location = new Point(row * size, size * col);
            this.Size = new Size(size, size);
            this.Click += onClick;
            this.Click += Reveal;

            this.IsBomb = false;
            this.Text = String.Empty;
            this.neighborsWithBombs = 0;

        }

        public override bool Equals(object? obj)
        {
            // Two cells are equal if they have the same row and col
            if (obj is not Cell other) { return false; }

            return other.row == row && other.col == col;
        }

        public bool IsBomb { get; set; }
        
        public bool IsNeighbor(in Cell other)
        {
            if (Equals(other)) { return false; }
            return Math.Abs(this.row - other.row) <= 1 && Math.Abs(this.col - other.col) <= 1;
        }

        public void Reveal(object? sender = null, EventArgs? e = null)
        {
            BackColor = IsBomb ? BombColor : RevealedColor;

            // Format text
            if (neighborsWithBombs > 0) { 
                Font = CellFont;
                ForeColor = NumberColorMap[neighborsWithBombs]; 
                Text =  IsBomb ? "X" : neighborsWithBombs.ToString(); 
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

}
