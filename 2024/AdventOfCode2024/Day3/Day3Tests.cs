using System.Text.RegularExpressions;
using FluentAssertions;

namespace AdventOfCode2024.Day3;

public class Day3Tests
{
    private static readonly Regex Regex = new Regex(@"mul\((\d{1,3}),(\d{1,3})\)", RegexOptions.Compiled | RegexOptions.Multiline);
    
    public static int ScanMemory(string mem)
    {
        var matches = Regex.Matches(mem);
        var total = 0;
        foreach (Match match in matches)
        {
            total += Multiply(match);
        }

        return total;
    }

    public static int Multiply(Match match)
    {
        var n1 = int.Parse(match.Groups[1].Value);
        var n2 = int.Parse(match.Groups[2].Value);
        return n1 * n2;
    }
    
    [Theory]
    [InlineData("mul(44,46)", 2024)]
    [InlineData("mul(44,46),mul(1,2)", 2026)]
    [InlineData("xmul(2,4)%&mul[3,7]!@^do_not_mul(5,5)+mul(32,64]then(mul(11,8)mul(8,5))", 161)]
    public void ScanMemoryTest(string mem, int expected)
    {
        var actual = ScanMemory(mem);
        actual.Should().Be(expected);
    }

    [Fact]
    public void FromFile()
    {
        var contents = File.ReadAllText("./Day3/Day3.txt");
        var total = ScanMemory(contents);
        total.Should().Be(188192787);
    }
}