using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
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

namespace UChat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        itemUser item = new itemUser();        
        public MainWindow()
        {
            InitializeComponent();
            downloadFile();
            listView.Items.Add(item);
        }
        private void downloadFile()
        {           
            // Change the url by the value you want (a textbox or something else)
            string url = "https://localhost/upload/Capture.PNG";
            // Get filename from URL
            string filename = getFilename(url);

            using (var client = new WebClient())
            {
                if (!Directory.Exists("image"))
                {
                    Directory.CreateDirectory("image");
                }
               
                client.DownloadFile(url,"image/"+ filename);                
            }
            Uri uri = new Uri(url);
            var bitmap = new BitmapImage(uri);
            item.img_user.Source = bitmap;
            //MessageBox.Show("Download ready");
        }
        private string getFilename(string hreflink)
        {
            Uri uri = new Uri(hreflink);

            string filename = System.IO.Path.GetFileName(uri.LocalPath);

            return filename;
        }
    }
}
