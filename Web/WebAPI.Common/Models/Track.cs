using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Common.Model
{
    public class Track
    {
        public int Id { get; set; }
        public Activity Activity { get; set; }
        public string Name { get; set; }
    }
}