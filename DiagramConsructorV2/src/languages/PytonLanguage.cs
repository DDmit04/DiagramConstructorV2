using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagramConsructorV2.fileTypes
{
    class PytonLanguage : Language
    {

        public PytonLanguage(): base("Pyton", new List<String>() { ".py" }, "Pyton file") { }
    }
}
