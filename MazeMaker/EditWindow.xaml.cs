using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MazeMaker
{
    /// <summary>
    /// Interaction logic for EditWindow.xaml
    /// </summary>
    public partial class EditWindow : Window
    {
        private bool _isRectDragInProg;
        public EditWindow()
        {
            InitializeComponent();
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
            if (top > 360)
            {
                top = 360;
            }
            if (top < 0)
            {
                top = 0;
            }

            if (left < 0)
            {
                left = 0;
            }
            if (left > 735)
            {
                left = 735;
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
