﻿using WebAPI.Common.Model;

namespace WebAPI.Common.Queries
{
    public interface IActivityQueries
    {
        Task<Activity> GetActivityByIdAsync(int activityId);
        Task<IList<Activity>> GetActivitiesByAthleteAsync(int athleteId, int pageSize, int pageIndex);
        Task<int> GetActivitiesByAthleteCountAsync(int athleteId);
    }
}