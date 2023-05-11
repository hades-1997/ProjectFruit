namespace ProjectFruit.Helpers
{
    public class DataFormatHelper
    {
        public static string Currency(decimal value)
        {
            return string.Format("{0:C}", value);
        }

        public static string FormatTime(DateTime value)
        {
            return value.ToString("dd/MM/yyyy HH:mm:ss");
        }

        public static string PhoneNumber(string value)
        {
            return string.Format("{0:(###) ###-####}", double.Parse(value));
        }
    }
}
