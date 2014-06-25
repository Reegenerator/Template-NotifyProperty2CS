//Formerly VB project-level imports:
using System;
using Test;

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


	}

}