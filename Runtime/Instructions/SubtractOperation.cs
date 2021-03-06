﻿using System;
using System.Collections.Generic;
using System.Numerics;

namespace CalculatorCompetition.Backend.Runtime.Instruction
{
    /// <summary>
    /// Represents the subtraction operation.
    /// </summary>
    public class SubtractOperation : IInstruction
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

                dynamic tmp_lhs = lhs.Value;
                dynamic tmp_rhs = rhs.Value;
                return new []{new Variable(tmp_lhs - tmp_rhs)};
            }else if(lhs.IsIntMatrix || lhs.IsDoubleMatrix){
                if(!rhs.IsIntMatrix && !rhs.IsDoubleMatrix){
                    throw new InvalidOperationException(
                        string.Format("Can not add type {0} to a matrix", rhs.TypeFlag)
                    );
                }

                dynamic tmp_lhs = lhs.Value;
                dynamic tmp_rhs = rhs.Value;
                return new []{new Variable(tmp_lhs - tmp_rhs)};
            }else if(lhs.IsDouble){
                double tmp_lhs = lhs.AsDouble;
                if(rhs.IsDouble){
                    double tmp_rhs = rhs.AsDouble;
                    return new []{new Variable(tmp_lhs - tmp_rhs)};
                }else if(rhs.IsInteger){
                    int tmp_rhs = rhs.AsInteger;
                    return new []{new Variable(tmp_lhs - tmp_rhs)};
                }else if(rhs.IsBigint){
                    double tmp_rhs = (double)rhs.AsBigint;
                    return new []{new Variable(tmp_lhs - tmp_rhs)};
                }else if(rhs.IsDecimal){
                    decimal lhs_decimal = (decimal)tmp_lhs;
                    decimal tmp_rhs = rhs.AsDecimal;
                    return new []{new Variable(lhs_decimal - tmp_rhs)};
                }else{
                    throw new InvalidOperationException(
                        string.Format("Can not add {0} to double!", rhs.TypeFlag)
                    );
                }
            }else if(lhs.IsInteger){
                int tmp_lhs = lhs.AsInteger;
                if(rhs.IsDouble){
                    double tmp_rhs = rhs.AsDouble;
                    return new []{new Variable(tmp_lhs - tmp_rhs)};
                }else if(rhs.IsInteger){
                    int tmp_rhs = rhs.AsInteger;
                    return new []{new Variable(tmp_lhs - tmp_rhs)};
                }else if(rhs.IsBigint){
                    double tmp_rhs = (double)rhs.AsBigint;
                    return new []{new Variable(tmp_lhs - tmp_rhs)};
                }else if(rhs.IsDecimal){
                    decimal tmp_rhs = rhs.AsDecimal;
                    return new []{new Variable(tmp_lhs - tmp_rhs)};
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
                    return new []{new Variable(lhs_double - tmp_rhs)};
                }else if(rhs.IsInteger){
                    int tmp_rhs = rhs.AsInteger;
                    return new []{new Variable(tmp_lhs - tmp_rhs)};
                }else if(rhs.IsBigint){
                    BigInteger tmp_rhs = rhs.AsBigint;
                    return new []{new Variable(tmp_lhs - tmp_rhs)};
                }else if(rhs.IsDecimal){
                    decimal lhs_decimal = (decimal)tmp_lhs;
                    decimal tmp_rhs = rhs.AsDecimal;
                    return new []{new Variable(lhs_decimal - tmp_rhs)};
                }else{
                    throw new InvalidOperationException(
                        string.Format("Can not add {0} to double!", rhs.TypeFlag)
                    );
                }
            }else if(lhs.IsDecimal){
                decimal tmp_lhs = lhs.AsDecimal;
                if(rhs.IsDouble){
                    decimal tmp_rhs = (decimal)rhs.AsDouble;
                    return new []{new Variable(tmp_lhs - tmp_rhs)};
                }else if(rhs.IsInteger){
                    int tmp_rhs = rhs.AsInteger;
                    return new []{new Variable(tmp_lhs - tmp_rhs)};
                }else if(rhs.IsBigint){
                    decimal tmp_rhs = (decimal)rhs.AsBigint;
                    return new []{new Variable(tmp_lhs - tmp_rhs)};
                }else if(rhs.IsDecimal){
                    decimal tmp_rhs = rhs.AsDecimal;
                    return new []{new Variable(tmp_lhs - tmp_rhs)};
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

