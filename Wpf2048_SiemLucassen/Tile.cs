using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Wpf2048_SiemLucassen
{
    /// <summary>
    /// The Tile class represents a tile used in the Board class.
    /// </summary>
    public class Tile : INotifyPropertyChanged
    {
        private int score;

        public int Score
        {
            get { return score; }
            set 
            { 
                score = value;
                RaisePropertyChanged();
            }
        }

        public int Row { get; set; }
        public int Column { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;


        public void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
