using Core.Events;

namespace Membership.Core.Services.EventHandlers
{
    public class EntityDataSavingEventHandlers : IConsumer<EntityBeingInserted>,
                                                          IConsumer<EntityBeingUpdated>
    {
        public int Order => 10;

        private readonly IMembership _membership;

        public EntityDataSavingEventHandlers(IMembership membership)
        {
            _membership = membership;
        }

        public void HandleEvent(EntityBeingInserted eventMessage)
        {
            if (_membership.GetCurrentUser() == null)
                return;

            try
            {
                eventMessage.Entity.CreatedBy = _membership.Name;
            }
            catch
            {

            }
        }


        public void HandleEvent(EntityBeingUpdated eventMessage)
        {
            if (_membership.GetCurrentUser() == null)
                return;

            try
            {
                eventMessage.Entity.ModifiedBy = _membership.Name;
            }
            catch
            {

            }
        }
    }
}