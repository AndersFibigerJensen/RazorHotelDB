using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using RazorHotelDB.Interfaces;
using RazorHotelDB.Models;
using RazorHotelDB.Services;

namespace RazorHotelDB.Pages.Rooms
{
    public class UpdateRoomModel : PageModel
    {

        private IRoomService _roomService;
        private IHotelService _hotelService;

        [BindProperty]
        public Room Room { get; set; }

        [BindProperty]
        public int selectId { get; set; }

        public List<Hotel> Hotels { get; set; }

        [BindProperty]
        public RoomType RoomType { get; set; }


        public SelectList hotelList { get; set; }
        public UpdateRoomModel(IRoomService roomservice, IHotelService hotelservice)
        {
            _roomService= roomservice;
            _hotelService= hotelservice;
            Hotels = _hotelService.GetAllHotelAsync().Result;
        }

        public async Task OnGet(int id,int HotelId)
        {
            Room = await _roomService.GetRoomFromIdAsync(id, HotelId);

        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            try 
            {
                Room.Types = RoomType.ToString()[0];
                await _roomService.UpdateRoomAsync(Room, id, Room.HotelNr);
                return RedirectToPage("/Hotels/GetAllHotels");

            }
            catch(Exception ex) 
            {
                ViewData["Errormessage"]= ex.Message;
            
            }
            return Page();



        }
    }
}
