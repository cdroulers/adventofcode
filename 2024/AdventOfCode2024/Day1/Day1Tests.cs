using FluentAssertions;

namespace AdventOfCode2024.Day1;

public class Day1Tests
{
    public static int Distance(List<int> first, List<int> second)
    {
        first.Sort();
        second.Sort();
        var total = 0;
        for (var i = 0; i < first.Count; i++)
        {
            total += Math.Abs(first[i] - second[i]);
        }

        return total;
    }

    [Fact]
    public void DistanceTest()
    {
        var actual = Distance([3, 4, 2, 1, 3, 3], [4, 3, 5, 3, 9, 3]);
        actual.Should().Be(11);
    }

    [Fact]
    public void FromFile()
    {
        var contents = File.ReadAllText("./Day1/Day1.txt");
        var first = new List<int>();
        var second = new List<int>();
        var lines = contents
            .Split(Environment.NewLine)
            .Select(x =>
                x.Split("   ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                    .Select(int.Parse)
                    .ToArray()
            )
            .ToList();
        lines.ForEach(l =>
        {
            first.Add(l[0]);
            second.Add(l[1]);
        });
        var total = Distance(first, second);
        total.Should().Be(2904518);
    }
}
