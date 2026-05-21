using System;

namespace Kino_Останин.Model
{
    public class SoldTicket
    {
        public int id { get; set; }
        public int afisha_id { get; set; }
        public int quantity { get; set; }
        public int total_price { get; set; }
        public DateTime purchase_date { get; set; }
        public string FilmName { get; set; }
        public string KinoteatrName { get; set; }
        public DateTime FilmTime { get; set; }
    }
}