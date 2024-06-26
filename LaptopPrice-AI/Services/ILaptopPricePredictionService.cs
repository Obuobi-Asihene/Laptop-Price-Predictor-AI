﻿using LaptopPrice_AI.Models;

namespace LaptopPrice_AI.Services
{
    public interface ILaptopPricePredictionService
    {
        float PredictPrice(LaptopDataViewModel input);
        List<string> LoadCPUsFromCSV();
    }
}
