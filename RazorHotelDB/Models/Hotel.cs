namespace RazorHotelDB.Models
{
    public class Hotel
    {
        /// <summary>
        /// bruges til at sortere de forskellige hoteller
        /// </summary>
        public int HotelNr { get; set; }

        /// <summary>
        /// bruges til at angive hotellets navn
        /// </summary>
        public String Navn { get; set; }

        /// <summary>
        /// bruges til at angive hotellets addresse
        /// </summary>
        public String Adresse { get; set; }

        /// <summary>
        ///  Hotel er til for at give kunderne en beskrivelse over hvilke hoteller brugeren kan vælge 
        /// </summary>
        public Hotel()
        {

        }
        public Hotel(int hotelNr, string navn, string adresse)
        {
            HotelNr = hotelNr;
            Navn = navn;
            Adresse = adresse;
        }

        public override string ToString()
        {
            return $"{nameof(HotelNr)}: {HotelNr}, {nameof(Navn)}: {Navn}, {nameof(Adresse)}: {Adresse}";
        }
    }
}
