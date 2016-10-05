using NewApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace NewApp.Controllers
{
    [RoutePrefix("api/Cars")]

    public class CarsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Route("Years")]

        public async Task<List<string>> GetYears()
        {
            return await db.GetYears();
        }

        [Route("Makes")]
        public async Task<List<string>> GetMakesByYear(string _year)
        {
            return await db.GetMakesByYear2(_year);
        }

        [Route("CarsByYear")]
        public async Task<List<Cars>> GetCarsByYear(string _year)
        {
            return await db.GetCarsByYear(_year);
        }

        [Route("Models")]
        public async Task<List<string>> GetModels(string _year, string _make)
        {
            return await db.GetModelsByYearAndMake(_year, _make);
        }

        [Route("CarsByYearAndMake")]
        public async Task<List<Cars>> GetCarsByYearAndMake(string _year, string _make)
        {
            return await db.GetCarsByYearAndMake(_year, _make);
        }

        [Route("Trims")]
        public async Task<List<string>> GetTrims(string _year, string _make, string _model)
        {
            return await db.GetTrimsByYearMakeAndModel(_year, _make, _model);
        }

        [Route("CarsByYearMakeAndModel")]
        public async Task<List<Cars>> GetCarsByYearMakeAndModel(string _year, string _make, string _model)
        {
            return await db.GetCarsByYearMakeAndModel(_year, _make, _model);
        }

        [Route("Car")]
        public async Task<Cars> GetCar(string _year, string _make, string _model, string _trim)

        {
            return await db.GetCar(_year, _make, _model, _trim);
        }

        [Route("NullableCar")]
        public async Task<Cars> GetNullCar(string _year, string _make, string _model, string _trim)
        {
            return await db.GetNullCar(_year, _make, _model, _trim);
        }

        [Route("CarData")]
        public async Task<IHttpActionResult> GetCarData(string _year, string _make, string _model, string _trim)
        {
            HttpResponseMessage response;
            //var content = "";
            var singleCar =  await GetCar(_year, _make, _model, _trim);
            var car = new carViewModel
            {
                Car = singleCar,
                Recalls = new carRecall(),
                Image = ""

            };


            //Get recall Data
            string result1 = "";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://www.nhtsa.gov/");
                try
                {
                    response = await client.GetAsync("webapi/api/Recalls/vehicle/modelyear/" + _year.ToLower() + "/make/"
                        + _make.ToLower() + "/model/" + _model.ToLower() + "?format=json");
                    result1 = await response.Content.ReadAsStringAsync();
                    car.Recalls = JsonConvert.DeserializeObject<carRecall>(result1);
                }
                catch (Exception e)
                {
                    return InternalServerError(e);
                }
            }



            //////////////////////////////   My Bing Search   //////////////////////////////////////////////////////////

            string query = _year + " " + _make + " " + _model + " " + _trim;

            string rootUri = "https://api.datamarket.azure.com/Bing/Search";

            var bingContainer = new Bing.BingSearchContainer(new Uri(rootUri));

            var accountKey = ConfigurationManager.AppSettings["searchKey"];

            bingContainer.Credentials = new NetworkCredential(accountKey, accountKey);


            var imageQuery = bingContainer.Image(query, null, null, null, null, null, null);

            var imageResults = imageQuery.Execute().ToList();


            car.Image = imageResults.First().MediaUrl;

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////

            return Ok(car);

        }

    }
       
  }
