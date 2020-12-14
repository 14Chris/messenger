using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Messenger.Facade.Models
{
    public class SocketRequestModel
    {
        public string type { get; set; }
        public dynamic data { get; set; }
    }
}
