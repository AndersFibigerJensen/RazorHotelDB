using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorHotelDB.Interfaces;
using RazorHotelDB.Models;

namespace RazorHotelDB.Pages.Hotels
{
    public class UpdateHotelModel : PageModel
    {
        private IHotelService _hotelService;


        /// <summary>
        /// Binder et hotel til en række properties
        /// </summary>
        [BindProperty]
        public Hotel Hotel { get; set; }

        public UpdateHotelModel(IHotelService hotelService)
        {
            _hotelService = hotelService;
        }

        /// <summary>
        /// Henter et hotel fra databasen ud fra id
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="ex">laver en fejl besked </exception>
        /// <returns></returns>
        public async Task OnGetAsync(int id)
        { 
            try
            {
                Hotel = await _hotelService.GetHotelFromIdAsync(id);
            }
            catch(Exception ex) 
            {
                ViewData["Errormessage"] = ex.Message;
            }
        }

        /// <summary>
        /// opdatere et hotel og vender tilbage til GetAllHotels
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="ex">laver en fejl besked </exception>
        /// <returns></returns>
        public async Task<IActionResult> OnpostAsync(int id)
        {
            try
            {
                await _hotelService.UpdateHotelAsync(Hotel, id);
                return RedirectToPage("GetAllHotels");

            }
            catch(Exception ex) 
            {
                ViewData["Errormessage"] = ex.Message;
            }
            return Page();

        }
    }
}
