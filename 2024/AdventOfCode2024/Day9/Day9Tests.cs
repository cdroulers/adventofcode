using System.Text;
using FluentAssertions;

namespace AdventOfCode2024.Day9;

public class Day9Tests
{
    public List<int?> DiskMapToId(string input)
    {
        bool isEmpty = false;
        var result = new List<int?>();
        int idx = 0;
        foreach (char c in input)
        {
            var parsed = short.Parse(c.ToString());
            int? val = isEmpty ? null : idx;

            var append = Enumerable.Range(0, parsed).Select(x => val);
            result.AddRange(append);

            idx += isEmpty ? 0 : 1;
            isEmpty = !isEmpty;
        }

        return result;
    }

    public List<int?> CompactDisk(List<int?> input)
    {
        var emptyIdx = input.IndexOf(null);
        for (var i = input.Count - 1; i >= 0; i--)
        {
            if (input[i] == null)
            {
                continue;
            }

            var len = 0;
            var end = i;
            var c = input[i];
            if (c == 0)
            {
                // 0 block can never be moved.
                continue;
            }
            do
            {
                len++;
                i--;
            } while (input[i] == c);

            //Fix idx because of reasons.
            i++;

            var start = i;

            var range = FindEmptyRange(input, emptyIdx, len);
            if (range != null && range.Value.Start.Value < i)
            {
                // Replace empty
                for (var ri = range.Value.Start.Value; ri <= range.Value.End.Value; ri++)
                {
                    input[ri] = c;
                }
                
                // Remove moved block
                for (var ri = start; ri <= end; ri++)
                {
                    input[ri] = null;
                }
            }
        }

        return input;
    }

    private Range? FindEmptyRange(List<int?> input, int startIdx, int length)
    {
        while (true)
        {
            var idx = input.IndexOf(null, startIdx);
            if (idx == -1)
            {
                return null;
            }

            var range = input.Skip(idx).Take(length).ToList();
            if (range.Count >= length && range.All(x => x == null))
            {
                return new Range(idx, idx + length - 1);
            }

            startIdx = idx + 1;
        }
    }

    public long Checksum(List<int?> input)
    {
        long total = 0;
        for (var i = 0; i < input.Count; i++)
        {
            total += i * input[i].GetValueOrDefault();
        }

        return total;
    }

    [Theory]
    [InlineData("12345", "0..111....22222")]
    [InlineData("2333133121414131402", "00...111...2...333.44.5555.6666.777.888899")]
    public void DiskMaptoIdTest(string input, string expected)
    {
        var actual = DiskMapToId(input);
        var stringified = string.Join(string.Empty, actual.Select(x => x == null ? "." : x.ToString()));
        stringified.Should().Be(expected);
    }

    [Theory]
    [InlineData("0..111....22222", "0..111....22222")]
    [InlineData("00...111...2...333.44.5555.6666.777.888899", "00992111777.44.333....5555.6666.....8888..")]
    public void CompactDiskTest(string input, string expected)
    {
        var parsed = input.ToCharArray().Select(x => x == '.' ? (int?)null : int.Parse(x.ToString())).ToList();
        var actual = CompactDisk(parsed);
        var stringified = string.Join(string.Empty, actual.Select(x => x == null ? "." : x.ToString()));
        stringified.Should().Be(expected);
    }

    [Theory]
    [InlineData("022111222", 60)]
    [InlineData("0099811188827773336446555566", 1928)]
    [InlineData("00992111777.44.333....5555.6666.....8888..", 2858)]
    public void ChecksumTest(string input, long expected)
    {
        var parsed = input.ToCharArray().Select(x => x == '.' ? (int?)null : int.Parse(x.ToString())).ToList();
        var actual = Checksum(parsed);
        actual.Should().Be(expected);
    }

    [Fact]
    public async Task FromFile()
    {
        var contents = await File.ReadAllTextAsync("./Day9/Day9.txt");
        var diskMap = DiskMapToId(contents);
        var compacted = CompactDisk(diskMap);
        var total = Checksum(compacted);
        total.Should().Be(6373055193464L);
    }
}