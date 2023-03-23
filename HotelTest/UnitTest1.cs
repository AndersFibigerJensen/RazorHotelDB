using RazorHotelDB.Interfaces;
using RazorHotelDB.Models;
using RazorHotelDB.Services;
using System.Data.Common;

namespace HotelTest
{
    [TestClass]
    public class UnitTest1
    {
        private string connectionstring = Secret.Connectionstring;

        [TestMethod]
        public void TestAddHotel()
        {
            //arrange
            HotelService hotelservice = new HotelService(connectionstring);
            List<Hotel> hotels= hotelservice.GetAllHotelAsync().Result;

            //act
            int hotelsbefore= hotels.Count;
            Hotel newhotel = new Hotel(10, "grenen", "morelhaven 113");
            bool ok=hotelservice.CreateHotelAsync(newhotel).Result;
            hotels = hotelservice.GetAllHotelAsync().Result;
            int hotelsAfter= hotels.Count;
            hotelservice.DeleteHotelAsync(10);


            //assert
            Assert.AreEqual(hotelsbefore + 1, hotelsAfter);
        }

        [TestMethod]
        public void UpdateHotel()
        {
            //arrange
            HotelService hotelservice = new HotelService(connectionstring);
            List<Hotel> hotels = hotelservice.GetAllHotelAsync().Result;

            //act
            Hotel newhotel = new Hotel(10, "grenen", "morelhaven 113");
            bool ok = hotelservice.CreateHotelAsync(newhotel).Result;
            hotelservice.UpdateHotelAsync(new Hotel(10, "grenåhotel", "hotelvej 112"),10);
            Hotel editedhotel=hotelservice.GetHotelFromIdAsync(10).Result;
            hotelservice.DeleteHotelAsync(10);

            //assert
            Assert.AreNotEqual(editedhotel, newhotel);
        }


        [TestMethod]
        public void DeleteHotel()
        {
            //arrange
            HotelService hotelservice = new HotelService(connectionstring);
            List<Hotel> hotels = hotelservice.GetAllHotelAsync().Result;
            //act
            Hotel newhotel = new Hotel(10, "grenen", "morelhaven 113");
            bool ok=hotelservice.CreateHotelAsync(newhotel).Result;
            hotels = hotelservice.GetAllHotelAsync().Result;
            int hotelsbefore = hotels.Count;
            hotelservice.DeleteHotelAsync(10);
            hotels = hotelservice.GetAllHotelAsync().Result;
            int hotelsAfter = hotels.Count;

            //assert
            Assert.AreEqual(hotelsbefore-1, hotelsAfter);
        }

        [TestMethod]
        public void AddRoom()
        {
            //arrange
            RoomService Roomservice = new RoomService(connectionstring);
            List<Room> Rooms = Roomservice.GetAllRoomAsync(1).Result;
            //act
            int roomsbefore = Rooms.Count;
            Room newRoom = new Room(100,'D', 250, 1);
            bool ok= Roomservice.CreateRoomAsync(1,newRoom).Result;
            Rooms = Roomservice.GetAllRoomAsync(1).Result;
            int roomsafter= Rooms.Count;
            Roomservice.DeleteRoomAsync(100, 1);


            ////assert
            Assert.AreEqual(roomsbefore+1, roomsafter);
        }

        [TestMethod]
        public void updateRoom()
        {
            //arrange
            RoomService Roomservice = new RoomService(connectionstring);
            List<Room> Rooms = Roomservice.GetAllRoomAsync(1).Result;
            //act
            Room newRoom = new Room(100,'D', 250, 1);
            bool ok = Roomservice.CreateRoomAsync(1, newRoom).Result;
            Roomservice.UpdateRoomAsync(new Room(100, 'D', 300, 1),100, 1);
            Room editedRoom = Roomservice.GetRoomFromIdAsync(100, 1).Result;
            Roomservice.DeleteRoomAsync(100,1);

            //assert
            Assert.AreNotEqual(newRoom, editedRoom);
        }

        [TestMethod]
        public void deleteRoom()
        {
            //arrange
            RoomService Roomservice = new RoomService(connectionstring);
            List<Room> Rooms = Roomservice.GetAllRoomAsync(1).Result;
            //act

            Room newRoom = new Room(100, 'D', 250, 1);
            bool ok = Roomservice.CreateRoomAsync(1, newRoom).Result;
            Rooms = Roomservice.GetAllRoomAsync(1).Result;
            int roomsbefore = Rooms.Count;
            Roomservice.DeleteRoomAsync(100, 1);
            Rooms = Roomservice.GetAllRoomAsync(1).Result;
            int roomsafter = Rooms.Count;

            //assert
            Assert.AreEqual(roomsbefore-1, roomsafter);

        }

    }
}