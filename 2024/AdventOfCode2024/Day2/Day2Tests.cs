using FluentAssertions;

namespace AdventOfCode2024.Day2;

public class Day2Tests
{
    public static bool IsSafe(int[] numbers, bool isFirstError = false)
    {
        if (numbers.Length == 0)
        {
            return false;
        }

        if (numbers.Length == 1)
        {
            return true;
        }

        int last = numbers[0];
        var isAscending = last < numbers[1];
        var idx = 0;
        foreach (var number in numbers.Skip(1))
        {
            if ((isAscending && number <= last)
                || (!isAscending && number >= last)
                || (Math.Abs(number - last) > 3))
            {
                if (!isFirstError)
                {
                    return IsSafeWithOneLevelRemoved(numbers);
                }

                return false;
            }

            last = number;
            idx++;
        }

        return true;
    }

    private static bool IsSafeWithOneLevelRemoved(int[] numbers)
    {
        for (var i = 0; i < numbers.Length; i++)
        {
            var wot = numbers.ToList();
            wot.RemoveAt(i);
            if (IsSafe(wot.ToArray(), isFirstError: true))
            {
                return true;
            }
        }

        return false;
    }

    [Theory]
    [InlineData(true, 7, 6, 4, 2, 1)]
    [InlineData(false, 1, 2, 7, 8, 9)]
    [InlineData(false, 9, 7, 6, 2, 1)]
    [InlineData(true, 1, 3, 2, 4, 5)]
    [InlineData(true, 8, 6, 4, 4, 1)]
    [InlineData(true, 1, 3, 6, 7, 9)]
    public void IsSafeTest(bool isSafe, params int[] numbers)
    {
        IsSafe(numbers).Should().Be(isSafe);
    }

    [Fact]
    public void FromFile()
    {
        var contents = File.ReadAllText("./Day2/Day2.txt");
        var lines = contents.Split(Environment.NewLine).Select(x => x.Split(" ").Select(int.Parse).ToArray()).ToArray();
        var total = lines.Count(x => IsSafe(x));
        total.Should().Be(386);
    }
}