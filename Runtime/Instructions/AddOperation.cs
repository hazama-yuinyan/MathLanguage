using System;
using System.Collections.Generic;
using System.Numerics;

namespace CalculatorCompetition.Backend.Runtime.Instruction
{
    /// <summary>
    /// Represents the addition operation.
    /// </summary>
    public class AddOperation : IInstruction
    {
        #region IInstruction implementation

        public IEnumerable<Variable> Operate(IList<Variable> arguments)
        {
            var lhs = arguments[0];
            var rhs = arguments[1];

            if(lhs.IsIntVector || lhs.IsDoubleVector){
                if(!rhs.IsIntVector && !rhs.IsDoubleVector){
                    throw new InvalidOperationException(
                        string.Format("Can not add a vector to {0}", rhs.TypeFlag)
                    );
                }

                if(lhs.IsInteger){
                    var lhs_vec = lhs.AsIntVector;
                    if(rhs.IsInteger){
                        var rhs_vec = rhs.AsIntVector;
                        return new []{new Variable(lhs_vec + rhs_vec)};
                    }else{
                        var rhs_vec = rhs.AsDoubleVector;
                        return new []{new Variable(rhs_vec.Add(lhs_vec))};
                    }
                }else{
                    var lhs_vec = lhs.AsDoubleVector;
                    if(rhs.IsInteger){
                        var rhs_vec = rhs.AsIntVector;
                        return new []{new Variable(lhs_vec.Add(rhs_vec))};
                    }else{
                        var rhs_vec = rhs.AsDoubleVector;
                        return new []{new Variable(lhs_vec + rhs_vec)};
                    }
                }
            }else if(lhs.IsIntMatrix || lhs.IsDoubleMatrix){
                if(!rhs.IsIntMatrix && !rhs.IsDoubleMatrix){
                    throw new InvalidOperationException(
                        string.Format("Can not add type {0} to a matrix", rhs.TypeFlag)
                    );
                }

                if(lhs.IsInteger){
                    var lhs_matrix = lhs.AsIntMatrix;
                    if(rhs.IsInteger){
                        var rhs_matrix = rhs.AsIntMatrix;
                        return new []{new Variable(lhs_matrix + rhs_matrix)};
                    }else{
                        var rhs_matrix = rhs.AsDoubleMatrix;
                        return new []{new Variable(rhs_matrix.Add(lhs_matrix))};
                    }
                }else{
                    var lhs_matrix = lhs.AsDoubleMatrix;
                    if(rhs.IsInteger){
                        var rhs_matrix = rhs.AsIntMatrix;
                        return new []{new Variable(lhs_matrix.Add(rhs_matrix))};
                    }else{
                        var rhs_matrix = rhs.AsDoubleMatrix;
                        return new []{new Variable(lhs_matrix + rhs_matrix)};
                    }
                }
            }else if(lhs.IsDouble){
                double tmp_lhs = lhs.AsDouble;
                if(rhs.IsDouble){
                    double tmp_rhs = rhs.AsDouble;
                    return new []{new Variable(tmp_lhs + tmp_rhs)};
                }else if(rhs.IsInteger){
                    int tmp_rhs = rhs.AsInteger;
                    return new []{new Variable(tmp_lhs + tmp_rhs)};
                }else if(rhs.IsBigint){
                    double tmp_rhs = (double)rhs.AsBigint;
                    return new []{new Variable(tmp_lhs + tmp_rhs)};
                }else if(rhs.IsDecimal){
                    decimal lhs_decimal = (decimal)tmp_lhs;
                    decimal tmp_rhs = rhs.AsDecimal;
                    return new []{new Variable(lhs_decimal + tmp_rhs)};
                }else{
                    throw new InvalidOperationException(
                        string.Format("Can not add {0} to double!", rhs.TypeFlag)
                    );
                }
            }else if(lhs.IsInteger){
                int tmp_lhs = lhs.AsInteger;
                if(rhs.IsDouble){
                    double tmp_rhs = rhs.AsDouble;
                    return new []{new Variable(tmp_lhs + tmp_rhs)};
                }else if(rhs.IsInteger){
                    int tmp_rhs = rhs.AsInteger;
                    return new []{new Variable(tmp_lhs + tmp_rhs)};
                }else if(rhs.IsBigint){
                    double tmp_rhs = (double)rhs.AsBigint;
                    return new []{new Variable(tmp_lhs + tmp_rhs)};
                }else if(rhs.IsDecimal){
                    decimal tmp_rhs = rhs.AsDecimal;
                    return new []{new Variable(tmp_lhs + tmp_rhs)};
                }else{
                    throw new InvalidOperationException(
                        string.Format("Can not add {0} to double!", rhs.TypeFlag)
                    );
                }
            }else if(lhs.IsBigint){
                BigInteger tmp_lhs = lhs.AsBigint;
                if(rhs.IsDouble){
                    double lhs_double = (double)tmp_lhs;
                    double tmp_rhs = rhs.AsDouble;
                    return new []{new Variable(lhs_double + tmp_rhs)};
                }else if(rhs.IsInteger){
                    int tmp_rhs = rhs.AsInteger;
                    return new []{new Variable(tmp_lhs + tmp_rhs)};
                }else if(rhs.IsBigint){
                    BigInteger tmp_rhs = rhs.AsBigint;
                    return new []{new Variable(tmp_lhs + tmp_rhs)};
                }else if(rhs.IsDecimal){
                    decimal lhs_decimal = (decimal)tmp_lhs;
                    decimal tmp_rhs = rhs.AsDecimal;
                    return new []{new Variable(lhs_decimal + tmp_rhs)};
                }else{
                    throw new InvalidOperationException(
                        string.Format("Can not add {0} to double!", rhs.TypeFlag)
                    );
                }
            }else if(lhs.IsDecimal){
                decimal tmp_lhs = lhs.AsDecimal;
                if(rhs.IsDouble){
                    decimal tmp_rhs = (decimal)rhs.AsDouble;
                    return new []{new Variable(tmp_lhs + tmp_rhs)};
                }else if(rhs.IsInteger){
                    int tmp_rhs = rhs.AsInteger;
                    return new []{new Variable(tmp_lhs + tmp_rhs)};
                }else if(rhs.IsBigint){
                    decimal tmp_rhs = (decimal)rhs.AsBigint;
                    return new []{new Variable(tmp_lhs + tmp_rhs)};
                }else if(rhs.IsDecimal){
                    decimal tmp_rhs = rhs.AsDecimal;
                    return new []{new Variable(tmp_lhs + tmp_rhs)};
                }else{
                    throw new InvalidOperationException(
                        string.Format("Can not add {0} to double!", rhs.TypeFlag)
                    );
                }
            }else{
                throw new InvalidOperationException(
                    string.Format("Can not add type {0}!", lhs.TypeFlag)
                );
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

