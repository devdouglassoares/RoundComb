using RoundComb.Commons;
using System;
using System.Collections.Generic;

namespace RoundComb.ServicesProvider
{
    public class ServiceValidator
    {
       /* public static bool isClientAuthorizedToCallWsMethod(string idMsgEntity, string wsMethodName, string wsEndPoint)
        {

            bool isAuthorized = true;

            AuthorizationModel model = new AuthorizationModel();

            model.GuidAuth = idMsgEntity;
            model.WebService = wsEndPoint;
            model.Method = wsMethodName;

            isAuthorized = WSAuthorization.Authorizations.isAuthorized(model);

            return isAuthorized;

        }

        public static bool isCpostalValid(string cPostal, int size)
        {
            bool result = false;

            if (!String.IsNullOrEmpty(cPostal))
            {
                if (cPostal.Length == size)
                {
                    int cPostalValue;

                    result = Int32.TryParse(cPostal, out cPostalValue);
                }
            }
            else
            {
                result = true;
            }

            return result;
        }
        */
    }
}
