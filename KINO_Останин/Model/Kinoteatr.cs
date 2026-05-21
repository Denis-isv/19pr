using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kino_Останин.Model
{
    public class Kinoteatr
    {
        public int id { get; set; }
        public string name { get; set; }
        public int countzal { get; set; }
        public int count { get; set; }

        public Kinoteatr(int id, string name, int countzal, int count)
        {
            this.id = id;
            this.name = name;
            this.countzal = countzal;
            this.count = count;
        }

    }
}
