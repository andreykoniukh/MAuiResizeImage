using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
namespace ImageResizer
{
    public class ImagesViewModel :INotifyPropertyChanged, IQueryAttributable
    {
        public string Name { get; set; }
        public ImageSource Source { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual void ApplyQueryAttributes(IDictionary<string, object> query)
        {

        }
    }
}
