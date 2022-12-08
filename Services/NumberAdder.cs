using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityBot.Services
{
    public class NumberAdder : ICalculate
    {
        public int Calculate(String text)
        {
            string trimmedText = text.Replace(" ", "");
            if (trimmedText.All(Char.IsDigit))
            {                
                string[] strNumbers = text.Split();
                int sum = 0;
                
                foreach (string num in strNumbers)
                {
                    sum += Int32.Parse(num);                    
                }
                return sum;
            }
            else throw new ArgumentException("Необходимо ввести только числа через пробел");
        }
    }
}
