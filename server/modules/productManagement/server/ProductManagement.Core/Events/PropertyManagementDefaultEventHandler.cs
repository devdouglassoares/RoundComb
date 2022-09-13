using Core.Events;

namespace ProductManagement.Core.Events
{
    public class PropertyManagementDefaultEventHandler : IConsumer<PropertyCreated>,
                                                        IConsumer<PropertyUpdated>,
                                                        IConsumer<PropertyDeleted>
    {
        public int Order => -1;

        public void HandleEvent(PropertyCreated eventMessage)
        {
        }

        public void HandleEvent(PropertyDeleted eventMessage)
        {
        }

        public void HandleEvent(PropertyUpdated eventMessage)
        {
        }
    }
}