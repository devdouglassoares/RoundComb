using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoundComb.Commons.Models
{

    public class RespostaContract<T> : RespostaBaseContract
    {
        public T entidade { get; set; }
    }

    public abstract class RespostaBaseContract
    {
        public int idErro { get; set; }
        public string msgErro { get; set; }
    }

}
