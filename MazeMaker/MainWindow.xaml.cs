using MazeMakerUtilities;
using System.Collections.Generic;
using System.Windows;

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
                MessageBox.Show("You must enter a map name","Error",MessageBoxButton.OK,MessageBoxImage.Error);
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
               Parameters parameters = Parameters.generateParameters((bool)btnVerticalWide.IsChecked, (bool)btnHorizontalWide.IsChecked, txtName.Text);
               List<List<Room>> maze = Room.makeMaze(parameters);
               Output.saveMap(maze, parameters.mapName);
                MessageBox.Show("Maze Saved");
            }
        }


    }
}
