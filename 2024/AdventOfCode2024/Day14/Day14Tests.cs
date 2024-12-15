using System.Diagnostics;
using System.Drawing;
using System.Text.RegularExpressions;
using FluentAssertions;

namespace AdventOfCode2024.Day14;

public class Day14Tests
{
    [DebuggerDisplay("{ToString()}")]
    public class Robot
    {
        public Point Position { get; private set; }
        public Point Velocity { get; }
        public Point BoardSize { get; }

        public Robot(Point position, Point velocity, Point boardSize)
        {
            Position = position;
            Velocity = velocity;
            BoardSize = boardSize;
        }

        private static readonly Regex Regex = new Regex(
            @"p=(-?\d{1,3}),(-?\d{1,3}) v=(-?\d{1,3}),(-?\d{1,3})",
            RegexOptions.Compiled | RegexOptions.Multiline
        );

        public static Robot Parse(string input, Point boardSize)
        {
            var parts = Regex.Match(input).Groups;
            return new Robot(
                new Point(int.Parse(parts[1].Value), int.Parse(parts[2].Value)),
                new Point(int.Parse(parts[3].Value), int.Parse(parts[4].Value)),
                boardSize
            );
        }

        public void Move()
        {
            var newX = this.Position.X + this.Velocity.X;
            if (newX < 0)
            {
                newX = this.BoardSize.X + newX;
            }
            else if (newX >= this.BoardSize.X)
            {
                newX = newX - this.BoardSize.X;
            }

            var newY = this.Position.Y + this.Velocity.Y;
            if (newY < 0)
            {
                newY = this.BoardSize.Y + newY;
            }
            else if (newY >= this.BoardSize.Y)
            {
                newY = newY - this.BoardSize.Y;
            }

            this.Position = new Point(newX, newY);
        }

        public override string ToString()
        {
            return this.Position.ToString() + " - " + this.Velocity.ToString();
        }
    }

    public int RobotSafetyFactor(string input, int iterations, Point boardSize)
    {
        var rows = input
            .Replace("\r\n", "\n")
            .Split("\n", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        var robits = rows.Select(x => Robot.Parse(x, boardSize)).ToList();

        for (var i = 0; i < iterations; i++)
        {
            robits.ForEach(x => x.Move());
        }

        var quad1 = 0;
        var quad2 = 0;
        var quad3 = 0;
        var quad4 = 0;

        foreach (var robot in robits)
        {
            if (robot.Position.X < boardSize.X / 2 && robot.Position.Y < boardSize.Y / 2)
            {
                quad1++;
            }
            else if (robot.Position.X >= boardSize.X / 2 + 1 && robot.Position.Y < boardSize.Y / 2)
            {
                quad2++;
            }
            else if (robot.Position.X < boardSize.X / 2 && robot.Position.Y >= boardSize.Y / 2 + 1)
            {
                quad3++;
            }
            else if (robot.Position.X >= boardSize.X / 2 + 1 && robot.Position.Y >= boardSize.Y / 2 + 1)
            {
                quad4++;
            }
        }

        return quad1 * quad2 * quad3 * quad4;
    }

    [Theory]
    [InlineData("p=0,4 v=3,-3", 1, 3, 1)]
    [InlineData("p=0,0 v=1,1", 5, 0, 0)]
    public void RobotMoveTest(string input, int iterations, int expectedX, int expectedY)
    {
        var robot = Robot.Parse(input, new Point(5, 5));
        for (var i = 0; i < iterations; i++)
        {
            robot.Move();
        }

        robot.Position.Should().Be(new Point(expectedX, expectedY));
    }

    [Theory]
    [InlineData(
        @"p=0,4 v=3,-3
p=6,3 v=-1,-3
p=10,3 v=-1,2
p=2,0 v=2,-1
p=0,0 v=1,3
p=3,0 v=-2,-2
p=7,6 v=-1,-3
p=3,0 v=-1,-2
p=9,3 v=2,3
p=7,3 v=-1,2
p=2,4 v=2,-3
p=9,5 v=-3,-3",
        12
    )]
    public void RobotSafetyFactorTest(string input, int safetyFactor)
    {
        var actual = RobotSafetyFactor(input, 100, new Point(11, 7));

        actual.Should().Be(safetyFactor);
    }

    [Fact]
    public async Task FromFile()
    {
        var contents = await File.ReadAllTextAsync("./Day14/Day14.txt");
        var total = RobotSafetyFactor(contents, 100, new Point(101, 103));
        total.Should().Be(209409792);
    }
}
