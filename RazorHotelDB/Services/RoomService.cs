using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using RazorHotelDB.Interfaces;
using RazorHotelDB.Models;

namespace RazorHotelDB.Services
{
    public class RoomService :Connection, IRoomService
    {
        private string queryString = "select * from Room where Hotel_No=@ID";
        private string queryStringFromID = "select * from Room where Hotel_No =@Hotel_NO and Room_No=@ID";
        private string insertSql = "insert into Room Values(@ID, @Hotel_No, @Types, @Price)";
        private string deleteSql = "delete from Room where Room_NO=@ID and Hotel_No=@Hotel_No";
        private string updateSql = "update Room Set Room_No=@ID, Hotel_No=@HotelNr, Types=@Types ,Price=@Price Where Room_No=@ID and Hotel_No=@HotelNr";
        private string queryStringFromPrice = "Select * from Room Where Price<=@Price and Hotel_No=@ID";

        public RoomService(IConfiguration configuration) : base(configuration)
        {
        }

        public RoomService(string connectionservice):base(connectionservice)
        {

        }

        public async Task<bool> CreateRoomAsync(int hotelNr, Room room)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(insertSql, connection);
                    command.Parameters.AddWithValue("ID", room.RoomNr);
                    command.Parameters.AddWithValue("Hotel_No", hotelNr);
                    command.Parameters.AddWithValue("Types", room.Types);
                    command.Parameters.AddWithValue("Price", room.Pris);
                    command.Connection.OpenAsync();
                    int noOfRows = await command.ExecuteNonQueryAsync();
                    return noOfRows == 1;
                }
                catch (SqlException sqlEx)
                {
                    Console.WriteLine("Database error " + sqlEx.Message);
                    throw sqlEx;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Generel fejl " + ex.Message);
                    throw ex;
                }
                return false;

            }
        }

        public async Task<Room> DeleteRoomAsync(int roomNr, int hotelNr)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    Room room=await GetRoomFromIdAsync(roomNr, hotelNr);
                    SqlCommand command = new SqlCommand(deleteSql, connection);
                    command.Parameters.AddWithValue("@ID", roomNr);
                    command.Parameters.AddWithValue("@Hotel_No", hotelNr);
                    await command.Connection.OpenAsync();
                    int noOfRows = await command.ExecuteNonQueryAsync();
                    if (noOfRows == 1)
                    {
                        return room;
                    }

                }
                catch (SqlException sqlEx)
                {
                    Console.WriteLine("Database error " + sqlEx.Message);
                    throw sqlEx;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Generel fejl " + ex.Message);
                    throw ex;
                }
            }
            return null;
        }

        public async Task<List<Room>> GetAllRoomAsync(int hotelNr)
        {
            List<Room> rooms = new List<Room>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand commmand = new SqlCommand(queryString, connection);
                    commmand.Parameters.AddWithValue("@ID", hotelNr);
                    commmand.Connection.Open();
                    SqlDataReader reader = commmand.ExecuteReader();
                    while (reader.Read())
                    {
                        int roomNo = reader.GetInt32(0);
                        string typestring = reader.GetString(2);
                        char Types = typestring[0];
                        int HotelNo = reader.GetInt32(1);
                        double Price = reader.GetDouble(3);
                        Room room = new Room(roomNo, Types, Price, HotelNo);
                        rooms.Add(room);
                    }
                }
                catch (SqlException sqlEx)
                {
                    Console.WriteLine("Database error " + sqlEx.Message);
                    throw sqlEx;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Generel fejl " + ex.Message);
                    throw ex;
                }
                return rooms;
            }
        }

        public async Task<Room> GetRoomFromIdAsync(int roomNr, int hotelNr)
        {
            List<Room> rooms = new List<Room>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand commmand = new SqlCommand(queryStringFromID, connection);
                    commmand.Parameters.AddWithValue("@ID", roomNr);
                    commmand.Parameters.AddWithValue("@Hotel_NO", hotelNr);
                    await commmand.Connection.OpenAsync();
                    SqlDataReader reader = await commmand.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        int roomNo = reader.GetInt32(0);
                        string typestring = reader.GetString(2);
                        char Types = typestring[0];
                        int HotelNo = reader.GetInt32(1);
                        double Price = reader.GetDouble(3);
                        Room room = new Room(roomNo, Types, Price, hotelNr);
                        return room;
                    }
                }
                catch (SqlException sqlEx)
                {
                    Console.WriteLine("Database error " + sqlEx.Message);
                    throw sqlEx;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Generel fejl " + ex.Message);
                    throw ex;
                }
                return null;
            }
        }

        public async Task<List<Room>> SortBypriceAsync(int hotelnr,int price)
        {
            using(SqlConnection connection = new SqlConnection(connectionString)) 
            {
                try 
                {
                    List<Room> rooms= new List<Room>();
                    SqlCommand commmand = new SqlCommand(queryStringFromPrice, connection);
                    commmand.Parameters.AddWithValue("@ID",hotelnr);
                    commmand.Parameters.AddWithValue("@Price",price);
                    await commmand.Connection.OpenAsync();
                    SqlDataReader reader = await commmand.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        int roomNo = reader.GetInt32(0);
                        string typestring = reader.GetString(2);
                        char Types = typestring[0];
                        int HotelNo = reader.GetInt32(1);
                        double Price = reader.GetDouble(3);
                        Room room = new Room(roomNo, Types, Price, HotelNo);
                        rooms.Add(room);
                    }
                    return rooms;
                }
                catch (SqlException sqlex)
                {
                    Console.WriteLine("Database error");
                    throw sqlex;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Generel error");
                    throw ex;
                }
                return null;

            }
        }

        public async Task<bool> UpdateRoomAsync(Room room, int roomNr, int hotelNr)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(updateSql, connection);
                    command.Parameters.AddWithValue("@ID", roomNr);
                    command.Parameters.AddWithValue("@HotelNr", hotelNr);
                    command.Parameters.AddWithValue("@Types", room.Types);
                    command.Parameters.AddWithValue("@Price", room.Pris);
                    command.Connection.Open();
                    int noOfRows = command.ExecuteNonQuery();
                    return noOfRows == 1;

                }
                catch (SqlException sqlEx)
                {
                    Console.WriteLine("Database error " + sqlEx.Message);
                    throw sqlEx;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Generel fejl " + ex.Message);
                    throw ex;
                }
                return false;
            }
        }
    }
}
