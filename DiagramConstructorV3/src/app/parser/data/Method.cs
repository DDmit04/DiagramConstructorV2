using System.Collections.Generic;

namespace DiagramConstructorV3.app.parser.data
{
    public class Method
    {
        public string MethodSignature { get; }
        public MethodType MethodType { get; }
        public List<Node> MethodNodes { get; }

        public Method(string methodSignature, List<Node> methodNodes, MethodType methodType = MethodType.COMMON)
        {
            MethodSignature = methodSignature;
            MethodNodes = methodNodes;
            MethodType = methodType;
        }
    }
}