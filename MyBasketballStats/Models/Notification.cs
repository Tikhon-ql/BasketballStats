using MyBasketballStats.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBasketballStats.Models
{
    public class Notification
    {
        public string Id { get; set; }
        public string Message { get; set; }
        public NotificationType Type { get; set; }
        public bool IsChecked { get; set; } = false;
        public string SenderId { get; set; }
        public DateTime Date { get; set; }
        public Person Receiver { get; set; }
        public string ReceiverId { get; set; }
    }
}
