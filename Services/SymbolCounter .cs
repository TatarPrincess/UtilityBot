using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace UtilityBot.Services
{
    public class SymbolCounter : ICount
    {
        public int Count(string text)
        { 
          return text.Length;
        }        
    }
}
