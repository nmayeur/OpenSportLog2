using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Common.Model
{
    public class TrackPoint
    {
        public int Id { get; set; }
        public TrackSegment TrackSegment { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public decimal Elevation { get; set; }
        public int HeartRate { get; set; }
        public int Cadence { get; set; }
        public int Power { get; set; }
        public decimal Temperature { get; set; }
        public DateTime Time { get; set; }
    }
}