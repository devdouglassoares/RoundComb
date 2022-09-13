using System;
using Core;

namespace Common.Services.Interfaces
{
    public interface IAnnouncementService : IDependency
    {
        void LogAnnouncementViewed(string announcementKey, long userId);
        bool IsAnnouncementViewed(string announcementKey, DateTime lastUpdateDate, long userId);
    }
}
