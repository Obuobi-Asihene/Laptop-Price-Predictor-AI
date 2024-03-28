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
            // initializing MLContext
            _mLContext = new MLContext();

            // Loading dataset from CSV file
            var dataView = _mLContext.Data.LoadFromTextFile<LaptopData>("laptopprices.csv", separatorChar: ',', hasHeader: true);
            
            // Defining the training pipeline
            var trainer = _mLContext.Regression.Trainers.LightGbm(labelColumnName: "Price", featureColumnName: "Features");
            var pipeline = _mLContext.Transforms.CopyColumns(outputColumnName: "Label", inputColumnName: "Price")
                .Append(_mLContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "CPUEncoded", inputColumnName: "CPU"))
                .Append(_mLContext.Transforms.NormalizeMinMax(outputColumnName: "GHzNormalized", inputColumnName: "GHz"))
                .Append(_mLContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "GPUEncoded", inputColumnName: "GPU"))
                .Append(_mLContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "RAMTypeEncoded", inputColumnName: "RAMType"))
                .Append(_mLContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "SSDEncoded", inputColumnName: "SSD"))
                .Append(_mLContext.Transforms.Concatenate("Features", "CPUEncoded", "GHzNormalized", "GPUEncoded", "RAM", "RAMTypeEncoded", "Screen", "Storage", "SSDEncoded", "Weight"))
                .Append(trainer);

            // Training the model
            var model = pipeline.Fit(dataView);

            // Create prediction engine
            _predictionEngine = _mLContext.Model.CreatePredictionEngine<LaptopData, LaptopDataPrediction>(model);
        }

        // method to predict laptop price based on specs
        public float PredictPrice(LaptopData input)
        {
            var prediction = _predictionEngine.Predict(input);
            return prediction.Price;
        }

        // method to load CPUs from CSV file
        public List<string> LoadCPUsFromCSV()
        {
            List<string> listCPUs = new List<string>();

            using (var reader = new StreamReader("laptopprices.csv"))
            {
                reader.ReadLine();

                // Read CPU data from each line and add to the list of CPUs
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
