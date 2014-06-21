using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attributes;

namespace Test {

    class Person : Test.INotifier {
        #region INotifier Functions Reegenerator:{Template:"NotifyProperty",Cat:"INotifierFunctions",Date:"2014-06-19T17:25:08.3788763+08:00"}

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;


        void Test.INotifier.Notify(string propertyName) {
            if (PropertyChanged != null) {
                PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion


        private System.String _Address;         /// <summary>
        /// Test
        /// </summary>
        [DefaultValue("")]
        public System.String Address {
            get {
                return _Address;
            }
            set {

                #region SetPropertyAndNotify Address Reegenerator:{Template:"NotifyProperty",Date:"2014-06-21T13:36:58.9486177+08:00"}
                this.SetPropertyAndNotify(ref _Address, value, "Address");
                #endregion
            }
        }



        [NotifyPropertyOption(IsIgnored=true)]
        public string Notes { get; set; }

        private System.Int32 _Value;
        public System.Int32 Value {
            get {
                return _Value;
            }
            set {

                #region SetPropertyAndNotify Value Reegenerator:{Template:"NotifyProperty",Date:"2014-06-21T13:36:58.9696184+08:00"}
                this.SetPropertyAndNotify(ref _Value, value, "Value");
                #endregion
            }
        }


        [NotifyPropertyOption(ExtraNotifications="Notes")]
        public void Test() {
            //Reegenerator:{ Template:"NotifyProperty",Type:"InsertPoint"}
            #region  Reegenerator:{Template:"NotifyProperty",Date:"2014-06-21T13:36:58.8896142+08:00"}
            this.NotifyChanged("Notes");
            #endregion
       
            Value = 1;

        
        }
    }
}
