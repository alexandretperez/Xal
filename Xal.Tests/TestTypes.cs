namespace Xal.Tests; // <- adjust to your test project's namespace

/// <summary>
/// Shared fixture types used across the <c>TypeExtensions</c> test classes.
/// </summary>
/// <remarks>
/// An implicitly declared default constructor has the same accessibility as its
/// declaring type (C# spec), so only <c>public</c> classes expose a public
/// parameterless constructor that <see cref="Type.GetConstructor(Type[])"/> can find.
/// </remarks>
internal static class TestTypes
{
    public sealed class PublicClassWithDefaultCtor
    { }

    public sealed class PublicClassWithoutDefaultCtor
    {
        public PublicClassWithoutDefaultCtor(int value)
        { }
    }

    public sealed class PublicClassWithOnlyPrivateDefaultCtor
    {
        private PublicClassWithOnlyPrivateDefaultCtor()
        { }
    }

    public abstract class PublicAbstractClassWithDefaultCtor
    { }

    public abstract class PublicAbstractClassWithoutDefaultCtor
    {
        protected PublicAbstractClassWithoutDefaultCtor(int value)
        { }
    }

    public sealed class GenericClass<T>
    { }

    public sealed record EmptyRecord;

    public sealed record RecordWithPositionalParameter(int Value);

    /// <summary>
    /// Its implicit default constructor is internal — not visible to
    /// <see cref="Type.GetConstructor(Type[])"/>, which only finds public constructors.
    /// </summary>
    internal class InternalClassWithImplicitDefaultCtor
    { }

    public interface ITestInterface
    { }

    public struct CustomStruct
    {
        public int Value { get; set; }
    }

    public enum CustomEnum
    { A, B }
}