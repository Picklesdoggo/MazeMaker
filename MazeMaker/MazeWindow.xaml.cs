using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml.Linq;
using MazeMakerUtilities;
using MessageBox = System.Windows.MessageBox;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;
using RadioButton = System.Windows.Controls.RadioButton;

namespace MazeMaker
{
    /// <summary>
    /// Interaction logic for MazeWindow.xaml
    /// </summary>
    public partial class MazeWindow : Window
    {


        public static Parameters parameters = new Parameters();
        public static Output baseFile = null;
        public static int horizontalLineThickness = 4;
        public static int verticalLineThickness = 4;             

        public static List<List<Room>> maze;
        public MazeWindow()
        {
            InitializeComponent();

        }

        public MazeWindow(bool verticalWideInput, bool horizontalWideInput, string mapNameInput)
        {
            InitializeComponent();

            parameters = Parameters.generateParameters(verticalWideInput, horizontalWideInput, mapNameInput);

            maze = Room.generateRooms(parameters);

            displayControls();
            generateGrid();
        }

        public MazeWindow(bool verticalWideInput, bool horizontalWideInput, string mapNameInput, Output outputInput)
        {
            InitializeComponent();

            parameters = Parameters.generateParameters(verticalWideInput, horizontalWideInput, mapNameInput);

            maze = Room.generateRooms(parameters);
            baseFile  = outputInput;

            displayControls();
            generateGrid();
        }


        public MazeWindow(Parameters parametersInput, List<List<Room>> mazeInput)
        {
            InitializeComponent();
            parameters = parametersInput;
            maze = mazeInput;
            displayControls();
            generateGrid();
            loadMaze();
        }

        private void loadMaze()
        {
            for (int r = 0; r < maze.Count; r++)
            {
                for (int c =0; c < maze[r].Count; c++)
                {
                    DockPanel room = grdMain.Children.Cast<DockPanel>().First(e => Grid.GetRow(e) == r && Grid.GetColumn(e) == c);
                    foreach(UIElement child in room.Children)
                    {
                        Rectangle wall = (Rectangle)child;
                        if (wall.Name.Contains("Top"))
                        {
                            if (parameters.horizontalWide)
                            {
                                string savedTop = getControlByObjectID(maze[r][c].top.element.ObjectID);
                                foreach(UIElement element in btnsVertical.Children)
                                {
                                    if (element.GetType().Name == "RadioButton")
                                    {
                                        RadioButton rb = (RadioButton)element;
                                        if (rb.Content.ToString() == savedTop)
                                        {
                                            SolidColorBrush solidColor = new SolidColorBrush();
                                            solidColor = (SolidColorBrush)rb.Foreground;
                                            wall.Fill = solidColor;
                                        }
                                    }

                                }
                            }
                            
                        }
                        else if (wall.Name.Contains("Bottom"))
                        {
                            if (parameters.horizontalWide)
                            {
                                string savedTop = getControlByObjectID(maze[r][c].bottom.element.ObjectID);
                                foreach (UIElement element in btnsVertical.Children)
                                {
                                    if (element.GetType().Name == "RadioButton")
                                    {
                                        RadioButton rb = (RadioButton)element;
                                        if (rb.Content.ToString() == savedTop)
                                        {
                                            SolidColorBrush solidColor = new SolidColorBrush();
                                            solidColor = (SolidColorBrush)rb.Foreground;
                                            wall.Fill = solidColor;
                                        }
                                    }

                                }
                            }

                        }
                        else if (wall.Name.Contains("Left"))
                        {
                            if (parameters.verticalWide)
                            {
                                string savedTop = getControlByObjectID(maze[r][c].left.element.ObjectID);
                                foreach (UIElement element in btnsHorizontal.Children)
                                {
                                    if (element.GetType().Name == "RadioButton")
                                    {
                                        RadioButton rb = (RadioButton)element;
                                        if (rb.Content.ToString() == savedTop)
                                        {
                                            SolidColorBrush solidColor = new SolidColorBrush();
                                            solidColor = (SolidColorBrush)rb.Foreground;
                                            wall.Fill = solidColor;
                                        }
                                    }

                                }
                            }

                        }
                        else if (wall.Name.Contains("Right"))
                        {
                            if (parameters.verticalWide)
                            {
                                string savedTop = getControlByObjectID(maze[r][c].right.element.ObjectID);
                                foreach (UIElement element in btnsHorizontal.Children)
                                {
                                    if (element.GetType().Name == "RadioButton")
                                    {
                                        RadioButton rb = (RadioButton)element;
                                        if (rb.Content.ToString() == savedTop)
                                        {
                                            SolidColorBrush solidColor = new SolidColorBrush();
                                            solidColor = (SolidColorBrush)rb.Foreground;
                                            wall.Fill = solidColor;
                                        }
                                    }

                                }
                            }

                        }
                    }
                }
            }
        }

