using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagramConsructorV2.fileTypes
{
    class CppLanguage : Language
    {

        public CppLanguage() : base("C++", new List<String>() { ".cpp", ".h"}, "C++ file") {}

    }
}
