using System;
using Attributes;

namespace Test {
    //Uncomment the attribute below and save to trigger code generation
    //[NotifyPropertyOption]
    public class Person {

    
        /// <summary>
        /// Test Comment
        /// </summary>
        [NotifyPropertyOption(ExtraNotifications = "Name")]
        public String FirstName { get;set; }

        [NotifyPropertyOption(IsIgnored = true)]
        public string LastName {get;set;}

        private int _Age;

        [NotifyPropertyOption(ExtraNotifications = "AgeString")]
        public int Age
        {
            get { return _Age; }
            set
            {
                if (value > 0)
                {
                    //sample custom insert point
                    //Reegenerator{Template:"NotifyProperty" Type="InsertPoint"}
                    //Some other code here
                }
            }
        }

        public string Name {
            get { return string.Format("{0} {1}", FirstName, LastName); }
        }

   
        public String Address {get;set;}

        [NotifyPropertyOption(ExtraNotifications = "LastName")]
        public void ChangeLastName(string s) {
            //_LastName = s;
        }


        public string AgeString {
            get { return string.Format("{0} years old", Age); }
        }
    }



}