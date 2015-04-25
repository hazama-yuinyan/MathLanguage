using System;
using System.Collections.Generic;
using System.Numerics;

namespace CalculatorCompetition.Backend.Runtime.Instruction
{
    /// <summary>
    /// Represents the factorial calculation operation.
    /// </summary>
    public class CalculateFactorialOperation : IInstruction
    {
        public CalculateFactorialOperation()
        {
        }

        #region IInstruction implementation

        public IEnumerable<Variable> Operate(IList<Variable> arguments)
        {
            var arg = arguments[0];

            if(!arg.IsInteger && !arg.IsBigint){
                throw new InvalidOperationException(
                    string.Format("Can not calculate the factorial on type {0}!", arg.TypeFlag)
                );
            }

            if(arg.IsInteger){
                int tmp = arg.AsInteger;
                tmp = BasicOperationHelper.DoFactorial(tmp);
                arg.AsInteger = tmp;
            }else{
                BigInteger tmp = arg.AsBigint;
                tmp = BasicOperationHelper.DoFactorial(tmp);
                arg.AsBigint = tmp;
            }

            return new []{arg};
        }

        public int NumberOfConsumingArguments {
            get {
                return 1;
            }
        }

        #endregion
    }
}

