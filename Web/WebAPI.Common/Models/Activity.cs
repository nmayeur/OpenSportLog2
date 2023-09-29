using System.Collections.ObjectModel;

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
        public DateTimeOffset Time { get; set; }
        public TimeSpan TimeSpan { get; set; }
    }
}