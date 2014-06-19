using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attributes;

namespace Test {

    class Person : Test.INotifier {
        #region INotifier Functions Reegenerator:{Template:"NotifyProperty",Cat:"INotifierFunctions",Regen:"Never",Date:"2014-06-19T15:09:08.7748992+08:00"}

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;


        void Test.INotifier.Notify(string propertyName) {
            if (PropertyChanged != null) {
                PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion



        #region test expanded by Reegenerator:{Template:"NotifyProperty",Regen:"Never",Date:"2014-06-19T15:10:39.8705408+08:00"}
        private System.String _test;


        public System.String test {
            get {
                return _test;
            }
            set {

                #region SetPropertyAndNotify test Reegenerator:{Template:"NotifyProperty",Regen:"Never",Date:"2014-06-19T15:10:43.0327125+08:00"}
                this.SetPropertyAndNotify(ref _test, value, "test");
                #endregion
 }
        }

        #endregion


        #region what expanded by Reegenerator:{Template:"NotifyProperty",Regen:"Never",Date:"2014-06-19T15:10:41.6886314+08:00"}
        private System.Int32 _what;


        public System.Int32 what {
            get {
                return _what;
            }
            set {

                #region SetPropertyAndNotify what Reegenerator:{Template:"NotifyProperty",Regen:"Never",Date:"2014-06-19T15:10:47.3886876+08:00"}
                this.SetPropertyAndNotify(ref _what, value, "what");
                #endregion
 }
        }

        #endregion



    }
}
