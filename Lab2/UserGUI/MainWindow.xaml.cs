using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Ookii.Dialogs.Wpf;
using Component;
using System.Threading;
using System.IO;

namespace UserGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        WPFViewModel wpfvm;
        string path;
        Perceptron perception;
        CancellationTokenSource cts;
        public MainWindow()
        {
            InitializeComponent();
            perception = new Perceptron();
            cts = new CancellationTokenSource();
        }

        private void SearchImages(object sender, RoutedEventArgs e)
        {
            VistaFolderBrowserDialog dialog = new VistaFolderBrowserDialog();
            bool? res = dialog.ShowDialog();
            if (res.Value)
            {
                Folder.Text = dialog.SelectedPath;
                path = dialog.SelectedPath;
                DirectoryInfo di = new DirectoryInfo(path);
                wpfvm = new WPFViewModel(di.GetFiles().Length);
                DataContext = wpfvm;
                FindAllImages();
            }

        }
        private void FindAllImages()
        {
            DirectoryInfo directory = new DirectoryInfo(path);
            foreach (var item in directory.GetFiles())
            {
                wpfvm.Images = wpfvm.Images.Add(item.FullName);
            }
        }
        private void Start(object sender, RoutedEventArgs e)
        {
            DirectoryInfo directory = new DirectoryInfo(path);
            perception.PerceptImagesAsync(directory.GetFiles(), wpfvm, cts.Token);
        }
        private void Stop(object sender, RoutedEventArgs e)
        {
            cts.Cancel();
        }
    }
}