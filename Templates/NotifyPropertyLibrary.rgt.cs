using Kodeo.Reegenerator.Wrappers;

namespace Templates {
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using Kodeo.Reegenerator;

    public partial class NotifyPropertyLibrary {
       

        public NotifyPropertyLibrary(string ns)
	    {
	        Namespace = ns;
	    }
		public const string DefaultClassName = "NotifyPropertyChanged_Gen_Extensions";
		public bool IsNet45 {get; set;}
		private string _ClassName = DefaultClassName;
		public string ClassName
		{
			get
			{
				return _ClassName;
			}
			set
			{
				_ClassName = value;
			}
		}
		public const string INotifierName = "INotifier";
		//Public Property GeneratorTag As String
		public override void PreRender()
		{
			base.PreRender();

		}


        public string Namespace{get;set;}
	

    }
}