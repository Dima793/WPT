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
        private ICommand startListenUserSpeech;

        private bool canExecute = true;

        public void ChangeCanExecute(object obj)
        {
            canExecute = !canExecute;
        }

        public string ButtonContent
        {
            get
            {
                return " Start listen\nuser speech";
            }
        }

        public bool CanExecute
        {
            get
            {
                return this.canExecute;
            }

            set
            {
                if (this.canExecute == value)
                {
                    return;
                }

                this.canExecute = value;
            }
        }

        public ICommand StartListenUserSpeech
        {
            get
            {
                return startListenUserSpeech;
            }
            set
            {
                startListenUserSpeech = value;
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
