using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileFinder.Models
{
    public enum Status
    {
        OK,
        InactiveConsumer,
        Full,
        Damaged
    }

    public class File
    {
        public int ID { get; set; }
        public int Quantity { get; set; } = 1;
        public Status? Status { get; set; }
        public DateTime? ShredDate { get; set; }

        public int ConsumerID { get; set; }
        public Consumer Consumer { get; set; }

        public int CaseManagerID { get; set; }
        public CaseManager CaseManager { get; set; }

        public int RoomID { get; set; }
        public Room Room { get; set; }

        public override bool Equals(object obj)
        {
            if(obj == this)
            {
                return true;
            }

            if(obj == null || obj.GetType() != GetType())
            {
                return false;
            }

            // Files are equal if they have the same ConsumerID, CaseManagerID, and RoomID
            File fileObj = obj as File;
            return ConsumerID == fileObj.ConsumerID && CaseManagerID == fileObj.CaseManagerID && RoomID == fileObj.RoomID;
        }

        public override int GetHashCode()
        {
            //return base.GetHashCode();
            return ID;
        }
    }
}
