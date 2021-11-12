using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChattingApplication.Models
{
    public class Chat
    {
        public int Id { get; set; }
        public int Sender { get; set; }
        public int Reciver { get; set; }
        public string Message { get; set; }

    }
}
