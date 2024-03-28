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
                .Append(_mLContext.Transforms.Categorical.OneHotEncoding("CPUEncoded", "CPU"))
                .Append(_mLContext.Transforms.NormalizeMinMax("GHzNormalized", "GHz"))
                .Append(_mLContext.Transforms.Categorical.OneHotEncoding("GPUEncoded", "GPU"))
                .Append(_mLContext.Transforms.Categorical.OneHotEncoding("RAMTypeEncoded", "RAMType"))
                .Append(_mLContext.Transforms.Categorical.OneHotEncoding("SSDEncoded", "SSD"))
                .Append(_mLContext.Transforms.Concatenate("Features", "CPUEncoded", "GHzNormalized", "GPUEncoded", "RAM", "RAMTypeEncoded", "Screen", "Storage", "SSDEncoded", "Weight"))
                .Append(trainer);

            // Training the model
            var model = pipeline.Fit(dataView);

            // Create prediction engine
            _predictionEngine = _mLContext.Model.CreatePredictionEngine<LaptopData, LaptopDataPrediction>(model);
        }

        // method to predict laptop price based on specs
        public float PredictPrice(LaptopDataViewModel input)
        {
            // Convert ViewModel to Model
            var laptopData = new LaptopData
            {
                CPU = input.CPU,
                GHz = input.GHz,
                GPU = input.GPU,
                RAM = input.RAM,
                RAMType = input.RAMType,
                Screen = input.Screen,
                Storage = input.Storage,
                SSD = input.SSD,
                Weight = input.Weight
            };

            // validate input
            if (input == null) 
                throw new ArgumentNullException(nameof(laptopData), "Input data cannot be null");

            try
            {
                var prediction = _predictionEngine.Predict(laptopData);

                return prediction.Price;
            }
            catch (Exception ex)
            {
                // Log exception
                Console.WriteLine($"Error Predicting Price: {ex.Message}");

                return 0;
            }
        }

        // method to load CPUs from CSV file
        public List<string> LoadCPUsFromCSV()
        {
            List<string> listCPUs = new List<string>();

            try
            {
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
            }
            catch (Exception ex)
            {
                // Log exception
                Console.WriteLine($"Error loading CPUs from CSV: {ex.Message}");
            }

            return listCPUs;
        }
    }
}
