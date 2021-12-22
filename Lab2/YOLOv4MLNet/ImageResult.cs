using System;
using System.Collections.Generic;
using System.Text;
using YOLOv4MLNet.DataStructures;

namespace Component
{
    public class ImageResult
    {
        int curCount = 0;
        double imageCount;
        Dictionary<string, int> counts;
        public ImageResult(double imageCount)
        {
            this.imageCount = imageCount;
            this.counts = new Dictionary<string, int>();
        }
        public override string ToString()
        {
            string str = ((double)curCount / imageCount).ToString() + "\n";
            foreach (var item in counts)
            {
                str += item.Key + ": " + item.Value.ToString() + "\n";
            }
            return str;
        }
        public void Add(IReadOnlyList<YoloV4Result> res)
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
            }
            ++curCount;
        }
    }
}