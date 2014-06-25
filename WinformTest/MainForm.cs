//Formerly VB project-level imports:
using System;

namespace WinformTest
{
	public partial class MainForm
	{
		internal MainForm()
		{
			InitializeComponent();
		}

		private Person Person;
		private void MainForm_Load(object sender, EventArgs e)
		{
			Init();
		}

		private void Init()
		{
			Person = new Person {FirstName = "Bill", LastName = "Gates", Address = "Earth", Age = 42};
			PersonBindingSource.DataSource = Person;
		}

		private void incrementAgeButton_Click(object sender, EventArgs e)
		{
			Person.ChangeLastName(Person.LastName + "Changed");
		}

		private static MainForm _DefaultInstance;
		public static MainForm DefaultInstance
		{
			get
			{
				if (_DefaultInstance == null)
					_DefaultInstance = new MainForm();

				return _DefaultInstance;
			}
		}

        private void TextBox5_TextChanged(object sender, EventArgs e) {

        }
	}

}