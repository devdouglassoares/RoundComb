using Membership.Core;
using Membership.Core.Contracts;
using Membership.Core.Entities;
using Membership.Library.Contracts;
using Membership.Library.Data;
using Membership.Library.Dto;
using Membership.Library.Entities;
//using NotifyService.RestClient.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;

namespace Membership.Library.Services
{
    public class ConnectionInfoService : IConnectionInfoService
    {
        private readonly IRepository repository;
        private readonly IMembership membership;
        private readonly ICustomerAuditService customerAuditService;
       // private readonly INotificationService _notificationService;

        private readonly int PIN_NUMBER_EXPIRATION = int.Parse(ConfigurationManager.AppSettings["ConnectionInfo_PinNumberExpirationInMinutes"]);

        public ConnectionInfoService(IRepository repository, IMembership membership, ICustomerAuditService customerAuditService/*, INotificationService notificationService*/)
        {
            this.repository = repository;
            this.membership = membership;
            this.customerAuditService = customerAuditService;
            //_notificationService = notificationService;
        }

        public ConnectionInfoDto GetConnectionInfo(long companyId)
        {
            var context = HttpContext.Current;
            string ip = String.Empty;

            if (context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
            {
                ip = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            }
            else if (!String.IsNullOrWhiteSpace(context.Request.UserHostAddress))
            {
                ip = context.Request.UserHostAddress;
                //replace '::1' host to '127.0.0.1'
                if (ip == "::1")
                {
                    ip = "127.0.0.1";
                }
            }

            var isAllowed = false;

            var ranges = ConfigurationManager.AppSettings["ConnectionInfo_IpRanges"] ?? string.Empty;
            foreach (var ipRange in ranges.Split('|').Where(r => !string.IsNullOrEmpty(r)))
            {
                if (ipRange == "*")
                {
                    isAllowed = true;
                    break;
                }

                var start = ipRange.Split('-').First();
                var end = ipRange.Split('-').Last();

                if (IsIPInRange(ip, start, end))
                {
                    isAllowed = true;
                    break;
                }
            }


            if (!isAllowed)
            {
                return null;
            }

            var connectionInfo =
                this.repository.GetAll<ConnectionInfo>().FirstOrDefault(x => x.Company.Id == companyId);

            var dto = new ConnectionInfoDto
            {
                Id = companyId,
                Content = string.Empty
            };

            if (connectionInfo != null)
            {
                dto.Content = connectionInfo.Content ?? string.Empty;
            }

            this.customerAuditService.LogCustomerViewed(companyId, this.membership.UserId);

            return dto;
        }

        public ConnectionInfoDto GetConnectionInfo(long companyId, int pinNumber)
        {
            var pin =
                repository.Get<ConnectionInfoPinNumber>(
                    p => p.Pin == pinNumber && p.ConnectionInfo.Company.Id == companyId);

            if (pin == null)
            {
                return null;
            }

            // pin number expired
            if ((DateTime.UtcNow - pin.Timestamp).TotalMinutes >= PIN_NUMBER_EXPIRATION)
            {
                return null;
            }

            var connectionInfo =
                this.repository.GetAll<ConnectionInfo>().FirstOrDefault(x => x.Company.Id == companyId);

            var dto = new ConnectionInfoDto
            {
                Id = companyId,
                Content = string.Empty
            };

            if (connectionInfo != null)
            {
                dto.Content = connectionInfo.Content ?? string.Empty;
            }

            this.customerAuditService.LogCustomerViewed(companyId, this.membership.UserId);

            return dto;
        }

        public void SaveConnectionInfo(ConnectionInfoDto model)
        {
            var connectionInfoOverride =
                this.repository.GetAll<ConnectionInfo>().FirstOrDefault(x => x.Company.Id == model.Id);

            if (connectionInfoOverride == null)
            {
                connectionInfoOverride = new ConnectionInfo
                {
                    Company = repository.Get<Company>(model.Id),
                    Content = model.Content
                };

                this.repository.Insert(connectionInfoOverride);
            }
            else
            {
                connectionInfoOverride.Content = model.Content;
                this.repository.Update(connectionInfoOverride);
            }

            this.repository.SaveChanges();
        }

        public ConnectionInfoRequestResultDto RequestPin(long companyId)
        {
            var connectionInfo = this.repository.GetAll<ConnectionInfo>().FirstOrDefault(x => x.Company.Id == companyId);
            if (connectionInfo == null)
            {
                connectionInfo = new ConnectionInfo
                {
                    Company = repository.Get<Company>(companyId)
                };

                repository.Insert(connectionInfo);
            }


            Random rand = new Random(DateTime.Now.Millisecond);
            int pin = rand.Next(999999);

            while (pin < 111111 || pin > 999999)
            {
                pin = rand.Next(999999);
            }

            var pinEntity = new ConnectionInfoPinNumber
            {
                ConnectionInfo = connectionInfo,
                Pin = pin,
                Timestamp = DateTime.UtcNow,
                UserId = membership.UserId
            };

            repository.Insert(pinEntity);

            var result = new ConnectionInfoRequestResultDto()
            {
            };
            var currentUser = membership.GetCurrentUser();
            if (!string.IsNullOrEmpty(currentUser.CellPhoneNumber))
            {
                string cellNumber = currentUser.CellPhoneNumber;

                result.PhoneNumber = cellNumber;

                var numberFrom = ConfigurationManager.AppSettings["TwilioFrom"];

                var message = "Your pin: " + pin + "\n\nThis pin will expire in " + PIN_NUMBER_EXPIRATION + " minutes.";
                repository.SaveChanges();
                /*_notificationService.SendSms(new List<string> { cellNumber }, message, numberFrom);*/
            }

            return result;
        }

        public static bool IsIPInRange(string ip, string ipStart, string ipEnd)
        {
            var pIP = IPAddress.Parse(ip);
            var pIPStart = IPAddress.Parse(ipStart);
            var pIPEnd = IPAddress.Parse(ipEnd);

            var bIP = pIP.GetAddressBytes().Reverse().ToArray();
            var bIPStart = pIPStart.GetAddressBytes().Reverse().ToArray();
            var bIPEnd = pIPEnd.GetAddressBytes().Reverse().ToArray();

            var uIP = BitConverter.ToUInt32(bIP, 0);
            var uIPStart = BitConverter.ToUInt32(bIPStart, 0);
            var uIPEnd = BitConverter.ToUInt32(bIPEnd, 0);

            return uIP >= uIPStart && uIP <= uIPEnd;
        }

    }
}
