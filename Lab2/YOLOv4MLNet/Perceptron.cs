using Component;
using Microsoft.ML;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using YOLOv4MLNet.DataStructures;
using static Microsoft.ML.Transforms.Image.ImageResizingEstimator;

namespace Component
{
    public class Perceptron
    {
        const string modelPath = @"D:\yolov4.onnx";

        const string imageFolder = @"D:\Images";

        const string imageOutputFolder = @"D:\Output";
        private PredictionEngine<YoloV4BitmapData, YoloV4Prediction> predictionEngine;

        static readonly string[] classesNames = new string[] { "person", "bicycle", "car", "motorbike", "aeroplane", "bus", "train", "truck", "boat", "traffic light", "fire hydrant", "stop sign", "parking meter", "bench", "bird", "cat", "dog", "horse", "sheep", "cow", "elephant", "bear", "zebra", "giraffe", "backpack", "umbrella", "handbag", "tie", "suitcase", "frisbee", "skis", "snowboard", "sports ball", "kite", "baseball bat", "baseball glove", "skateboard", "surfboard", "tennis racket", "bottle", "wine glass", "cup", "fork", "knife", "spoon", "bowl", "banana", "apple", "sandwich", "orange", "broccoli", "carrot", "hot dog", "pizza", "donut", "cake", "chair", "sofa", "pottedplant", "bed", "diningtable", "toilet", "tvmonitor", "laptop", "mouse", "remote", "keyboard", "cell phone", "microwave", "oven", "toaster", "sink", "refrigerator", "book", "clock", "vase", "scissors", "teddy bear", "hair drier", "toothbrush" };
        public Perceptron()
        {
            MLContext mlContext = new MLContext();
            var pipeline = mlContext.Transforms.ResizeImages(inputColumnName: "bitmap", outputColumnName: "input_1:0", imageWidth: 416, imageHeight: 416, resizing: ResizingKind.IsoPad)
                .Append(mlContext.Transforms.ExtractPixels(outputColumnName: "input_1:0", scaleImage: 1f / 255f, interleavePixelColors: true))
                .Append(mlContext.Transforms.ApplyOnnxModel(
                    shapeDictionary: new Dictionary<string, int[]>()
                    {
                        { "input_1:0", new[] { 1, 416, 416, 3 } },
                        { "Identity:0", new[] { 1, 52, 52, 3, 85 } },
                        { "Identity_1:0", new[] { 1, 26, 26, 3, 85 } },
                        { "Identity_2:0", new[] { 1, 13, 13, 3, 85 } },
                    },
                    inputColumnNames: new[]
                    {
                        "input_1:0"
                    },
                    outputColumnNames: new[]
                    {
                        "Identity:0",
                        "Identity_1:0",
                        "Identity_2:0"
                    },
                    modelFile: modelPath, recursionLimit: 100));
            var model = pipeline.Fit(mlContext.Data.LoadFromEnumerable(new List<YoloV4BitmapData>()));
            predictionEngine = mlContext.Model.CreatePredictionEngine<YoloV4BitmapData, YoloV4Prediction>(model);
        }

        public async Task<List<ImageResult>> PerceptImagesAsync(FileInfo[] files, ViewModel vm, CancellationToken token)
        {
            List<ImageResult> res = new List<ImageResult>();
            return await Task.Factory.StartNew(() =>
            {
                Parallel.ForEach(files, (file) =>
                {
                    IReadOnlyList<YoloV4Result> results;
                    if (token.IsCancellationRequested)
                    {
                        return;
                    }
                    using (var bitmap = new Bitmap(Image.FromFile(file.FullName)))
                    {
                        lock (predictionEngine)
                        {
                            if (token.IsCancellationRequested)
                            {
                                return;
                            }
                            var predict = predictionEngine.Predict(new YoloV4BitmapData() { Image = bitmap });
                            results = predict.GetResults(classesNames, 0.3f, 0.7f);
                            ImageResult r = new ImageResult(file.FullName, results);
                            Draw(file.FullName, results);
                            vm.DisplayResult(r, file.FullName);
                            res.Add(r);
                        }
                        ImageResultDB imgResDB = new ImageResultDB();
                        Image image = Image.FromFile(file.FullName);
                        System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
                        image.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                        imgResDB.pic = memoryStream.ToArray();
                        imgResDB.path = file.FullName;
                        imgResDB.hashCode = GetHashFromBytes(imgResDB.pic);
                        List<ObjectsData> tmp = new List<ObjectsData>();
                        foreach (var item in results)
                        {
                            ObjectsData objD = new ObjectsData(item.Label, item.BBox[0], item.BBox[1], item.BBox[2], item.BBox[3]);
                            tmp.Add(objD);
                        }
                        imgResDB.descs = tmp;
                        using (var db = new ImageResultContext())
                        {
                            var q = db.Images;
                            foreach (var item in q)
                            {
                                if (item.hashCode == imgResDB.hashCode)
                                {
                                    if (item.pic.SequenceEqual(imgResDB.pic))
                                    {
                                        return;
                                    }
                                }
                            }
                            db.Images.Add(imgResDB);
                            db.SaveChanges();
                        }
                    }
                });
                return res;
            });
        }
        private void Draw(string path, IReadOnlyList<YoloV4Result> res)
        {
            using (Bitmap bitmap = new Bitmap(Image.FromFile(path)))
            {
                lock (bitmap)
                {
                    foreach (var item in res)
                    {
                        var x1 = item.BBox[0];
                        var y1 = item.BBox[1];
                        var x2 = item.BBox[2];
                        var y2 = item.BBox[3];
                        using (var g = Graphics.FromImage(bitmap))
                        {
                            g.DrawRectangle(Pens.Red, x1, y1, x2 - x1, y2 - y1);
                            using (var brushes = new SolidBrush(Color.FromArgb(50, Color.Red)))
                            {
                                g.FillRectangle(brushes, x1, y1, x2 - x1, y2 - y1);
                            }
                            g.DrawString(item.Label + " " + item.Confidence.ToString("0.00"),
                                                 new Font("Arial", 12), Brushes.Blue, new PointF(x1, y1));
                        }
                    }
                    bitmap.Save(path + "Done.jpg");
                }
            }
        }
        private static int GetHashFromBytes(byte[] bytes)
        {
            unchecked
            {
                var result = 0;
                foreach (byte b in bytes)
                    result = (result * 31) ^ b;
                return result;
            }
        }
    }
}
