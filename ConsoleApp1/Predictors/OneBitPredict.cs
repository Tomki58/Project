using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//deprecated
namespace BranchPredictionSim.Predictors
{
    class OneBitPredict : IBranchPredictor
    {
        private bool lastJumpExecuted = false;
        public void notfyJump(int lineNum, bool jmpTaken)
        {
            lastJumpExecuted = jmpTaken;
        }

        public bool shouldJump(List<string> codeLine, int lineNumber, Executor executor)
        {
            return lastJumpExecuted;
        }
    }
}
