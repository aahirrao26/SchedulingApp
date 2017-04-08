using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalAppointmentScheduler.Core.Data
{
    [MetadataType(typeof(SlotsMetadata))]
    public partial class Slots
    {        
        public String SlotTime
        {
            get
            {
                //return StartTime + "-" + EndTime;
                DateTime time = DateTime.Today.Add(StartTime.Value);
                string DisplayStartTime = time.ToString("hh:mm tt");

                time = DateTime.Today.Add(EndTime.Value);
                string DisplayEndTime = time.ToString("hh:mm tt");

                return DisplayStartTime + "-" + DisplayEndTime;
            }
        }
    }

    public class SlotsMetadata
    {
        [Display(Name ="Time Slot")]
        public String SlotTime { get; }
    }

        /// <summary>
        /// This class is created to bind with the Json object while booking appointment
        /// </summary>
        public class AvailableSlots
        {
        public TimeSpan? StartTime { get; set; }

        public TimeSpan? EndTime { get; set; }
        public String SlotTime
        {
            get
            {
                DateTime time = DateTime.Today.Add(StartTime.Value);
                string DisplayStartTime = time.ToString("hh:mm tt");

                time = DateTime.Today.Add(EndTime.Value);
                string DisplayEndTime = time.ToString("hh:mm tt");

                return DisplayStartTime + "-" + DisplayEndTime;
            }
        }

        public int ID { get; set; }
    }
}
