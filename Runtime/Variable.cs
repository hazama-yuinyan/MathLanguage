using System;
using System.Numerics;
using System.Collections.Generic;
using CalculatorCompetition.Backend.Runtime.Instruction;
using CalculatorCompetition.Backend.TypeSystem;

namespace CalculatorCompetition.Backend.Runtime
{
    /// <summary>
    /// Represents a variable.
    /// </summary>
    public class Variable
    {
        VariableType type_flag;
        dynamic value;

        public dynamic Value{
            get{return value;}
        }

        public VariableType TypeFlag{
            get{return type_flag;}
        }

        public int AsInteger{
            get{return (int)value;}
            set{this.value = value;}
        }

        public bool IsInteger{
            get{return (type_flag & VariableType.Integer) == VariableType.Integer;}
        }

        public double AsDouble{
            get{return (double)value;}
            set{this.value = value;}
        }

        public bool IsDouble{
            get{return (type_flag & VariableType.Double) == VariableType.Double;}
        }

        public decimal AsDecimal{
            get{return (decimal)value;}
            set{this.value = value;}
        }

        public bool IsDecimal{
            get{return (type_flag & VariableType.Decimal) == VariableType.Decimal;}
        }

        public BigInteger AsBigint{
            get{return (BigInteger)value;}
            set{this.value = value;}
        }

        public bool IsBigint{
            get{return (type_flag & VariableType.BigInteger) == VariableType.BigInteger;}
        }

        public Vector<int> AsIntVector{
            get{return (Vector<int>)value;}
            set{this.value = value;}
        }

        public bool IsVector{
            get{return (type_flag & VariableType.Vector) == VariableType.Vector;}
        }

        public bool IsIntVector{
            get{
                return (type_flag & VariableType.Vector) == VariableType.Vector
                    && (type_flag & VariableType.Integer) == VariableType.Integer;
            }
        }

        public Vector<double> AsDoubleVector{
            get{return (Vector<double>)value;}
            set{this.value = value;}
        }

        public bool IsDoubleVector{
            get{
                return (type_flag & VariableType.Vector) == VariableType.Vector
                    && (type_flag & VariableType.Double) == VariableType.Double;
            }
        }

        public bool IsMatrix{
            get{return (type_flag & VariableType.Matrix) == VariableType.Matrix;}
        }

        public Matrix<int> AsIntMatrix{
            get{return (Matrix<int>)value;}
            set{this.value = value;}
        }

        public bool IsIntMatrix{
            get{
                return (type_flag & VariableType.Matrix) == VariableType.Matrix
                    && (type_flag & VariableType.Integer) == VariableType.Integer;
            }
        }

        public Matrix<double> AsDoubleMatrix{
            get{return (Matrix<double>)value;}
            set{this.value = value;}
        }

        public bool IsDoubleMatrix{
            get{
                return (type_flag & VariableType.Matrix) == VariableType.Matrix
                    && (type_flag & VariableType.Double) == VariableType.Double;
            }
        }

        public List<IInstruction> Operations{
            get{return (List<IInstruction>)value;}
            set{this.value = value;}
        }

        public bool IsInstructions{
            get{return (type_flag & VariableType.ListOfOperations) == VariableType.ListOfOperations;}
        }

        public Variable(int integer)
        {
            value = integer;
            type_flag = VariableType.Integer;
        }

        public Variable(double input_double)
        {
            value = input_double;
            type_flag = VariableType.Double;
        }

        public Variable(decimal input_decimal)
        {
            value = input_decimal;
            type_flag = VariableType.Decimal;
        }

        public Variable(BigInteger bigint)
        {
            value = bigint;
            type_flag = VariableType.BigInteger;
        }

        public Variable(Vector<int> int_vector)
        {
            value = int_vector;
            type_flag = VariableType.Vector | VariableType.Integer;
        }

        public Variable(Vector<double> double_vector)
        {
            value = double_vector;
            type_flag = VariableType.Vector | VariableType.Double;
        }

        public Variable(Matrix<int> int_matrix)
        {
            value = int_matrix;
            type_flag = VariableType.Matrix | VariableType.Integer;
        }

        public Variable(Matrix<double> double_matrix)
        {
            value = double_matrix;
            type_flag = VariableType.Matrix | VariableType.Double;
        }

        public Variable(List<IInstruction> instructions)
        {
            value = instructions;
            type_flag = VariableType.ListOfOperations;
        }

        public override string ToString()
        {
            return string.Format("<Var: {0} (- {1}>", value, TypeFlag);
        }
    }
}

