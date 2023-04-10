using MazeMakerUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.AccessControl;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace MazeMaker
{
    /// <summary>
    /// Interaction logic for EditWindow.xaml
    /// </summary>
    public partial class EditWindow : Window
    {

        private bool _isSPDragInProg;
        private bool _isSPRotateInProg;

        private static Room selectedRoom;
        private static Posoffset top;
        private static Posoffset bottom;
        private static Posoffset left;
        private static Posoffset right;
        private static int rotate = 0;
        private static Point mousePostion;
        private static Point mousePositionPrevious;
        private static int targetIndex;
        private static List<Target> targets;

        private double defaultX = 50;
        private double defaultZ = 50;
        public EditWindow()
        {
            InitializeComponent();
        }

        public EditWindow(Room roomInput)
        {
            InitializeComponent();
            targets = new List<Target>();
            selectedRoom = roomInput;

            top = selectedRoom.top.element.PosOffset;
            bottom = selectedRoom.bottom.element.PosOffset;
            left = selectedRoom.left.element.PosOffset;
            right = selectedRoom.right.element.PosOffset;

            List<string> manualTargets = Target.getManualTargets();
            foreach (string t in manualTargets)
            {
                cmbTargets.Items.Add(t);
            }
            cmbTargets.SelectedIndex = 0;
        }

        private decimal getAdjustedX(double rawX)
        {
            double rawBottom = grdCanvas.ActualHeight;

            double percentX = rawX / rawBottom;
            decimal roomTopX = top.x;
            decimal roomBottomX = bottom.x;
            decimal difX = Math.Abs(roomTopX - roomBottomX);
            decimal adjustX = difX * (decimal)percentX;
            decimal adjustedX = roomTopX - adjustX;
            return adjustedX;
        }

        private decimal getAdjustedZ(double rawZ)
        {

            double rawLeft = grdCanvas.ActualWidth;

            double percentZ = rawZ / rawLeft;
            decimal roomLeftZ = left.z;
            decimal roomRightZ = right.z;
            decimal difZ = Math.Abs(roomLeftZ - roomRightZ);
            decimal adjustZ = difZ * (decimal)percentZ;
            decimal adjustedZ = roomLeftZ - adjustZ;
            return adjustedZ;
        }

        private void newTarget_Click(object sender, RoutedEventArgs e)
        {

            StackPanel sp = new StackPanel();
            sp.Orientation = Orientation.Horizontal;


            Rectangle newRec = new Rectangle();
            newRec.Height = 50;
            newRec.Width = 50;
            newRec.Fill = Brushes.Tan;



            Border border = new Border();
            border.Background = Brushes.Black;
            border.BorderBrush = Brushes.Black;
            border.BorderThickness = new Thickness(0, 3, 0, 0);
            border.Child = newRec;


            sp.Children.Add(border);
            sp.MouseLeftButtonDown += Sp_MouseLeftButtonDown;
            sp.MouseLeftButtonUp += Sp_MouseLeftButtonUp;
            sp.MouseMove += Sp_MouseMove;
            sp.MouseRightButtonDown += Sp_MouseRightButtonDown;
            sp.MouseRightButtonUp += Sp_MouseRightButtonUp;
            sp.Name = "sp_" + targetIndex;
            targetIndex++;

            Target newTarget = Target.getManualTarget((decimal)defaultX, (decimal)defaultZ, cmbTargets.SelectedItem.ToString());
            targets.Add(newTarget);

            canvas.Children.Add(sp);
            Canvas.SetLeft(sp, defaultZ);
            Canvas.SetTop(sp, defaultX);
        }

        private void Sp_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            _isSPRotateInProg = false;
        }

        private void Sp_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            _isSPRotateInProg = true;
        }

        private void Sp_MouseMove(object sender, MouseEventArgs e)
        {

            if (!_isSPDragInProg) return;

            // get the position of the mouse relative to the Canvas
            mousePostion = e.GetPosition(canvas);

            if (Mouse.RightButton == MouseButtonState.Pressed)
            {
                if (mousePostion.X > mousePositionPrevious.X)
                {
                    rotate++;
                }
                else
                {
                    rotate--;
                }
                RotateTransform rt = new RotateTransform(rotate);
                StackPanel selectedStackPanel = (StackPanel)sender;
                selectedStackPanel.RenderTransform = rt;
            }


            StackPanel selectedSP = (StackPanel)sender;

            // center the rect on the mouse
            double left = mousePostion.X - (selectedSP.ActualWidth / 2);
            double top = mousePostion.Y - (selectedSP.ActualHeight / 2);
            if (top >= grdCanvas.ActualHeight - (selectedSP.ActualHeight + newTarget.ActualHeight))
            {
                top = grdCanvas.ActualHeight - (selectedSP.ActualHeight + newTarget.ActualHeight);
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
            mousePositionPrevious = mousePostion;
        }

        private void Sp_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _isSPDragInProg = false;
            StackPanel selectedStackPanel = (StackPanel)sender;
            selectedStackPanel.ReleaseMouseCapture();

            // Get adjusted position          
            decimal adjustedX = getAdjustedX(Canvas.GetTop(selectedStackPanel) + newTarget.ActualHeight);
            decimal adjustedZ = getAdjustedZ(Canvas.GetLeft(selectedStackPanel));

            // Check to see if target exists
            List<string> stackPanelName = selectedStackPanel.Name.Split('_').ToList();

            int targetIndex = Convert.ToInt32(stackPanelName[1]);

            Target selectedTarget = targets[targetIndex];
            selectedTarget.element.PosOffset.x = adjustedX;
            selectedTarget.element.PosOffset.z = adjustedZ;


        }

        private void Sp_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _isSPDragInProg = true;
            StackPanel selectedStackPanel = (StackPanel)sender;
            selectedStackPanel.CaptureMouse();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            selectedRoom.targets = new List<Target>();
            foreach (Target target in targets)
            {
                selectedRoom.targets.Add(target);
            }
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.Left) || Keyboard.IsKeyDown(Key.Right))
            {
                
                bool overElement = false;
                foreach (var spChild in canvas.Children)
                {

                    StackPanel sp = (StackPanel)spChild;
                    // Are we over the stackPanel?
                    if (Mouse.DirectlyOver == sp)
                    {
                        
                    }
                    // Are we over the border
                    foreach (var borderChild in sp.Children)
                    {
                        Border border = (Border)borderChild;
                        if (Mouse.DirectlyOver == border)
                        {
                            
                            overElement = true;
                            break;
                        }
                        // Are we over the rectange
                        if (Mouse.DirectlyOver == border.Child)
                        {
                            
                            overElement = true;
                            break;
                        }
                    }

                    if (overElement)
                    {
                        RotateTransform rotation = sp.RenderTransform as RotateTransform;
                        if (rotation != null)
                        {
                            rotate = (int)rotation.Angle;
                        }
                        else
                        {
                            rotate = 0;
                        }
                        
                        if (Keyboard.IsKeyDown(Key.Left))
                        {
                            rotate--;
                            if (rotate < 0)
                            {
                                rotate = 359;
                            }
                            
                        }
                        if (Keyboard.IsKeyDown(Key.Right))
                        {
                            rotate++;
                            if (rotate >= 359)
                            {
                                rotate = 0;
                            }
                        }
                        RotateTransform rt = new RotateTransform(rotate);
                        sp.RenderTransform = rt;
                        rotation = sp.RenderTransform as RotateTransform;

                        // Check to see if target exists
                        List<string> stackPanelName = sp.Name.Split('_').ToList();

                        int targetIndex = Convert.ToInt32(stackPanelName[1]);

                        Target selectedTarget = targets[targetIndex];

                        double xRotate = Math.Sin((Math.PI / 180) * rotation.Angle);
                        double zRotate = Math.Cos((Math.PI / 180) * rotation.Angle);


                        selectedTarget.element.OrientationForward.x = (decimal)xRotate;
                        selectedTarget.element.OrientationForward.y = 0;
                        selectedTarget.element.OrientationForward.z = (decimal)zRotate;


                        var temp = rotation.Angle;
                        e.Handled = true;
                        break;
                    }

                }
            }
        }

    }
}
