using System.Drawing;
using FluentAssertions;

namespace AdventOfCode2024.Day6;

public class Day6Tests
{
    // Array of directions, roll between them.
    private static readonly Point[] Directions = { new(-1, 0), new(0, 1), new(1, 0), new(0, -1) };

    private (List<(Point, Point)> PointsVisited, bool IsLoop) GuardPath(
        string input,
        Point? extraBlock = null
    )
    {
        input = input.Replace("\r\n", "\n").Trim();
        var grid = input.Split('\n').Select(x => x.ToCharArray().ToList()).ToList();
        var rowLength = grid.Count;
        var columnHeight = grid[0].Count;
        var guardPos = grid.Index()
            .Where(x => x.Item.Contains('^'))
            .Select(x => new Point(x.Index, x.Item.IndexOf('^')))
            .First();

        var passed = new List<(Point, Point)>();
        if (extraBlock.HasValue)
        {
            if (!IsInBounds(extraBlock.Value, rowLength, columnHeight))
            {
                return (passed, false);
            }
            grid[extraBlock.Value.X][extraBlock.Value.Y] = '#';
        }

        var directionIdx = 0;
        while (IsInBounds(guardPos, rowLength, columnHeight))
        {
            var direction = Directions[directionIdx];
            var p = (guardPos, direction);
            if (passed.Contains(p))
            {
                return (passed, true);
            }
            
            passed.Add(p);
            var newPos = new Point(guardPos.X + direction.X, guardPos.Y + direction.Y);
            if (IsInBounds(newPos, rowLength, columnHeight) && grid[newPos.X][newPos.Y] == '#')
            {
                directionIdx = directionIdx >= Directions.Length - 1 ? 0 : directionIdx + 1;
                direction = Directions[directionIdx];
                newPos = new Point(guardPos.X + direction.X, guardPos.Y + direction.Y);
            }
            guardPos = newPos;
        }

        return (passed.Distinct().ToList(), false);
    }

    private (List<(Point, Point)> PointsVisited, List<Point> Loops) GuardPathWithLoops(
        string input
    )
    {
        var result = GuardPath(input);

        var loops = new List<Point>();
        foreach (var point in result.PointsVisited)
        {
            var block = new Point(point.Item1.X + point.Item2.X, point.Item1.Y + point.Item2.Y);
            var (_, IsLoop) = GuardPath(input, extraBlock: block);
            if (IsLoop)
            {
                loops.Add(block);
            }
        }

        return (result.PointsVisited, loops);
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
        2,
        0
    )]
    [InlineData(
        @"
.#.
...
.^.",
        3,
        0
    )]
    [InlineData(
        @"
.#.
.^.",
        2,
        0
    )]
    [InlineData(
        @"
.#...
....#
.....
.^.#.",
        8,
        1
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
        41,
        6
    )]
    public void GuardPathTest(string input, int expected, int expectedLoops)
    {
        var actual = GuardPathWithLoops(input);

        actual.PointsVisited.Select(x=> x.Item1).Distinct().Should().HaveCount(expected);
        actual.Loops.Distinct().Should().HaveCount(expectedLoops);
    }

    [Fact]
    public async Task FromFile()
    {
        var contents = await File.ReadAllTextAsync("./Day6/Day6.txt");
        var total = GuardPathWithLoops(contents);
        total.PointsVisited.Select(x=> x.Item1).Distinct().Should().HaveCount(5551);
        total.Loops.Distinct().Should().HaveCount(2012);
    }
}
