using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RoundComb.Commons.Models;
using RoundComb.ServicesProvider;

namespace RoundComb.Testing
{
    public partial class TestLocal : Form
    {
        public TestLocal()
        {
            InitializeComponent();
        }

        private void bt_createchatroom_Click(object sender, EventArgs e)
        {
            ServiceProvider _serviceprovider = new ServiceProvider();
            /*
            ChatRoomModel chatroom = new ChatRoomModel
            {
                guid = new Guid(),
                name = "Luis",
                iduserA = "1",
                iduserB = "1",
                iduserC = "1",
                IDProperty = "1",
                IDEvent = "1",
                eventStartedByUserName = "1",
                firstmessage = "hello there",
            };

            RespostaContract<string> resposta = _serviceprovider.createNewChatRoom(chatroom);
            */
            /*
            RespostaContract<string> resposta = _serviceprovider.GetMyListOfEvents(100225);
            */
        }

        private void btDownload_Click(object sender, EventArgs e)
        {
            ServiceProvider _serviceprovider = new ServiceProvider();

            var x = _serviceprovider.getSignatureDocument("12129685-7093-4eec-964f-a077918f9725");

        }

        private void button1_Click(object sender, EventArgs e)
        {

            ServiceProvider _serviceprovider = new ServiceProvider();

            PropertyContractTemplate newcontract = new PropertyContractTemplate();
            
            newcontract.fileURL = "https://mega.nz/file/puA2CCwA#EB451dn-s5YsmNidb6NLjoQzdrVV--1x6dLxFfMM1Po";
            newcontract.Message = "test create new template";
            newcontract.Subject = "test subject";
            newcontract.TenantName = "Luis";
            newcontract.Title = "test template";

            var x = _serviceprovider.setPropertyContractTemplate(newcontract);

        }
    }
}
