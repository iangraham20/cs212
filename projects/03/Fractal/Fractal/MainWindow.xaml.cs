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
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Square s = new Square(depthSlider.Value, 0, sizeSlider.Value, reduxSlider.Value, redSlider.Value, greenSlider.Value, blueSlider.Value, canvas);
        }


        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            canvas.Children.Clear();
            Square s = new Square(depthSlider.Value, 0, sizeSlider.Value, reduxSlider.Value, redSlider.Value, greenSlider.Value, blueSlider.Value, canvas);
        }
    }
}
