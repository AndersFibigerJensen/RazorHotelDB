using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using RazorHotelDB.Interfaces;
using RazorHotelDB.Models;

namespace RazorHotelDB.Services
{
    public class HotelService : Connection, IHotelService
    {
        private String queryString = "select * from Hotel";
        private String queryStringFromID = "select * from Hotel where Hotel_No=@ID";
        private String insertSql = "insert into Hotel Values (@ID, @Navn, @Adresse)";
        private String deleteSql = "delete from Hotel where Hotel_NO=@ID";
        private String updateSql = "update Hotel " +
                                   "set Hotel_No= @HotelID, Name=@Navn, Address=@Adresse " +
                                   "where Hotel_No = @HotelID";
        private string queryStringFromName = "Select * from Hotel Where Name like '%'+@Navn+'%'";
        private string queryStringFromAddress = "Select * from Hotel Where Address like '%'+@Address+'%'";

        public HotelService(IConfiguration configuration) : base(configuration)
        {
        }

        public HotelService(string connectionstring) : base(connectionstring)
        {

        }

        /// <summary>
        /// skaber et hotel i databasen
        /// </summary>
        /// <param name="hotel"></param>
        /// <exception cref="sqlex"> exceptionen bliver kastet videre i systemet hvis der er en fejl i databasen  </exception>
        /// <exception cref="ex"> exceptionen bliver kastet videre i systemet hvis fejlen er i programmet  </exception>
        /// <returns> returner enten true hvis et hotel blev skabt ellers returnere den er false </returns>
        public async Task<bool> CreateHotelAsync(Hotel hotel)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(insertSql, connection))
                {
                    command.Parameters.AddWithValue("@ID", hotel.HotelNr);
                    command.Parameters.AddWithValue("@Navn", hotel.Navn);
                    command.Parameters.AddWithValue("@Adresse", hotel.Adresse);
                    try
                    {
                        command.Connection.Open();
                        int noOfRows = await command.ExecuteNonQueryAsync(); //bruges ved update, delete, insert
                        if (noOfRows == 1)
                        {
                            return true;
                        }

                        return false;
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
                }

            }
            return false;
        }

        /// <summary>
        /// Sletter et hotel fra databasen
        /// </summary>
        /// <param name="hotelNr"></param>
        /// <exception cref="sqlex"> kaster funktion videre i systemet hvis der er en fejl i databasen </exception>
        /// <exception cref="ex"> kaster funktion videre i systemet hvis der er en fejl koden eller de data der bliver sendt ind i programmet </exception>
        /// <returns>returnere det slettet hotel </returns>
        public async Task<Hotel> DeleteHotelAsync(int hotelNr)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    Hotel hotel = await GetHotelFromIdAsync(hotelNr);
                    SqlCommand command = new SqlCommand(deleteSql, connection);
                    command.Parameters.AddWithValue("@ID", hotelNr);
                    command.Connection.OpenAsync();
                    int noOfRows = await command.ExecuteNonQueryAsync();
                    if (noOfRows == 1)
                    {
                        return hotel;
                    }

                }
                catch (SqlException sqlex)
                {
                    Console.WriteLine("Database error " + sqlex.Message);
                    throw sqlex;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Generel fejl " + ex.Message);
                    throw ex;
                }
            }
            return null;
        }

        /// <summary>
        /// Henter alle hoteller fra databasen
        /// </summary>
        /// <exception cref="sqlex"> sender fejlen videre i systemet hvis der er en fejl i databasen </exception>
        /// <exception cref="ex"> sender fejlen videre hvis der er en fejl i koden eller de data programmet bruger</exception>
        /// <returns> returnere en liste af hoteller </returns>
        public async Task<List<Hotel>> GetAllHotelAsync()
        {
            List<Hotel> hoteller = new List<Hotel>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    try
                    {
                        await command.Connection.OpenAsync();//aSynkront
                        SqlDataReader reader = await command.ExecuteReaderAsync();//aSynkront
                        while (await reader.ReadAsync())
                        {
                            int hotelNr = reader.GetInt32(0);
                            String hotelNavn = reader.GetString(1);
                            String hotelAdr = reader.GetString(2);
                            Hotel hotel = new Hotel(hotelNr, hotelNavn, hotelAdr);
                            hoteller.Add(hotel);
                        }
                    }
                    catch (SqlException sqlex)
                    {
                        Console.WriteLine("Database error " + sqlex.Message);
                        throw sqlex;
                        //return null;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Generel fejl" + ex.Message);
                        throw ex;
                        //return null;
                    }
                }
            }
            return hoteller;
        }

        /// <summary>
        /// Henter et hotel fra databasen ud fra hotelnr
        /// </summary>
        /// <param name="hotelNr"></param>
        /// <exception cref="sqlex"> sender fejlen videre i systemet hvis der er en fejl i databasen </exception>
        /// <exception cref="ex"> sender fejlen videre hvis der er en fejl i koden eller de data programmet bruger</exception>
        /// <returns>returnere et hotel ud der har et hotelnr der svarer til parameteren HotelNr</returns>
        public async Task<Hotel> GetHotelFromIdAsync(int hotelNr)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(queryStringFromID, connection);
                    command.Parameters.AddWithValue("@ID", hotelNr);
                    command.Connection.Open();
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        int hotelNo = reader.GetInt32(0);
                        string hotelNavn = reader.GetString(1);
                        string hotelAdr = reader.GetString(2);
                        Hotel hotel = new Hotel(hotelNo, hotelNavn, hotelAdr);
                        return hotel;
                    }

                }
                catch (SqlException sqlex)
                {
                    Console.WriteLine("Database error " + sqlex.Message);
                    throw sqlex;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Generel fejl " + ex.Message);
                    throw ex;
                }
            }
            return null;
        }

        /// <summary>
        /// laver en liste af hoteller ud fra databasen med et navn der svarer til hvad der står i
        /// </summary>
        /// <param name="name"></param>
        /// <exception cref="sqlex"> sender fejlen videre i systemet hvis der er en fejl i databasen </exception>
        /// <exception cref="ex"> sender fejlen videre hvis der er en fejl i koden eller de data programmet bruger</exception>
        /// <returns></returns>
        public async Task<List<Hotel>> GetHotelsByNameAsync(string name)
        {
            List<Hotel> hoteller = new List<Hotel>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(queryStringFromName, connection);
                    command.Parameters.AddWithValue("@Navn", name);
                    command.Connection.OpenAsync();
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        int hotelNo = reader.GetInt32(0);
                        string hotelNavn = reader.GetString(1);
                        string hotelAdr = reader.GetString(2);
                        Hotel hotel = new Hotel(hotelNo, hotelNavn, hotelAdr);
                        hoteller.Add(hotel);
                    }

                }
                catch (SqlException sqlex)
                {
                    Console.WriteLine("Database error " + sqlex.Message);
                    throw sqlex;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Generel fejl " + ex.Message);
                    throw ex;
                }
                return hoteller;
            }
        }

        /// <summary>
        /// opdater et hotel 
        /// </summary>
        /// <param name="hotel"></param>
        /// <param name="hotelNr"></param>
        /// <exception cref="sqlex"> sender fejlen videre i systemet hvis der er en fejl i databasen </exception>
        /// <exception cref="ex"> sender fejlen videre hvis der er en fejl i koden eller de data programmet bruger</exception>
        /// <returns> returnere sandt hvis hotellet bliver opdateret ellers returnere metoden falsk</returns>
        public async Task<bool> UpdateHotelAsync(Hotel hotel, int hotelNr)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(updateSql, connection);
                try
                {
                    command.Parameters.AddWithValue("@HotelID", hotel.HotelNr);
                    command.Parameters.AddWithValue("@Navn", hotel.Navn);
                    command.Parameters.AddWithValue("@Adresse", hotel.Adresse);
                    command.Connection.Open();
                    int NoofRows = await command.ExecuteNonQueryAsync();
                    if (NoofRows == 1)
                        return true;

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
                return false;

            }
        }

        /// <summary>
        /// laver en liste af hoteller ud fra parameteren address
        /// </summary>
        /// <param name="address"></param>
        /// <exception cref="sqlex"> sender fejlen videre i systemet hvis der er en fejl i databasen </exception>
        /// <exception cref="ex"> sender fejlen videre hvis der er en fejl i koden eller de data programmet bruger</exception>
        /// <returns>returnere en liste af hoteller</returns>
        public async Task<List<Hotel>> GetHotelsByAddressAsync(string address)
        {
            List<Hotel> hoteller = new List<Hotel>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(queryStringFromAddress, connection);
                    command.Parameters.AddWithValue("@Address", address);
                    command.Connection.OpenAsync();
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        int hotelNo = reader.GetInt32(0);
                        string hotelNavn = reader.GetString(1);
                        string hotelAdr = reader.GetString(2);
                        Hotel hotel = new Hotel(hotelNo, hotelNavn, hotelAdr);
                        hoteller.Add(hotel);
                    }

                }
                catch (SqlException sqlex)
                {
                    Console.WriteLine("Database error " + sqlex.Message);
                    throw sqlex;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Generel fejl " + ex.Message);
                    throw ex;
                }
                return hoteller;
            }
        }
    }
}
