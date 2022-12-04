using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;
using System.Runtime.Intrinsics.X86;

namespace BombBrusher
{
    internal partial class Cell : Button
    {

        internal struct ReferenceCell
        {
            public readonly Color forecolor = DefaultColor;
            public readonly Color backcolor = DefaultColor;
            public readonly Font font = CellFont;
            public readonly string text = String.Empty;

            public ReferenceCell(Color fc, Color bc, Font f, string t)
            {
                forecolor = fc;
                backcolor = bc;
                font = f;
                text = t;
            }

            public ReferenceCell(Color fc, Color bc, string t) : this(fc, bc, CellFont, t) { }
        }

        public enum CellType
        {
            Bomb,
            BombGuess,
            RevealedEmpty,
            Revealed,
            Default
        }

        public readonly int row;
        public readonly int col;
        public int neighborsWithBombs;

        public Cell(int row, int col, int size, in EventHandler onClick)
        {
            this.row = row;
            this.col = col;

            this.Location = new(row * size, size * col);
            this.Size = new(size, size);

            this.Click += onClick;
            this.Click += Reveal;
            this.MouseDown += OnMouseDown;

            this.IsBomb = false;
            this.Text = String.Empty;

            AllCells.Add(this);

        }

        public bool IsBomb { get; set; }
        
        public bool IsNeighbor(in Cell other)
        {
            if (Equals(other)) { return false; }
            return Math.Abs(this.row - other.row) <= 1 && Math.Abs(this.col - other.col) <= 1;
        }

        public void Reveal(object? sender = null, EventArgs? e = null)
        {
            Font = CellFont;
            if (IsBomb) { Type = CellType.Bomb; }
            else
            {
                BackColor = RevealedColor;
                ForeColor = NumberColorMap[neighborsWithBombs];
                if (neighborsWithBombs > 0) { Text = neighborsWithBombs.ToString(); }
            }
        }

        public void MarkAsBomb(int totalBombs)
        {
            if (Type == CellType.BombGuess) { Type = CellType.Default; }
            else if (MarkedBombs < totalBombs) { Type = CellType.BombGuess; }
        }


    }

}
