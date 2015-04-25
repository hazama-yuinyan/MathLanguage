using System;
using System.Collections.Generic;
using System.Numerics;
using CalculatorCompetition.Backend.TypeSystem;

namespace CalculatorCompetition.Backend.Runtime.Instruction
{
    /// <summary>
    /// Represents the negate operation.
    /// </summary>
    public class NegateOperation : IInstruction
    {
        #region IInstruction implementation

        public IEnumerable<Variable> Operate(IList<Variable> arguments)
        {
            var arg = arguments[0];
            if(arg.IsInstructions)
                throw new InvalidOperationException("Can not negate a list of instructions!");

            switch(arg.TypeFlag){
            case VariableType.Integer:
                int tmp = -arg.AsInteger;
                return new []{new Variable(tmp)};

            case VariableType.Double:
                double tmp2 = -arg.AsDouble;
                return new []{new Variable(tmp2)};

            case VariableType.BigInteger:
                BigInteger tmp3 = -arg.AsBigint;
                return new []{new Variable(tmp3)};

            case VariableType.Decimal:
                decimal tmp4 = -arg.AsDecimal;
                return new []{new Variable(tmp4)};

            default:
                if(arg.IsVector){
                    if(arg.IsInteger){
                        Vector<int> tmp5 = -arg.AsIntVector;
                        return new []{new Variable(tmp5)};
                    }else if(arg.IsDouble){
                        Vector<double> tmp6 = -arg.AsDoubleVector;
                        return new []{new Variable(tmp6)};
                    }
                }else if(arg.IsMatrix){
                    if(arg.IsInteger){
                        Matrix<int> tmp7 = -arg.AsIntMatrix;
                        return new []{new Variable(tmp7)};
                    }else if(arg.IsDouble){
                        Matrix<double> tmp8 = -arg.AsDoubleMatrix;
                        return new []{new Variable(tmp8)};
                    }
                }
                break;
            }

            return null;
        }

        public int NumberOfConsumingArguments {
            get {
                return 1;
            }
        }

        #endregion
    }
}

