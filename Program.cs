using System;
using System.Data;
using System.Data.SQLite;
using System.IO;

namespace SQLITE_Demo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // create db
            var sqlite = new sqlite3();
            sqlite.CreateDB();


            var concert = new Concert()
            {
                id = 3,
                Title = "Clash",
                ShowDate = new DateTime(2020, 12, 31, 19, 0, 0),
            };

            var ticket = new ConcertTickets
            {
                id = 5,
                ConcertId = 8,
                ReservedBy = "TIKK",
                ReservedDate = new DateTime(2022, 12, 31, 19, 0, 0),
            };

            string isBooked = BookConcertTicket(concert, ticket);

            if (isBooked == "0")
            {
                Console.WriteLine("Uavailable...");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine($"Ticket Id :  {isBooked} is booked");
                Console.ReadKey();
            }

        }

        static string BookConcertTicket(Concert concert, ConcertTickets concertTickets)
        {
            string bookingStatus = "";

            string cs = @"URI=file:sqlite.db";
            using var con = new SQLiteConnection(cs);
            con.Open();
            DataTable dt = new DataTable();
            string sql = $"SELECT * FROM ConcertTickets WHERE ConcertId = '{concert.id}' AND ReservedDate = '{concert.ShowDate}' AND StatusId= 1;";
            using var adapter = new SQLiteDataAdapter(sql, cs);
            adapter.Fill(dt);
            try
            {
                if (dt.Rows.Count != 0)
                {
                    bookingStatus = "0";
                }
                else
                {
                    using var cmd = new SQLiteCommand(con);
                    cmd.CommandText = $"INSERT INTO ConcertTickets (Id, ConcertId, StatusId, ReservedDate, ReservedBy) VALUES ('{concertTickets.id}', '{concert.id}', 1, '{concertTickets.ReservedDate}', '{concertTickets.ReservedBy}')";
                    cmd.ExecuteNonQuery();
                    return concertTickets.id.ToString();
                }
            }
            catch
            {
                bookingStatus = "0";
            }


            return bookingStatus;
        }
    }

    public class sqlite3
    {
        public void CreateDB()
        {
            //string cs = "Data Source=:memory:";
            //string stm = "SELECT SQLITE_VERSION()";

            //using var con = new SQLiteConnection(cs);
            //con.Open();

            //using var cmd = new SQLiteCommand(stm, con);
            //string version = cmd.ExecuteScalar().ToString();
            //Console.WriteLine($"SQLite version: {version}");
            //Console.ReadKey();
            if (!File.Exists("sqlite.db"))
            {
                File.Create("sqlite.db");
            }
            string cs = @"URI=file:sqlite.db";
            using var con = new SQLiteConnection(cs);
            con.Open();

            using var cmd = new SQLiteCommand(con);

            cmd.CommandText = "DROP TABLE IF EXISTS Concert";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "DROP TABLE IF EXISTS Concert";
            cmd.ExecuteNonQuery();

            cmd.CommandText = @"CREATE TABLE Concert (id INTEGER PRIMARY KEY, Title VARCHAR(100), ShowDate DATETIME, Location VARCHAR(100))";
            cmd.ExecuteNonQuery();

            DataTable dt1 = new DataTable();
            string sql1 = "SELECT * FROM Concert";
            using var adapter1 = new SQLiteDataAdapter(sql1, cs);
            adapter1.Fill(dt1);

            cmd.CommandText = "DROP TABLE IF EXISTS ConcertTickets";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "DROP TABLE IF EXISTS ConcertTickets";
            cmd.ExecuteNonQuery();

            cmd.CommandText = @"CREATE TABLE ConcertTickets (id INTEGER PRIMARY KEY, ConcertId INTEGER, StatusId SMALLINT, ReservedBy VARCHAR(50), ReservedDate DATETIME)";
            cmd.ExecuteNonQuery();

            DataTable dt2 = new DataTable();
            string sql2 = "SELECT * FROM ConcertTickets";
            using var adapter2 = new SQLiteDataAdapter(sql2, cs);
            adapter2.Fill(dt2);
        }




    }

    public class Concert
    {
        public int id { get; set; }
        public string Title { get; set; }
        public DateTime ShowDate { get; set; }
        public string Location { get; set; }
    }

    public class ConcertTickets
    {
        public int id { get; set; }
        public int ConcertId { get; set; }
        public int StatusId { get; set; }
        public string ReservedBy { get; set; }
        public DateTime ReservedDate { get; set; }
    }
}
