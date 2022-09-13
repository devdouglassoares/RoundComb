using Common.Core.Entities;
using Common.Core.Repositories;
using Common.Services.Interfaces;
using System;

namespace Common.Services.Implementations
{
    public class AnnouncementService : IAnnouncementService
    {
        private readonly IRepository _repository;

        public AnnouncementService(IRepository repository)
        {
            _repository = repository;
        }

        public void LogAnnouncementViewed(string announcementKey, long userId)
        {
            var audit = _repository.Get<AnnouncementViewAudit>(x => x.UserId == userId && x.AnnouncementKey == announcementKey);

            if (audit == null)
            {
                audit = new AnnouncementViewAudit
                {
                    UserId = userId,
                    AnnouncementKey = announcementKey,
                    Date = DateTime.UtcNow
                };

                _repository.Insert(audit);
                _repository.SaveChanges();
            }
            else
            {
                audit.Date = DateTime.UtcNow;
                _repository.Update(audit);
                _repository.SaveChanges();
            }
        }

        public bool IsAnnouncementViewed(string announcementKey, DateTime lastUpdateDate, long userId)
        {
            var audit = _repository.Any<AnnouncementViewAudit>(x => x.UserId == userId && x.AnnouncementKey == announcementKey && x.Date >= lastUpdateDate);

            return audit;
        }
    }
}