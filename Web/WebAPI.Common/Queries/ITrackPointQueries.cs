using WebAPI.Common.Model;

namespace WebAPI.Common.Queries
{
    public interface ITrackPointQueries
    {
        Task<IList<TrackPoint>> GetTrackPointsFromActivityIdAsync(int activityId);
    }
}
