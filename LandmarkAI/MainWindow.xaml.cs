using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
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
using LandmarkAI.Classes;
using Newtonsoft.Json;

namespace LandmarkAI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _urlKey, _predictionKey, _contentKey;

        public MainWindow()
        {
            InitializeComponent();
            ParseKeyFile();
        }

        private void ParseKeyFile()
        {
            //Change file directory to mattch keys
            string[] lines = File.ReadAllLines(@"F:\Github\WPF_VisualIntelligence\LandmarkAI\keys.txt");
            _urlKey = lines[0];
            _predictionKey = lines[1];
            _contentKey = lines[2];
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image files (*.png, *.jpg)|*.png;*.jpg;*.jpeg|All files (*.*)|*.*";
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            if (dialog.ShowDialog() == true)
            {
                string fileName = dialog.FileName;
                SelectedImage.Source = new BitmapImage(new Uri(fileName));
                MakePredictionAsync(fileName);
            }
        }

        private async void MakePredictionAsync(string fileName)
        {
            var file = File.ReadAllBytes(fileName);
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Prediction-Key", _predictionKey);
                using (var content = new ByteArrayContent(file))
                {
                    content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(_contentKey);
                    var response = await client.PostAsync(_urlKey, content);
                    var responseString = await response.Content.ReadAsStringAsync();
                    List<Prediction> predictions = (List<Prediction>)JsonConvert.DeserializeObject<CustomVision>(responseString).Predictions;
                    PredictionListView.ItemsSource = predictions;
                }
            }
        }
    }
}