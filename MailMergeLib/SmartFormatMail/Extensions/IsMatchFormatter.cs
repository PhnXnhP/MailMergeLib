﻿using System;
using System.Linq;
using System.Text.RegularExpressions;
using MailMergeLib.SmartFormatMail.Core.Extensions;

namespace MailMergeLib.SmartFormatMail.Extensions
{
    /// <summary>
    /// Formatter with evaluation of regular expressions.
    /// </summary>
    /// <remarks>
    /// Syntax:
    /// {value:ismatch(regex): format | default}
    /// Or in context of a list:
    /// {myList:list:{:ismatch(^regex$):{:format}|'no match'}|, | and }
    /// </remarks>
    [System.Obsolete("Use classes in namespace 'SmartFormat' instead of 'MailMergeLib.SmartFormatMail'", false)] public class IsMatchFormatter : IFormatter
    {
        public string[] Names { get; set; } = { "ismatch" };

        public bool TryEvaluateFormat(IFormattingInfo formattingInfo)
        {
            var expression = formattingInfo.FormatterOptions;
            var formats = formattingInfo.Format.Split('|');

            if (formats.Count == 0)
                return true;

            if (formats.Count != 2)
                throw new FormatException("Exactly 2 format options are required.");

            var regEx = new Regex(expression, RegexOptions);

            if (regEx.IsMatch(formattingInfo.CurrentValue.ToString()))
                formattingInfo.Write(formats[0], formattingInfo.CurrentValue);
            else if (formats.Count == 2)
                formattingInfo.Write(formats[1], formattingInfo.CurrentValue);

            return true;
        }

        public RegexOptions RegexOptions { get; set; }
    }
}