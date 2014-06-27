using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace WpfTest {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        private Person _person;
        public MainWindow() {
            InitializeComponent();
            DataContext = _person = new Person { FirstName = "Bill", LastName = "Gates", Address = "Earth", Age = 42 };
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _person.ChangeLastName(DateTime.Now.ToString(CultureInfo.InvariantCulture));
        }
    }
}
