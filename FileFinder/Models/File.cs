using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using FileFinder.ViewModels;

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
    // FIELDS
        public int ID { get; set; }
        public int Quantity { get; set; } = 1;
        public Status? Status { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? ShredDate { get; private set; }

        [ForeignKey("Consumer")]
        public int ConsumerID { get; set; }

        public Consumer Consumer { get; set; }

        [ForeignKey("CaseManager")]
        public int CaseManagerID { get; set; }

        [Display(Name = "Case Manager")]
        public CaseManager CaseManager { get; set; }

        [ForeignKey("Room")]
        public int RoomID { get; set; }
        //[ForeignKey("Room")]
        public Room Room { get; set; }

    // METHODS

        public void SetShredDate(EditConsumerViewModel consumerVM)
        {
            if(consumerVM.EndDate.HasValue)
            {
                DateTime newDate = consumerVM.EndDate.Value;
                this.ShredDate = newDate.AddYears(7).AddDays(1);
            }
        }

        public override bool Equals(object obj)
        {
            // Is it same object?
            if(obj == this)
            {
                return true;
            }
            // Is it null or not same type?
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
