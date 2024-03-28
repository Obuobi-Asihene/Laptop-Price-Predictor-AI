using LaptopPrice_AI.Models;

namespace LaptopPrice_AI.Services
{
    public interface ILaptopPricePredictionService
    {
        float PredictPrice(LaptopData input);
        List<string> LoadCPUsFromCSV();
    }
}
