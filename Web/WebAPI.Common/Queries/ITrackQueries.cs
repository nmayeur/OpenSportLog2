using WebAPI.Common.Model;

namespace WebAPI.Common.Queries
{
    public interface ITrackQueries
    {
        Task<IList<Track>> GetTracksByActivityAsync(int activityId, int pageSize, int pageIndex);
        Task<int> GetTracksByActivityCountAsync(int activityId);
    }
}