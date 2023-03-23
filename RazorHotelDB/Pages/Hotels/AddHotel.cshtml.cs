using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorHotelDB.Interfaces;
using RazorHotelDB.Models;

namespace RazorHotelDB.Pages.Hotels
{
    public class AddHotelModel : PageModel
    {

        private IHotelService _hotelService;

        /// <summary>
        /// Binder forskellige properties til siden 
        /// </summary>
        [BindProperty]
        public Hotel Hotel { get; set; }

        public AddHotelModel(IHotelService hotelService)
        {
            _hotelService = hotelService;
        }

        /// <summary>
        /// skaber et hotel og går tilbage til GetAllHotels
        /// </summary>
        /// <exception cref="ex"> laver en fejl besked </exception>
        /// <returns></returns>
        public async Task<IActionResult> OnpostAsync()
        {
            try
            {
                await _hotelService.CreateHotelAsync(Hotel);
                return RedirectToPage("GetAllHotels");
            }
            catch(Exception ex)
            {
                ViewData["Errormessage"]=ex.Message;
            }
            return Page();

        }

        public async Task OnGet()
        {

        }
    }
}
