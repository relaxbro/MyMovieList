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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyMovieList.Utilities
{
    /// <summary>
    /// Interaction logic for RatingControl.xaml
    /// </summary>
    public partial class RatingControlOld : UserControl
    {
        private double MaxRatingIMDB = 10.0;

        public RatingControlOld()
        {
            InitializeComponent();
        }

        static DependencyProperty RateValueProperty = DependencyProperty.Register("RateValue", typeof(double), typeof(RatingControlOld), new PropertyMetadata(0.1, UpdateValue));

        private static void UpdateValue(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            RatingControlOld control = d as RatingControlOld;
            control.Value = (double)e.NewValue;
        }

        private double Value
        {
            get
            {
                return (double)GetValue(ValueProperty);
            }
            set
            { 
                SetValue(ValueProperty, value/MaxRatingIMDB);
            }
        }

        private static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(double), typeof(RatingControlOld), new PropertyMetadata());

        public double RateValue
        {
            get
            {
                return (double)GetValue(RateValueProperty);
            }
            set
            {
                SetValue(RateValueProperty, value);
            }
        }
    }
}
