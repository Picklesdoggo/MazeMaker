using MazeMakerUtilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;

namespace MazeMaker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


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
                MazeWindow mazeWindow = new MazeWindow((bool)btnVerticalWide.IsChecked, (bool)btnHorizontalWide.IsChecked, txtName.Text);
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
                string selectedFolder;
                if (File.Exists("config.txt"))
                {
                    selectedFolder = File.ReadAllText("config.txt");
                    Parameters parameters = Parameters.generateParameters(true, true, txtName.Text);
                    List<List<Room>> maze = Room.makeMaze(parameters);
                    Output.saveMap(maze, parameters, selectedFolder);
                    MessageBox.Show("Maze Saved");
                }
                else
                {
                    var dialog = new FolderBrowserDialog();
                    if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        selectedFolder = dialog.SelectedPath;
                        Parameters parameters = Parameters.generateParameters(true, true, txtName.Text);
                        List<List<Room>> maze = Room.makeMaze(parameters);
                        Output.saveMap(maze, parameters, selectedFolder);
                        MessageBox.Show("Maze Saved");
                    }
                }
                

               
            }
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = AppContext.BaseDirectory;
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
    }
}
