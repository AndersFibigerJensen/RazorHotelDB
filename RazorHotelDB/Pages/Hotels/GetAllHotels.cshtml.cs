using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using RazorHotelDB.Interfaces;
using RazorHotelDB.Models;
using RazorHotelDB.Services;
using System.Net;

namespace RazorHotelDB.Pages.Hotels
{
    public class GetAllHotelsModel : PageModel
    {

        private IHotelService hservice;

        /// <summary>
        /// Binder en sætning eller et ord til filtercriteria
        /// </summary>

        [BindProperty(SupportsGet =true)]
        public string FilterCriteria { get; set; }


        /// <summary>
        /// binder et tal til FilterID
        /// </summary>
        [BindProperty(SupportsGet =true)]
        public int FilterID { get; set; }

        public List<Hotel> Hotels { get; set; }

        public GetAllHotelsModel(IHotelService hotelservice)
        {
            hservice= hotelservice;
        }

        public async Task OnPost()
        {


        }

        /// <summary>
        /// bruger et FilterID til at vælge om der bliver filtret på navn eller addresse hvis der ikke er valgt noget eller filtercriteria er null henter den alle hoteller
        /// </summary>
        /// <exception cref="ex">laver en tom liste af hoteller og laver en fejl besked </exception>
        /// <returns></returns>
        public async Task OnGetAsync()
        {
            try
            {
                if (!FilterCriteria.IsNullOrEmpty())
                {
                    if (FilterID == 1)
                    {
                        Hotels = await hservice.GetHotelsByNameAsync(FilterCriteria);
                    }
                    else if (FilterID == 2)
                    {
                        Hotels = await hservice.GetHotelsByAddressAsync(FilterCriteria);
                    }

                }
                else
                {
                    Hotels = await hservice.GetAllHotelAsync();
                }

            }
            catch(Exception ex) 
            {
                Hotels = new List<Hotel>();
                ViewData["Errormessage"] = ex.Message;

            }

            
        }
    }
}
