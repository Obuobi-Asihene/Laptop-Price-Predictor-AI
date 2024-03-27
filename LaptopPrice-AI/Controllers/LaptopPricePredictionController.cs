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
            var predictedPrice = _predictionService.PredictPrice(input);
            ViewBag.PredictedPrice = predictedPrice;
            return View("Index", input);
        }
    }
}
