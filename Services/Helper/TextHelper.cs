namespace Services.Helper;

public static class TextHelper {
    public static string CapitalText(string text) {
        if (text == null || text == string.Empty) {
            return string.Empty;
        }

        return string.Concat(text[0].ToString().ToUpper(), text.AsSpan(1));
    }
}
