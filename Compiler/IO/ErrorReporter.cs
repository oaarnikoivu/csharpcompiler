using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compiler.IO
{
    public class ErrorReporter
    {

        /// <summary>
        /// The number of errors encountered so far
        /// </summary>
        public int ErrorCount { get; private set; } = 0;
        
        /// <summary>
        /// Whether or not any errors have been encountered
        /// </summary>
        public bool HasErrors
        {
            get { return ErrorCount > 0; }
        }
        
        public List<String> Errors { get; private set; } = new List<string>();

        public void ReportError(string message)
        {
            ErrorCount += 1;
            Errors.Add(message);
            Console.WriteLine($"ERROR: {message}");
        }
        
    }
}