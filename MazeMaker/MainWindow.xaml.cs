using MazeMakerUtilities;
using System.Collections.Generic;
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
                this.Close();
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
                var dialog = new FolderBrowserDialog();                
                
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string selectedFolder = dialog.SelectedPath;
                    Parameters parameters = Parameters.generateParameters((bool)btnVerticalWide.IsChecked, (bool)btnHorizontalWide.IsChecked, txtName.Text);
                    List<List<Room>> maze = Room.makeMaze(parameters);
                    Output.saveMap(maze, parameters.mapName, selectedFolder);
                    MessageBox.Show("Maze Saved");
                }

               
            }
        }


    }
}
