using LaptopPrice_AI.Models;
using LaptopPrice_AI.Services;
using Microsoft.AspNetCore.Mvc;

namespace LaptopPrice_AI.Controllers
{
    public class LaptopPricePredictionController : Controller
    {
        private readonly ILaptopPricePredictionService _predictionService;

        public LaptopPricePredictionController(ILaptopPricePredictionService predictionService)
        {
            _predictionService = predictionService;
        }

        public IActionResult Index()
        {
            return View(new LaptopDataViewModel());
        }

        [HttpPost]
        public IActionResult PredictPrice(LaptopDataViewModel input)
        {
            
            // Load list of CPU in CSV file
            var listCPUs = _predictionService.LoadCPUsFromCSV();

            // validate CPU
            if (!listCPUs.Contains(input.CPU))
            {
                ViewBag.ErrorMessage = "Unrecognized CPU. Please provide a valid CPU.";
                return View("Index", input);
            }

            //predict the price using input data
             var predictedPrice = _predictionService.PredictPrice(input);
             ViewBag.PredictedPrice = predictedPrice;

            return View("Index", input);
            
        }
    }
}
