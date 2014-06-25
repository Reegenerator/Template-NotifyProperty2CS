namespace Templates {
    public partial class NotifyPropertyLibrary {
       

        public NotifyPropertyLibrary(string ns)
	    {
	        Namespace = ns;
	    }
		public const string DefaultClassName = "NotifyPropertyLibrary";
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
	


        public string Namespace{get;set;}
	

    }
}