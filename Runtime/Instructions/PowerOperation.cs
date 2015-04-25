using System;
using CalculatorCompetition.Backend.Runtime.Instruction;
using System.Collections.Generic;
using System.Numerics;
using CalculatorCompetition.Backend.TypeSystem;

namespace CalculatorCompetition.Backend.Runtime
{
    /// <summary>
    /// Represents the exponentiation operation.
    /// </summary>
    public class PowerOperation : IInstruction
    {
        #region IInstruction implementation

        public IEnumerable<Variable> Operate(IList<Variable> arguments)
        {
            var x = arguments[0];
            if(x.IsVector || x.IsInstructions){
                throw new InvalidOperationException(
                    string.Format("Can not compute the exponentiation of type `{0}`.", x.TypeFlag)
                );
            }

            var y = arguments[1];
            if(y.IsIntMatrix && y.IsIntVector && y.IsDoubleMatrix && y.IsDoubleVector && y.IsInstructions){
                throw new InvalidOperationException(
                    string.Format("Can not compute {0} to the power of vector or matrix.", x.TypeFlag)
                );
            }

            if(x.IsMatrix){
                if(y.IsDouble || y.IsDecimal){
                    throw new InvalidOperationException(
                        string.Format("Can not calculate a matrix to the power of type `{0}`", y.TypeFlag)
                    );
                }

                if(x.IsInteger){
                    Matrix<int> a = x.AsIntMatrix;
                    // Retrieve y as integer because y as BigInteger is considered too large
                    // for an exponent
                    int n = (int)y.Value - 1;
                    for(; n > 0; --n)
                        a = a.DotProduct(a);

                    return new []{new Variable(a)};
                }else{
                    Matrix<double> a = x.AsDoubleMatrix;
                    // Retrieve y as integer because y as BigInteger is cosidered too large
                    // for an exponent
                    int n = (int)y.Value - 1;
                    for(; n > 0; --n)
                        a = a.DotProduct(a);

                    return new []{new Variable(a)};
                }
            }else if(x.IsInteger || x.IsDouble || x.IsDecimal){
                double tmp1 = (double)x.Value;
                double power = (double)y.Value;
                return new []{new Variable(Math.Pow(tmp1, power))};
            }else{
                BigInteger tmp3 = x.AsBigint;
                if(y.IsInteger || y.IsBigint){
                    int exponent = (int)y.Value;
                    return new []{new Variable(BigInteger.Pow(tmp3, exponent))};
                }else{
                    double x_double = (double)tmp3;
                    double exponent = (double)y.Value;
                    return new []{new Variable(Math.Pow(x_double, exponent))};
                }
            }
        }

        public int NumberOfConsumingArguments {
            get {
                return 2;
            }
        }

        #endregion
    }
}

