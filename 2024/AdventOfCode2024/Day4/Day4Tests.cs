using System.Drawing;
using FluentAssertions;

namespace AdventOfCode2024.Day4;

public class Day4Tests
{
    private static readonly string Xmas = "XMAS";
    public int XmasFinder(string input)
    {
        var lines = input
            .Split(
                Environment.NewLine,
                StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries
            )
            .Select(x => x.ToCharArray())
            .ToArray();
        var count = 0;
        for (var lineIdx = 0; lineIdx < lines.Length; lineIdx++)
        {
            var line = lines[lineIdx];
            for (var colIdx = 0; colIdx < line.Length; colIdx++)
            {
                var l = line[colIdx];
                if (l == 'X')
                {
                    count += CheckXmas(lines, 1, new(colIdx, lineIdx), new(1, 0))
                        + CheckXmas(lines, 1, new(colIdx, lineIdx), new(0, 1))
                        + CheckXmas(lines, 1, new(colIdx, lineIdx), new(-1, 0))
                        + CheckXmas(lines, 1, new(colIdx, lineIdx), new(0, -1))
                        + CheckXmas(lines, 1, new(colIdx, lineIdx), new(-1, -1))
                        + CheckXmas(lines, 1, new(colIdx, lineIdx), new(1, 1))
                        + CheckXmas(lines, 1, new(colIdx, lineIdx), new(-1, 1))
                        + CheckXmas(lines, 1, new(colIdx, lineIdx), new(1, -1));
                }
            }
        }

        return count;
    }

    private int CheckXmas(char[][] input, int xmasIdx, Point pos, Point diff)
    {
        var nextPos = new Point(pos.X + diff.X, pos.Y + diff.Y);
        if (nextPos.Y < 0 
            || nextPos.Y >= input.Length
            || nextPos.X < 0
            || nextPos.X >= input[nextPos.Y].Length)
        {
            return 0;
        }

        if (input[nextPos.Y][nextPos.X] == Xmas[xmasIdx])
        {
            return xmasIdx >= Xmas.Length - 1 ? 1 : CheckXmas(input, xmasIdx + 1, nextPos, diff);
        }

        return 0;
    }

    [Theory]
    [InlineData(@"XMAS", 1)]
    [InlineData(@"XXAMAS", 0)]
    [InlineData(
        @"
XMAS
XMAS
XMAS
XMAS
SAMX",
        7
    )]
    [InlineData(
        @"
MMMSXXMASM
MSAMXMSMSA
AMXSXMAAMM
MSAMASMSMX
XMASAMXAMM
XXAMMXXAMA
SMSMSASXSS
SAXAMASAAA
MAMMMXMMMM
MXMXAXMASX",
        18
    )]
    public void XmasFinderTest(string input, int expected)
    {
        var count = XmasFinder(input);

        count.Should().Be(expected);
    }

    [Fact]
    public async Task FromFile()
    {
        var contents = await File.ReadAllTextAsync("./Day4/Day4.txt");
        var total = XmasFinder(contents);
        total.Should().Be(2427);
    }
}
