using System;
using Attributes;

namespace Test
{

    public class Cat : Test.INotifier 
	{
        #region INotifier Functions	<Gen Renderer='NotifyProperty' Date='2014-06-25T15:24' Trig_Type='CodeSnippet' Cat='INotifierFunctions' Regen='Never' />
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        void Test.INotifier.Notify(string propertyName) {
            if (PropertyChanged != null) PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }
        #endregion
        private System.Boolean _PlaysPiano;
        public System.Boolean PlaysPiano {
            get {
                return _PlaysPiano;
            }
            set {
                //<Gen Renderer='NotifyProperty' Type='InsertPoint'/>
                #region SetPropertyAndNotify PlaysPiano	<Gen Renderer='NotifyProperty' Date='2014-06-25T20:32' Trig_Type='CodeSnippet' Regen='Never' Type='Generated' />
                this.SetPropertyAndNotify(ref _PlaysPiano, value, "PlaysPiano");
                #endregion
                //Test
            }
        }


        private int _MaxLife = 9;
		public int MaxLife
		{
			get
			{
              
				return _MaxLife;
			}
		}

		[NotifyPropertyOption(ExtraNotifications="MaxLife")]
		public void Kill()
		{

			_MaxLife -= 1;

            #region <Gen Renderer='NotifyProperty' Date='2014-06-25T20:32' Trig_Type='CodeSnippet' Regen='Never' Type='Generated' />
            this.NotifyChanged("MaxLife");
            #endregion
        }
	}

}