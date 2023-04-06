using MazeMakerUtilities;
using System;
using System.Collections.Generic;
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
        private bool _isRectDragInProg;
        private static int selectedRow;
        private static int selectedColumn;
        private static Room selectedRoom;
        private static Posoffset top;
        private static Posoffset bottom;
        private static Posoffset left;
        private static Posoffset right;
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

        private void rect_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _isRectDragInProg = true;
            Rectangle selectedRectangle = (Rectangle)sender;
            selectedRectangle.CaptureMouse();
        }

        private void rect_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _isRectDragInProg = false;
            Rectangle selectedRectangle = (Rectangle)sender;
            selectedRectangle.ReleaseMouseCapture();

            // Get adjusted position y =mx +b

            double rawX = Canvas.GetTop(selectedRectangle) + newRec.ActualHeight;
            double rawBottom = grdCanvas.ActualHeight;

            double percentX = rawX / rawBottom;
            decimal roomTopX = top.x;
            decimal roomBottomX = bottom.x;
            decimal difX = Math.Abs(roomTopX - roomBottomX);
            decimal adjustX = difX * (decimal)percentX;
            decimal adjustedX = roomTopX - adjustX;

            double rawZ = Canvas.GetLeft(selectedRectangle);
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

        private void rect_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_isRectDragInProg) return;

            // get the position of the mouse relative to the Canvas
            var mousePos = e.GetPosition(canvas);

            Rectangle selectedRectangle = (Rectangle)sender;

            // center the rect on the mouse
            double left = mousePos.X - (selectedRectangle.ActualWidth / 2);
            double top = mousePos.Y - (selectedRectangle.ActualHeight / 2);
            if (top >= grdCanvas.ActualHeight - (selectedRectangle.ActualHeight + newRec.ActualHeight))
            {
                top = grdCanvas.ActualHeight - (selectedRectangle.ActualHeight + newRec.ActualHeight);
            }
            if (top < 0)
            {
                top = 0;
            }

            if (left < 0)
            {
                left = 0;
            }
            if (left >= grdCanvas.ActualWidth - (selectedRectangle.ActualWidth))
            {
                left = grdCanvas.ActualWidth - (selectedRectangle.ActualWidth);
            }
            Canvas.SetLeft(selectedRectangle, left);
            Canvas.SetTop(selectedRectangle, top);
            
        }

        private void newRec_Click(object sender, RoutedEventArgs e)
        {
            Rectangle newRec = new Rectangle();
            newRec.Height = 50;
            newRec.Width = 50;
            newRec.Fill = Brushes.Blue;
            newRec.MouseLeftButtonDown += rect_MouseLeftButtonDown;
            newRec.MouseLeftButtonUp += rect_MouseLeftButtonUp;
            newRec.MouseMove += rect_MouseMove;

            canvas.Children.Add(newRec);
            Canvas.SetLeft(newRec, 0);
            Canvas.SetTop(newRec, 0);
        }
    }
}
