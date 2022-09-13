using DocumentsManagement.Library.Entities;
using Membership.Core.Entities;

namespace Membership.Library.Entities
{
    public class CompanyDocument : BaseDocumentEntity
    {
        public virtual Company Master { get; set; }
    }
}
