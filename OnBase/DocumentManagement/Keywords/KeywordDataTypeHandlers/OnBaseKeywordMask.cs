namespace HyRest.OnBase.Core;

public class OnBaseKeywordMask
{

    private readonly char[] _staticStringArray;
    private readonly char[] _maskArray;
    private List<char> AllowedFor9 = ['+', '-', '.', '*'];
    private readonly string _formatString;
    public OnBaseKeywordMask(string mask, string? staticString)
    {
        if (!string.IsNullOrEmpty(staticString))
            _staticStringArray = staticString.Trim().Replace("\\32", " ").ToCharArray();
        else
            _staticStringArray = " ".ToCharArray();
        _maskArray = mask.Trim().ToCharArray();
        for (int i = 0; i < _maskArray.Length; i++)
        {
            _formatString = _formatString + $"{{{i}}}";
        }
    }
    public bool TryApplyMask(string value, out string result)
    {
        if (ValidateMask(value))
        {
            result = value;
            return true;
        }
        result = string.Empty;
        var valueArray = value.ToCharArray();
        int incrementV = 0;
        List<char> validChars = [];
        for (int i = 0; i < _maskArray.Length; i++)
        {
            try
            {
                var v = valueArray[i + incrementV];
                var c = _maskArray[i];
                char? s = null;
                try
                {
                    s = _staticStringArray[i];
                }
                catch { }
                if (c == 'A')
                {
                    if (Char.IsLetter(v))
                        validChars.Add(v);
                    else
                        throw new Exception($"The character {v} in position {i} is not a valid letter.");
                }
                else if (c == '0')
                {
                    if (Char.IsDigit(v))
                        validChars.Add(v);
                    else
                        throw new Exception($"The character {v} in position {i} is not a valid number.");
                }
                else if (c == '9')
                {
                    if (Char.IsDigit(v) || AllowedFor9.Contains(v))
                        validChars.Add(v);
                    else
                        throw new Exception($"The character {v} in position {i} is not a valid number or special character.");
                }
                else if (c == 'X')
                {
                    if (!Char.IsControl(v) && !Char.IsWhiteSpace(v) && !Char.IsSurrogate(v) && !Char.IsSymbol(v))
                        validChars.Add(v);
                    else
                        throw new Exception($"The character {v} in position {i} is not a valid alphanumeric or special character.");
                }
                else if (c == 'S' && s != null)
                {
                    if (s == v)
                        validChars.Add(v);
                    else if (s != v)
                    {
                        validChars.Add(s.Value);
                        incrementV--;
                    }
                }
                else
                    throw new Exception($"The mask did not validate.");
            }
            catch (Exception ex)
            {
                result = ex.Message;
                return false;
            }
        }
        result = string.Format(_formatString, validChars.ToArray());
        return ValidateMask(result);
    }
    public bool ValidateMask(string value)
    {
        try
        {
            var valueArray = value.ToCharArray();
            int succesCount = 0;
            for (int i = 0; i < valueArray.Length; i++)
            {
                var v = valueArray[i];
                var c = _maskArray[i];
                char? s = null;
                try
                {
                    s = _staticStringArray[i];
                }
                catch { }
                if (c == 'A' && Char.IsLetter(v))
                    succesCount++;
                else if (c == '0' && Char.IsDigit(v))
                    succesCount++;
                else if (c == '9' && (Char.IsDigit(v) || AllowedFor9.Contains(v)))
                    succesCount++;
                else if (c == 'X' && (!Char.IsControl(v) && !Char.IsWhiteSpace(v) && !Char.IsSurrogate(v) && !Char.IsSymbol(v)))
                    succesCount++;
                else if (c == 'S' && s != null && s == v)
                    succesCount++;
            }
            return succesCount == valueArray.Length;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return false;
        }
    }
}