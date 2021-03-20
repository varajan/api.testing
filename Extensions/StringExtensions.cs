﻿using System;
using System.Text.RegularExpressions;

namespace api.testing.Extensions
{
    public static class StringExtensions
    {
        public static string SubString(this string line, string from, string to)
        {
            var start = line.IndexOf(from, StringComparison.OrdinalIgnoreCase) + from.Length;
            var count = line.IndexOf(to, Math.Min(start, line.Length), StringComparison.OrdinalIgnoreCase) - start;

            return line.IndexOf(from, StringComparison.OrdinalIgnoreCase) >= 0 ? line.Substring(start, count) : string.Empty;
        }

        public static string SubString(this string line, string from)
        {
            var start = line.ContainsIgnoreCase(from)
                ? line.IndexOf(from, StringComparison.OrdinalIgnoreCase) + from.Length
                : line.Length;

            return line.Substring(start);
        }

        public static bool ContainsIgnoreCase(this string line, string word) => line.IndexOf(word ?? string.Empty, StringComparison.OrdinalIgnoreCase) >= 0;

        public static bool IsValidEmail(this string email) => new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$").IsMatch(email);
    }
}