
using Attributes;
namespace WinformTest
{

    public class Cat 
	{

        public System.Boolean PlaysPiano { get; set; }

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

	}
	}

}