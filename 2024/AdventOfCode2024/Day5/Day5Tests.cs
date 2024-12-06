using FluentAssertions;

namespace AdventOfCode2024.Day5;

public class Day5Tests
{
    public record Rule(int Before, int After);

    private int PageOrderingRules(string input)
    {
        int total = 0;

        var sections = input.Split("\n\n", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        var rules = sections[0]
            .Split("\n", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(x => x.Split("|"))
            .Select(x => new Rule(int.Parse(x[0]), int.Parse(x[1])))
            .ToArray();
        var pages = sections[1]
            .Split("\n", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(x => x.Split(",").Select(int.Parse).ToArray())
            .ToArray();

        foreach (var page in pages)
        {
            if (IsValid(rules, page))
            {
                total += page[page.Length / 2];
            }
        }

        return total;
    }

    private bool IsValid(Rule[] rules, int[] page)
    {
        for (int i = 0; i < page.Length; i++)
        {
            int n = page[i];
            foreach (var rule in rules)
            {
                if (rule.Before == n)
                {
                    if (page.Take(i).Any(x => x == rule.After))
                    {
                        return false;
                    }
                }
                else if (rule.After == n)
                {
                    if (page.Skip(i + 1).Any(x => x == rule.Before))
                    {
                        return false;
                    }
                }
            }
        }

        return true;
    }

    [Theory]
    [InlineData(@"
47|61
75|47
97|61
97|47
97|53
53|61

75,47,61
97,61,53", 47)]
    [InlineData(@"
47|53
97|13
97|61
97|47
75|29
61|13
75|53
29|13
97|29
53|29
61|53
97|53
61|29
47|13
75|47
97|75
47|61
75|61
47|29
75|13
53|13

75,47,61,53,29
97,61,53,29,13
75,29,13
75,97,47,61,53
61,13,29
97,13,75,29,47", 143)]
    public void PageOrderingRulesTest(string input, int expected)
    {
        var actual = PageOrderingRules(input);

        actual.Should().Be(expected);
    }

    [Fact]
    public async Task FromFile()
    {
        var contents = await File.ReadAllTextAsync("./Day5/Day5.txt");
        var total = PageOrderingRules(contents);
        total.Should().Be(5588);
    }
}