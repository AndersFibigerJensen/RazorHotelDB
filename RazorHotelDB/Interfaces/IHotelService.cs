using RazorHotelDB.Models;

namespace RazorHotelDB.Interfaces
{
    public interface IHotelService
    {
        /// <summary>
        /// Henter alle Hoteller fra databasen
        /// </summary>
        /// <returns>returnere en liste af hoteller</returns>
        Task<List<Hotel>> GetAllHotelAsync();

        /// <summary>
        /// Henter et specifik hotel fra database 
        /// </summary>
        /// <param name="hotelNr">Udpeger det hotel der ønskes fra databasen</param>
        /// <returns>Det fundne hotel eller null hvis hotellet ikke findes</returns>
        Task<Hotel> GetHotelFromIdAsync(int hotelNr);

        /// <summary>
        /// Indsætter et nyt hotel i databasen
        /// </summary>
        /// <param name="hotel">hotellet der skal indsættes</param>
        /// <returns>Sand hvis der er gået godt ellers falsk</returns>
        Task<bool> CreateHotelAsync(Hotel hotel);

        /// <summary>
        /// Opdaterer en hotel i databasen
        /// </summary>
        /// <param name="hotel">De nye værdier til hotellet</param>
        /// <param name="hotelNr">Nummer på den hotel der skal opdateres</param>
        /// <returns>Sand hvis der er gået godt ellers falsk</returns>
        Task<bool> UpdateHotelAsync(Hotel hotel, int hotelNr);

        /// <summary>
        /// Sletter et hotel fra databasen
        /// </summary>
        /// <param name="hotelNr">Nummer på det hotel der skal slettes</param>
        /// <returns>Det hotel der er slettet fra databasen, returnere null hvis hotellet ikke findes</returns>
        Task<Hotel> DeleteHotelAsync(int hotelNr);

        /// <summary>
        /// henter en sorteret liste af hotteller der svarer til navnet fra databasen
        /// </summary>
        /// <param name="name">Angiver navn på hotel der hentes fra databasen</param>
        /// <returns>returnere en liste af hoteller hvor navnet kunne passe ind</returns>
        Task<List<Hotel>> GetHotelsByNameAsync(string name);

        /// <summary>
        /// Henter en sorteret liste af hotteller der svarer til adressen fra databasen
        /// </summary>
        /// <param name="address"></param>
        /// <returns>returnere en liste af hoteller hvor adressen kunne passe ind</returns>
        Task<List<Hotel>> GetHotelsByAddressAsync(string address);
    }
}
