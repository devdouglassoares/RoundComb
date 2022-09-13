namespace DTWrapper.Core.DTModel
{
    using System.Collections.Generic;

    public class DataTablesModel
    {

        public List<DataTablesColumnModel> columns { get; set; }

        public dynamic customFilter { get; set; }

        public int draw { get; set; }

        public int length { get; set; }

        public List<DataTablesOrderModel> order { get; set; }

        public DataTablesSearchModel search { get; set; }

        public int start { get; set; }
    }
}

