using MazeMakerUtilities;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;
using Valve.Newtonsoft.Json;

namespace MazeMaker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public static Output baseFile = null;

        public MainWindow()
        {
            InitializeComponent();

        }


        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtName.Text))
            {
                MessageBox.Show("You must enter a map name", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                MazeWindow mazeWindow = new MazeWindow((bool)btnVerticalWide.IsChecked, (bool)btnHorizontalWide.IsChecked, txtName.Text, baseFile);
                mazeWindow.Show();
                Close();
            }
        }

        private void btnRandom_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtName.Text))
            {
                MessageBox.Show("You must enter a map name", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {                
                List<List<Room>> maze = Room.makeMaze(txtName.Text);
                Parameters parameters = Parameters.generateParameters(true, true, txtName.Text);
                MazeWindow mazeWindow = new MazeWindow(parameters, maze);
                mazeWindow.Show();
                Close();

                //string selectedFolder;
                //if (File.Exists("config.txt"))
                //{
                //    selectedFolder = File.ReadAllText("config.txt");
                //    Parameters parameters = Parameters.generateParameters(true, true, txtName.Text);
                //    List<List<Room>> maze = Room.makeMaze(parameters);
                //    Output.saveMap(maze, parameters, selectedFolder, baseFile);
                //    MessageBox.Show("Maze Saved");
                //}
                //else
                //{
                //    var dialog = new FolderBrowserDialog();
                //    if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                //    {
                //        selectedFolder = dialog.SelectedPath;
                //        Parameters parameters = Parameters.generateParameters(true, true, txtName.Text);
                //        List<List<Room>> maze = Room.makeMaze(parameters);
                //        Output.saveMap(maze, parameters, selectedFolder, baseFile);
                //        MessageBox.Show("Maze Saved");
                //    }
                //}



            }
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            //openFileDialog.InitialDirectory = AppContext.BaseDirectory;
            openFileDialog.Filter = "Json files (*.json)|*.json";
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string json = File.ReadAllText(openFileDialog.FileName);
                MazeSave mazeSave = JsonConvert.DeserializeObject<MazeSave>(json);

                MazeWindow mazeWindow = new MazeWindow(mazeSave.parameters, mazeSave.maze);
                mazeWindow.Show();
                Close();
            }
        }

        private void btnBase_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            //openFileDialog.InitialDirectory = AppContext.BaseDirectory;
            openFileDialog.Filter = "Json files (*.json)|*.json";
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                lblBase.Content = openFileDialog.FileName;
                string json = File.ReadAllText(openFileDialog.FileName);
                baseFile = JsonConvert.DeserializeObject<Output>(json);

            }
        }
    }
}
