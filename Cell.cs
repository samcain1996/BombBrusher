using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;
using System.Runtime.Intrinsics.X86;
using System.Reflection;
using System.ComponentModel;

namespace BombBrusher
{
    public class Cell : Button
    {

        // Map the number of neighbors with bombs to cell font color
        private static readonly Dictionary<int, Color> NumberColorMap = new(){

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
        private static readonly Color BombColor = Color.Orange;
        private static readonly Color FlaggedColor = Color.Red;
        private static readonly Color BombTextColor = Color.Black;

        private readonly int row = 0;
        private readonly int col = 0;
       
        public static Size CellSize { get { return new Size(100, 100); } }

        public Cell(int xMargin, int yMargin, int Row, int Col, MouseEventHandler OnClick)
        {
            row = Row;
            col = Col;
            Size = CellSize;

            Location = new(xMargin + row * Width, yMargin + Height * col);

            MouseDown += OnClick;

            Revealed = false;
            IsBomb = false;
        }

        public int NeighborsWithBombs { get; set; }
        public bool Revealed { get; private set; }
        public bool Flagged { get; set; }
        public bool IsBomb { get; set; }

        // Returns whether this cell is neighboring other
        public bool IsNeighbor(in Cell other)
        {
            if (row == other.row && col == other.col) { return false; }
            return Math.Abs(this.row - other.row) <= 1 && Math.Abs(this.col - other.col) <= 1;
        }

        public void Reveal()
        {
            BackColor = IsBomb ? BombColor : BackColor;
            ForeColor = IsBomb ? BombTextColor : NumberColorMap[NeighborsWithBombs];
            Text = IsBomb ? "X" : (NeighborsWithBombs > 0 ? NeighborsWithBombs.ToString() : Text);
            Revealed = true;
        }

        public void Flag()
        {
            Flagged = !Flagged;
            ForeColor = Flagged ? FlaggedColor : BombTextColor;
            Text = Flagged ? "X" : "";
        }
    }

}
