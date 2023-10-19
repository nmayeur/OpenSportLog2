using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Common.Model
{
    public class TrackSegment
    {
        public int Id { get; set; }
        public Track Track { get; set; }
    }
}