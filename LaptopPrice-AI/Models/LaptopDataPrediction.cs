using Microsoft.ML.Data;

namespace LaptopPrice_AI.Models
{
    public class LaptopDataPrediction
    {
        [ColumnName("Score")]
        public float Price { get; set; }
    }
}
