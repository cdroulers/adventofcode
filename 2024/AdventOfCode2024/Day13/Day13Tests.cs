using System.Diagnostics;
using System.Drawing;
using System.Text.RegularExpressions;
using FluentAssertions;

namespace AdventOfCode2024.Day13;

public class Day13Tests
{
    private static int ClawMachinePrizes(string input)
    {
        var total = 0;
        
        var prizes = input.Replace("\r\n", "\n").Split("\n\n", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

        foreach (var prize in prizes)
        {
            total += ClawMachinePrize(prize);
        }

        return total;
    }
    
    private static int ClawMachinePrize(string input)
    {
        var parameters = input.Split('\n', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

        var buttonA = ExtractParameter(parameters[0]);
        var buttonB = ExtractParameter(parameters[1]);
        var prize = ExtractParameter(parameters[2]);

        for (var x = 0; x < 100; x++)
        {
            for (var y = 0; y < 100; y++)
            {
                var totalX = buttonA.X * x + buttonB.X * y;
                var totalY = buttonA.Y * x + buttonB.Y * y;
                if (totalX == prize.X && totalY == prize.Y)
                {
                    return x * 3 + y;
                }
            }
        }

        return 0;
    }

    private static readonly Regex NumberRegex = new Regex(@"X[\+|=](\d+).*Y[\+|=](\d+)", RegexOptions.Compiled);
    
    private static Point ExtractParameter(string line)
    {
        var match = NumberRegex.Match(line);
        return new Point(int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value));
    }

    [Theory]
    [InlineData(@"
Button A: X+94, Y+34
Button B: X+22, Y+67
Prize: X=8400, Y=5400",
        280)]
    [InlineData(@"
Button A: X+94, Y+34
Button B: X+22, Y+67
Prize: X=8400, Y=5400

Button A: X+26, Y+66
Button B: X+67, Y+21
Prize: X=12748, Y=12176

Button A: X+17, Y+86
Button B: X+84, Y+37
Prize: X=7870, Y=6450

Button A: X+69, Y+23
Button B: X+27, Y+71
Prize: X=18641, Y=10279
",
        480)]
    public void ClawMachinePrizesTest(string input, int expected)
    {
        var actual = ClawMachinePrizes(input);
        actual.Should().Be(expected);
    }

    [Fact]
    public async Task FromFile()
    {
        var contents = await File.ReadAllTextAsync("./Day13/Day13.txt");
        var total = ClawMachinePrizes(contents);
        total.Should().Be(31065);
    }
}