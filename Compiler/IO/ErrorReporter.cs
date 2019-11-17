using System.Text;

namespace Compiler.IO
{
    public class ErrorReporter
    {
        /// <summary>
        /// Whether or not any errors have been encountered
        /// </summary>
        public bool HasErrors { get; }
        
        public StringBuilder Errors { get; set; } = new StringBuilder();
        
    }
}