using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Component;

namespace UserGUI
{
    class WPFViewModel : ViewModel, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        ImageResult imgRes;
        ImmutableList<string> images;
        public WPFViewModel(int count)
        {
            imgRes = new ImageResult(count);
            images = ImmutableList<string>.Empty;
        }
        public override void DisplayResult(ImageResult res, string fileName)
        {
            ImgRes = res;
            lock (images) 
            {
                if (images.Contains(fileName))
                {
                    int index = images.IndexOf(fileName);
                    Images = images.RemoveAt(index);
                    Images = images.Insert(index, fileName + "Done.jpg");
                }
            }
        }
        public ImageResult ImgRes
        {
            get
            {
                return imgRes;
            }
            set
            {
                imgRes = value;
                OnPropertyChanged(nameof(ImgRes));
            }
        }
        public ImmutableList<string> Images
        {
            get
            {
                return images;
            }
            set
            {
                images = value;
                OnPropertyChanged(nameof(Images));
            }
        }
        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}
