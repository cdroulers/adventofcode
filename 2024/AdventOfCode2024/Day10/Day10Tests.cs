using System.Drawing;
using FluentAssertions;

namespace AdventOfCode2024.Day10;

public class Day10Tests
{
    public (int Score, int Rating) TrailScore(string input)
    {
        var totalScore = 0;
        var totalRating = 0;
        var grid = input
            .Replace("\r\n", "\n")
            .Split('\n', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.ToCharArray().Select(x => x != '.' ? int.Parse(x.ToString()) : -1).ToList())
            .ToList();

        for (var x = 0; x < grid.Count; x++)
        {
            for (var y = 0; y < grid.Count; y++)
            {
                var p = new Point(x, y);
                if (grid[x][y] == 0)
                {
                    totalScore += Trailends(grid, p).Distinct().Count();
                    totalRating += TrailRatings(grid, p).Distinct().Count();
                }
            }
        }
        
        return (totalScore, totalRating);
    }

    private static readonly Point[] Directions = { new(-1, 0), new(0, 1), new(1, 0), new(0, -1) };
    
    private List<Point> Trailends(List<List<int>> grid, Point trailhead, int idx = 0)
    {
        var trailends = new List<Point>();
        foreach (var direction in Directions)
        {
            var p = new Point(trailhead.X + direction.X, trailhead.Y + direction.Y);
            if (IsInBounds(p, grid) && grid[p.X][p.Y] == idx + 1)
            {
                if (idx + 1 == 9)
                {
                    trailends.Add(p);
                }
                else
                {
                    trailends.AddRange( Trailends(grid, p, idx + 1));
                }
            }
        }

        return trailends;
    }
    
    private List<List<Point>> TrailRatings(List<List<int>> grid, Point trailhead, int idx = 0, List<Point>? current = null)
    {
        var trails = new List<List<Point>>();
        current ??= new List<Point>();
        foreach (var direction in Directions)
        {
            var p = new Point(trailhead.X + direction.X, trailhead.Y + direction.Y);
            if (IsInBounds(p, grid) && grid[p.X][p.Y] == idx + 1)
            {
                current.Add(p);
                if (idx + 1 == 9)
                {
                    trails.Add(current);
                    current = new List<Point>();
                }
                else
                {
                    trails.AddRange( TrailRatings(grid, p, idx + 1, current.ToList()));
                }
            }
        }

        return trails.Distinct().ToList();
    }

    private static bool IsInBounds(Point guardPos, List<List<int>> grid)
    {
        return guardPos.X >= 0
               && guardPos.X < grid.Count
               && guardPos.Y >= 0
               && guardPos.Y < grid[0].Count;
    }

    [Theory]
    [InlineData(@"
0123
1234
8765
9876", 1, 16)]
    [InlineData(@"
...0...
...1...
...2...
6543456
7.....7
8.....8
9.....9", 2, 2)]
    [InlineData(@"
..90..9
...1.98
...2..7
6543456
765.987
876....
987....", 4, 13)]
    [InlineData(@"
10..9..
2...8..
3...7..
4567654
...8..3
...9..2
.....01", 3, 3)]
    [InlineData(@"
89010123
78121874
87430965
96549874
45678903
32019012
01329801
10456732", 36, 81)]
    public void TrailScoreTest(string input, int expectedScore, int expectedRating)
    {
        var total = TrailScore(input);
        total.Score.Should().Be(expectedScore);
        total.Rating.Should().Be(expectedRating);
    }

    [Fact]
    public async Task FromFile()
    {
        var contents = await File.ReadAllTextAsync("./Day10/Day10.txt");
        var total = TrailScore(contents);
        total.Score.Should().Be(717);
        total.Rating.Should().Be(1686);
    }
}