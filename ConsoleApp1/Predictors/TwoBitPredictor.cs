using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//deprecated
namespace BranchPredictionSim.Predictors
{
    class TwoBitPredictor : IBranchPredictor
    {
        public void notfyJump(int lineNum, bool jmpTaken)
        {
            throw new NotImplementedException();
        }

        public bool shouldJump(List<string> codeLine, int lineNumber, Executor executor)
        {
            throw new NotImplementedException();
        }
    }
}
