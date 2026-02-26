namespace Bulgarikon.Common.Helpers
{
    public static class YearFormatter
    {
        public static string Format(int year)
        {
            if (year == 0)
                return "—";

            if (year > 0)
                return $"{year} г.";

            int absYear = Math.Abs(year);

            if (absYear >= 1001)
            {
                int millennium = (absYear - 1) / 1000 + 1;
                return $"{millennium} хилядолетие пр. Хр.";
            }

            int century = (absYear - 1) / 100 + 1;
            return $"{century} век пр. Хр.";
        }
    }
}