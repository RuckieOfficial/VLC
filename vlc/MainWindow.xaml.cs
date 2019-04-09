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
using Vlc.DotNet.Core;

namespace vlc {
    /// <summary>
    /// Interakční logika pro MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public int a = 0; 
        public int c = 0;
        public MainWindow() {
            InitializeComponent();
            var currentAssembly = Assembly.GetEntryAssembly();
            var currentDirectory = new FileInfo(currentAssembly.Location).DirectoryName;

            var vlcLibDirectory = new DirectoryInfo(System.IO.Path.Combine(currentDirectory, "libvlc", IntPtr.Size == 4 ? "win-x86" : "win-x64"));

            var options = new string[]
            {
                // VLC options can be given here. Please refer to the VLC command line documentation.
            };

            this.MyControl.SourceProvider.CreatePlayer(vlcLibDirectory, options);

            // Load libvlc libraries and initializes stuff. It is important that the options (if you want to pass any) and lib directory are given before calling this method.
            this.MyControl.SourceProvider.MediaPlayer.Play("http://download.blender.org/peach/bigbuckbunny_movies/big_buck_bunny_480p_h264.mov");

            //Event handler for 'current media time' label
            this.MyControl.SourceProvider.MediaPlayer.PositionChanged += new System.EventHandler<Vlc.DotNet.Core.VlcMediaPlayerPositionChangedEventArgs>(MyControl_positionchanged);
            //Event handler for setting trackBar1.Maximum on media load
            //MyControl.SourceProvider.MediaPlayer.Playing += new System.EventHandler<VlcMediaPlayerPlayingEventArgs>(SetProgresMax);
    }

        private void MyControl_positionchanged(object sender, EventArgs e) {
            zmenabaru();
            //trackBar1.Value = pos;
        }

        void zmenabaru() {
            trackBar1.Value = this.MyControl.SourceProvider.MediaPlayer.Position;
        }

        // This is the main function which you looking.
        private void trackBar1_Scroll(object sender, EventArgs e) {
            //MyControl.SourceProvider.MediaPlayer.Time = trackBar1.Value * 1000;
            int b = (int)MyControl.SourceProvider.MediaPlayer.Time / 1000;
            int d = b / 60;
            b = b - d * 60;
            //label1.Text = d + ":" + b + "/" + c + ":" + a;
            // The Time value is milisecond, you have divide 1000 for be second.
        }

        private void formdeneme_Load(object sender, EventArgs e) {
            a = (int)MyControl.SourceProvider.MediaPlayer.Length / 1000;
            trackBar1.Maximum = a;
            c = a / 60;
            a = a - c * 60;
           // label1.Text = 0 + "/" + c + ":" + a;
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
