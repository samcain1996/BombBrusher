using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Design;
using System.Windows.Forms.VisualStyles;

namespace BombBrusher
{
    internal static class UI
    {
        private static Button   startButton = new();
        private static ComboBox sizeCBox = new();
        private static ComboBox difficultyCbox = new();
        private static Label    bombCountLabel = new();

        private static List<Control> menu = new() { startButton, sizeCBox, difficultyCbox };

        private static Size MAXXY = new();
        public static int padding = 50;
        private static Rectangle MenuBounds
        {
            get
            {
                int x = menu.MinBy(c => c.Location.X).Location.X;
                int y = menu.MinBy(c => c.Location.Y).Location.Y;
                return new(x, y, menu.MaxBy(c => c.Location.X).Location.X - x,
                    menu.MaxBy(c => c.Location.Y).Location.Y - y);
            }
        }
        public static bool ShowMenu
        {
            set
            {
                if (value) { menu.ForEach(c => c.Show()); }
                else { menu.ForEach(c => c.Hide()); }

            }
        }

        public static string Message
        {
            set
            {
                bombCountLabel.Text = value;
            }
        }

        public static string GetSize { get { return (string)sizeCBox.SelectedItem; } }
        public static string GetDifficulty { get { return (string)difficultyCbox.SelectedItem; } }
        public static void Init(in Control parent, string[] boardSizes, string[] difficulties, EventHandler start)
        {
            startButton = new();
            sizeCBox = new();
            difficultyCbox = new();
            bombCountLabel = new();
            menu = new() { startButton, sizeCBox, difficultyCbox };

            sizeCBox.Size = new Size(275, 40);
            difficultyCbox.Size = new Size(275, 40);
            startButton.Size = new Size(250, 40);
            startButton.Text = "Start!";
            bombCountLabel.Size = new(250, 25);

            startButton.Click += start;
            sizeCBox.Items.AddRange(boardSizes);
            difficultyCbox.Items.AddRange(difficulties);

            MAXXY = parent.Size;

            difficultyCbox.SelectedItem = difficultyCbox.Items[0];
            sizeCBox.SelectedItem = sizeCBox.Items[0];

            parent.Controls.AddRange(menu.ToArray());
            parent.Controls.Add(bombCountLabel);

            GroupMenu();
        }

        // Arrange menu relative to itself
        public static void GroupMenu()
        {
            int width = sizeCBox.Size.Width + difficultyCbox.Size.Width + padding;
            int height = sizeCBox.Size.Height + padding + startButton.Size.Height;

            int x = (MAXXY.Width - width) / 2;
            int y = (MAXXY.Height - height) / 2;

            sizeCBox.Location = new Point(x, y);
            x += sizeCBox.Size.Width + padding;

            difficultyCbox.Location = new Point(x, y);

            x -= ( (padding / 2) + sizeCBox.Size.Width / 2); 
            y += sizeCBox.Size.Height + padding;
            startButton.Location = new Point(x, y);

        }

        // Move Menu to X,Y position
        private static void MoveMenu(int newX, int newY)
        {
            int menuTopX = MenuBounds.X;
            int menuTopY = MenuBounds.Y;

            menu.ForEach(control => {
                int oldX = control.Location.X + (newX - menuTopX);
                int oldY = control.Location.Y + (newY - menuTopY);

                control.Location = new Point(oldX, oldY);
            });
        }

        // Positions UI and returns the size needed to fit UI
        public static Size InitializeUI(int n)
        {
            Rectangle boardRect = new(padding, padding, n * Cell.CellSize.Width, n * Cell.CellSize.Height);

            MoveMenu( boardRect.X + (boardRect.Width - MenuBounds.Width) / 2, boardRect.Bottom + padding + padding);

            int x = boardRect.Right + padding;
            int y = MenuBounds.Bottom + padding * 2;

            bombCountLabel.Location = new(x, 200);

            x += bombCountLabel.Size.Width + padding;

            return new Size(x, y + padding);
        }

    }
}
