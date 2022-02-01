using System.Collections.Generic;

namespace DiagramConstructorV3.app.utils
{
    public class LoopNameGenerator
    {
        protected char CurrentChar;
        protected int AdditionNum;

        public LoopNameGenerator()
        {
            CurrentChar = 'A';
            AdditionNum = 0;
        }

        public string GetNextName()
        {
            var res = CurrentChar.ToString();
            if (AdditionNum != 0)
            {
                res += AdditionNum.ToString();
            }
            CurrentChar++;
            if (CurrentChar == 'Z')
            {
                CurrentChar = 'A';
                AdditionNum++;
            }
            return res;
        }
    }
}