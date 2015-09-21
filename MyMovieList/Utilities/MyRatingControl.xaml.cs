using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Interaction logic for MyRatingControl.xaml
    /// </summary>
    public partial class MyRatingControl: UserControl
    {

        #region IsEnabled
        /// <summary>
        /// IsEnab;ed Dependency Property
        /// </summary>
        public static readonly DependencyProperty IsMyRatingEnabledProperty =
            DependencyProperty.Register("IsMyRatingEnabled", typeof(bool),
            typeof(MyRatingControl),
                new FrameworkPropertyMetadata((bool)false,
                    new PropertyChangedCallback(OnValueChanged)));

        /// <summary>
        /// Gets or sets the IsRatingEnabled property.  
        /// </summary>
        public bool IsMyRatingEnabled
        {
            get { return (bool)GetValue(IsMyRatingEnabledProperty); }
            set { SetValue(IsMyRatingEnabledProperty, value); }
        }

        /// <summary>
        /// Handles changes to the IsRatingEnabled property.
        /// </summary>
        private static void OnValueChanged(DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            System.Console.WriteLine("inside IsMyRatingEnabled onvaluechanged");
            return;
        }
        #endregion

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(int), typeof(MyRatingControl),
            new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(MyRatingControlChanged)));

        private int _max = 10;

        public int Value
        {
            get
            {
                return (int)GetValue(ValueProperty);
            }
            set
            {
                if (value < 0)
                {
                    SetValue(ValueProperty, 0);
                }
                else if (value > _max)
                {
                    SetValue(ValueProperty, _max);
                }
                else
                {
                    SetValue(ValueProperty, value);
                }
            }
        }

        private static void MyRatingControlChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            MyRatingControl item = sender as MyRatingControl;
            int newval = (int)e.NewValue;
            UIElementCollection childs = ((Grid)(item.Content)).Children;

            ToggleButton button = null;

            for (int i = 0; i < newval; i++)
            {
                button = childs[i] as ToggleButton;
                if (button != null)
                    button.IsChecked = true;
            }

            for (int i = newval; i < childs.Count; i++)
            {
                button = childs[i] as ToggleButton;
                if (button != null)
                    button.IsChecked = false;
            }

        }

        private void ClickEventHandler(object sender, RoutedEventArgs args)
        {
            ToggleButton button = sender as ToggleButton;
            int newvalue = int.Parse(button.Tag.ToString());
            Value = newvalue;
        }

        public MyRatingControl()
        {
            InitializeComponent();
        }
    }
}
