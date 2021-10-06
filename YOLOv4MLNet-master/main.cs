using System;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using YOLOv4MLNet;

namespace Main
{
    public class Main
    {
        private static void Handler(object sender, ConsoleCancelEventArgs args)
        {
            Environment.Exit(0);
        }
        static async Task Main()
        {

            Console.WriteLine("Для отмены выполнения нажмите Ctrl+с на английской раскладке.");
            YOLO yolo = new Perception(@"D:\yolov4.onnx");
            DirectoryInfo di = new DirectoryInfo(@"D:\Images");
            FileInfo[] imgs = di.GetFiles();
            ConsoleView cv = new ConsoleView(imgs.Length);
            CancellationTokenSource cts = new CancellationTokenSource();
            CancellationToken ct = cts.Token;
            Console.CancelKeyPress += Handler;
            foreach (FileInfo flinf in imgs)
            {
                await yolo.StartYOLO(cv.Model, flinf.FullName, ct);
            }
        }
    }
}
