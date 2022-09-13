
using System.Collections.Generic;

namespace Membership.Core.Dto
{
    public class GroupModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public List<UserBaseModel> Users { get; set; }
    }
}
