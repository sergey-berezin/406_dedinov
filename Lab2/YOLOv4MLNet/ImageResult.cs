using System;
using System.Collections.Generic;
using System.Text;
using YOLOv4MLNet.DataStructures;

namespace Component
{
    public class ImageResult
    {
        Dictionary<string, int> counts;
        string fileName;
        List<float[]> rectangles;
        public ImageResult(string name, IReadOnlyList<YoloV4Result> res)
        {
            this.counts = new Dictionary<string, int>();
            rectangles = new List<float[]>();
            fileName = name;
            this.Add(res);
        }
        public override string ToString()
        {
            string str = fileName + ":\n";
            foreach (var item in counts)
            {
                str += item.Key + ": " + item.Value.ToString() + "\n";
            }
            return str;
        }
        private void Add(IReadOnlyList<YoloV4Result> res)
        {
            foreach (var item in res)
            {
                if (counts.ContainsKey(item.Label))
                {
                    counts[item.Label] += 1;
                }
                else
                {
                    counts.Add(item.Label, 1);
                }
                rectangles.Add(item.BBox);
            }
        }
    }
}