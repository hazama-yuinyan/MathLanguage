using System;

namespace CalculatorCompetition.Backend.Runtime
{
    /// <summary>
    /// Abstract class for classes that have the capability of manipulating the symbol table.
    /// </summary>
    public abstract class SymbolManipulator
    {
        protected SymbolTable symbols;

        public SymbolManipulator(SymbolTable table)
        {
            symbols = table;
        }
    }
}

