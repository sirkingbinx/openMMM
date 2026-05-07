// Quick parser for a custom txt format
// Comments prefixed with #, blank lines are filtered

using System.Collections.Generic;

namespace AwesomeTxt;

public class AwesomeTxtFile
{
    public string[] Results;

    public AwesomeTxtFile(string text)
    {
        List<string> lines = [];
        Results = text.Split("\n").Where(l => l.Trim() != string.Empty && !l.Trim().StartsWith('#')).ToArray();
    }
}