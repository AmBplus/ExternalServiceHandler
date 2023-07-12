using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace PefectMoney.Shared.Helper
{
    public static class ConvertHelper
    {
        public static bool IsIranianPhoneNumber(string text)
        {
            // تعریف الگوی منظم برای تشخیص شماره تلفن ایرانی
            string pattern = @"^(\+98|0)?9\d{9}$";

            // استفاده از تابع Match برای تست الگو در متن ورودی
            Match match = Regex.Match(text, pattern);

            // بررسی مطابقت الگو با متن ورودی
            return match.Success;
        }

        public static string ExtractNumberFromText(string text)
        {
            
            string pattern = @"(09\d{9}|\+989\d{9}|\+98\d{9})";

            // ایجاد یک شیء از کلاس Regex
            Regex regex = new Regex(pattern);

            // پیدا کردن تمام تطبیق‌های عبارت منظم در متن
            MatchCollection matches = regex.Matches(text);

            string result = "";
            // نمایش شماره‌های تلفن جدا شده
            foreach (Match match in matches)
            {
                result  += match.Value;
            }

            return result;
        }
    }
}
