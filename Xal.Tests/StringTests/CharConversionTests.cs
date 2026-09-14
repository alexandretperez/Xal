namespace Xal.Tests.StringTests;

[TestClass]
public class CharConversionTests
{
    [TestMethod]
    [DataRow("A", 'A')]
    [DataRow("z", 'z')]
    [DataRow("7", '7')]
    [DataRow(" ", ' ')]
    public void AsChar_SingleCharacter_ReturnsParsedValue(string input, char expected)
        => Assert.AreEqual((char?)expected, input.AsChar());

    [TestMethod]
    [DataRow("")]
    [DataRow("AB")]
    [DataRow("A B")]
    public void AsChar_TextWithoutExactlyOneCharacter_ReturnsNull(string input)
        => Assert.IsNull(input.AsChar());

    [TestMethod]
    public void AsChar_Null_ReturnsNull()
    {
        string? input = null;
        Assert.IsNull(input.AsChar());
    }

    [TestMethod]
    public void ToChar_ReturnsParsedValueOrDefaultNullChar()
    {
        Assert.AreEqual('A', "A".ToChar());
        Assert.AreEqual('\0', "AB".ToChar());
    }

    [TestMethod]
    public void ToChar_Null_ReturnsNullChar()
    {
        string? input = null;
        Assert.AreEqual('\0', input.ToChar());
    }
}
