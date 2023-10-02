using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Common.Model
{
    public class Activity
    {
        public int Id { get; set; }
        public Athlete Athlete { get; set; }
        public string OriginId { get; set; }
        public string OriginSystem { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public int Calories { get; set; }
        public int HeartRate { get; set; }
        public int Cadence { get; set; }
        public int Power { get; set; }
        public int Temperature { get; set; }
        public ACTIVITY_SPORT Sport { get; set; }
        public DateTime Time { get; set; }
        [NotMapped]
        public TimeSpan TimeSpan
        {
            get { return TimeSpan.FromTicks(TimeSpanTicks); }
            set { 
                TimeSpanTicks = value.Ticks; 
            }
        }
        public Int64 TimeSpanTicks { get; set; }
    }
}