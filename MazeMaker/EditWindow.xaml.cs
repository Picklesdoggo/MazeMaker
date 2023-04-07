using MazeMakerUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MazeMaker
{
    /// <summary>
    /// Interaction logic for EditWindow.xaml
    /// </summary>
    public partial class EditWindow : Window
    {
        
        private bool _isSPDragInProg;
        private static int selectedRow;
        private static int selectedColumn;
        private static Room selectedRoom;
        private static Posoffset top;
        private static Posoffset bottom;
        private static Posoffset left;
        private static Posoffset right;
        private static int rotate = 0;
        private static StackPanel rotateStackPanel;
        public EditWindow()
        {
            InitializeComponent();
        }

        public EditWindow(List<List<Room>> maze, int r, int c)
        {
            InitializeComponent();
            selectedRow = r;
            selectedColumn = c;
            selectedRoom = maze[selectedRow][selectedColumn];

            top = selectedRoom.top.element.PosOffset;
            bottom = selectedRoom.bottom.element.PosOffset;
            left = selectedRoom.left.element.PosOffset;
            right = selectedRoom.right.element.PosOffset;
        }

        private void newRec_Click(object sender, RoutedEventArgs e)
        {

            StackPanel sp = new StackPanel();
            sp.Orientation = Orientation.Horizontal;


            Rectangle newRec = new Rectangle();
            newRec.Height = 50;
            newRec.Width = 50;
            newRec.Fill = Brushes.Blue;
           
            

            Border border = new Border();
            border.Background = Brushes.Black;
            border.BorderBrush = Brushes.Black;
            border.BorderThickness = new Thickness(0, 3, 0, 0);
            border.Child = newRec;


            sp.Children.Add(border);
            sp.MouseLeftButtonDown += Sp_MouseLeftButtonDown;
            sp.MouseLeftButtonUp += Sp_MouseLeftButtonUp;
            sp.MouseMove += Sp_MouseMove;
            


            canvas.Children.Add(sp);
            Canvas.SetLeft(sp, 50);
            Canvas.SetTop(sp, 50);
        }

      

        private void Sp_MouseMove(object sender, MouseEventArgs e)
        {

            if (Mouse.RightButton == MouseButtonState.Pressed)
            {
                rotate++;
                RotateTransform rt = new RotateTransform(rotate);
                StackPanel selectedStackPanel = (StackPanel)sender;
                selectedStackPanel.RenderTransform = rt;
            }

            if (!_isSPDragInProg) return;

            // get the position of the mouse relative to the Canvas
            var mousePos = e.GetPosition(canvas);

            StackPanel selectedSP = (StackPanel)sender;

            // center the rect on the mouse
            double left = mousePos.X - (selectedSP.ActualWidth / 2);
            double top = mousePos.Y - (selectedSP.ActualHeight / 2);
            if (top >= grdCanvas.ActualHeight - (selectedSP.ActualHeight + newRec.ActualHeight))
            {
                top = grdCanvas.ActualHeight - (selectedSP.ActualHeight + newRec.ActualHeight);
            }
            if (top < 0)
            {
                top = 0;
            }

            if (left < 0)
            {
                left = 0;
            }
            if (left >= grdCanvas.ActualWidth - (selectedSP.ActualWidth))
            {
                left = grdCanvas.ActualWidth - (selectedSP.ActualWidth);
            }
            Canvas.SetLeft(selectedSP, left);
            Canvas.SetTop(selectedSP, top);
        }

        private void Sp_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _isSPDragInProg = false;
            StackPanel selectedStackPanel = (StackPanel)sender;
            selectedStackPanel.ReleaseMouseCapture();

            // Get adjusted position y =mx +b

            double rawX = Canvas.GetTop(selectedStackPanel) + newRec.ActualHeight;
            double rawBottom = grdCanvas.ActualHeight;

            double percentX = rawX / rawBottom;
            decimal roomTopX = top.x;
            decimal roomBottomX = bottom.x;
            decimal difX = Math.Abs(roomTopX - roomBottomX);
            decimal adjustX = difX * (decimal)percentX;
            decimal adjustedX = roomTopX - adjustX;

            double rawZ = Canvas.GetLeft(selectedStackPanel);
            double rawLeft = grdCanvas.ActualWidth;

            double percentZ = rawZ / rawLeft;
            decimal roomLeftZ = left.z;
            decimal roomRightZ = right.z;
            decimal difZ = Math.Abs(roomLeftZ - roomRightZ);
            decimal adjustZ = difZ * (decimal)percentZ;
            decimal adjustedZ = roomLeftZ - adjustZ;

            Target target = new Target();
            Element element = new Element();
            element.PosOffset = new Posoffset()
            {
                x = adjustedX,
                y = 0,
                z = adjustedZ
            };

            element.OrientationForward = new Orientationforward()
            {
                x = -1,
                y = 0,
                z = 0
            };

            element.ObjectID = "StandingSteelIPSCSimpleRed";

            element.Flags = new Flags()
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

            target.element = element;
            if (selectedRoom.targets == null)
            {
                selectedRoom.targets = new List<Target>();
            }

            selectedRoom.targets.Add(target);
        }

        private void Sp_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _isSPDragInProg = true;
            StackPanel selectedStackPanel = (StackPanel)sender;
            selectedStackPanel.CaptureMouse();            
        }
    }
}
