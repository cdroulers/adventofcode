using System.Drawing;
using FluentAssertions;

namespace AdventOfCode2024.Day8;

public class Day8Tests
{
    private record Node(int X, int Y, char Character);
    public int Antinodes(string input)
    {
        var antinodes = new List<Point>();

        var grid = input
            .Replace("\r\n", "\n")
            .Split('\n', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.ToCharArray().ToList())
            .ToList();
        var mapped = grid.SelectMany((x, i) => x.Select((y, j) => new Node(i, j, y))).ToList();
        var groups = mapped.GroupBy(x => x.Character).ToList();
        foreach (var group in groups.Where((x) => x.Key != '.'))
        {
            var pairs = group.SelectMany((first, i) => group.Skip(i + 1).Select(second => (first, second))).ToList();
            foreach (var pair in pairs)
            {
                var diff = new Point(pair.second.X - pair.first.X, pair.second.Y - pair.first.Y);
                var antinode1 = new Point(pair.first.X + diff.X * -1, pair.first.Y + diff.Y * -1);
                var antinode2 = new Point(pair.second.X + diff.X, pair.second.Y + diff.Y);
                if (IsInBounds(antinode1, grid))
                {
                    antinodes.Add(antinode1);
                }
                
                if (IsInBounds(antinode2, grid))
                {
                    antinodes.Add(antinode2);
                }
            }
        }

        return antinodes.Distinct().Count();
    }

    private static bool IsInBounds(Point guardPos, List<List<char>> grid)
    {
        return guardPos.X >= 0
               && guardPos.X < grid.Count
               && guardPos.Y >= 0
               && guardPos.Y < grid[0].Count;
    }

    [Theory]
    [InlineData(
        @"
....
.A..
..A.
....",
        2
    )]
    [InlineData(
        @"
....a.
.A.ab.
..Ab..
......",
        5
    )]
    public void AntinodesTest(string input, int expected)
    {
        var actual = Antinodes(input);

        actual.Should().Be(expected);
    }

    [Fact]
    public async Task FromFile()
    {
        var contents = await File.ReadAllTextAsync("./Day8/Day8.txt");
        var total = Antinodes(contents);
        total.Should().Be(344);
    }
}
