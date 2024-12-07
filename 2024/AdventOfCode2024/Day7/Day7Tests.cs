using FluentAssertions;

namespace AdventOfCode2024.Day7;

public class Day7Tests
{
    public record Row(long Sum, long[] Items);

    public long PossibleOperations(string input)
    {
        long total = 0;
        var lines = input
            .Replace("\r\n", "\n")
            .Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(x => x.Split(": "))
            .Select(x => new Row(long.Parse(x[0]), x[1].Split(" ").Select(long.Parse).ToArray()))
            .ToArray();

        foreach (var line in lines)
        {
            if (PossibleOperation(line, 0, line.Items[0]))
            {
                total += line.Sum;
            }
        }

        return total;
    }

    public bool PossibleOperation(Row row, long idx, long sum)
    {
        if (idx == row.Items.Length - 1)
        {
            return sum == row.Sum;
        }

        return PossibleOperation(row, idx + 1, sum + row.Items[idx + 1])
               || PossibleOperation(row, idx + 1, sum * row.Items[idx + 1])
               || PossibleOperation(row, idx + 1, long.Parse(sum.ToString() + row.Items[idx + 1].ToString()));
    }

    [Theory]
    [InlineData(
        @"
190: 10 19
3267: 81 40 27
156: 15 6",
        3613
    )]
    [InlineData(
        @"
190: 10 19
3267: 81 40 27
83: 17 5
156: 15 6
7290: 6 8 6 15
161011: 16 10 13
192: 17 8 14
21037: 9 7 18 13
292: 11 6 16 20",
        11387
    )]
    public void PossibleOperationsTest(string input, long expected)
    {
        var actual = PossibleOperations(input);
        actual.Should().Be(expected);
    }

    [Fact]
    public async Task FromFile()
    {
        var contents = await File.ReadAllTextAsync("./Day7/Day7.txt");
        var total = PossibleOperations(contents);
        total.Should().Be(95297119227552L);
    }
}
