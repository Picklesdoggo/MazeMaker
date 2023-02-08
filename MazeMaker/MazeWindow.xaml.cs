using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MazeMaker
{
    /// <summary>
    /// Interaction logic for MazeWindow.xaml
    /// </summary>
    public partial class MazeWindow : Window
    {


        public static float XHorizontalStart;
        public static float XHorizontalOffset;
        public static float ZHorizontalStart;
        public static float ZHorizontalOffset;
        public static float YHorizontalStart;

        public static float XVerticalStart;
        public static float XVerticalOffset;
        public static float ZVerticalStart;
        public static float ZVerticalOffset;
        public static float YVerticalStart;

        public static string mapName;

        public static int gridColumns;
        public static int gridRows;
        public static int wallRows;
        public static int wallColumns;

        public static int horizontalLineThickness = 4;
        public static int verticalLineThickness = 4;

        public static bool horizontalWide;
        public static bool verticalWide;

        public static Output generatedOutput = new Output();

        public static List<List<Wall>> maze = new List<List<Wall>>();
        public MazeWindow()
        {
            InitializeComponent();

        }

        public MazeWindow(bool verticalWideInput, bool horizontalWideInput, string mapNameInput)
        {
            InitializeComponent();
            seedValues(verticalWideInput, horizontalWideInput, mapNameInput);
            displayControls();
            generateGrid();
            generateWallGrid();
        }

        private void seedValues(bool verticalWideInput, bool horizontalWideInput, string mapNameInput)
        {
            verticalWide = verticalWideInput;
            horizontalWide = horizontalWideInput;
            mapName = mapNameInput;

            if (verticalWide)
            {
                gridRows = 9;

                XVerticalStart = 12.5F;
                XVerticalOffset = 3.2F;

                ZVerticalStart = 14.2F;
                ZVerticalOffset = 3.0F;

                YVerticalStart = 0;
            }
            else
            {
                gridRows = 5;

                XVerticalStart = 13.1F;
                XVerticalOffset = 1.2F;

                ZVerticalStart = 14.2F;
                ZVerticalOffset = 1.0F;

                YVerticalStart = 0;
            }

            if (horizontalWide)
            {
                gridColumns = 9;

                XHorizontalStart = 14.1F;
                XHorizontalOffset = 3.2F;

                ZHorizontalStart = 12.8F;
                ZHorizontalOffset = 3F;

                YHorizontalStart = 0;

            }
            else
            {
                gridColumns = 5;

                XHorizontalStart = 12.5F;
                XHorizontalOffset = 1.2F;

                ZHorizontalStart = 13.9F;
                ZHorizontalOffset = 0.8F;

                YHorizontalStart = 0;

            }
            wallColumns = gridColumns * 2 + 1;
            wallRows = gridRows * 2 + 1;

        }

        private void displayControls()
        {
            if (!verticalWide && !horizontalWide)
            {
                grdControls.Visibility = Visibility.Collapsed;
            }
            else
            {
                // Display vertical controls?
                if (verticalWide)
                {
                    btnsVertical.Visibility = Visibility.Visible;
                }
                else
                {
                    btnsVertical.Visibility = Visibility.Collapsed;
                }

                // Display horizontal controls
                if (horizontalWide)
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
            for (int c = 0; c < gridColumns; c++)
            {
                ColumnDefinition column = new ColumnDefinition();
                grdMain.ColumnDefinitions.Add(column);
            }

            // Add gridRows
            for (int r = 0; r < gridRows; r++)
            {
                RowDefinition row = new RowDefinition();
                grdMain.RowDefinitions.Add(row);
            }

            // Loop through columnns
            for (int c = 0; c < gridColumns; c++)
            {
                // Loop through gridRows
                for (int r = 0; r < gridRows; r++)
                {
                    DockPanel panel = new DockPanel
                    {
                        LastChildFill = true
                    };

                    // Only top row should have TOP element
                    if (r == 0)
                    {
                        Rectangle horizontalTop = new Rectangle();
                        horizontalTop.MouseDown += WallClicked;
                        horizontalTop.MouseEnter += WallHover;
                        horizontalTop.Height = verticalLineThickness;
                        horizontalTop.Fill = new SolidColorBrush(Colors.Black);
                        horizontalTop.Name = "Horizontal_" + r + "_" + (c * 2 + 1);
                        DockPanel.SetDock(horizontalTop, Dock.Top);
                        panel.Children.Add(horizontalTop);
                    }

                    // Add bottom element
                    Rectangle horizontalBottom = new Rectangle();
                    horizontalBottom.MouseDown += WallClicked;
                    horizontalBottom.MouseEnter += WallHover;
                    horizontalBottom.Height = verticalLineThickness;
                    horizontalBottom.Fill = new SolidColorBrush(Colors.Black);
                    horizontalBottom.Name = "Horizontal_" + (r * 2 + 2) + "_" + (c * 2 + 1);
                    DockPanel.SetDock(horizontalBottom, Dock.Bottom);
                    panel.Children.Add(horizontalBottom);

                    // Only left column should have LEFT element
                    if (c == 0)
                    {
                        Rectangle verticalLeft = new Rectangle();
                        verticalLeft.MouseDown += WallClicked;
                        verticalLeft.MouseEnter += WallHover;
                        verticalLeft.Width = horizontalLineThickness;
                        verticalLeft.Fill = new SolidColorBrush(Colors.Black);
                        verticalLeft.Name = "Vertical_" + (r * 2 + 1) + "_" + c;
                        DockPanel.SetDock(verticalLeft, Dock.Left);
                        panel.Children.Add(verticalLeft);

                    }

                    // Add right element
                    Rectangle verticalRight = new Rectangle();
                    verticalRight.MouseDown += WallClicked;
                    verticalRight.MouseEnter += WallHover;
                    verticalRight.Width = horizontalLineThickness;
                    verticalRight.Fill = new SolidColorBrush(Colors.Black);
                    if (r != 0)
                    {
                        verticalRight.Name = "Vertical_" + (r * 2 + 1) + "_" + (c * 2 + 2);
                    }
                    else
                    {
                        verticalRight.Name = "Vertical_" + (r * 2 + 1) + "_" + (c * 2 + 2);
                    }
                    DockPanel.SetDock(verticalRight, Dock.Right);
                    panel.Children.Add(verticalRight);

                    // Add middle cell
                    Rectangle middleCell = new Rectangle
                    {
                        Fill = new SolidColorBrush(Colors.LightGray)
                    };
                    panel.Children.Add(middleCell);

                    panel.SetValue(Grid.RowProperty, r);
                    panel.SetValue(Grid.ColumnProperty, c);
                    grdMain.Children.Add(panel);
                }
            }

        }

        private void generateWallGrid()
        {

            float startingXHorizontals = XHorizontalStart;
            float startingYHorizontals = YHorizontalStart;
            float startingZHorizontals;

            float startingXVerticals = XVerticalStart;
            float startingYVerticals = YVerticalStart;
            float startingZVerticals;

            for (int r = 0; r < wallRows; r++)
            {
                List<Wall> row = new List<Wall>();
                startingZHorizontals = ZHorizontalStart;
                startingZVerticals = ZVerticalStart;

                for (int c = 0; c < wallColumns; c++)
                {

                    Wall wall = new Wall
                    {
                        render = false
                    };

                    if (r == 0 || r % 2 == 0)
                    {
                        if (c != 0 && c % 2 != 0)
                        {
                            wall.render = true;
                            Element e = new Element
                            {
                                Index = 0
                            };
                            if (horizontalWide)
                            {
                                e.ObjectID = "ShoothouseBarrierWall";
                            }
                            else
                            {
                                e.ObjectID = "ShoothouseBarrierWallNarrow";
                            }
                            e.Type = "object";

                            e.PosOffset = new Posoffset
                            {
                                x = startingXHorizontals,
                                y = startingYHorizontals,
                                z = startingZHorizontals
                            };

                            e.OrientationForward = new Orientationforward
                            {
                                x = 1,
                                y = 0,
                                z = 0
                            };

                            e.OrientationUp = new Orientationup
                            {
                                x = 0,
                                y = 1,
                                z = 0
                            };

                            e.ObjectAttachedTo = -1;
                            e.MountAttachedTo = -1;
                            e.LoadedRoundsInChambers = new List<string>();
                            e.LoadedRoundsInMag = new List<string>();
                            e.GenericInts = new List<string>();
                            e.GenericStrings = new List<string>();
                            e.GenericVector3s = new List<string>();
                            e.GenericRotations = new List<string>();
                            e.Flags = new Flags
                            {
                                _keys = new List<string>()
                            {
                                "IsKinematicLocked",
                                "IsPickupLocked",
                                "QuickBeltSpecialStateEngaged"
                            },

                                _values = new List<string>()
                            {
                                "True",
                                "True",
                                "False"
                            }
                            };

                            wall.element = e;
                            startingZHorizontals -= ZHorizontalOffset;
                        }

                    }
                    else
                    {

                        if (c == 0 || c % 2 == 0)
                        {
                            wall.render = true;
                            Element e = new Element
                            {
                                Index = 0
                            };
                            if (verticalWide)
                            {
                                e.ObjectID = "ShoothouseBarrierWall";
                            }
                            else
                            {
                                e.ObjectID = "ShoothouseBarrierWallNarrow";
                            }
                            e.Type = "object";

                            e.PosOffset = new Posoffset
                            {
                                x = startingXVerticals,
                                y = startingYVerticals,
                                z = startingZVerticals
                            };

                            e.OrientationForward = new Orientationforward
                            {
                                x = 0,
                                y = 0,
                                z = 1
                            };

                            e.OrientationUp = new Orientationup
                            {
                                x = 0,
                                y = 1,
                                z = 0
                            };

                            e.ObjectAttachedTo = -1;
                            e.MountAttachedTo = -1;
                            e.LoadedRoundsInChambers = new List<string>();
                            e.LoadedRoundsInMag = new List<string>();
                            e.GenericInts = new List<string>();
                            e.GenericStrings = new List<string>();
                            e.GenericVector3s = new List<string>();
                            e.GenericRotations = new List<string>();
                            e.Flags = new Flags
                            {
                                _keys = new List<string>()
                            {
                                "IsKinematicLocked",
                                "IsPickupLocked",
                                "QuickBeltSpecialStateEngaged"
                            },

                                _values = new List<string>()
                            {
                                "True",
                                "True",
                                "False"
                            }
                            };
                            startingZVerticals -= ZVerticalOffset;

                            wall.element = e;

                        }
                    }


                    row.Add(wall);
                }
                if (r == 0 || r % 2 == 0)
                {
                    startingXHorizontals -= XHorizontalOffset;
                }
                else
                {
                    startingXVerticals -= XVerticalOffset;
                }

                maze.Add(row);
            }

        }

        private void generateJson()
        {
            generatedOutput.FileName = mapName;
            generatedOutput.ReferencePath = @"Vault\SceneConfigs\gp_hangar\" + mapName + "gp_hangar_VFS.json";
            generatedOutput.Creator = "picklesDoggo";
            generatedOutput.ModsUsed = new List<string>();
            generatedOutput.Objects = new List<Object>();

            int index = 0;
          
            for (int i = 0; i < maze.Count; i++)
            {

                for (int ii = 0; ii < maze[i].Count; ii++)
                {
                    if (maze[i][ii].render)
                    {
                        Object rowObject = new Object
                        {
                            IsContainedIn = -1,
                            QuickbeltSlotIndex = -1,
                            InSlotOfRootObjectIndex = -1,
                            InSlotOfElementIndex = -1,
                            Elements = new List<Element>(),
                            Index = index
                        };
                        rowObject.Elements.Add(maze[i][ii].element);
                        generatedOutput.Objects.Add(rowObject);
                        index++;
                    }
                }
            }



            string jsonUpdated = JsonConvert.SerializeObject(generatedOutput, Formatting.Indented);
            File.WriteAllText(mapName + "_gp_hangar_VFS.json", jsonUpdated);
        }

        private void fillRectangle(Rectangle selected)
        {
            Color selectedColor = ((SolidColorBrush)selected.Fill).Color;

            // find the name of the piece
            List<string> split = selected.Name.Split('_').ToList();
            int r = Convert.ToInt32(split[1]);
            int c = Convert.ToInt32(split[2]);

            // Was a vertical clicked?
            if (selected.Name.Contains("Vertical"))
            {
                // is Vertical a wide piece?
                if (verticalWide)
                {
                    // determine selected radio button
                    if (btnVerticalNone.IsChecked == true)
                    {
                        maze[r][c].render = false;
                        selected.Fill = btnVerticalNone.Foreground;
                    }
                    else
                    {
                        maze[r][c].render = true;

                        if (btnVerticalSingleDoor.IsChecked == true)
                        {
                            selected.Fill = btnVerticalSingleDoor.Foreground;
                            maze[r][c].element.ObjectID = "ShoothouseBarrierDoorSingle";
                        }
                        else if (btnVerticalDoubleDoor.IsChecked == true)
                        {
                            selected.Fill = btnVerticalDoubleDoor.Foreground;
                            maze[r][c].element.ObjectID = "ShoothouseBarrierDoorDouble";
                        }
                        else if (btnVerticalWindow.IsChecked == true)
                        {
                            selected.Fill = btnVerticalWindow.Foreground;
                            maze[r][c].element.ObjectID = "ShoothouseBarrierWindowNarrow";
                        }
                        else if (btnVerticalWall.IsChecked == true)
                        {
                            selected.Fill = btnVerticalWall.Foreground;
                            maze[r][c].element.ObjectID = "ShoothouseBarrierWall";
                        }
                        else if (btnVerticalCompBarrierLow.IsChecked == true)
                        {
                            selected.Fill = btnVerticalCompBarrierLow.Foreground;
                            maze[r][c].element.ObjectID = "CompBarrierLow";
                        }
                    }

                    selected.Width = verticalLineThickness;
                }
                else
                {
                    if (selectedColor == Colors.Gray)
                    {
                        selected.Fill = new SolidColorBrush(Colors.Black);
                        selected.Width = verticalLineThickness;
                        maze[r][c].render = true;
                        maze[r][c].element.ObjectID = "ShoothouseBarrierWallNarrow";
                    }
                    else
                    {
                        selected.Fill = new SolidColorBrush(Colors.Gray);
                        selected.Width = verticalLineThickness;
                        maze[r][c].render = false;
                    }

                }
            }
            if (selected.Name.Contains("Horizontal"))
            {
                // is horizontal a wide piece?
                if (horizontalWide)
                {
                    // determine selected radio button
                    if (btnHorizontalNone.IsChecked == true)
                    {
                        maze[r][c].render = false;
                        selected.Fill = btnHorizontalNone.Foreground;
                    }
                    else
                    {
                        maze[r][c].render = true;

                        if (btnHorizontalSingleDoor.IsChecked == true)
                        {
                            selected.Fill = btnHorizontalSingleDoor.Foreground;
                            maze[r][c].element.ObjectID = "ShoothouseBarrierDoorSingle";
                        }
                        else if (btnHorizontalDoubleDoor.IsChecked == true)
                        {
                            selected.Fill = btnHorizontalDoubleDoor.Foreground;
                            maze[r][c].element.ObjectID = "ShoothouseBarrierDoorDouble";
                        }
                        else if (btnHorizontalWindow.IsChecked == true)
                        {
                            selected.Fill = btnHorizontalWindow.Foreground;
                            maze[r][c].element.ObjectID = "ShoothouseBarrierWindowNarrow";
                        }
                        else if (btnHorizontalWall.IsChecked == true)
                        {
                            selected.Fill = btnHorizontalWall.Foreground;
                            maze[r][c].element.ObjectID = "ShoothouseBarrierWall";
                        }
                        else if (btnHorizontalCompBarrierLow.IsChecked == true)
                        {
                            selected.Fill = btnHorizontalCompBarrierLow.Foreground;
                            maze[r][c].element.ObjectID = "CompBarrierLow";
                        }
                    }

                    selected.Height = horizontalLineThickness;
                }
                else
                {
                    if (selectedColor == Colors.Gray)
                    {
                        selected.Fill = new SolidColorBrush(Colors.Black);
                        selected.Height = horizontalLineThickness;
                        maze[r][c].render = true;
                        maze[r][c].element.ObjectID = "ShoothouseBarrierWallNarrow";
                    }
                    else
                    {
                        selected.Fill = new SolidColorBrush(Colors.Gray);
                        selected.Height = horizontalLineThickness;
                        maze[r][c].render = false;
                    }

                }
            }
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

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            generateJson();
        }
    }
}
