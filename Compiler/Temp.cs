using System.Collections.Immutable;

namespace Compiler
{
    public interface IRuntimeEntity { }
    public class TriangleAbstractMachine
    {
        public enum Primitive { }
        public enum Type { }
        public static ImmutableDictionary<Type, byte> TypeSize { get; }
    }
}