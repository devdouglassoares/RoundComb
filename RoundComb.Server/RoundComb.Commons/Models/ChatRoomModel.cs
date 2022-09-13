using System;

namespace RoundComb.Commons.Models
{
    public class ChatRoomModel
    {
        public Guid guid { get; set; }
        public string name { get; set; }
        public string iduserA { get; set; }
        public string iduserB { get; set; }
        public string iduserC { get; set; }
        public string IDProperty { get; set; }
        public string IDEvent { get; set; }
        public string eventStartedByUserName { get; set; }
        public string firstmessage { get; set; }
    }
}