namespace EvnetSample14
{
    public partial class MyForm : Form
    {
        public MyForm()
        {
            InitializeComponent();
        }

        private void MyButton_Click(object sender, EventArgs e)
        {
            MyTextBox.Text = "Hello World";
        }
    }
}
