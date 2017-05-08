namespace ImageBuilder
{
    public partial class Window
    {
        public Window()
        {
            InitializeComponent();

            DataContext  = new WindowVm();
        }
    }
}
