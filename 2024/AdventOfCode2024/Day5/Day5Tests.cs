using FluentAssertions;

namespace AdventOfCode2024.Day5;

public class Day5Tests
{
    public record Rule(int Before, int After);

    private int PageOrderingRules(string input)
    {
        int total = 0;

        var sections = input.Replace("\r\n", "\n").Split("\n\n", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        var rules = sections[0]
            .Split("\n", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(x => x.Split("|"))
            .Select(x => new Rule(int.Parse(x[0]), int.Parse(x[1])))
            .ToList();
        var pages = sections[1]
            .Split("\n", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(x => x.Split(",").Select(int.Parse).ToList())
            .ToList();

        foreach (var page in pages)
        {
            var closurePage = page;
            if (!IsValid(rules, ref closurePage))
            {
                total += page[page.Count / 2];
            }
        }

        return total;
    }

    private bool IsValid(List<Rule> rules, ref List<int> page)
    {
        for (int i = 0; i < page.Count; i++)
        {
            int n = page[i];
            foreach (var rule in rules)
            {
                if (rule.Before == n)
                {
                    var idx = page.IndexOf(rule.After, 0, i);
                    if (idx >= 0)
                    {
                        (page[i], page[idx]) = (page[idx], page[i]);
                        IsValid(rules, ref page);
                        return false;
                    }
                }
                else if (rule.After == n)
                {
                    var idx = page.IndexOf(rule.Before, i + 1);
                    if (idx >= 0)
                    {
                        (page[i], page[idx]) = (page[idx], page[i]);
                         IsValid(rules, ref page);
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
97,61,53", 53)]
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
97,13,75,29,47", 123)]
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
        total.Should().Be(5331);
    }
}