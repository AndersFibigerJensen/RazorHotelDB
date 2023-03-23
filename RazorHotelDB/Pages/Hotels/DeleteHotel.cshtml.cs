using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorHotelDB.Interfaces;
using RazorHotelDB.Models;
using RazorHotelDB.Services;

namespace RazorHotelDB.Pages.Hotels
{
    public class DeleteHotelModel : PageModel
    {

        private IHotelService hotelService;


        /// <summary>
        /// Binder forskellige attributter til hotel
        /// </summary>
        [BindProperty]
        public Hotel Hotel { get; set; }


        public DeleteHotelModel(IHotelService Hotelservice)
        {
            hotelService= Hotelservice;
        }

        /// <summary>
        /// Henter et hotel ud fra id og sætter Hotel til at være lig med det hentet hotel
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task OnGetAsync(int id)
        {
            try
            {
                Hotel = await hotelService.GetHotelFromIdAsync(id);
            }
            catch (Exception ex)
            {
                ViewData["Errormessage"] = ex.Message;
            }

        }

        /// <summary>
        /// sletter et hotel fra databasen og fører siden tilbage til RazorPage GetAllHotels
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="ex"> opretter en besked som viser hvad fejlen er </exception>
        /// <returns> går tilbage til GetAllHotels </returns>
        public async Task<IActionResult> OnpostAsync(int id)
        {
            try
            {
                await hotelService.DeleteHotelAsync(id);
                return RedirectToPage("GetAllHotels");
            }
            catch (Exception ex)
            {
                ViewData["Errormessage"] = ex.Message;
            }
            return Page();
        }


    }
}
