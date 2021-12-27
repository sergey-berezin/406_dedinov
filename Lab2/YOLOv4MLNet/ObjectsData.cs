using System;
using System.Collections.Generic;
using System.Text;

namespace Component
{
    public class ObjectsData
    {
        public int id { get; set; }
        public ObjectsData(string label, double x1, double x2, double x3, double x4)
        {
            this.label = label;
            this.x1 = x1;
            this.x2 = x2;
            this.x3 = x3;
            this.x4 = x4;
        }
        virtual public string label { get; set; }
        virtual public double x1 { get; set; }
        virtual public double x2 { get; set; }
        virtual public double x3 { get; set; }
        virtual public double x4 { get; set; }
    }
}
