using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoundComb.Commons.ExceptionHandling
{
    public class ExceptionUtils
    {
        public static string toThrow(int resposta, string dbmessage, string exmessage)
        {
            string msgErro = Convert.ToString(resposta) + "§" + dbmessage;

            if (dbmessage == string.Empty)
            {
                msgErro = Convert.ToString(resposta) + "§" + exmessage;
            }

            return msgErro;
        }
        
    }
}
