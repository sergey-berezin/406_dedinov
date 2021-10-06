using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Concurrent;

namespace YOLOv4MLNet
{
    public class Images
    {
        string name;
        ConcurrentBag<Objects> objs;
        public Images(string name)
        {
            this.name = name;
            objs = new ConcurrentBag<Objects>();
        }
        public void Add(Objects obj)
        {
            objs.Add(obj);
        }
        public override string ToString()
        {
            string res = name + "\n";
            foreach (Objects obj in objs)
            {
                res += obj.ToString();
            }
            return res;
        }
        public ConcurrentBag<Objects> Objs
        {
            get
            {
                return objs;
            }
        }
    }
}
