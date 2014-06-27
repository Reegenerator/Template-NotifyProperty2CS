using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test {
    public partial class PersonForm : Form {
        public PersonForm() {
            InitializeComponent();
            this.Load += PersonForm_Load;
           
        }

        void PersonBindingSource_ListChanged(object sender, ListChangedEventArgs e) {
            //throw new NotImplementedException();
        }

        void PersonBindingSource_CurrentItemChanged(object sender, EventArgs e) {
            //throw new NotImplementedException();
        }

        void PersonForm_Load(object sender, EventArgs e)
        {
            Init();
        }

        private Person Person;


        private void Init() {
            Person = new Person { FirstName = "Bill", LastName = "Gates", Address = "Earth", Age = 42 };
            PersonBindingSource.DataSource = Person;
        }

        private void incrementAgeButton_Click(object sender, EventArgs e) {
        }

        private void TextBox1_TextChanged(object sender, EventArgs e) {

        }

        private void button1_Click(object sender, EventArgs e) {
            Person.ChangeLastName(Person.LastName + "Changed");
        }

    }
}
