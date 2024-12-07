using System.Drawing;
using FluentAssertions;

namespace AdventOfCode2024.Day6;

public class Day6Tests
{
    // Array of directions, roll between them.
    private static readonly Point[] Directions = { new(-1, 0), new(0, 1), new(1, 0), new(0, -1) };

    private int GuardPath(string input)
    {
        int total = 0;

        input = input.Replace("\r\n", "\n").Trim();
        var rowLength = input.IndexOf('\n');
        var grid = input.Split('\n').Select(x => x.ToCharArray().ToList()).ToList();
        var columnHeight = grid[0].Count;
        var guardPos = grid.Index()
            .Where(x => x.Item.Contains('^'))
            .Select(x => new Point(x.Index, x.Item.IndexOf('^')))
            .First();

        var passed = new List<Point>();

        var directionIdx = 0;
        while (IsInBounds(guardPos, rowLength, columnHeight))
        {
            var direction = Directions[directionIdx];
            total += passed.Contains(guardPos) ? 0 : 1;
            passed.Add(guardPos);
            var newPos = new Point(guardPos.X + direction.X, guardPos.Y + direction.Y);
            if (IsInBounds(newPos, rowLength, columnHeight) && grid[newPos.X][newPos.Y] == '#')
            {
                directionIdx = directionIdx >= Directions.Length - 1 ? 0 : directionIdx + 1;
                direction = Directions[directionIdx];
                newPos = new Point(guardPos.X + direction.X, guardPos.Y + direction.Y);
            }

            guardPos = newPos;
        }

        return total;
    }

    private static bool IsInBounds(Point guardPos, int rowLength, int columnHeight)
    {
        return guardPos.X >= 0
            && guardPos.X < rowLength
            && guardPos.Y >= 0
            && guardPos.Y < columnHeight;
    }

    [Theory]
    [InlineData(
        @"
..
.^",
        2
    )]
    [InlineData(
        @"
.#.
...
.^.",
        3
    )]
    [InlineData(
        @"
.#.
.^.",
        2
    )]
    [InlineData(
        @"
.#...
....#
.....
.^.#.",
        8
    )]
    [InlineData(
        @"
....#.....
.........#
..........
..#.......
.......#..
..........
.#..^.....
........#.
#.........
......#...",
        41
    )]
    public void GuardPathTest(string input, int expected)
    {
        var actual = GuardPath(input);

        actual.Should().Be(expected);
    }

    [Fact]
    public async Task FromFile()
    {
        var contents = await File.ReadAllTextAsync("./Day6/Day6.txt");
        var total = GuardPath(contents);
        total.Should().Be(5551);
    }
}
