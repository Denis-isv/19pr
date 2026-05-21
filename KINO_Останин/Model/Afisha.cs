using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kino_Останин.Model
{
    public class Afisha
    {
        public int id { get; set; }
        public int idkinoteatr { get; set; }
        public string name { get; set; }
        public DateTime time { get; set; }
        public int price { get; set; }
        public string KinoteatrName { get; set; }

        public Afisha(int id, int idkinoteatr, string name, DateTime time, int price)
        {
            this.id = id;
            this.idkinoteatr = idkinoteatr;
            this.name = name;
            this.time = time;
            this.price = price;
        }
    }
}
