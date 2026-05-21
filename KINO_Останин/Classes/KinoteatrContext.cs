using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Kino_Останин.Classes.Common;
using KINO_Останин.Model;

namespace Kino_Останин.Classes
{
    internal class KinoteatrContext
    {
        private Connection connection = new Connection();

        public List<Kinoteatr> GetKinoteatrs()
        {
            List<Kinoteatr> list = new List<Kinoteatr>();
            MySqlConnection conn = null;
            MySqlDataReader reader = null;

            try
            {
                conn = connection.OpenConnection();
                reader = connection.Query("SELECT * FROM kinoteatr", conn);

                while (reader.Read())
                {
                    list.Add(new Kinoteatr(
                        reader.GetInt32("id"),
                        reader.GetString("name"),
                        reader.GetInt32("countzal"),
                        reader.GetInt32("count")
                    ));
                }
            }
            finally
            {
                reader?.Close();
                connection.CloseConnection(conn);
            }

            return list;
        }

        public void AddKinoteatr(string name, int countzal, int count)
        {
            MySqlConnection conn = null;
            try
            {
                conn = connection.OpenConnection();
                string sql = "INSERT INTO kinoteatr (name, countzal, count) VALUES (@name, @countzal, @count)";

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@countzal", countzal);
                cmd.Parameters.AddWithValue("@count", count);

                cmd.ExecuteNonQuery();
            }
            finally
            {
                connection.CloseConnection(conn);
            }
        }

        public void UpdateKinoteatr(int id, string name, int countzal, int count)
        {
            MySqlConnection conn = null;
            try
            {
                conn = connection.OpenConnection();
                string sql = "UPDATE kinoteatr SET name=@name, countzal=@countzal, count=@count WHERE id=@id";

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@countzal", countzal);
                cmd.Parameters.AddWithValue("@count", count);
                cmd.Parameters.AddWithValue("@id", id);

                cmd.ExecuteNonQuery();
            }
            finally
            {
                connection.CloseConnection(conn);
            }
        }

        public void DeleteKinoteatr(int id)
        {
            MySqlConnection conn = null;
            try
            {
                conn = connection.OpenConnection();
                string sql = "DELETE FROM kinoteatr WHERE id=@id";

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", id);

                cmd.ExecuteNonQuery();
            }
            finally
            {
                connection.CloseConnection(conn);
            }
        }

        public List<Afisha> GetAfishas()
        {
            List<Afisha> list = new List<Afisha>();
            MySqlConnection conn = null;
            MySqlDataReader reader = null;

            try
            {
                conn = connection.OpenConnection();
                string sql = @"SELECT a.*, k.name as kino_name 
                              FROM afisha a 
                              LEFT JOIN kinoteatr k ON a.id_kinoteatr = k.id";
                reader = connection.Query(sql, conn);

                while (reader.Read())
                {
                    Afisha a = new Afisha(
                        reader.GetInt32("id"),
                        reader.GetInt32("id_kinoteatr"),
                        reader.GetString("name"),
                        reader.GetDateTime("time"),
                        reader.GetInt32("price")
                    );

                    if (!reader.IsDBNull(reader.GetOrdinal("kino_name")))
                        a.KinoteatrName = reader.GetString("kino_name");

                    list.Add(a);
                }
            }
            finally
            {
                reader?.Close();
                connection.CloseConnection(conn);
            }

            return list;
        }

        public void AddAfisha(int idkinoteatr, string name, DateTime time, int price)
        {
            MySqlConnection conn = null;
            try
            {
                conn = connection.OpenConnection();
                string sql = "INSERT INTO afisha (id_kinoteatr, name, time, price) VALUES (@idkinoteatr, @name, @time, @price)";

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@idkinoteatr", idkinoteatr);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@time", time); // Передаем DateTime напрямую
                cmd.Parameters.AddWithValue("@price", price);

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при добавлении афиши: {ex.Message}");
            }
            finally
            {
                connection.CloseConnection(conn);
            }
        }

