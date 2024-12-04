using System.Drawing;
using FluentAssertions;

namespace AdventOfCode2024.Day4;

public class Day4Tests
{
    private static readonly string Xmas = "MMSS";
    private static readonly Point[] Rotation = [new(-1, -1), new(-1, 1), new(1, 1), new(1, -1)];
    
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
                if (l == 'A')
                {
                    var point = new Point(colIdx, lineIdx);
                    count += CheckXmas(lines, 0, point, 0)
                        + CheckXmas(lines, 0, point, 1)
                        
                        + CheckXmas(lines, 0, point, 2)
                        + CheckXmas(lines, 0, point, 3);
                }
            }
        }

        return count;
    }

    private int CheckXmas(char[][] input, int xmasIdx, Point pos, int rotationIdx)
    {
        var diff = Rotation[rotationIdx];
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
            if (xmasIdx >= Xmas.Length - 1)
            {
                return 1;
            }

            var nextRotationIdx = rotationIdx >= Rotation.Length - 1 ? 0 : rotationIdx + 1;
            return CheckXmas(input, xmasIdx + 1, pos, nextRotationIdx);
        }

        return 0;
    }

    [Theory]
    [InlineData(@"
M.S
.A.
M.S
S.S
.A.
M.M
M.M
.A.
S.S", 3)]
    [InlineData(@"XXAMAS", 0)]
    [InlineData(
        @"
M.S
.A.
M.S",
        1
    )]
    [InlineData(
        @"
.M.S......
..A..MSMS.
.M.S.MAA..
..A.ASMSM.
.M.S.M....
..........
S.S.S.S.S.
.A.A.A.A..
M.M.M.M.M.
..........",
        9
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
