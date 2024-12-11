using FluentAssertions;

namespace AdventOfCode2024.Day11;

public class Day11Tests
{
    public long BlinkingStones(string input, int blinks)
    {
        var rocks = input.Split(" ").Select(long.Parse).ToDictionary(l => l, l => (long)1);

        for (var i = 0; i < blinks; i++)
        {
            rocks = BlinkOnce(rocks);
        }
        
        return rocks.Sum(x => x.Value);
    }
    
    private Dictionary<long, long> BlinkOnce(Dictionary<long, long> rocks)
    {
        var newDict  = new Dictionary<long, long>();
        foreach (var kv in rocks)
        {
            var n = kv.Key;
            if (n == 0)
            {
                newDict[1] = Add(newDict, 1, kv.Value);
                continue;
            }
            
            var nAsString = n.ToString();
            if (nAsString.Length % 2 == 0)
            {
                var n1 = long.Parse(nAsString.Substring(0, nAsString.Length / 2));
                var n2 = long.Parse(nAsString.Substring(nAsString.Length / 2));
                newDict[n1] = Add(newDict, n1, kv.Value);
                newDict[n2] = Add(newDict, n2, kv.Value);
            }
            else
            {
                var val = n * 2024;
                newDict[val] = Add(newDict, val, kv.Value);
            }
        }

        return newDict;
    }

    private long Add(Dictionary<long, long> rocks, long key, long value)
    {
        return rocks.ContainsKey(key) ? rocks[key] + value : value;
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

    [Fact]
    public async Task FromFile75()
    {
        var contents = await File.ReadAllTextAsync("./Day11/Day11.txt");
        var total = BlinkingStones(contents, 75);
        total.Should().Be(218279375708592L);
    }
}