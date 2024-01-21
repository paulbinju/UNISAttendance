using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HilalComputersUnis.Models
{
    public class myModels
    {
    }

    public class StaffEntry
    {
        public int UID { get; set; }
        public string Name { get; set; }
        public DateTime DateTimez { get; set; }
        public DateTime Dateonly { get; set; }

    }

    public class StaffPunching
    {
        public int UID { get; set; }
        public string Name { get; set; }
        public DateTime DateTimeIn { get; set; }
        public DateTime DateTimeOut { get; set; }

    }

    public class StaffMovement
    {
        public int UID { get; set; }
        public string Name { get; set; }
        public DateTime Entrytime { get; set; }
        public DateTime Exittime { get; set; }
    }
}