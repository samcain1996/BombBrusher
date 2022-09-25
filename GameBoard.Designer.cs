namespace BombBrusher
{
    partial class GameBoard
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.sizeCBox = new System.Windows.Forms.ComboBox();
            this.startButton = new System.Windows.Forms.Button();
            this.retryButton = new System.Windows.Forms.Button();
            this.DifficultyCbox = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // sizeCBox
            // 
            this.sizeCBox.FormattingEnabled = true;
            this.sizeCBox.Location = new System.Drawing.Point(84, 163);
            this.sizeCBox.Name = "sizeCBox";
            this.sizeCBox.Size = new System.Drawing.Size(276, 33);
            this.sizeCBox.TabIndex = 0;

            this.sizeCBox.Items.AddRange(SizeChoices.Keys.ToArray());
                
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(261, 270);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(247, 34);
            this.startButton.TabIndex = 1;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);

            this.retryButton.Location = new System.Drawing.Point(261, 270);
            this.retryButton.Name = "retryButton";
            this.retryButton.Size = new System.Drawing.Size(247, 34);
            this.retryButton.Text = "Retry";
            this.retryButton.UseVisualStyleBackColor = true;
            this.retryButton.Click += new System.EventHandler(this.retryButton_Click);
            this.retryButton.Hide();
            // 
            // DifficultyCbox
            // 
            this.DifficultyCbox.FormattingEnabled = true;
            this.DifficultyCbox.Location = new System.Drawing.Point(425, 163);
            this.DifficultyCbox.Name = "DifficultyCbox";
            this.DifficultyCbox.Size = new System.Drawing.Size(294, 33);
            this.DifficultyCbox.TabIndex = 2;

            this.DifficultyCbox.Items.AddRange(Difficulty.Keys.ToArray());
            // 
            // GameBoard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.DifficultyCbox);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.retryButton);
            this.Controls.Add(this.sizeCBox);
            this.Name = "GameBoard";
            this.Text = "GameBoard";
            this.ResumeLayout(false);

        }

        #endregion

        private ComboBox sizeCBox;
        private Button startButton;
        private Button retryButton;
        private ComboBox DifficultyCbox;
    }
}