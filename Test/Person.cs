using System;
using Attributes;

namespace Test {
    //To trigger code generation, either right click anywhere inside the class and choose the Menu Reegenerator > Run Snippet
    //Or uncomment the attribute below, and attach the NotifyProperty template
    //[NotifyPropertyOption]
    public class Person {

    
        /// <summary>
        /// Test Comment
        /// </summary>
        [NotifyPropertyOption(ExtraNotifications = "Name")]
        public String FirstName { get;set; }

        [NotifyPropertyOption(IsIgnored = true)]
        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }

        private int _Age;
        private string _lastName;

        [NotifyPropertyOption(ExtraNotifications = "AgeString")]
        public int Age
        {
            get { return _Age; }
            set
            {
                if (value > 0)
                {
                    //sample custom insert point
                    //Reegenerator:{Template:"NotifyProperty", Type:"InsertPoint"}
                    //Some other code here
                    _Age = value;
                }
            }
        }

        public string Name {
            get { return string.Format("{0} {1}", FirstName, Age); }
        }

        public string Name2 {
            get { return string.Format("{0} years old", _Age); }
        }

        public String Address {get;set;}

        [NotifyPropertyOption(ExtraNotifications = "LastName")]
        public void ChangeLastName(string s) {
            LastName = s;
        }


        public string AgeString {
            get { return string.Format("{0} years old", _Age); }
        }
    }



}