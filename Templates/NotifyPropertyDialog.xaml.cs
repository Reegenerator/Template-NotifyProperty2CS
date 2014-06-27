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
using EnvDTE80;

namespace Templates {
    /// <summary>
    /// Interaction logic for NotifyPropertyDialog.xaml
    /// </summary>
    public partial class NotifyPropertyDialog : Window {
        public NotifyPropertyDialog() {
            InitializeComponent();
        }

        public static void ShowDialog(CodeProperty2[] props)
        {
            var win = new NotifyPropertyDialog();
            win.PropertyList.ItemsSource = props;
        }
    }
}
