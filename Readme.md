# Supported Patterns
## Wildcards:
  * — matches any sequence of characters
  ? — matches a single character
## Logical Operators:
  and — both expressions must be present
  or — at least one of the expressions must be present
## Grouping:
  Use parentheses () to group conditions
## Pattern Examples:
  "Error:* and database" — searches for lines containing both "Error" and "database".
  "(Error:* or Warning:*) and (space or disk)" — searches for lines containing "Error" or "Warning" and "space" or "disk".
