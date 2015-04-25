using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;

using CalculatorCompetition.Backend.Runtime;
using CalculatorCompetition.Backend.Runtime.Instruction;
using CalculatorCompetition.Backend.TypeSystem;




using System;

namespace MathLanguage {



public class Parser {
	public const int _EOF = 0;
	public const int _lparen = 1;
	public const int _rparen = 2;
	public const int _equal = 3;
	public const int _definer = 4;
	public const int _space = 5;
	public const int _eol = 6;
	public const int _ident = 7;
	public const int _integer = 8;
	public const int _float = 9;
	public const int maxT = 20;

	const bool T = true;
	const bool x = false;
	const int minErrDist = 2;
	
	public Scanner scanner;
	public Errors  errors;

	public Token t;    // last recognized token
	public Token la;   // lookahead token
	int errDist = minErrDist;

static StackMachine stack_machine = new StackMachine();
    public static TextWriter Output{
        get; set;
    }

    /// <summary>
    /// Gets all the variables that are on the stack.
    /// </summary>
    public static IEnumerable<Variable> Results{
        get{
            var tmp = new List<Variable>();
            while(!StackMachine.IsStackEmpty)
                tmp.Add(stack_machine.PopOffStack());
            
            return tmp;
        }
    }

    bool IsDefiningFunction{
        get{return instructions != null;}
    }   

    List<IInstruction> instructions = null;

    Delegate CreateDelegate(IEnumerable<string> argNames)
    {
        var method = typeof(UserDefinedFunction).GetMethod("Call");
        var instance = new UserDefinedFunction(stack_machine, instructions, argNames);
        var @delegate = Delegate.CreateDelegate(typeof(Action<IList<Variable>>), instance, method);
        instructions = null;
        return @delegate;
    }

    bool IsAssignment()
    {
        var x = scanner.Peek();
        scanner.ResetPeek();
        return x.kind == _equal;
    }

    bool IsFunctionDefinition()
    {
        var x = scanner.Peek();
        while(x.kind != _EOF && x.kind != _eol && x.kind != _definer)
            x = scanner.Peek();

        scanner.ResetPeek();
        return x.kind == _definer;
    }



	public Parser(Scanner scanner) {
		this.scanner = scanner;
		errors = new Errors();
	}

	void SynErr (int n) {
		if (errDist >= minErrDist) errors.SynErr(la.line, la.col, n);
		errDist = 0;
	}

	public void SemErr (string msg) {
		if (errDist >= minErrDist) errors.SemErr(t.line, t.col, msg);
		errDist = 0;
	}
	
	void Get () {
		for (;;) {
			t = la;
			la = scanner.Scan();
			if (la.kind <= maxT) { ++errDist; break; }

			la = t;
		}
	}
	
	void Expect (int n) {
		if (la.kind==n) Get(); else { SynErr(n); }
	}
	
	bool StartOf (int s) {
		return set[s, la.kind];
	}
	
	void ExpectWeak (int n, int follow) {
		if (la.kind == n) Get();
		else {
			SynErr(n);
			while (!StartOf(follow)) Get();
		}
	}


	bool WeakSeparator(int n, int syFol, int repFol) {
		int kind = la.kind;
		if (kind == n) {Get(); return true;}
		else if (StartOf(repFol)) {return false;}
		else {
			SynErr(n);
			while (!(set[syFol, kind] || set[repFol, kind] || set[0, kind])) {
				Get();
				kind = la.kind;
			}
			return StartOf(syFol);
		}
	}

	
	void MathLanguage() {
		Statement();
		while (StartOf(1)) {
			Statement();
		}
	}

	void Statement() {
		if (IsAssignment()) {
			Assignment();
		} else if (StartOf(1)) {
			Expression();
			if(Output != null) Output.WriteLine(stack_machine.PeekAtStack()); 
		} else SynErr(21);
		if (la.kind == 6) {
			while (!(la.kind == 0 || la.kind == 6)) {SynErr(22); Get();}
			Get();
		} else if (la.kind == 0) {
			Get();
		} else SynErr(23);
	}

	void Assignment() {
		Expect(7);
		string name = t.val; 
		Expect(3);
		Expression();
		var inst = new AssignOperation(StackMachine.Symbols, name);
		stack_machine.Operate(inst);
		if(Output != null)
		   Output.WriteLine(StackMachine.Symbols.GetVariable(name));
		
	}

	void Expression() {
		Additive();
	}

	void Additive() {
		OperatorType type; 
		Multiplicative();
		if (la.kind == 10 || la.kind == 11) {
			AdditiveOperator(out type);
			Additive();
			IInstruction inst = (type == OperatorType.Plus) ? new AddOperation() as IInstruction : new SubtractOperation() as IInstruction;
			if(IsDefiningFunction)
			   instructions.Add(inst);
			else
			   stack_machine.Operate(inst);
			
		}
	}

