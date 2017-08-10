using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace RadCms.Helpers
{
    public class ValidationHelper
    {
        [Flags]
        public enum PasswordRules
        {
            /// <summary>
            /// Password must contain a digit
            /// </summary>
            Digit = 1,
            /// <summary>
            /// Password must contain an uppercase letter
            /// </summary>
            UpperCase = 2,
            /// <summary>
            /// Password must contain a lowercase letter
            /// </summary>
            LowerCase = 4,
            /// <summary>
            /// Password must have both upper and lower case letters
            /// </summary>
            MixedCase = 6,
            /// <summary>
            /// Password must include a non-alphanumeric character
            /// </summary>
            SpecialChar = 8,
            /// <summary>
            /// Have both upper and lower case letters
            /// Include a non-alphanumeric character
            /// </summary>
            DOT = 15,
            /// <summary>
            /// All rules should be checked
            /// </summary>
            All = 15,
            /// <summary>
            /// No rules should be checked
            /// </summary>
            None = 0
        }

        public static bool IsPasswordValid(string password,
                                   PasswordRules rules,
                                   params string[] ruleOutList)
        {
            bool result = true;
            const string lower = "abcdefghijklmnopqrstuvwxyz";
            const string upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string digits = "0123456789";
            string allChars = lower + upper + digits;
            //Check Lowercase if rule is enforced
            if (Convert.ToBoolean(rules & PasswordRules.LowerCase))
            {
                result &= (password.IndexOfAny(lower.ToCharArray()) >= 0);
            }
            //Check Uppercase if rule is enforced
            if (Convert.ToBoolean(rules & PasswordRules.UpperCase))
            {
                result &= (password.IndexOfAny(upper.ToCharArray()) >= 0);
            }
            //Check to for a digit in password if digit is required
            if (Convert.ToBoolean(rules & PasswordRules.Digit))
            {
                result &= (password.IndexOfAny(digits.ToCharArray()) >= 0);
            }
            //Check to make sure special character is included if required
            if (Convert.ToBoolean(rules & PasswordRules.SpecialChar))
            {
                result &= (password.Trim(allChars.ToCharArray()).Length > 0);
            }
            if (ruleOutList != null)
            {
                for (int i = 0; i < ruleOutList.Length; i++)
                    result &= (password != ruleOutList[i]);
            }
            return result;
        }


        public static bool isEmailValid(string inputEmail)
        {
            inputEmail = String.IsNullOrEmpty(inputEmail) ? "" : inputEmail;
            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                  @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                  @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex re = new Regex(strRegex);
            if (re.IsMatch(inputEmail))
                return (true);
            else
                return (false);
        }
    }
}
