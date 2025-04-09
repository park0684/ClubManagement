using System;

namespace ClubManagement.Common.Hlepers
{
    public static class DataConvertHelper
    {
        public static double ConvertRate(object denominator, object numerator)
        {
            if (!double.TryParse(Convert.ToString(denominator), out double _denominator))
                _denominator = 0;

            if (!double.TryParse(Convert.ToString(numerator), out double _numerator))
                _numerator = 0;

            if (_denominator == 0)
                return _numerator > 0 ? 100 : 0;


            return Math.Round((double)(_numerator / _denominator) * 100, 2);
        }
    }
}