        private string getControlByObjectID(string objectID)
        {
            string control = "";

            if (objectID == "ShoothouseBarrierWall")
            {
                control = "Wall";
            }
            else if (objectID == "ShoothouseBarrierDoorSingle")
            {
                control = "Single Door";
            }
            else if (objectID == "ShoothouseBarrierDoorDouble")
            {
                control = "Double Door";
            }
            else if (objectID == "ShoothouseBarrierWindowNarrow")
            {
                control = "Window";
            }
            else if (objectID == "CompBarrierLow")
            {
                control = "Low Barrier";
            }
            else if (objectID == "None")
            {
                control = "None";
            }

            return control;
        }

        private void displayControls()
        {
            if (!parameters.verticalWide && !parameters.horizontalWide)
            {
                grdControls.Visibility = Visibility.Collapsed;
            }
            else
            {
                // Display vertical controls?
                if (parameters.verticalWide)
                {
                    btnsVertical.Visibility = Visibility.Visible;
                }
                else
                {
                    btnsVertical.Visibility = Visibility.Collapsed;
                }

                // Display horizontal controls
                if (parameters.horizontalWide)
                {
                    btnsHorizontal.Visibility = Visibility.Visible;
                }
                else
                {
                    btnsHorizontal.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void generateGrid()
        {

            int marginSize = 10;

            Thickness margin = grdMain.Margin;
            margin.Top = marginSize;
            margin.Left = marginSize;
            margin.Bottom = marginSize;
            margin.Right = marginSize;
            grdMain.Margin = margin;

            // Add gridColumns
            for (int c = 0; c < parameters.gridColumns; c++)
            {
                ColumnDefinition column = new ColumnDefinition();
                grdMain.ColumnDefinitions.Add(column);
            }

            // Add gridRows
            for (int r = 0; r < parameters.gridRows; r++)
            {
                RowDefinition row = new RowDefinition();
                grdMain.RowDefinitions.Add(row);
            }

            // Loop through columnns
            for (int c = 0; c < parameters.gridColumns; c++)
            {
                // Loop through gridRows
                for (int r = 0; r < parameters.gridRows; r++)
                {
                    DockPanel panel = new DockPanel
                    {
                        LastChildFill = true,
                        Name = "Room_" + r + "_" + c
                    };

                    // Only top row should have TOP element

                    Rectangle horizontalTop = new Rectangle();
                    horizontalTop.MouseDown += WallClicked;
                    horizontalTop.MouseEnter += WallHover;
                    horizontalTop.Height = verticalLineThickness;
                    horizontalTop.Fill = new SolidColorBrush(Colors.Black);
                    horizontalTop.Name = "Top_" + r + "_" + c;
                    DockPanel.SetDock(horizontalTop, Dock.Top);
                    panel.Children.Add(horizontalTop);


                    if (r == parameters.gridRows - 1)
                    {
                        // Add bottom element
                        Rectangle horizontalBottom = new Rectangle();
                        horizontalBottom.MouseDown += WallClicked;
                        horizontalBottom.MouseEnter += WallHover;
                        horizontalBottom.Height = verticalLineThickness;
                        horizontalBottom.Fill = new SolidColorBrush(Colors.Black);
                        horizontalBottom.Name = "Bottom_" + r + "_" + c;
                        DockPanel.SetDock(horizontalBottom, Dock.Bottom);
                        panel.Children.Add(horizontalBottom);
                    }


                    Rectangle verticalLeft = new Rectangle();
                    verticalLeft.MouseDown += WallClicked;
                    verticalLeft.MouseEnter += WallHover;
                    verticalLeft.Width = horizontalLineThickness;
                    verticalLeft.Fill = new SolidColorBrush(Colors.Black);
                    verticalLeft.Name = "Left_" + r + "_" + c;
                    DockPanel.SetDock(verticalLeft, Dock.Left);
                    panel.Children.Add(verticalLeft);



                    // Add right element
                    if (c == parameters.gridColumns - 1)
                    {
                        Rectangle verticalRight = new Rectangle();
                        verticalRight.MouseDown += WallClicked;
                        verticalRight.MouseEnter += WallHover;
                        verticalRight.Width = horizontalLineThickness;
                        verticalRight.Fill = new SolidColorBrush(Colors.Black);
                        verticalRight.Name = "Right_" + r + "_" + c;
                        DockPanel.SetDock(verticalRight, Dock.Right);
                        panel.Children.Add(verticalRight);
                    }
                    // Add middle cell
                    Rectangle middleCell = new Rectangle
                    {
                        Fill = new SolidColorBrush(Colors.LightGray)
                    };
                    if (maze[r][c].path)
                    {
                        middleCell.Fill = new SolidColorBrush(Colors.White);
                    }

                    panel.Children.Add(middleCell);

                    panel.SetValue(Grid.RowProperty, r);
                    panel.SetValue(Grid.ColumnProperty, c);
                    grdMain.Children.Add(panel);
                }
            }

        }

        private void fillRectangle(Rectangle selected)
        {
            Color selectedColor = ((SolidColorBrush)selected.Fill).Color;
                       

            // find the name of the piece
            List<string> split = selected.Name.Split('_').ToList();
            int r = Convert.ToInt32(split[1]);
            int c = Convert.ToInt32(split[2]);
           
            if (selected.Name.Contains("Right") || selected.Name.Contains("Left"))
            {
                if (parameters.verticalWide)
                {
                    System.Tuple<SolidColorBrush, string> selectedValues = GetSelectedVerticalValues();
                    selected.Fill = selectedValues.Item1;
                    if (selectedValues.Item2 != "None")
                    {
                        if (selected.Name.Contains("Right"))
                        {
                            maze[r][c].right.element.ObjectID = selectedValues.Item2;
                            maze[r][c].right.render = true;
                        }
                        else
                        {
                            maze[r][c].left.element.ObjectID = selectedValues.Item2;
                            maze[r][c].left.render = true;
                        }

                    }
                    else
                    {
                        if (selected.Name.Contains("Right"))
                        {
                            maze[r][c].right.render = false;
                        }
                        else
                        {
                            maze[r][c].left.render = false;
                        }
                    }
                }
                else
                {
                    if (selectedColor == Colors.Black)
                    {
                        if (selected.Name.Contains("Right"))
                        {
                            selected.Fill = new SolidColorBrush(Colors.LightGray);
                            maze[r][c].right.render = false;
                        }
                        else
                        {
                            selected.Fill = new SolidColorBrush(Colors.LightGray);
                            maze[r][c].left.render = false;
                        }
                    }
                    else
                    {
                        if (selected.Name.Contains("Right"))
                        {
                            selected.Fill = new SolidColorBrush(Colors.Black);
                            maze[r][c].right.render = true;
                        }
                        else
                        {
                            selected.Fill = new SolidColorBrush(Colors.Black);
                            maze[r][c].left.render = true;
                        }
                    }
                }
                            
            }
            else if (selected.Name.Contains("Top") || selected.Name.Contains("Bottom"))
            {
                if (parameters.horizontalWide)
                {
                    System.Tuple<SolidColorBrush, string> selectedValues = GetSelectedHorizontalValues();
                    selected.Fill = selectedValues.Item1;
                    if (selectedValues.Item2 != "None")
                    {
                        if (selected.Name.Contains("Top"))
                        {
                            maze[r][c].top.element.ObjectID = selectedValues.Item2;
                            maze[r][c].top.render = true;
                        }
                        else
                        {
                            maze[r][c].bottom.element.ObjectID = selectedValues.Item2;
                            maze[r][c].bottom.render = true;
                        }

                    }
                    else
                    {
                        if (selected.Name.Contains("Top"))
                        {
                            maze[r][c].top.render = false;
                        }
                        else
                        {
                            maze[r][c].bottom.render = false;
                        }
                    }
                }
                else
                {
                    if (selectedColor == Colors.Black)
                    {
                        if (selected.Name.Contains("Top"))
                        {
                            selected.Fill = new SolidColorBrush(Colors.LightGray);
                            maze[r][c].top.render = false;
                        }
                        else
                        {
                            selected.Fill = new SolidColorBrush(Colors.LightGray);
                            maze[r][c].bottom.render = false;
                        }
                    }
                    else
                    {
                        if (selected.Name.Contains("Top"))
                        {
                            selected.Fill = new SolidColorBrush(Colors.Black);
                            maze[r][c].top.render = true;
                        }
                        else
                        {
                            selected.Fill = new SolidColorBrush(Colors.Black);
                            maze[r][c].bottom.render = true;
                        }
                    }
                }

            }


        }

        public System.Tuple<SolidColorBrush, string> GetSelectedVerticalValues()
        {
            SolidColorBrush solidColor = new SolidColorBrush();
            string objectID = "None";
            if (btnVerticalSingleDoor.IsChecked == true)
            {
                solidColor = (SolidColorBrush)btnVerticalSingleDoor.Foreground;
                objectID = "ShoothouseBarrierDoorSingle";                
            }
            else if (btnVerticalDoubleDoor.IsChecked == true)
            {
                solidColor = (SolidColorBrush)btnVerticalDoubleDoor.Foreground;
                objectID = "ShoothouseBarrierDoorDouble";
            }
            else if (btnVerticalWindow.IsChecked == true)
            {
                solidColor = (SolidColorBrush)btnVerticalWindow.Foreground;
                objectID = "ShoothouseBarrierWindowNarrow";
            }
            else if (btnVerticalWall.IsChecked == true)
            {
                solidColor = (SolidColorBrush)btnVerticalWall.Foreground;
                objectID = "ShoothouseBarrierWall";
            }
            else if (btnVerticalCompBarrierLow.IsChecked == true)
            {
                solidColor = (SolidColorBrush)btnVerticalCompBarrierLow.Foreground;
                objectID = "CompBarrierLow";
            }
            else if (btnVerticalNone.IsChecked == true)
            {
                solidColor = (SolidColorBrush)btnVerticalNone.Foreground;                
            }
           
            return new System.Tuple<SolidColorBrush, string>(solidColor, objectID);
        }

        public System.Tuple<SolidColorBrush, string> GetSelectedHorizontalValues()
        {
            SolidColorBrush solidColor = new SolidColorBrush();
            string objectID = "None";
            if (btnHorizontalSingleDoor.IsChecked == true)
            {
                solidColor = (SolidColorBrush)btnHorizontalSingleDoor.Foreground;
                objectID = "ShoothouseBarrierDoorSingle";
            }
            else if (btnHorizontalDoubleDoor.IsChecked == true)
            {
                solidColor = (SolidColorBrush)btnHorizontalDoubleDoor.Foreground;
                objectID = "ShoothouseBarrierDoorDouble";
            }
            else if (btnHorizontalWindow.IsChecked == true)
            {
                solidColor = (SolidColorBrush)btnHorizontalWindow.Foreground;
                objectID = "ShoothouseBarrierWindowNarrow";
            }
            else if (btnHorizontalWall.IsChecked == true)
            {
                solidColor = (SolidColorBrush)btnHorizontalWall.Foreground;
                objectID = "ShoothouseBarrierWall";
            }
            else if (btnHorizontalCompBarrierLow.IsChecked == true)
            {
                solidColor = (SolidColorBrush)btnHorizontalCompBarrierLow.Foreground;
                objectID = "CompBarrierLow";
            }
            else if (btnHorizontalNone.IsChecked == true)
            {
                solidColor = (SolidColorBrush)btnHorizontalNone.Foreground;
            }

            return new System.Tuple<SolidColorBrush, string>(solidColor, objectID);
        }

        private void WallClicked(object sender, MouseButtonEventArgs e)
        {
            Rectangle selected = (Rectangle)sender;
            fillRectangle(selected);
        }

        private void WallHover(object sender, MouseEventArgs e)
        {
            if (!Keyboard.IsKeyDown(Key.LeftCtrl) && !Keyboard.IsKeyDown(Key.RightCtrl))
            {
                e.Handled = true;
                return;
            }
            Rectangle selected = (Rectangle)sender;
            fillRectangle(selected);
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {            
            // Check for config file
            if (File.Exists("config.txt"))
            {
                string selectedFolder = File.ReadAllText("config.txt");                
                Output.saveMap(maze, parameters, selectedFolder, baseFile);
                MessageBox.Show("Maze Saved");
            }
            else
            {
                var dialog = new FolderBrowserDialog();
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string selectedFolder = dialog.SelectedPath;
                    Output.saveMap(maze, parameters, selectedFolder, baseFile);
                    MessageBox.Show("Maze Saved");
                }
            }           
        }

        private void New_Click(object sender, RoutedEventArgs e)
        {            
            maze = Room.makeMaze(parameters);
            MazeWindow mazeWindow = new MazeWindow(parameters, maze);
            mazeWindow.Show();
            Close();
        }
    }
}
