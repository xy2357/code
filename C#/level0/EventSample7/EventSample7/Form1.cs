namespace EventSample7
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void ButtonClicked(object sender, EventArgs e)
        {
            if (sender == button1)
            {
                this.textBox1.Text = "Hello";
            }
            if (sender == this.button2)
            {
                this.textBox1.Text = "World";
            }
            if (sender == this.button3)
            {
                this.button1 = this.button3;
                this.textBox1.Text = "Hello World";
            }
        }
    }
}
