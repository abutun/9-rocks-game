namespace _9Rocks
{
    partial class Square
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.selectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.disselectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveToHereToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectToolStripMenuItem,
            this.disselectToolStripMenuItem,
            this.moveToHereToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(140, 70);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // selectToolStripMenuItem
            // 
            this.selectToolStripMenuItem.Name = "selectToolStripMenuItem";
            this.selectToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.selectToolStripMenuItem.Text = "Pick Up";
            this.selectToolStripMenuItem.Click += new System.EventHandler(this.selectToolStripMenuItem_Click);
            // 
            // disselectToolStripMenuItem
            // 
            this.disselectToolStripMenuItem.Name = "disselectToolStripMenuItem";
            this.disselectToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.disselectToolStripMenuItem.Text = "Release";
            this.disselectToolStripMenuItem.Click += new System.EventHandler(this.disselectToolStripMenuItem_Click);
            // 
            // moveToHereToolStripMenuItem
            // 
            this.moveToHereToolStripMenuItem.Name = "moveToHereToolStripMenuItem";
            this.moveToHereToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.moveToHereToolStripMenuItem.Text = "Move to Here";
            this.moveToHereToolStripMenuItem.Click += new System.EventHandler(this.moveToHereToolStripMenuItem_Click);
            // 
            // Square
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.ContextMenuStrip = this.contextMenuStrip1;
            this.DoubleBuffered = true;
            this.Name = "Square";
            this.Size = new System.Drawing.Size(50, 50);
            this.MouseLeave += new System.EventHandler(this.Square_MouseLeave);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Square_Paint);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Square_MouseMove);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Square_MouseClick);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Square_MouseDown);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Square_MouseUp);
            this.MouseEnter += new System.EventHandler(this.Square_MouseEnter);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem selectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem disselectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveToHereToolStripMenuItem;

    }
}
