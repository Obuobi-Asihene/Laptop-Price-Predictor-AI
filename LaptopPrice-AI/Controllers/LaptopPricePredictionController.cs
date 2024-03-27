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
            return View();
        }

        [HttpPost]
        public IActionResult PredictPrice(LaptopData input)
        {
            // validate input
            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessage = "Form Invalid";
                return View("Index", input);
            }
            
            // Load list of CPU in csv
            var listCPUs = LoadCPUsFromCSV();

            // validate CPU
            if (!listCPUs.Contains(input.CPU))
            {
                ViewBag.ErrorMessage = "Unrecognized CPU. Please provide a valid CPU.";
                return View("Index", input);
            } 
            

            var predictedPrice = _predictionService.PredictPrice(input);
            ViewBag.PredictedPrice = predictedPrice;
            return View("Index", input);
            
        }

        private static List<string> LoadCPUsFromCSV()
        {
            List<string> listCPUs = new List<string>();

            using (var reader = new StreamReader("laptopprices.csv"))
            {
                reader.ReadLine();

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    var cpu = values[0].Trim();
                    listCPUs.Add(cpu);
                }
            }

            return listCPUs;
        }
    }
}
