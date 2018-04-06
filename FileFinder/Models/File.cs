using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileFinder.Models
{
    public enum Status
    {
        Normal,
        Damaged,
        InactiveConsumer,
        Full
    }

    public class File
    {
        public int ID { get; set; }

        public Status? Status { get; set; }
        public DateTime? ShredDate { get; set; }

        public int ConsumerID { get; set; }
        public Consumer Consumer { get; set; }

        public int CaseManagerID { get; set; }
        public CaseManager CaseManager { get; set; }

        public int RoomID { get; set; }
        public Room Room { get; set; }
    }
}