	void Multiplicative() {
		OperatorType type; 
		PowerOp();
		if (la.kind == 12 || la.kind == 13 || la.kind == 14) {
			MultiplicativeOperator(out type);
			Multiplicative();
			IInstruction inst = (type == OperatorType.Times) ? new MultiplyOperation() as IInstruction :
			                   (type == OperatorType.Divide) ? new DivideOperation() as IInstruction : new DotProductOperation() as IInstruction;
			if(IsDefiningFunction)
			   instructions.Add(inst);
			else
			   stack_machine.Operate(inst);
			
		}
	}

	void AdditiveOperator(out OperatorType type) {
		type = OperatorType.None; 
		if (la.kind == 10) {
			Get();
			type = OperatorType.Plus; 
		} else if (la.kind == 11) {
			Get();
			type = OperatorType.Minus; 
		} else SynErr(24);
	}

	void PowerOp() {
		Factor();
		if (la.kind == 15) {
			Get();
			Factor();
			var inst = new PowerOperation();
			if(IsDefiningFunction)
			   instructions.Add(inst);
			else
			   stack_machine.Operate(inst);
			
		}
	}

	void MultiplicativeOperator(out OperatorType type) {
		type = OperatorType.None; 
		if (la.kind == 12) {
			Get();
			type = OperatorType.Times; 
		} else if (la.kind == 13) {
			Get();
			type = OperatorType.Divide; 
		} else if (la.kind == 14) {
			Get();
			type = OperatorType.Dot; 
		} else SynErr(25);
	}

	void Factor() {
		OperatorType type; 
		if (StartOf(2)) {
			Primary();
		} else if (la.kind == 10 || la.kind == 11) {
			PrefixUnaryOperator(out type);
			Factor();
			if(type == OperatorType.Minus){
			   var inst = new NegateOperation();
			   if(IsDefiningFunction)
			       instructions.Add(inst);
			   else
			       stack_machine.Operate(inst);
			}
			
		} else SynErr(26);
		if (la.kind == 16) {
			Get();
			var inst = new CalculateFactorialOperation();
			if(IsDefiningFunction)
			   instructions.Add(inst);
			else
			   stack_machine.Operate(inst);
			
		}
	}

	void Primary() {
		IList<Delegate> funcs = null; int num_args = 0; string name; 
		if (la.kind == 7) {
			Get();
			name = t.val;
			if(la.kind == _lparen){
			   if(!IsFunctionDefinition()){
			       funcs = StackMachine.Symbols.GetFunction(name);
			       if(funcs == null){
			           SemErr(string.Format("{0} is an unknown function name!", name));
			           return;
			       }
			   }
			}else{
			   if(IsDefiningFunction){
			       var inst = new ReferenceSymbolOperation(stack_machine, name);
			       instructions.Add(inst);
			   }else{
			       var value = StackMachine.Symbols.GetVariable(name);
			       stack_machine.PushOnStack(value);
			   }
			}
			
			if (la.kind == 1) {
				if (IsFunctionDefinition()) {
					FunctionDefinitionTrailer(name);
				} else {
					Get();
					if (StartOf(1)) {
						Expression();
						++num_args; 
						while (la.kind == 17) {
							Get();
							Expression();
							++num_args; 
						}
					}
					Expect(2);
					var inst = new CallOperation(funcs);
					if(IsDefiningFunction)
					   instructions.Add(inst);
					else
					   stack_machine.Operate(inst);
					
				}
			}
		} else if (la.kind == 8 || la.kind == 9 || la.kind == 18) {
			LiteralExpression();
		} else if (la.kind == 1) {
			Get();
			scanner.IsSpaceSignificant = true;
			int num_elems = 0;
			var elems = new List<Variable>();
			
			Expression();
			if(la.kind != _rparen){
			   ++num_elems;
			   elems.Add(stack_machine.PopOffStack());
			}
			
			while (la.kind == 5) {
				Get();
				Expression();
				++num_elems; elems.Add(stack_machine.PopOffStack()); 
			}
			Expect(2);
			if(elems[0].IsInteger){
			   var vector = new Vector<int>(elems.Select(x => x.AsInteger));
			   stack_machine.PushOnStack(new Variable(vector));
			}else if(elems[0].IsDouble){
			   var vector = new Vector<double>(elems.Select(x => x.AsDouble));
			   stack_machine.PushOnStack(new Variable(vector));
			}else{
			   SemErr(string.Format("Can not create a vector of type `{0}`", elems[0].TypeFlag));
			}
			scanner.IsSpaceSignificant = false;
			
		} else SynErr(27);
	}

	void PrefixUnaryOperator(out OperatorType type) {
		type = OperatorType.None; 
		if (la.kind == 10) {
			Get();
			type = OperatorType.Plus; 
		} else if (la.kind == 11) {
			Get();
			type = OperatorType.Minus; 
		} else SynErr(28);
	}

	void FunctionDefinitionTrailer(string name) {
		var args = new List<string>(); 
		Expect(1);
		while (la.kind == 7) {
			Get();
			args.Add(t.val); 
		}
		Expect(2);
		Expect(4);
		instructions = new List<IInstruction>(); 
		Expression();
		StackMachine.Symbols.AddFunction(name, CreateDelegate(args));
		
	}

