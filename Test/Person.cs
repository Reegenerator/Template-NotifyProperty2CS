using System;
using System.Collections.Generic;
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

        #region Name expanded by Reegenerator:{Template:"NotifyProperty",Date:"2014-06-19T17:25:08.4428653+08:00"}
        private System.String _Name;


        public System.String Name {
            get {
                return _Name;
            }
            set {

                #region SetPropertyAndNotify Name Reegenerator:{Template:"NotifyProperty",Date:"2014-06-19T17:25:08.5048666+08:00"}
                this.SetPropertyAndNotify(ref _Name, value, "Name");
                #endregion
            }
        }

        #endregion


        #region Age expanded by Reegenerator:{Template:"NotifyProperty",Date:"2014-06-19T17:25:08.4808652+08:00"}
        private System.Int32 _Age;

        [NotifyPropertyOption(ExtraNotifications = "Name")]
        public System.Int32 Age {
            get {
                return _Age;
            }
            set {

                #region SetPropertyAndNotify Age Reegenerator:{Template:"NotifyProperty",Date:"2014-06-19T17:25:08.5198677+08:00"}
                this.SetPropertyAndNotify(ref _Age, value, "Age");
                this.NotifyChanged("Name");
                #endregion
            }
        }

        #endregion


    }
}
