namespace EvnetSample14
{
    partial class MyForm
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
            MyButton = new Button();
            MyTextBox = new TextBox();
            SuspendLayout();
            // 
            // MyButton
            // 
            MyButton.Location = new Point(17, 168);
            MyButton.Name = "MyButton";
            MyButton.Size = new Size(762, 68);
            MyButton.TabIndex = 0;
            MyButton.Text = "Say Hello";
            MyButton.UseVisualStyleBackColor = true;
            MyButton.Click += MyButton_Click;
            // 
            // MyTextBox
            // 
            MyTextBox.Location = new Point(18, 29);
            MyTextBox.Name = "MyTextBox";
            MyTextBox.Size = new Size(755, 34);
            MyTextBox.TabIndex = 1;
            // 
            // MyForm
            // 
            AutoScaleDimensions = new SizeF(13F, 28F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(MyTextBox);
            Controls.Add(MyButton);
            Name = "MyForm";
            Text = "SayHello";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button MyButton;
        private TextBox MyTextBox;
    }
}
