using System;
using System.Collections.Generic;
using System.Text;

namespace YOLOv4MLNet
{
    public class Objects
    {
        double x, y;
        double height, width;
        string cls;
        public Objects(double x, double y, double height, double width, string cls)
        {
            this.x = x;
            this.y = y;
            this.height = height;
            this.width = width;
            this.cls = cls;
        }
        public override string ToString()
        {
            return "Class: " + cls + ".\nЛевая нижняя координата: ("
                + x.ToString() + "; " + y.ToString() + ").\nВысота = " +
                height.ToString() + ".\nШирина = " + width.ToString() + ".\n";
        }
        public string Cls
        {
            get
            {
                return cls;
            }
        }
    }
}
