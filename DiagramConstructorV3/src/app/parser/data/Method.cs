using System.Collections.Generic;

namespace DiagramConstructorV3.app.parser.data
{
    public class Method
    {
        public string MethodSignature { get; }
        public List<Node> MethodNodes { get; }

        public Method(string methodSignature, List<Node> methodNodes)
        {
            MethodSignature = methodSignature;
            MethodNodes = methodNodes;
        }
    }
}