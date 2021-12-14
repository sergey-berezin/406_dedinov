using Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class ConsoleViewModel : ViewModel
    {
        public override void DisplayResult(ImageResult res)
        {
            Console.WriteLine(res.ToString());
        }
    }
}
