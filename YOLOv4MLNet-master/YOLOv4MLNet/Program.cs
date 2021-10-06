using System;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using YOLOv4MLNet;

namespace YOLOv4MLNet
{
    public class Program
    {
        private static void Handler(object sender, ConsoleCancelEventArgs args)
        {
            Environment.Exit(0);
        }
        static async Task Main()
        {

            Console.WriteLine("Для отмены выполнения нажмите Ctrl+с.");
            YOLO yolo = new YOLO(@"D:\yolov4.onnx");
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