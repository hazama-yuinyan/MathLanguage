using System;
using System.IO;
using System.Text;
using MathLanguage;

namespace CalculatorConsole
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("ぐふふ(・_・)");
            while(true){
                var line = Console.In.ReadLine();
                if(line == "q")
                    break;

                var bytes = Encoding.UTF8.GetBytes(line);
                using(var stream = new MemoryStream(bytes)){
                    var parser = new Parser(new Scanner(stream));
                    Parser.Output = Console.Out;
                    parser.Parse();
                }
            }

            Console.WriteLine("(*゜ω゜*)");
        }
    }
}
