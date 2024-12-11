using FluentAssertions;

namespace AdventOfCode2024.Day11;

public class Day11Tests
{
    public int BlinkingStones(string input, int blinks)
    {
        var rocks = input.Split(" ").Select(long.Parse).ToList();

        for (var i = 0; i < blinks; i++)
        {
            BlinkOnce(ref rocks);
        }
        
        return rocks.Count;
    }
    
    private void BlinkOnce(ref List<long> rocks)
    {
        for (var i = 0; i < rocks.Count; i++)
        {
            var n = rocks[i];
            var nAsString = n.ToString();
            if (n == 0)
            {
                rocks[i] = 1;
            }
            else if (nAsString.Length % 2 == 0)
            {
                var n1 = nAsString.Substring(0, nAsString.Length / 2);
                var n2 = nAsString.Substring(nAsString.Length / 2);
                rocks[i] = long.Parse(n1);
                rocks.Insert(i + 1, long.Parse(n2));
                i++;
            }
            else
            {
                rocks[i] = n * 2024;
            }
        }
    }

    [Theory]
    [InlineData("0 1 10 99 999", 1, 7)]
    [InlineData("125 17", 1, 3)]
    [InlineData("125 17", 2, 4)]
    [InlineData("125 17", 3, 5)]
    [InlineData("125 17", 4, 9)]
    [InlineData("125 17", 5, 13)]
    [InlineData("125 17", 6, 22)]
    [InlineData("125 17", 25, 55312)]
    public void BlinkingStonesTest(string input, int blinks, int expected)
    {
        var actual = BlinkingStones(input, blinks);
        
        actual.Should().Be(expected);
    }

    [Fact]
    public async Task FromFile()
    {
        var contents = await File.ReadAllTextAsync("./Day11/Day11.txt");
        var total = BlinkingStones(contents, 25);
        total.Should().Be(183435);
    }
}