using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Component
{
    public class ImageResultDB
    {
        [Key]
        public int id { get; set; }
        public string path { get; set; }
        public byte[] pic { get; set; }
        public int hashCode { get; set; }
        virtual public ICollection<ObjectsData> descs { get; set; }
        public override string ToString()
        {
            return this.path;
        }
    }
}
