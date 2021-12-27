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
        ImmutableList<ImageResult> imgResults;
        ImmutableList<string> images;
        public WPFViewModel(int count)
        {
            imgResults = ImmutableList<ImageResult>.Empty;
            images = ImmutableList<string>.Empty;
        }
        public override void DisplayResult(ImageResult res, string fileName)
        {
            ImgResults = imgResults.Add(res);
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
        public ImmutableList<ImageResult> ImgResults
        {
            get
            {
                return imgResults;
            }
            set
            {
                imgResults = value;
                OnPropertyChanged(nameof(ImgResults));
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
