using System.Globalization;
using HyRest.Utilities;

namespace HyRest.OnBase.Core;

public sealed class CurrencyFormat : OnBaseItemTypeService<OnBaseCore, CurrencyFormatModel>
{
    private CultureInfo _formatProvider;
    public IFormatProvider FormatProvider => _formatProvider;
    public CurrencyFormat(OnBaseCore core, CurrencyFormatModel currencyFormat) : base(core,currencyFormat)
    {
        _formatProvider = CreateCultureFromCurrencyFormat(Item);
    }
    public string CurrencySymbol => Item.CurrencySymbol ?? string.Empty;
    public long DecimalPlaces => Item.DecimalPlaces;
    public string DecimalSymbol => Item.DecimalSymbol ?? string.Empty;
    public long GroupingDigitas => Item.GroupingDigits;
    public string IsoCurrencyName => Item.SystemName ?? string.Empty;
    public bool HasCurrencySymbol => Item.HasCurrencySymbol;
    public bool HasGroupSeparator => Item.HasGroupSeparator;
    public bool HasLeadingZero => Item.HasLeadingZero;
    public bool HasMinusSign => Item.HasMinusSign;
    public bool HasWhitespace => Item.HasWhitespace;
    public bool HasWhitespaceOnNegative => Item.HasWhitespaceOnNegative;
    public bool IsMinusSignAfter => Item.IsMinusSignAfter;
    public bool IsSymbolAfter => Item.IsSymbolAfter;
    public bool IsSymbolAfterOnNegative => Item.IsSymbolAfterOnNegative;
    public bool IsSymbolInsideNegative => Item.IsSymbolInsideNegative;

    private static CultureInfo CreateCultureFromCurrencyFormat(CurrencyFormatModel Item)
    {
        if(Item.SystemName != null)
        {
            foreach(var c in CultureInfo.GetCultures(CultureTypes.AllCultures))
            {
                if (c.ThreeLetterISOLanguageName == Item.SystemName)
                    return c;
            }
        }
        var culture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
        var numberFormat = (NumberFormatInfo)culture.NumberFormat.Clone();

        // Currency symbol
        if (Item.HasCurrencySymbol && !string.IsNullOrEmpty(Item.CurrencySymbol))
            numberFormat.CurrencySymbol = Item.CurrencySymbol;

        // Decimal settings
        numberFormat.CurrencyDecimalDigits = (int)Item.DecimalPlaces;
        if (!string.IsNullOrEmpty(Item.DecimalSymbol))
            numberFormat.CurrencyDecimalSeparator = Item.DecimalSymbol;

        // Grouping settings
        if (Item.HasGroupSeparator && !string.IsNullOrEmpty(Item.GroupingSymbol))
            numberFormat.CurrencyGroupSeparator = Item.GroupingSymbol;
        numberFormat.CurrencyGroupSizes = new[] { (int)Item.GroupingDigits };
        numberFormat.CurrencyNegativePattern = GetCurrencyNegativePattern(Item);
        numberFormat.CurrencyPositivePattern = GetCurrencyPositivePattern(Item);

        culture.NumberFormat = numberFormat;
        return culture;
    }

    private static int GetCurrencyPositivePattern(CurrencyFormatModel Item)
    {
        if (Item.IsSymbolAfter)
            return Item.HasWhitespace ? 3 : 1;
        else
            return Item.HasWhitespace ? 2 : 0;
    }
    private static int GetCurrencyNegativePattern(CurrencyFormatModel Item)
    {
        bool symbolAfter = Item.IsSymbolAfterOnNegative;
        bool hasWhitespace = Item.HasWhitespaceOnNegative;
        bool hasMinusSign = Item.HasMinusSign;
        bool minusAfter = Item.IsMinusSignAfter;
        bool symbolInsideNeg = Item.IsSymbolInsideNegative;

        if (!hasMinusSign) // Parentheses
        {
            if (symbolAfter)
                return hasWhitespace ? 15 : 4; // (n $) or (n$)
            else
                return hasWhitespace ? 14 : 0; // ($ n) or ($n)
        }

        if (symbolAfter)
        {
            if (minusAfter)
                return hasWhitespace ? 10 : 7; // n $- or n$-
            else
                return hasWhitespace ? 8 : 5; // -n $ or -n$
        }
        else
        {
            if (minusAfter)
                return hasWhitespace ? 11 : 3; // $ n- or $n-
            else
                return hasWhitespace ? 9 : 1; // -$ n or -$n
        }
    }
    public override string? ToJson()
        => JsonUtility.Serialize(this);
}
