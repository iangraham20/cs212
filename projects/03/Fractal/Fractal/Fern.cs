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

namespace SquareNamespace
{
    /*
     * This class is a fern class which creates a fern-like plant.
     * It was modified to its current state by Matthew Nykamp.
     * Project last modified: 10/26/17
     * 
     * 
     */
    class Square
    {

        private static double THETA = Math.PI / 100;
        private static int NUMLEVELS = 0;
        //private static double RATIO = .8;

        public Square(double depth, double angle, double size, double redux, double R, double G, double B, Canvas canvas)
        {
            DrawSquare(depth, angle, size, redux, R, G, B, canvas);
        }

        public void DrawSquare(double depth, double angle, double size, double redux, double R, double G, double B, Canvas canvas)
        {
            DrawLine(-1 * size * Math.Sin(angle), size * Math.Cos(angle), -1 * size * Math.Cos(angle), -1 * size * Math.Sin(angle), R, G, B, canvas);
            DrawLine(-1 * size * Math.Sin(angle), size * Math.Cos(angle), size * Math.Cos(angle), size * Math.Sin(angle), R, G, B, canvas);
            DrawLine(size * Math.Sin(angle), -1 * size * Math.Cos(angle), -1 * size * Math.Cos(angle), -1 * size * Math.Sin(angle), R, G, B, canvas);
            DrawLine(size * Math.Sin(angle), -1 * size * Math.Cos(angle), size * Math.Cos(angle), size * Math.Sin(angle), R, G, B, canvas);
            if (depth>1)
                DrawSquare(depth-1, angle+THETA, size/redux, redux, R / 1.1, G / 1.1, B / 1.1, canvas);
        }
        
        private void DrawLine(double x1, double y1, double x2, double y2, double R, double G, double B, Canvas canvas)
        {
            Line myLine = new Line();
            SolidColorBrush mySolidColorBrush = new SolidColorBrush();
            mySolidColorBrush.Color = Color.FromArgb(255, (byte)R, (byte)G, (byte)B);
            myLine.X1 = x1 + canvas.Width/2;
            myLine.X2 = x2 + canvas.Width/2;
            myLine.Y1 = y1 + canvas.Height/2;
            myLine.Y2 = y2 + canvas.Height/2;
            myLine.Stroke = mySolidColorBrush;
            myLine.StrokeThickness = 2;
            canvas.Children.Add(myLine);
        }
}
/*
 * this class is needed to enable us to set the center for an ellipse (not built in?!)
 */
public static class EllipseX
{
    public static void SetCenter(this Ellipse ellipse, double X, double Y)
    {
        Canvas.SetTop(ellipse, Y - ellipse.Height / 2);
        Canvas.SetLeft(ellipse, X - ellipse.Width / 2);
    }
}
}

//    class Fern
//    {
//        private static int BERRYMIN = 10;
//        private static int TENDRILS = 4;
//        private static int TENDRILMIN = 10;
//        private static double DELTATHETA = 0.1;
//        private static double SEGLENGTH = 3.0;

//        /* 
//* Fern constructor erases screen and draws a fern
//* 
//* Size: number of 3-pixel segments of tendrils
//* Redux: how much smaller children clusters are compared to parents
//* Turnbias: how likely to turn right vs. left (0=always left, 0.5 = 50/50, 1.0 = always right)
//* canvas: the canvas that the fern will be drawn on
//*/
//        public Fern(double size, double redux, double turnbias, Canvas canvas)
//        {
//            canvas.Children.Clear();                                // delete old canvas contents
//            // draw a new fern at the center of the canvas with given parameters
//            Cluster((int)(canvas.Width / 2), (int)(canvas.Height / 2), size, redux, turnbias, canvas);
//        }

//        /*
//         * cluster draws a cluster at the given location and then draws a bunch of tendrils out in 
//         * regularly-spaced directions out of the cluster.
//         */
//        private void Cluster(int x, int y, double size, double redux, double turnbias, Canvas canvas)
//        {
//            for (int i = 0; i < TENDRILS; i++)
//            {
//                // compute the angle of the outgoing tendril
//                double theta = (2 * i - 1) * Math.PI / 4;
//                Tendril(x, y, size, redux, turnbias, theta, canvas);
//            }
//        }

//        /*
//         * tendril draws a tendril (a randomly-wavy line) in the given direction, for the given length, 
//         * and draws a cluster at the other end if the line is big enough.
//         */
//        private void Tendril(int x1, int y1, double size, double redux, double turnbias, double direction, Canvas canvas)
//        {
//            int x2 = x1, y2 = y1;
//            Random random = new Random();

//            for (int i = 0; i < size / 10; i++)
//            {
//                // direction += 0.05 * turnbias;
//                x1 = x2; y1 = y2;
//                x2 = x1 + (int)(SEGLENGTH * Math.Sin(direction));
//                y2 = y1 + (int)(SEGLENGTH * Math.Cos(direction));
//                byte red = (byte)(100 + size / 2);
//                byte green = (byte)(220 - size / 3);
//                if (size>120) red = 138; green = 108;
//                Line(x1, y1, x2, y2, red, green, 0, 1 + size / 80, canvas);
//                // Berry(x2, y2, 5, canvas);
//            }
//            if (size > TENDRILMIN)
//            {
//                Cluster(x2, y2, size / 4, redux, turnbias, canvas);
//            }
//        }

//        /*
//         * draw a red circle centered at (x,y), radius radius, with a black edge, onto canvas
//         */
//        private void Berry(int x, int y, double radius, Canvas canvas)
//        {
//            Ellipse myEllipse = new Ellipse();
//            SolidColorBrush mySolidColorBrush = new SolidColorBrush();
//            mySolidColorBrush.Color = Color.FromArgb(255, 255, 0, 0);
//            myEllipse.Fill = mySolidColorBrush;
//            myEllipse.StrokeThickness = 1;
//            myEllipse.Stroke = Brushes.Black;
//            myEllipse.HorizontalAlignment = HorizontalAlignment.Center;
//            myEllipse.VerticalAlignment = VerticalAlignment.Center;
//            myEllipse.Width = 2 * radius;
//            myEllipse.Height = 2 * radius;
//            myEllipse.SetCenter(x, y);
//            canvas.Children.Add(myEllipse);
//        }

//        /*
//         * draw a line segment (x1,y1) to (x2,y2) with given color, thickness on canvas
//         */
//        private void Line(int x1, int y1, int x2, int y2, byte r, byte g, byte b, double thickness, Canvas canvas)
//        {
//            Line myLine = new Line();
//            SolidColorBrush mySolidColorBrush = new SolidColorBrush();
//            mySolidColorBrush.Color = Color.FromArgb(255, r, g, b);
//            myLine.X1 = x1;
//            myLine.Y1 = y1;
//            myLine.X2 = x2;
//            myLine.Y2 = y2;
//            myLine.Stroke = mySolidColorBrush;
//            myLine.VerticalAlignment = VerticalAlignment.Center;
//            myLine.HorizontalAlignment = HorizontalAlignment.Left;
//            myLine.StrokeThickness = thickness;
//            canvas.Children.Add(myLine);
//        }
//    }
//}
///*
// * this class is needed to enable us to set the center for an ellipse (not built in?!)
// */
//public static class EllipseX
//{
//    public static void SetCenter(this Ellipse ellipse, double X, double Y)
//    {
//        Canvas.SetTop(ellipse, Y - ellipse.Height / 2);
//        Canvas.SetLeft(ellipse, X - ellipse.Width / 2);
//    }
//}
//
