using System;
using System.IO;
using System.Threading;
using WorkOne;

namespace ConsoleApp
{
    class Program
    {
        static CancellationTokenSource cts;
        private static void Cancel(object sender, ConsoleCancelEventArgs args)
        {
            args.Cancel = true;
            cts.Cancel();
        }
        static void Main(string[] args)
        {
            cts = new CancellationTokenSource();
            ConsoleViewModel cvm = new ConsoleViewModel();
            Console.CancelKeyPress += Cancel;
            Perceptron perceptron = new Perceptron();
            DirectoryInfo dirInfo;
            while (true)
            {
                Console.Write("Введите путь к директории с изображениями: ");
                string path = Console.ReadLine();
                try
                {
                    dirInfo = new DirectoryInfo(path);
                    break;
                } catch
                {
                    Console.WriteLine("Введена некорректная директория.");
                }
            }
            cts = new CancellationTokenSource();
            var task = perceptron.PerceptImagesAsync(dirInfo.GetFiles(), cvm, cts.Token);
            var res = task.Result;
        }
    }
}
