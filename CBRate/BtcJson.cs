using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CBRate
{

    public class BtcJson
    {
        public Data data { get; set; }
    }


    public class Data
    {
        //public string base { get; set; }
        public string currency { get; set; }
        public string amount { get; set; }
    }

 }
