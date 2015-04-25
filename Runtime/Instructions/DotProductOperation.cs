using System;
using System.Collections.Generic;

namespace CalculatorCompetition.Backend.Runtime.Instruction
{
    /// <summary>
    /// Represents the dot product operation.
    /// </summary>
    public class DotProductOperation : IInstruction
    {
        #region IInstruction implementation

        public IEnumerable<Variable> Operate(IList<Variable> arguments)
        {
            var a = arguments[0];
            if(!a.IsVector && !a.IsMatrix){
                throw new InvalidOperationException(
                    string.Format("Can not calculate the dot product on type {0}", a.TypeFlag)
                );
            }

            var b = arguments[1];
            if(!b.IsVector && !b.IsMatrix){
                throw new InvalidOperationException(
                    string.Format("Can not calculate the dot product on type {0}", b.TypeFlag)
                );
            }

            if(a.IsVector){
                if(a.IsInteger){
                    var tmp_lhs = a.AsIntVector;
                    if(b.IsInteger){
                        var tmp_rhs = b.AsIntVector;
                        return new []{new Variable(tmp_lhs.DotProduct(tmp_rhs))};
                    }else if(b.IsDouble){
                        var tmp_rhs = b.AsDoubleVector;
                        return new []{new Variable(tmp_rhs.DotProduct(tmp_lhs))};
                    }
                }else if(a.IsDouble){
                    var tmp_lhs = a.AsDoubleVector;
                    if(b.IsInteger){
                        var tmp_rhs = b.AsIntVector;
                        return new []{new Variable(tmp_lhs.DotProduct(tmp_rhs))};
                    }else if(b.IsDouble){
                        var tmp_rhs = b.AsDoubleVector;
                        return new []{new Variable(tmp_lhs.DotProduct(tmp_rhs))};
                    }
                }
            }else if(a.IsMatrix){
                if(a.IsInteger){
                    var tmp_lhs = a.AsIntMatrix;
                    if(b.IsInteger){
                        var tmp_rhs = b.AsIntMatrix;
                        return new []{new Variable(tmp_lhs.DotProduct(tmp_rhs))};
                    }else if(b.IsDouble){
                        var tmp_rhs = b.AsDoubleMatrix;
                        return new []{new Variable(tmp_rhs.DotProduct(tmp_lhs.Convert<double>()))};
                    }
                }else if(a.IsDouble){
                    var tmp_lhs = a.AsDoubleMatrix;
                    if(b.IsInteger){
                        var tmp_rhs = b.AsIntMatrix;
                        return new []{new Variable(tmp_lhs.DotProduct(tmp_rhs.Convert<double>()))};
                    }else if(b.IsDouble){
                        var tmp_rhs = b.AsDoubleMatrix;
                        return new []{new Variable(tmp_lhs.DotProduct(tmp_rhs))};
                    }
                }
            }

            return null;
        }

        public int NumberOfConsumingArguments {
            get {
                return 2;
            }
        }

        #endregion
    }
}

