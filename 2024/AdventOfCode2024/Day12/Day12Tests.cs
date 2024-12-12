using System.Drawing;
using FluentAssertions;

namespace AdventOfCode2024.Day12;

public class Day12Tests
{
    private static readonly Point[] Directions = { new(-1, 0), new(0, 1), new(1, 0), new(0, -1) };
    
    private static int FencePrice(string input)
    {
        var areas = new List<KeyValuePair<char, List<Point>>>();
        var grid = input
            .Replace("\r\n", "\n")
            .Split('\n', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.ToCharArray().ToList())
            .ToList();

        for (var x = 0; x < grid.Count; x++)
        {
            var row = grid[x];
            for (var y = 0; y < row.Count; y++)
            {
                var point = new Point(x, y);
                var c = row[y];
                if (areas.Exists(a => a.Value.Contains(point)))
                {
                    continue;
                }

                var area = new List<Point>() { point };
                BuildArea(grid, area, point, c);
                areas.Add(new KeyValuePair<char, List<Point>>(c, area));
            }
        }

        var total = 0;
        foreach (var area in areas)
        {
            var nbOfSides = NumberOfSides(area.Value);
            total += area.Value.Count * nbOfSides;
        }

        return total;
    }

    private static void BuildArea(List<List<char>> grid, List<Point> area, Point current, char c)
    {
        foreach (var direction in Directions)
        {
            var p = new Point(current.X + direction.X, current.Y + direction.Y);
            if (IsInBounds(p, grid) && grid[p.X][p.Y] == c && !area.Contains(p))
            {
                area.Add(p);
                BuildArea(grid, area, p, c);
            }
        }
    }

    private static bool IsInBounds(Point guardPos, List<List<char>> grid)
    {
        return guardPos.X >= 0
               && guardPos.X < grid.Count
               && guardPos.Y >= 0
               && guardPos.Y < grid[0].Count;
    }

    private static int NumberOfSides(List<Point> area)
    {
        var total = 0;
        foreach (var point in area)
        {
            // Goes around each letter and checks if it's a "corner" by checking for empty
            // in 2 directions.
            for (var i = 0; i < Directions.Length; i++)
            {
                var dir1 = Directions[i];
                var dir2 = Directions[i == Directions.Length - 1 ? 0 : i + 1];
                if (!area.Contains(new Point(point.X + dir1.X, point.Y + dir1.Y))
                    && !area.Contains(new Point(point.X + dir2.X, point.Y + dir2.Y)))
                {
                    total++;
                }
                else if (area.Contains(new Point(point.X + dir1.X, point.Y + dir1.Y))
                    && area.Contains(new Point(point.X + dir2.X, point.Y + dir2.Y))
                    && !area.Contains(new Point(point.X + dir1.X + dir2.X, point.Y + dir1.Y + dir2.Y)))
                {
                    total++;
                }
            }
        }

        return total;
    }

    [Theory]
    [InlineData(
        @"
AAAA
BBCD
BBCC
EEEC",
        80
    )]
    [InlineData(
        @"
OOOOO
OXOXO
OOOOO
OXOXO
OOOOO",
        436
    )]
    [InlineData(
        @"
RRRRIICCFF
RRRRIICCCF
VVRRRCCFFF
VVRCCCJFFF
VVVVCJJCFE
VVIVCCJJEE
VVIIICJJEE
MIIIIIJJEE
MIIISIJEEE
MMMISSJEEE",
        1206
    )]
    [InlineData(
        @"
EEEEE
EXXXX
EEEEE
EXXXX
EEEEE",
        236
    )]
    [InlineData(
        @"
AAAAAA
AAABBA
AAABBA
ABBAAA
ABBAAA
AAAAAA",
        368
    )]
    public void FencePriceTest(string input, int expected)
    {
        var actual = FencePrice(input);

        actual.Should().Be(expected);
    }

    [Fact]
    public async Task FromFile()
    {
        var contents = await File.ReadAllTextAsync("./Day12/Day12.txt");
        var total = FencePrice(contents);
        total.Should().Be(904114);
    }
}
