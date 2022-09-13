using Core;
using Membership.Library.Dto;

namespace Membership.Library.Contracts
{
    public interface IConnectionInfoService : IDependency
    {
        ConnectionInfoDto GetConnectionInfo(long companyId);
        ConnectionInfoDto GetConnectionInfo(long companyId, int pinNumber);
        void SaveConnectionInfo(ConnectionInfoDto model);
        ConnectionInfoRequestResultDto RequestPin(long companyId);
    }
}