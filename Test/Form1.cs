using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using Newtonsoft.Json;
//using RgenLib.TaggedSegment;

namespace Test {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
            //var serializer = new JsonSerializer { NullValueHandling = NullValueHandling.Ignore, ContractResolver = new RgenLib.TaggedSegment.Json.OrderedContractResolver(p=> p.PropertyName)};
            //var stringWriter = new StringWriter();
            //var writer = new JsonTextWriter(stringWriter) { QuoteName = false };
            //serializer.Serialize(writer, new Manager<Templates.NotifyProperty,Attributes.NotifyPropertyOptionAttribute>.OptionTag());
            //writer.Close();
            //var json = stringWriter.ToString();
        }
    }
}
