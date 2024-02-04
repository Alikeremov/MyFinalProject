﻿using QuickFoodDelivery.Application.ViewModels;
using QuickFoodDelivery.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace QuickFoodDelivery.Application.Utilites.Extensions
{
    public static class UserValidator
    {
        public static string Capitalize(this string str)
        {
            return str.Trim().Substring(0, 1).ToUpper() + str.ToUpper().Substring(1).ToLower();
        }

        public static bool CheeckEmail(this string str)
        {
            string pattern = "^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+.[a-zA-Z]{2,}$";
            Regex regex = new Regex(pattern);
            if (regex.IsMatch(str))
            {
                return true;
            }
            return false;
        }
        public static bool IsLetter(this string str)
        {
            bool result = false;

            foreach (Char letter in str)
            {
                result = Char.IsLetter(letter);
            }
            return result;
        }
        public static bool CheeckGender(this int gender)
        {
            foreach (Genders item in Enum.GetValues(typeof(Genders)))
            {
                if (item.Equals(gender))
                {
                    return true;
                }
            }
            return false;

        }
    }
}
