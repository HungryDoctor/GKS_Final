using System;
using System.Windows;

namespace GKS6
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static readonly DependencyProperty ScaleValueProperty = DependencyProperty.Register("ScaleValue", typeof(double), typeof(MainWindow),
            new UIPropertyMetadata(1.0, new PropertyChangedCallback(OnScaleValueChanged), new CoerceValueCallback(OnCoerceScaleValue)));

        private GKS6.Controller.Controller ctrl;


        public double ScaleValue
        {
            get
            {
                return (double)GetValue(ScaleValueProperty);
            }
            set
            {
                SetValue(ScaleValueProperty, value);
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            ctrl = new GKS6.Controller.Controller(this);

            this.Width = 920;
            this.Height = 696;
        }



        private void buttonPage0_Click(object sender, RoutedEventArgs e)
        {
            ctrl.GoToPage0();
        }

        private void buttonPage1_Click(object sender, RoutedEventArgs e)
        {
            ctrl.GoToPage1();
        }

        private void buttonPage2_Click(object sender, RoutedEventArgs e)
        {
            ctrl.GoToPage2();
        }

      

        private static object OnCoerceScaleValue(DependencyObject o, object value)
        {
            MainWindow mainWindow = o as MainWindow;
            if (mainWindow != null)
                return mainWindow.OnCoerceScaleValue((double)value);
            else
                return value;
        }

        private static void OnScaleValueChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            MainWindow mainWindow = o as MainWindow;
            if (mainWindow != null)
                mainWindow.OnScaleValueChanged((double)e.OldValue, (double)e.NewValue);
        }

        protected virtual double OnCoerceScaleValue(double value)
        {
            if (double.IsNaN(value))
                return 1.0f;

            value = Math.Max(0.1, value);
            return value;
        }

        protected virtual void OnScaleValueChanged(double oldValue, double newValue)
        {

        }

        private void MainGrid_SizeChanged(object sender, EventArgs e)
        {
            CalculateScale();
        }


        private void CalculateScale()
        {
            double yScale = ActualHeight / 760f;
            double xScale = ActualWidth / 1020f;
            double value = Math.Min(xScale, yScale);
            ScaleValue = (double)OnCoerceScaleValue(mainWindow1, value);
        }

    }
}