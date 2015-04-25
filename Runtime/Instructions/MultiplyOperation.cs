using System;
using System.Collections.Generic;
using System.Numerics;
using CalculatorCompetition.Backend.TypeSystem;

namespace CalculatorCompetition.Backend.Runtime.Instruction
{
    /// <summary>
    /// Represents the multiplication operation.
    /// </summary>
    public class MultiplyOperation : IInstruction
    {
        #region IInstruction implementation

        public IEnumerable<Variable> Operate(IList<Variable> arguments)
        {
            var lhs = arguments[0];
            var rhs = arguments[1];

            if(lhs.IsIntVector || lhs.IsDoubleVector){
                if(!rhs.IsIntVector && !rhs.IsDoubleVector){
                    throw new InvalidOperationException(
                        string.Format("Can not multiply a vector by {0}", rhs.TypeFlag)
                    );
                }

                if(lhs.IsInteger){
                    Vector<int> lhs_vec = lhs.AsIntVector;
                    if(rhs.IsIntVector){
                        Vector<int> rhs_vec = rhs.AsIntVector;
                        return new []{new Variable(lhs_vec * rhs_vec)};
                    }else if(rhs.IsDoubleVector){
                        Vector<double> rhs_vec = rhs.AsDoubleVector;
                        return new []{new Variable(rhs_vec.CrossProduct(lhs_vec))};
                    }else if(rhs.IsInteger){
                        int rhs_tmp = rhs.AsInteger;
                        return new []{new Variable(lhs_vec * rhs_tmp)};
                    }else if(rhs.IsDouble){
                        double rhs_tmp = rhs.AsDouble;
                        return new []{new Variable(lhs_vec * rhs_tmp)};
                    }else{
                        throw new InvalidOperationException(
                            string.Format("Can not multiply a vector by {0}", rhs.TypeFlag)
                        );
                    }
                }else if(lhs.IsDouble){
                    Vector<double> lhs_vec = lhs.AsDoubleVector;
                    if(rhs.IsIntVector){
                        Vector<int> rhs_vec = rhs.AsIntVector;
                        return new []{new Variable(lhs_vec.CrossProduct(rhs_vec))};
                    }else if(rhs.IsDoubleVector){
                        Vector<double> rhs_vec = rhs.AsDoubleVector;
                        return new []{new Variable(lhs_vec * rhs_vec)};
                    }else if(rhs.IsInteger){
                        int rhs_tmp = rhs.AsInteger;
                        return new []{new Variable(lhs_vec * rhs_tmp)};
                    }else if(rhs.IsDouble){
                        double rhs_tmp = rhs.AsDouble;
                        return new []{new Variable(lhs_vec * rhs_tmp)};
                    }else{
                        throw new InvalidOperationException(
                            string.Format("Can not multiply a vector by {0}", rhs.TypeFlag)
                        );
                    }
                }
            }else if(lhs.IsIntMatrix || lhs.IsDoubleMatrix){
                if(!rhs.IsIntMatrix && !rhs.IsDoubleMatrix){
                    throw new InvalidOperationException(
                        string.Format("Can not multiply type {0} by a matrix", rhs.TypeFlag)
                    );
                }

                dynamic tmp_lhs = lhs.Value;
                dynamic tmp_rhs = rhs.Value;
                return new []{new Variable(tmp_lhs * tmp_rhs)};
            }else if(lhs.IsDouble){
                double tmp_lhs = lhs.AsDouble;
                if(rhs.IsVector){
                    if(rhs.IsInteger){
                        Vector<int> vec = rhs.AsIntVector;
                        return new []{new Variable(tmp_lhs * vec)};
                    }else{
                        Vector<double> vec = rhs.AsDoubleVector;
                        return new []{new Variable(tmp_lhs * vec)};
                    }
                }else if(rhs.IsMatrix){
                    if(rhs.IsInteger){
                        Matrix<int> matrix = rhs.AsIntMatrix;
                        return new []{new Variable(tmp_lhs * matrix)};
                    }else{
                        Matrix<double> matrix = rhs.AsDoubleMatrix;
                        return new []{new Variable(tmp_lhs * matrix)};
                    }
                }else if(rhs.IsDouble){
                    double tmp_rhs = rhs.AsDouble;
                    return new []{new Variable(tmp_lhs * tmp_rhs)};
                }else if(rhs.IsInteger){
                    int tmp_rhs = rhs.AsInteger;
                    return new []{new Variable(tmp_lhs * tmp_rhs)};
                }else if(rhs.IsBigint){
                    double tmp_rhs = (double)rhs.AsBigint;
                    return new []{new Variable(tmp_lhs * tmp_rhs)};
                }else if(rhs.IsDecimal){
                    decimal lhs_decimal = (decimal)tmp_lhs;
                    decimal tmp_rhs = rhs.AsDecimal;
                    return new []{new Variable(lhs_decimal * tmp_rhs)};
                }else{
                    throw new InvalidOperationException(
                        string.Format("Can not multiply a double by {0}!", rhs.TypeFlag)
                    );
                }
            }else if(lhs.IsInteger){
                int tmp_lhs = lhs.AsInteger;
                if(rhs.IsVector){
                    if(rhs.IsInteger){
                        Vector<int> vec = rhs.AsIntVector;
                        return new []{new Variable(tmp_lhs * vec)};
                    }else{
                        Vector<double> vec = rhs.AsDoubleVector;
                        return new []{new Variable(tmp_lhs * vec)};
                    }
                }else if(rhs.IsMatrix){
                    if(rhs.IsInteger){
                        Matrix<int> matrix = rhs.AsIntMatrix;
                        return new []{new Variable(tmp_lhs * matrix)};
                    }else{
                        Matrix<double> matrix = rhs.AsDoubleMatrix;
                        return new []{new Variable(tmp_lhs * matrix)};
                    }
                }else if(rhs.IsDouble){
                    double tmp_rhs = rhs.AsDouble;
                    return new []{new Variable(tmp_lhs * tmp_rhs)};
                }else if(rhs.IsInteger){
                    int tmp_rhs = rhs.AsInteger;
                    return new []{new Variable(tmp_lhs * tmp_rhs)};
                }else if(rhs.IsBigint){
                    BigInteger tmp_rhs = rhs.AsBigint;
                    return new []{new Variable(tmp_lhs * tmp_rhs)};
                }else if(rhs.IsDecimal){
                    decimal tmp_rhs = rhs.AsDecimal;
                    return new []{new Variable(tmp_lhs * tmp_rhs)};
                }else{
                    throw new InvalidOperationException(
                        string.Format("Can not multiply an int by {0}!", rhs.TypeFlag)
                    );
                }
            }else if(lhs.IsBigint){
                BigInteger tmp_lhs = lhs.AsBigint;
                if(rhs.IsDouble){
                    double lhs_double = (double)tmp_lhs;
                    double tmp_rhs = rhs.AsDouble;
                    return new []{new Variable(lhs_double * tmp_rhs)};
                }else if(rhs.IsInteger){
                    int tmp_rhs = rhs.AsInteger;
                    return new []{new Variable(tmp_lhs * tmp_rhs)};
                }else if(rhs.IsBigint){
                    BigInteger tmp_rhs = rhs.AsBigint;
                    return new []{new Variable(tmp_lhs * tmp_rhs)};
                }else if(rhs.IsDecimal){
                    decimal lhs_decimal = (decimal)tmp_lhs;
                    decimal tmp_rhs = rhs.AsDecimal;
                    return new []{new Variable(lhs_decimal * tmp_rhs)};
                }else{
                    throw new InvalidOperationException(
                        string.Format("Can not multiply a bigint by {0}!", rhs.TypeFlag)
                    );
                }
            }else if(lhs.IsDecimal){
                decimal tmp_lhs = lhs.AsDecimal;
                if(rhs.IsDouble){
                    decimal tmp_rhs = (decimal)rhs.AsDouble;
                    return new []{new Variable(tmp_lhs * tmp_rhs)};
                }else if(rhs.IsInteger){
                    int tmp_rhs = rhs.AsInteger;
                    return new []{new Variable(tmp_lhs * tmp_rhs)};
                }else if(rhs.IsBigint){
                    decimal tmp_rhs = (decimal)rhs.AsBigint;
                    return new []{new Variable(tmp_lhs * tmp_rhs)};
                }else if(rhs.IsDecimal){
                    decimal tmp_rhs = rhs.AsDecimal;
                    return new []{new Variable(tmp_lhs * tmp_rhs)};
                }else{
                    throw new InvalidOperationException(
                        string.Format("Can not multiply a decimal by {0}!", rhs.TypeFlag)
                    );
                }
            }else{
                throw new InvalidOperationException(
                    string.Format("Can not multiply type {0}!", lhs.TypeFlag)
                );
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

