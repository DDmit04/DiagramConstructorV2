using System;
using System.Collections.Generic;

namespace DiagramConsructorV2.src.data
{
    public class Method
    {
        public String methodSignature { get; set; }
        public List<Node> methodNodes { get; }

        public Method(String methodSignature, List<Node> methodNodes)
        {
            this.methodSignature = methodSignature;
            this.methodNodes = methodNodes;
        }
    }
}
