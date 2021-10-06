using System.Collections.Concurrent;
using System.Collections.Specialized;
using System.ComponentModel;

namespace YOLOv4MLNet
{
    public class Model : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        ConcurrentDictionary<string, int> classCount;
        ConcurrentBag<Images> descs;
        public Model()
        {
            descs = new ConcurrentBag<Images>();
            classCount = new ConcurrentDictionary<string, int>();
            for (int i = 0; i < YOLO.classesNames.Length; ++i)
            {
                classCount.TryAdd(YOLO.classesNames[i], 0);
            }
        }
        public void Add(Images desc)
        {
            lock (descs)
            {
                descs.Add(desc);
                foreach (Objects o in desc.Objs)
                {
                    ++classCount[o.Cls];
                }
                OnPropertyChanged();
            }
        }
        public ConcurrentDictionary<string, int> ClassCount
        {
            get
            {
                return classCount;
            }
        }
        public ConcurrentBag<Images> Descs
        {
            get
            {
                return descs;
            }
        }
        private void OnPropertyChanged()
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("Descs"));
            }
        }
    }
}