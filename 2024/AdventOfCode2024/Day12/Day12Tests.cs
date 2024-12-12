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

        return areas.Sum(x => x.Value.Count * Perimeter(x.Value));
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

    private static int Perimeter(List<Point> area)
    {
        var total = 0;
        foreach (var point in area)
        {
            foreach (var direction in Directions)
            {
                var p = new Point(point.X + direction.X, point.Y + direction.Y);
                total += area.Contains(p) ? 0 : 1;
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
        140
    )]
    [InlineData(
        @"
OOOOO
OXOXO
OOOOO
OXOXO
OOOOO",
        772
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
        1930
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
        total.Should().Be(1461752);
    }
}
