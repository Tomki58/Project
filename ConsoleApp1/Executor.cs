using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchPredictionSim
{

    class Executor
    {


        private static string COMMENT_SYMB = ";";
        private static string LABEL_SYMB = ":";
        private static string HEX_DIGIT_HEADER = "h";
        private static string OCTO_DIGIT_HEADER = "q";
        private static string BYTE_DIGIT_HEADER = "b";
        private static string DECIMAL_DIGIT_HEADER = "d";

        private List<List<string>> asmCode;

        private static string[] JMP_OPS = new string[] { "je", "jne", "jg" };

        public Dictionary<string, int> labelDict { get; private set; } = new Dictionary<string, int>();

        public float eax = 0f;
        public float ebx = 0f;
        public float ecx = 0f;
        public float edx = 0f;
        public float cf = 0f;
        public float zf = 0f;

        private IBranchPredictor predictor;

        private Dictionary<string, Func<List<string>, Executor, bool>> cmdDict = new Dictionary<string, Func<List<string>, Executor, bool>>() {
            {"add", ASMFunctions.add },
            {"sub", ASMFunctions.sub },
            {"mul", ASMFunctions.mul },
            {"div", ASMFunctions.div },
            {"mov", ASMFunctions.mov },
            {"cmp", ASMFunctions.cmp },
        };

        public Executor(string[] asmCodeStr, IBranchPredictor predictor)
        {
            asmCode = ParseAsmCode(asmCodeStr);
            this.predictor = predictor;
            InitLabels();
        }

        private void InitLabels()
        {
            for (int i = 0; i < asmCode.Count; i++)
            {
                var cmd = asmCode[i];
                if (cmd[0].EndsWith(LABEL_SYMB))
                {
                    labelDict[cmd[0].Remove(cmd[0].Length - 1)] = i; //remove ':' and point to next line
                }
            }
        }

        public ref float operandToVar(string operand)
        {
            if (operand.Equals("EAX", StringComparison.CurrentCultureIgnoreCase))
                return ref eax;
            if (operand.Equals("EBX", StringComparison.CurrentCultureIgnoreCase))
                return ref ebx;
            if (operand.Equals("ECX", StringComparison.CurrentCultureIgnoreCase))
                return ref ecx;
            if (operand.Equals("EDX", StringComparison.CurrentCultureIgnoreCase))
                return ref edx;
            if (operand.Equals("CF", StringComparison.CurrentCultureIgnoreCase))
                return ref cf;
            if (operand.Equals("ZF", StringComparison.CurrentCultureIgnoreCase))
                return ref zf;
            throw new FormatException("чекай дауна такого регистра нет " + operand);
        }

        private static List<List<string>> ParseAsmCode(string[] asmCodeLines)
        {
            var splitCode = new List<List<string>>();
            ////split by line
            //var asmCodeSplit = asmCode.Split(
            //    new[] { "\r\n", "\r", "\n" },
            //    StringSplitOptions.RemoveEmptyEntries);

            //split each line by params
            foreach (var line in asmCodeLines)
            {
                if (line.StartsWith(COMMENT_SYMB))
                    continue;

                var lineSplit = line.Split(new[] { " ", ", ", "," },
                    StringSplitOptions.RemoveEmptyEntries);
                splitCode.Add(new List<string>(lineSplit));
            }

            return splitCode;
        }

        public void RunProgram()
        {
            
            for (int i = 0; i < asmCode.Count; i++)
            {
                
                var codeLine = asmCode[i];
                var op = codeLine[0].ToLower();

                Console.Write("\n" + String.Join(" ", codeLine));

                if (cmdDict.ContainsKey(op))
                {
                    var tempList = new List<string>(codeLine); // leave only operands
                    tempList.RemoveAt(0);
                    cmdDict[op](tempList, this);
                }
                else if (JMP_OPS.Contains(op))
                {
                    //real execution
                    bool execJump = true;
                    switch (op)
                    {
                        //check condition
                    }
                    if (execJump)
                    {

                        i = labelDict[codeLine[1]]; // set i to line num where label is
                    }
                    Console.Write(" Real jump: " + execJump);
                    //prediction
                    bool jumpPrediction = predictor.shouldJump(codeLine);
                    Console.Write(" Prediction jump: " + jumpPrediction);
                }
                else
                    Console.WriteLine("No such command " + codeLine[0]);
    //            Console.WriteLine(String.Join(" ", codeLine) +
    //$"\neax {eax}, ebx {ebx}, ecx {ecx}, edx {edx}" +
    //$"\ncf {cf}, zf {zf}");
            }
        }
    }
}


//public bool choseMethod(string operand)
//{
//    if (operand.Equals("EAX", StringComparison.CurrentCultureIgnoreCase))
//        return true;
//    if (operand.Equals("EBX", StringComparison.CurrentCultureIgnoreCase))
//        return true;
//    if (operand.Equals("ECX", StringComparison.CurrentCultureIgnoreCase))
//        return true;
//    if (operand.Equals("EDX", StringComparison.CurrentCultureIgnoreCase))
//        return true;
//    if (operand.Equals("CF", StringComparison.CurrentCultureIgnoreCase))
//        return true;
//    if (operand.Equals("ZF", StringComparison.CurrentCultureIgnoreCase))
//        return true;
//    return false;
//}

//public float operandToValue(string operand)
//{
//    if (operand.EndsWith(HEX_DIGIT_HEADER, StringComparison.CurrentCultureIgnoreCase))
//        return Convert.ToInt32(operand.Substring(0, operand.Length - 1), 16);
//    if (operand.EndsWith(OCTO_DIGIT_HEADER, StringComparison.CurrentCultureIgnoreCase))
//        return Convert.ToInt32(operand.Substring(0, operand.Length - 1), 8);
//    if (operand.EndsWith(BYTE_DIGIT_HEADER, StringComparison.CurrentCultureIgnoreCase))
//        return Convert.ToInt32(operand.Substring(0, operand.Length - 1), 2);
//    if (operand.EndsWith(DECIMAL_DIGIT_HEADER, StringComparison.CurrentCultureIgnoreCase))
//        return Convert.ToInt32(operand.Substring(0, operand.Length - 1), 10);
//    return Convert.ToInt32(operand);
//}