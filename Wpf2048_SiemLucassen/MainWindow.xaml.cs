using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;


namespace Wpf2048_SiemLucassen
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        //Public property for binding.
        public Board Board { get; set; }
        
        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            Board = new Board();
            KeyDown += MoveTiles;
            
        }

        public void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void MoveTiles(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up || e.Key == Key.W)
                Board.MoveTo(Move.Up);
            else if (e.Key == Key.Down || e.Key == Key.S)
                Board.MoveTo(Move.Down);
            else if (e.Key == Key.Left || e.Key == Key.A)
                Board.MoveTo(Move.Left);
            else if (e.Key == Key.Right || e.Key == Key.D)
                Board.MoveTo(Move.Right);
            else
            {
                MessageBox.Show("Use the arrow or AWSD keys to move the numbers.");
            }
        }

        private void btnGame_Click(object sender, RoutedEventArgs e)
        {
             
        }
    }
}
