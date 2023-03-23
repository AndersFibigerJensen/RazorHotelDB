namespace RazorHotelDB.Models
{
    public class Room
    {

        public int RoomNr { get; set; }
        public char Types { get; set; }
        public double Pris { get; set; }
        public int HotelNr { get; set; }


        /// <summary>
        ///  Formålet med værelse er at give brugeren et overblik over det værelse de gerne ville booke
        /// </summary>
        /// <param name="nr"></param>
        /// <param name="types"></param>
        /// <param name="pris"></param>
   

        public Room(int nr, char types, double pris)
        {
            RoomNr = nr;
            Types = types;
            Pris = pris;
        }

        public Room(int nr, char types, double pris, int hotelNr) : this(nr, types, pris)
        {
            HotelNr = hotelNr;
        }

        public Room()
        {

        }
    }
}
