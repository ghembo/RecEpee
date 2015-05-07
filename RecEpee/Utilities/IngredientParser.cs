using RecEpee.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace RecEpee.Framework
{
    static class IngredientParser
    {
        private static Regex containsANumber = new Regex(@"\d+");
        private static Regex getNotNumericPart = new Regex(@"[^\d]+");

        static public Ingredient Parse(string ingredientString)
        {
            if (String.IsNullOrWhiteSpace(ingredientString))
            {
                return null;
            }

            var words = getWords(ingredientString);
            
            return new Ingredient()
            {
                Unit = getUnit(words),            
                Quantity = getQuantity(words),
                Name = getName(words),
            };
        }

        private static IList<string> getWords(string ingredientString)
        {
            return ingredientString.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        private static int getQuantity(IList<string> words)
        {
            int quantity = 0;

            if (words.Count > 1 && int.TryParse(words.Last(), out quantity) == true)
            {
                words.RemoveLast();
            }

            return quantity;
        }

        private static string getUnit(IList<string> words)
        {
            int quantity = 0;
            string unit = null;

            if (words.Count > 2 && int.TryParse(words[words.Count - 2], out quantity) == true)
            {
                unit = words.Last();
                words.RemoveLast();
            }
            else if (words.Count > 1)
            {
                var numberMatch = containsANumber.Match(words.Last());

                if (numberMatch.Success)
                {
                    var quantityString = numberMatch.Value;
                    unit = getNotNumericPart.Match(words.Last()).Value;
                    words[words.Count - 1] = quantityString;                    
                }
            }

            return unit;
        }

        private static string getName(IList<string> words)
        {
            return String.Join(" ", words);
        }

        public static void RemoveLast<T>(this IList<T> list)
        {
            list.RemoveAt(list.Count - 1);
        }
    }
}
