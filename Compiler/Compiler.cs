using Compiler.IO;
using Compiler.Tokenization;
using System.Collections.Generic;
using System.IO;
using Compiler.SyntaticAnalysis;
using static System.Console;

namespace Compiler
{
    public class Compiler
    {
        /// <summary>
        /// The error reporter
        /// </summary>
        public ErrorReporter Reporter { get; }

        /// <summary>
        /// The file reader
        /// </summary>
        public IFileReader Reader { get; }

        /// <summary>
        /// The tokenizer
        /// </summary>
        public Tokenizer Tokenizer { get; }
        
        /// <summary>
        /// The parser
        /// </summary>
        public Parser Parser { get;  }

        /// <summary>
        /// Creates a new compiler
        /// </summary>
        /// <param name="inputFile">The file containing the source code</param>
        public Compiler(string inputFile)
        {
            Reporter = new ErrorReporter();
            Reader = new FileReader(inputFile);
            Tokenizer = new Tokenizer(Reader, Reporter);
            Parser = new Parser(Reporter);
        }

        /// <summary>
        /// Performs the compilation process
        /// </summary>
        public void Compile()
        {
            // Tokenize
            Write("Tokenising...");
            List<Token> tokens = Tokenizer.GetAllTokens();
            if (Reporter.HasErrors) return;
            WriteLine("Done");
            WriteLine(string.Join("\n", tokens));
            
            // Parse
            WriteLine("Parsing...");
            Parser.Parse(tokens);
            if (Reporter.HasErrors) return;
            WriteLine("Done");
        }

        /// <summary>
        /// Writes a message reporting on the success of compilation
        /// </summary>
        private void WriteFinalMessage()
        {
            // Write output to tell the user whether it worked or not here
        }

        /// <summary>
        /// Compiles the code in a file
        /// </summary>
        /// <param name="args">Should be one argument, the input file (*.tri)</param>
        public static void Main(string[] args)
        {
            if (args == null || args.Length != 1 || args[0] == null)
                WriteLine("ERROR: Must call the program with exactly one argument, the input file (*.tri)");
            else if (!File.Exists(args[0]))
                WriteLine($"ERROR: The input file \"{Path.GetFullPath(args[0])}\" does not exist");
            else
            {
                string inputFile = args[0];
                Compiler compiler = new Compiler(inputFile);
                WriteLine("Compiling...");
                compiler.Compile();
                compiler.WriteFinalMessage();
            }
        }
    }
}