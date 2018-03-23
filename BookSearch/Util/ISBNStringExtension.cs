using System;
using System.Linq;
namespace BookSearch.Util
{
    public static class ISBNStringExtension
    {
        public static bool IsISBN(this string str)
        {
            if (str.Length == 10)
            {
                return str.IsISBN10();
            }
            else if (str.Length == 13)
            {
                return str.IsISBN13();
            }
            return false;
        }

        public static bool IsISBN10(this string str)
        {
            if (str.Length != 10) return false;

            try
            {
                var list = str.ToCharArray().Select(c => int.Parse(c.ToString())).ToList();

                var sum = 0;
                for (var i = 0; i < list.Count() - 1; i++)
                {
                    sum += list[i] * (10 - i);
                }
                return (11 - sum % 11) == list.Last();
            }
            catch (FormatException e)
            {
                return false;
            }
        }

        public static bool IsISBN13(this string str)
        {

            if (str.Length != 13) return false;

            try
            {
                var list = str.ToCharArray().Select(c => int.Parse(c.ToString())).ToList();

                var sum = 0;
                for (var i = 0; i < list.Count() - 1; i++)
                {
                    sum += list[i] * (i % 2 == 0 ? 1 : 3);
                }

                var checkDigit = (10 - sum % 10) % 10;
                return checkDigit == list.Last();
            }
            catch (FormatException e)
            {
                return false;
            }
        }
    }
}
