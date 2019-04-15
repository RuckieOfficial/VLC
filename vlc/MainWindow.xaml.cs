using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;
using Vlc.DotNet.Core;

namespace vlc {
    /// <summary>
    /// Interakční logika pro MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        public int a = 0; 
        public int c = 0;
        public double i;

        DispatcherTimer timer = new DispatcherTimer();

        public MainWindow() {
            InitializeComponent();
            mainFunction();
        }

        private void mainFunction() {
            timer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            timer.Tick += new EventHandler(MyControl2_positionchanged);
            timer.Start();

            var currentAssembly = Assembly.GetEntryAssembly();
            var currentDirectory = new FileInfo(currentAssembly.Location).DirectoryName;
            var vlcLibDirectory = new DirectoryInfo(System.IO.Path.Combine(currentDirectory, "libvlc", IntPtr.Size == 4 ? "win-x86" : "win-x64"));
            var options = new string[] { };
            this.MyControl.SourceProvider.CreatePlayer(vlcLibDirectory, options);
            this.MyControl.SourceProvider.MediaPlayer.Play("http://download.blender.org/peach/bigbuckbunny_movies/big_buck_bunny_480p_h264.mov");
            this.MyControl.SourceProvider.MediaPlayer.PositionChanged += new System.EventHandler<Vlc.DotNet.Core.VlcMediaPlayerPositionChangedEventArgs>(MyControl_positionchanged);

            a = (int)this.MyControl.SourceProvider.MediaPlayer.Length;
            //MessageBox.Show(a.ToString());
        }

        private void MyControl_positionchanged(object sender, EventArgs e) {
            i = Math.Round(MyControl.SourceProvider.MediaPlayer.Position * 1000, 0);
        }

        private void MyControl2_positionchanged(object sender, EventArgs e) {
            trackBar1.Value = i;
        }

        private void trackBar1_Scroll(object sender, EventArgs e) {
            int b = (int)MyControl.SourceProvider.MediaPlayer.Time / 1000;
            int d = b / 60;
            b = b - d * 60;
            //label1.Text = d + ":" + b + "/" + c + ":" + a;
        }

        private void VideoLoad(object sender, EventArgs e) {
            //trackBar1.Maximum = a;
            c = a / 60;
            a = a - c * 60;
            label1.Content = 0 + "/" + c + ":" + a;
        }


        private void lod(object sender, RoutedEventArgs e) {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            if (ofd.ShowDialog() == true) {
                ThreadPool.QueueUserWorkItem((Object stateInfo) => MyControl.SourceProvider.MediaPlayer.Play(new Uri(ofd.FileName)));

            }
            //this.MyControl.SourceProvider.
        }
        private void ply(object sender, RoutedEventArgs e) {
            this.MyControl.SourceProvider.MediaPlayer.Play();
        }
        private void stp(object sender, RoutedEventArgs e) {
            ThreadPool.QueueUserWorkItem((Object stateInfo) => MyControl.SourceProvider.MediaPlayer.Stop());
        }
        private void pse(object sender, RoutedEventArgs e) {
            this.MyControl.SourceProvider.MediaPlayer.Pause();
        }
}
}
