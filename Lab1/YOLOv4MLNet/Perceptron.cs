using Component;
using Microsoft.ML;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using YOLOv4MLNet.DataStructures;
using static Microsoft.ML.Transforms.Image.ImageResizingEstimator;

namespace WorkOne
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
            ImageResult r = new ImageResult(files.Length);
            return await Task.Factory.StartNew(() =>
            {
                Parallel.ForEach(files, (file) =>
                {
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
                            var results = predict.GetResults(classesNames, 0.3f, 0.7f);
                            r.Add(results);
                            vm.DisplayResult(r);
                            res.Add(r);
                        }
                    }
                });
                return res;
            });
        }
    }
}
