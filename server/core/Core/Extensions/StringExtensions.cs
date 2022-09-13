using System;
using System.Text;

namespace Core.Extensions
{
    public static class StringExtensions
    {
        public static string GenerateRandomCode(int size)
        {
            var random = new Random();
            const string input = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwyxz0123456789";
            var builder = new StringBuilder();
            for (var i = 0; i < size; i++)
            {
                var ch = input[random.Next(0, input.Length)];
                builder.Append(ch);
            }
            return builder.ToString();
        }

    }
}