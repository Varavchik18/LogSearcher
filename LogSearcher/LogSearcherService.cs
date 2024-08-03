using System.Collections.Concurrent;
using System.Text.RegularExpressions;

public class LogSearcherService
{
    private readonly ConcurrentDictionary<string, List<string>> _cache = new ConcurrentDictionary<string, List<string>>();

    public async Task<IEnumerable<string>> SearchAsync(string filePath, string pattern)
    {
        if (_cache.TryGetValue(pattern, out var cachedResults))
        {
            return cachedResults;
        }

        var results = new ConcurrentBag<string>();
        var regexPattern = BuildRegexFromPattern(pattern);
        var regex = new Regex(regexPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);

        var lines = await File.ReadAllLinesAsync(filePath);

        Parallel.ForEach(lines, (line) =>
        {
            if (regex.IsMatch(line))
            {
                results.Add(line);
            }
        });

        var resultList = new List<string>(results);
        _cache.TryAdd(pattern, resultList);
        return resultList;
    }

    private string BuildRegexFromPattern(string pattern)
    {
        var tokens = Tokenize(pattern);

        var expressions = new Stack<string>();
        var operators = new Stack<string>();

        foreach (var token in tokens)
        {
            if (token == "(")
            {
                operators.Push(token);
            }
            else if (token == ")")
            {
                while (operators.Count > 0 && operators.Peek() != "(")
                {
                    ProcessOperator(expressions, operators.Pop());
                }
                operators.Pop(); // Remove the opening parentheses
            }
            else if (token.Equals("and", StringComparison.OrdinalIgnoreCase) || token.Equals("or", StringComparison.OrdinalIgnoreCase))
            {
                while (operators.Count > 0 && Precedence(operators.Peek()) >= Precedence(token))
                {
                    ProcessOperator(expressions, operators.Pop());
                }
                operators.Push(token);
            }
            else
            {
                // Convert wildcards to regex
                var convertedToken = Regex.Escape(token).Replace(@"\*", ".*").Replace(@"\?", ".");
                expressions.Push(convertedToken);
            }
        }

        while (operators.Count > 0)
        {
            ProcessOperator(expressions, operators.Pop());
        }

        // Wrap the final expression to match any line
        return "^.*" + expressions.Pop() + ".*$";
    }

    private void ProcessOperator(Stack<string> expressions, string op)
    {
        if (op.Equals("and", StringComparison.OrdinalIgnoreCase))
        {
            var right = expressions.Pop();
            var left = expressions.Pop();
            expressions.Push($"(?=.*{left})(?=.*{right})");
        }
        else if (op.Equals("or", StringComparison.OrdinalIgnoreCase))
        {
            var right = expressions.Pop();
            var left = expressions.Pop();
            expressions.Push($"({left}|{right})");
        }
    }

    private int Precedence(string op)
    {
        if (op.Equals("and", StringComparison.OrdinalIgnoreCase))
        {
            return 2; 
        }
        else if (op.Equals("or", StringComparison.OrdinalIgnoreCase))
        {
            return 1; 
        }
        return 0;
    }

    private List<string> Tokenize(string pattern)
    {
        var tokens = new List<string>();
        var currentToken = string.Empty;

        for (int i = 0; i < pattern.Length; i++)
        {
            char c = pattern[i];

            if (char.IsWhiteSpace(c))
            {
                if (!string.IsNullOrWhiteSpace(currentToken))
                {
                    tokens.Add(currentToken);
                    currentToken = string.Empty;
                }
            }
            else if (c == '(' || c == ')')
            {
                if (!string.IsNullOrWhiteSpace(currentToken))
                {
                    tokens.Add(currentToken);
                    currentToken = string.Empty;
                }
                tokens.Add(c.ToString());
            }
            else
            {
                currentToken += c;

                // Check if currentToken matches an operator
                if (currentToken.Equals("and", StringComparison.OrdinalIgnoreCase) ||
                    currentToken.Equals("or", StringComparison.OrdinalIgnoreCase))
                {
                    tokens.Add(currentToken);
                    currentToken = string.Empty;
                }
            }
        }

        if (!string.IsNullOrWhiteSpace(currentToken))
        {
            tokens.Add(currentToken);
        }

        return tokens;
    }

}
