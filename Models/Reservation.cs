using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coffee_Project.Models
{
    public class Reservation
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime ReservationDate { get; set; }
        public TimeSpan ReservationTime { get; set; }
        public int PersonCount { get; set; }
    }
}