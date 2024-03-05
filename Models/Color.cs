using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jido.Models
{
    public class Color : INotifyPropertyChanged
    {
        private byte[] _RGB = [0, 0, 0];

        public byte[] RGB
        {
            get { return _RGB; }
            set
            {
                if (!_RGB.SequenceEqual(value))
                {
                    _RGB = value;
                    OnPropertyChanged(nameof(RGB));
                }
            }
        }

        private string _name = "Default";

        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
