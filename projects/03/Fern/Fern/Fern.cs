using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FernNamespace
{
    /*
     * this class draws a fractal fern when the constructor is called.
     */
    class Fern
    {
        /* 
         * Fern constructor erases screen and draws a fern
         * 
         * Size: number of 3-pixel segments of tendrils
         * Redux: how much smaller children clusters are compared to parents
         * Turnbias: how likely to turn right vs. left (0=always left, 0.5 = 50/50, 1.0 = always right)
         * canvas: the canvas that the fern will be drawn on
         */
        public Fern(double size, double angle, double turnbias, Canvas canvas)
        {
            Random random = new Random();
            canvas.Children.Clear(); // clear canvas
            Shapes(canvas); // generate the other shapes
            Tendril(random.Next(100, (int)(canvas.Width - 100)), (int)(canvas.Height-50), size, angle, turnbias, canvas);
            // randomize sprouting location
        }

        private void Tendril(double x1, double y1, double size, double angle, double turnbias, Canvas canvas)
        {
            Random random = new Random();
            double limit = (random.NextDouble() * 2 - 1) / 12; // set the limit of the 
                if (size >=2)
            {
                double x2 = x1 + size * Math.Cos(angle) + random.NextDouble()-0.5; // randomize the x destination to create a more realistic fern
                double y2 = y1 - size * Math.Sin(angle) + random.NextDouble()-0.5; // randomize the y destination to create a more realistic fern

                byte red = (byte)(100 + random.Next(minValue: 0, maxValue: 100) / 2);
                byte green = (byte)(220 - random.Next(minValue: 0, maxValue: 100) / 1.1);

                Line(x1, y1, x2, y2, red, green, 0, 1 + size / 80, canvas);
                Tendril(x2, y2, size / 1.9, angle + (Math.PI / 4) - turnbias, turnbias, canvas); //left
                Tendril(x2, y2, size / 1.9, angle - (Math.PI / 4) - turnbias, turnbias, canvas); //right
                Tendril(x2, y2, size / 1.3, angle - turnbias - limit, turnbias, canvas);         //final
            }
        }

        private void Shapes(Canvas canvas)
        {
            Ellipse(canvas.Width-20, 20, 20, 255, 255, 0, canvas);
            //Line(0, canvas.Height - 25, canvas.Width, canvas.Height - 25, 139, 69, 19, 50, canvas);
            Rectangle(canvas.Width, 0, 50, canvas.Width, 139, 69, 19, canvas);
            
        }

        /*
         * draw a red circle centered at (x,y), radius radius, with a black edge, onto canvas
         */
        private void Ellipse(double x, double y, double radius, byte r, byte g, byte b, Canvas canvas)
        {
            Ellipse myEllipse = new Ellipse();
            SolidColorBrush mySolidColorBrush = new SolidColorBrush
            {
                Color = Color.FromArgb(255, r, g, b)
            };
            myEllipse.Fill = mySolidColorBrush;
            myEllipse.StrokeThickness = 1;
            myEllipse.Stroke = Brushes.Black;
            myEllipse.HorizontalAlignment = HorizontalAlignment.Center;
            myEllipse.VerticalAlignment = VerticalAlignment.Center;
            myEllipse.Width = 2 * radius;
            myEllipse.Height = 2 * radius;
            myEllipse.SetCenter(x, y);
            canvas.Children.Add(myEllipse);
        }

        /*
         * draw a rectangle centered at (x,y), radius radius, with a black edge, onto canvas
         */
        private void Rectangle(double x, double y, double height, double width, byte r, byte g, byte b, Canvas canvas)
        {
            Rectangle myRectangle = new Rectangle();
            SolidColorBrush mySolidColorBrush = new SolidColorBrush
            {
                Color = Color.FromArgb(255, r, g, b)
            };
            myRectangle.StrokeThickness = 1;
            myRectangle.Stroke = Brushes.Black;
            myRectangle.HorizontalAlignment = HorizontalAlignment.Center;
            myRectangle.VerticalAlignment = VerticalAlignment.Center;
            myRectangle.Width = width;
            myRectangle.Height = height;
            Canvas.SetLeft(myRectangle, 0);
            Canvas.SetTop(myRectangle, canvas.Height - 50);
            canvas.Children.Add(myRectangle);
            myRectangle.Fill = mySolidColorBrush;
        }

        /*
         * draw a line segment (x1,y1) to (x2,y2) with given color, thickness on canvas
         */
        private void Line(double x1, double y1, double x2, double y2, byte r, byte g, byte b, double thickness, Canvas canvas)
        {
            Line myLine = new Line();
            SolidColorBrush mySolidColorBrush = new SolidColorBrush
            {
                Color = Color.FromArgb(255, r, g, b)
            };
            myLine.X1 = x1;
            myLine.Y1 = y1;
            myLine.X2 = x2;
            myLine.Y2 = y2;
            myLine.Stroke = mySolidColorBrush;
            myLine.VerticalAlignment = VerticalAlignment.Center;
            myLine.HorizontalAlignment = HorizontalAlignment.Left;
            myLine.StrokeThickness = thickness;
            canvas.Children.Add(myLine);
        }
    }
}

/*
 * this class is needed to enable us to set the center for an ellipse
 */
public static class EllipseX
{
    public static void SetCenter(this Ellipse ellipse, double X, double Y)
    {
        Canvas.SetTop(ellipse, Y - ellipse.Height / 2);
        Canvas.SetLeft(ellipse, X - ellipse.Width / 2);
    }
}

