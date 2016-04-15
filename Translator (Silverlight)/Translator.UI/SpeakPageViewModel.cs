using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Translator.Core;

namespace Translator.UI
{
    class SpeakPageViewModel
    {
        TextSpeaker speaker;

        public SpeakPageViewModel()
        {
            speaker = new TextSpeaker();
        }
    }
}
