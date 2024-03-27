using LaptopPrice_AI.Models;
using Microsoft.ML;

namespace LaptopPrice_AI.Services
{
    public class LaptopPricePredictionService : ILaptopPricePredictionService
    {
        private readonly MLContext _mLContext;
        private readonly PredictionEngine<LaptopData, LaptopDataPrediction> _predictionEngine;

        public LaptopPricePredictionService()
        {
            _mLContext = new MLContext();
            var dataView = _mLContext.Data.LoadFromTextFile<LaptopData>("laptopprices.csv", separatorChar: ',', hasHeader: true);
            var trainer = _mLContext.Regression.Trainers.LightGbm(labelColumnName: "Price", featureColumnName: "Features");
            var pipeline = _mLContext.Transforms.Concatenate("Features", "CPU", "GHz", "GPU", "RAM", "RAMType", "Screen", "Storage", "SSD", "Weight")
                .Append(trainer);
            var model = pipeline.Fit(dataView);
            _predictionEngine = _mLContext.Model.CreatePredictionEngine<LaptopData, LaptopDataPrediction>(model);
        }

        public float PredictPrice(LaptopData input)
        {
            var prediction = _predictionEngine.Predict(input);
            return prediction.Price;
        }
    }
}
