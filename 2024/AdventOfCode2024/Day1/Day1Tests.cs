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

    public static int SimilaryScore(List<int> first, List<int> second)
    {
        var instances = second.GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count());
        var total = 0;
        foreach (var n in first)
        {
            total += n * (instances.TryGetValue(n, out var i) ? i : 0);
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
    public void SimilaryScoreTest()
    {
        var actual = SimilaryScore([3, 4, 2, 1, 3, 3], [4, 3, 5, 3, 9, 3]);
        actual.Should().Be(31);
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
                x.Split(
                        "   ",
                        StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries
                    )
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
        
        var similaryScore = SimilaryScore(first, second);
        similaryScore.Should().Be(18650129);
    }
}
