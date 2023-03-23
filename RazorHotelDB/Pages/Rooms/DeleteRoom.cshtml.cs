using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorHotelDB.Interfaces;
using RazorHotelDB.Models;

namespace RazorHotelDB.Pages.Rooms
{
    public class DeleteRoomModel : PageModel
    {

        private IRoomService _roomService;

        [BindProperty]
        public Room Room { get; set; }

        public DeleteRoomModel(IRoomService roomService)
        {
            _roomService = roomService;
        }

        public async Task OnGetAsync(int id,int hotelid)
        {
            Room= await _roomService.GetRoomFromIdAsync(id,hotelid);
            
        }

        public async Task<IActionResult> OnPostAsync(int id,int hotelid)
        {
            try
            {
                await _roomService.DeleteRoomAsync(id, hotelid);
            }
            catch (Exception ex)
            {
                ViewData["Errormessage"] = ex.Message;
            }
            return Page();

        }
    }
}
