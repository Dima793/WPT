using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ViewModel
{
    class TranslatePageViewModel
    {
        public ICommand StartListenUserSpeech { get; set; }

        private bool canExecute = true;

        public void ChangeCanExecute(object obj)
        {
            canExecute = !canExecute;
        }

        public string ButtonContent
        {
            get
            {
                return " Start listen\nuser speach";
            }
        }

        public void CallListenUserSpeach(object obj)
        {
            Model.Translator.ListenUserSpeach(obj.ToString());
        }

        public TranslatePageViewModel()
        {
            StartListenUserSpeech = new Command.StartListenUserSpeechCommand(CallListenUserSpeach, param => this.canExecute);
        }
    }
}
