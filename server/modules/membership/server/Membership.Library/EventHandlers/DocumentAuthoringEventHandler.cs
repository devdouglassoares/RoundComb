using Core.Events;
using DocumentsManagement.Library.Events;
using Membership.Core;

namespace Membership.Library.EventHandlers
{
    public class DocumentAuthoringEventHandler: IConsumer<CurrentDocumentAuthoring>
    {
        private readonly IMembership _membership;

        public DocumentAuthoringEventHandler(IMembership membership)
        {
            _membership = membership;
        }

        public int Order { get { return 10; } }

        public void HandleEvent(CurrentDocumentAuthoring eventMessage)
        {
            try
            {
                eventMessage.AuthorName = _membership.Name;
            }
            catch
            {
                // when user is not logged in
            }
        }
    }
}