	void LiteralExpression() {
		if (la.kind == 8) {
			Get();
			int tmp = int.Parse(t.val);
			var variable = new Variable(tmp);
			stack_machine.PushOnStack(variable);
			
		} else if (la.kind == 9) {
			Get();
			Variable variable;
			if(t.val.Length > 8){
			   decimal tmp = decimal.Parse(t.val);
			   variable = new Variable(tmp);
			}else{
			   double tmp = double.Parse(t.val);
			   variable = new Variable(tmp);
			}
			stack_machine.PushOnStack(variable);
			
		} else if (la.kind == 18) {
			Get();
			scanner.IsSpaceSignificant = true;
			int num_elems = 0;
			int rows = 0, cols = 0;
			var elems = new List<Variable>();
			
			Expression();
			++num_elems; elems.Add(stack_machine.PopOffStack()); 
			Expect(5);
			Expression();
			++num_elems; elems.Add(stack_machine.PopOffStack()); 
			while (la.kind == 5 || la.kind == 17) {
				if (la.kind == 17) {
					Get();
					++rows;
					if(cols == 0)
					   cols = num_elems;
					
					while (la.kind == 5) {
						Get();
					}
				} else {
					Get();
				}
				Expression();
				++num_elems; elems.Add(stack_machine.PopOffStack()); 
			}
			Expect(19);
			if(num_elems % cols != 0){
			   SemErr(string.Format(
			       "The matrix should be {0} x {1}, only {2} elements given.",
			       rows, cols, num_elems
			   ));
			}else{
			   ++rows;
			}
			
			if(elems[0].IsInteger){
			   var matrix = new Matrix<int>(rows, cols, elems.Select(x => x.AsInteger));
			   var variable = new Variable(matrix);
			   stack_machine.PushOnStack(variable);
			}else{
			   var matrix = new Matrix<double>(rows, cols, elems.Select(x => x.AsDouble));
			   var variable = new Variable(matrix);
			   stack_machine.PushOnStack(variable);
			}
			scanner.IsSpaceSignificant = false;
			
		} else SynErr(29);
	}



	public void Parse() {
		la = new Token();
		la.val = "";		
		Get();
		MathLanguage();
		Expect(0);

	}
	
	static readonly bool[,] set = {
		{T,x,x,x, x,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x},
		{x,T,x,x, x,x,x,T, T,T,T,T, x,x,x,x, x,x,T,x, x,x},
		{x,T,x,x, x,x,x,T, T,T,x,x, x,x,x,x, x,x,T,x, x,x}

	};
} // end Parser


public class Errors {
	public int count = 0;                                    // number of errors detected
	public System.IO.TextWriter errorStream = Console.Out;   // error messages go to this stream
	public string errMsgFormat = "-- line {0} col {1}: {2}"; // 0=line, 1=column, 2=text

	public virtual void SynErr (int line, int col, int n) {
		string s;
		switch (n) {
			case 0: s = "EOF expected"; break;
			case 1: s = "lparen expected"; break;
			case 2: s = "rparen expected"; break;
			case 3: s = "equal expected"; break;
			case 4: s = "definer expected"; break;
			case 5: s = "space expected"; break;
			case 6: s = "eol expected"; break;
			case 7: s = "ident expected"; break;
			case 8: s = "integer expected"; break;
			case 9: s = "float expected"; break;
			case 10: s = "\"+\" expected"; break;
			case 11: s = "\"-\" expected"; break;
			case 12: s = "\"*\" expected"; break;
			case 13: s = "\"/\" expected"; break;
			case 14: s = "\".\" expected"; break;
			case 15: s = "\"^\" expected"; break;
			case 16: s = "\"!\" expected"; break;
			case 17: s = "\",\" expected"; break;
			case 18: s = "\"[\" expected"; break;
			case 19: s = "\"]\" expected"; break;
			case 20: s = "??? expected"; break;
			case 21: s = "invalid Statement"; break;
			case 22: s = "this symbol not expected in Statement"; break;
			case 23: s = "invalid Statement"; break;
			case 24: s = "invalid AdditiveOperator"; break;
			case 25: s = "invalid MultiplicativeOperator"; break;
			case 26: s = "invalid Factor"; break;
			case 27: s = "invalid Primary"; break;
			case 28: s = "invalid PrefixUnaryOperator"; break;
			case 29: s = "invalid LiteralExpression"; break;

			default: s = "error " + n; break;
		}
		errorStream.WriteLine(errMsgFormat, line, col, s);
		count++;
	}

	public virtual void SemErr (int line, int col, string s) {
		errorStream.WriteLine(errMsgFormat, line, col, s);
		count++;
	}
	
	public virtual void SemErr (string s) {
		errorStream.WriteLine(s);
		count++;
	}
	
	public virtual void Warning (int line, int col, string s) {
		errorStream.WriteLine(errMsgFormat, line, col, s);
	}
	
	public virtual void Warning(string s) {
		errorStream.WriteLine(s);
	}
} // Errors


public class FatalError: Exception {
	public FatalError(string m): base(m) {}
}
}