        public void UpdateAfisha(int id, int idkinoteatr, string name, DateTime time, int price)
        {
            MySqlConnection conn = null;
            try
            {
                conn = connection.OpenConnection();
                string sql = "UPDATE afisha SET id_kinoteatr=@idkinoteatr, name=@name, time=@time, price=@price WHERE id=@id";

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@idkinoteatr", idkinoteatr);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@time", time); // Передаем DateTime напрямую
                cmd.Parameters.AddWithValue("@price", price);
                cmd.Parameters.AddWithValue("@id", id);

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при обновлении афиши: {ex.Message}");
            }
            finally
            {
                connection.CloseConnection(conn);
            }
        }

        public void DeleteAfisha(int id)
        {
            MySqlConnection conn = null;
            try
            {
                conn = connection.OpenConnection();
                string sql = "DELETE FROM afisha WHERE id=@id";

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", id);

                cmd.ExecuteNonQuery();
            }
            finally
            {
                connection.CloseConnection(conn);
            }
        }

        public List<Afisha> FilterAfisha(int? idKinoteatr, string filmName, DateTime? dateFrom, DateTime? dateTo, int? priceFrom, int? priceTo)
        {
            List<Afisha> list = new List<Afisha>();
            MySqlConnection conn = null;
            MySqlDataReader reader = null;

            try
            {
                conn = connection.OpenConnection();

                string sql = @"SELECT a.*, k.name as kino_name 
                              FROM afisha a 
                              LEFT JOIN kinoteatr k ON a.id_kinoteatr = k.id WHERE 1=1";

                if (idKinoteatr.HasValue)
                    sql += $" AND a.id_kinoteatr = {idKinoteatr}";

                if (!string.IsNullOrEmpty(filmName))
                    sql += $" AND a.name LIKE '%{filmName}%'";

                if (dateFrom.HasValue)
                    sql += $" AND a.time >= '{dateFrom.Value:yyyy-MM-dd HH:mm:ss}'";

                if (dateTo.HasValue)
                    sql += $" AND a.time <= '{dateTo.Value:yyyy-MM-dd HH:mm:ss}'";

                if (priceFrom.HasValue)
                    sql += $" AND a.price >= {priceFrom}";

                if (priceTo.HasValue)
                    sql += $" AND a.price <= {priceTo}";

                reader = connection.Query(sql, conn);

                while (reader.Read())
                {
                    Afisha a = new Afisha(
                        reader.GetInt32("id"),
                        reader.GetInt32("id_kinoteatr"),
                        reader.GetString("name"),
                        reader.GetDateTime("time"),
                        reader.GetInt32("price")
                    );

                    if (!reader.IsDBNull(reader.GetOrdinal("kino_name")))
                        a.KinoteatrName = reader.GetString("kino_name");

                    list.Add(a);
                }
            }
            finally
            {
                reader?.Close();
                connection.CloseConnection(conn);
            }

            return list;
        }
        public void AddSoldTicket(int afishaId, int quantity, int totalPrice)
        {
            MySqlConnection conn = null;
            try
            {
                conn = connection.OpenConnection();
                string sql = @"INSERT INTO sold_tickets (afisha_id, quantity, total_price, purchase_date) 
                       VALUES (@afishaId, @quantity, @totalPrice, @purchaseDate)";

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@afishaId", afishaId);
                cmd.Parameters.AddWithValue("@quantity", quantity);
                cmd.Parameters.AddWithValue("@totalPrice", totalPrice);
                cmd.Parameters.AddWithValue("@purchaseDate", DateTime.Now);

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при сохранении продажи: {ex.Message}");
            }
            finally
            {
                connection.CloseConnection(conn);
            }
        }

        // Получить все проданные билеты
        public List<SoldTicket> GetSoldTickets()
        {
            List<SoldTicket> list = new List<SoldTicket>();
            MySqlConnection conn = null;
            MySqlDataReader reader = null;

            try
            {
                conn = connection.OpenConnection();
                string sql = @"SELECT st.*, a.name as film_name, a.time as film_time, k.name as kinoteatr_name
                      FROM sold_tickets st
                      LEFT JOIN afisha a ON st.afisha_id = a.id
                      LEFT JOIN kinoteatr k ON a.id_kinoteatr = k.id
                      ORDER BY st.purchase_date DESC";

                reader = connection.Query(sql, conn);

                while (reader.Read())
                {
                    SoldTicket ticket = new SoldTicket
                    {
                        id = reader.GetInt32("id"),
                        afisha_id = reader.GetInt32("afisha_id"),
                        quantity = reader.GetInt32("quantity"),
                        total_price = reader.GetInt32("total_price"),
                        purchase_date = reader.GetDateTime("purchase_date")
                    };

                    if (!reader.IsDBNull(reader.GetOrdinal("film_name")))
                        ticket.FilmName = reader.GetString("film_name");

                    if (!reader.IsDBNull(reader.GetOrdinal("kinoteatr_name")))
                        ticket.KinoteatrName = reader.GetString("kinoteatr_name");

                    if (!reader.IsDBNull(reader.GetOrdinal("film_time")))
                        ticket.FilmTime = reader.GetDateTime("film_time");

                    list.Add(ticket);
                }
            }
            finally
            {
                reader?.Close();
                connection.CloseConnection(conn);
            }

            return list;
        }

        // Получить статистику по фильму
        public Dictionary<string, object> GetFilmStatistics(int afishaId)
        {
            Dictionary<string, object> stats = new Dictionary<string, object>();
            MySqlConnection conn = null;
            MySqlDataReader reader = null;

            try
            {
                conn = connection.OpenConnection();

                // Общее количество проданных билетов и сумма
                string sql = @"SELECT SUM(quantity) as total_tickets, SUM(total_price) as total_revenue 
                      FROM sold_tickets 
                      WHERE afisha_id = @afishaId";

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@afishaId", afishaId);
                reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    stats["total_tickets"] = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                    stats["total_revenue"] = reader.IsDBNull(1) ? 0 : reader.GetInt32(1);
                }
                reader.Close();

                // Количество покупок
                cmd.CommandText = "SELECT COUNT(*) FROM sold_tickets WHERE afisha_id = @afishaId";
                stats["purchases_count"] = Convert.ToInt32(cmd.ExecuteScalar());

                return stats;
            }
            finally
            {
                connection.CloseConnection(conn);
            }
        }
    }
}