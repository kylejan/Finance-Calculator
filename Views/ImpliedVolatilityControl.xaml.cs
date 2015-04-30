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

using Financer.Utilities;

namespace Financer.Views
{
    /// <summary>
    /// Interaction logic for ImpliedVolatilityControl.xaml
    /// </summary>
    public partial class ImpliedVolatilityControl : UserControl
    {
        public ImpliedVolatilityControl()
        {
            InitializeComponent();
        }

        private void FilterTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Utility.IsTextAllowed((sender as TextBox).Text, e.Text);
        }
    }
}
