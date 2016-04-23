using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translator.Core
{
    public class Language
    {
        public string Code { get; set; }

        public string Full { get; set; }

        public Language(string code, string full)
        {
            Code = code;
            Full = full;
        }

        public Language()
        {
            Code = "default code";
            Full = "default full";
        }
    }
}
