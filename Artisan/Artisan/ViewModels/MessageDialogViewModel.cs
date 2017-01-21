using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Artisan.MVVMShared;

namespace Artisan.ViewModels
{
    public class MessageDialogViewModel : BindableBase
    {
        public string Message { get; set; }
        public string Heading { get; set; }

        public MessageDialogViewModel(string heading,  string errorMessage)
        {
            Heading = heading;
            Message = errorMessage;
        }
    }
}
