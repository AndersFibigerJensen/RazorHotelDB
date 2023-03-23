using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorHotelDB.Interfaces;
using RazorHotelDB.Models;

namespace RazorHotelDB.Pages.Rooms
{
    public class AddRoomModel : PageModel
    {
        private IRoomService _roomService;

        [BindProperty]
        public Room Room { get; set; }

        [BindProperty]
        public RoomType RoomType { get; set; }

        [BindProperty]
        public int HotelId { get; set; }

        public AddRoomModel(IRoomService roomservice)
        {
            _roomService= roomservice;
        }

        public async Task OnGetAsync(int id)
        {
            HotelId= id;
        }

        public async Task<IActionResult> OnpostAsync()
        {
            try 
            {
                Room.Types = RoomType.ToString()[0];
                await _roomService.CreateRoomAsync(HotelId, Room);
                return RedirectToPage("/Hotels/GetAllHotels");
            }
            catch (Exception ex)
            {
                ViewData["Errormessage"] = ex.Message;
            }
            return Page();

        }



    }
}
