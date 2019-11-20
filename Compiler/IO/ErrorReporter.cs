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

        public void ReportError(string message)
        {
            ErrorCount += 1;
            Console.WriteLine($"ERROR: {message}");
        }

        /// <summary>
        /// Set of errors 
        /// </summary>
        public List<string> Errors { get; set; } = new List<string>();

        /// <summary>
        /// Method for adding error to error set
        /// </summary>
        /// <param name="error"></param>
        public void AddError(string error)
        {
            Errors.Add(error);
        }

        /// <summary>
        /// Display all errors in set
        /// </summary>
        public void DisplayErrors()
        {
            Console.WriteLine("\nErrors:");
            foreach (var error in Errors)
            {
                Console.WriteLine(error);
            }
        }
        
    }
}