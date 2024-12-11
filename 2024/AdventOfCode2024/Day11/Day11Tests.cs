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
    [InlineData("125 17", 7, 31)]
    [InlineData("125 17", 8, 42)]
    [InlineData("125 17", 9, 68)]
    [InlineData("125 17", 10, 109)]
    [InlineData("125 17", 11, 170)]
    [InlineData("125 17", 12, 235)]
    [InlineData("125 17", 13, 342)]
    [InlineData("125 17", 14, 557)]
    [InlineData("125 17", 15, 853)]
    [InlineData("125 17", 16, 1298)]
    [InlineData("125 17", 17, 1951)]
    [InlineData("125 17", 18, 2869)]
    [InlineData("125 17", 19, 4490)]
    [InlineData("125 17", 20, 6837)]
    [InlineData("125 17", 21, 10362)]
    [InlineData("125 17", 22, 15754)]
    [InlineData("125 17", 23, 23435)]
    [InlineData("125 17", 24, 36359)]
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