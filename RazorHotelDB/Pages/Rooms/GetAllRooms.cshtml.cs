using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using RazorHotelDB.Interfaces;
using RazorHotelDB.Models;

namespace RazorHotelDB.Pages.Rooms
{
    public class GetAllRoomsModel : PageModel
    {

        private IRoomService _roomService;
        public int ID { get; set; }

        public List<Room> Rooms { get; set; }
        
        [BindProperty(SupportsGet = true)]
        public string FilterCriteria { get; set; }

        [BindProperty(SupportsGet = true)]
        public int FilterID { get; set; }

        public GetAllRoomsModel(IRoomService roomservice)
        {
            _roomService= roomservice;
        }

        public async Task OnGetAsync( int id)
        {
            try
            {
                if (!FilterCriteria.IsNullOrEmpty())
                {
                    if (FilterID == 1)
                    {
                        Rooms = await _roomService.SortBypriceAsync(1, int.Parse(FilterCriteria));
                    }
                }
                else
                {
                    Rooms = await _roomService.GetAllRoomAsync(id);
                    ID = Rooms[0].HotelNr;
                }

            }
            catch (Exception ex)
            {
                Rooms= new List<Room>();
                ViewData["Errormessage"] = ex.Message;
            }   
        }
    }
}
