using System;
using System.Runtime.Serialization;


namespace RoundComb.Commons.ExceptionHandling
{
    [DataContract]
    public class ServiceExceptionMessage
    {
        /// <summary>;
        /// Exception Message
        /// </summary>;
        [DataMember]
        public string Message { get; set; }
        [DataMember]
        public string Reason { get; set; }
        [DataMember]
        public string Source { get; set; }
    }
}

