using Microsoft.ML.Data;

namespace LaptopPrice_AI.Models
{
    public class LaptopData
    {
        [LoadColumn(0)]
        public string CPU { get; set; }

        [LoadColumn(1)]
        public float GHz { get; set;}

        [LoadColumn(2)]
        public string GPU { get; set; }

        [LoadColumn(3)]
        public float RAM { get; set;}

        [LoadColumn(4)]
        public string RAMType { get; set; }

        [LoadColumn(5)]
        public float Screen { get; set; }

        [LoadColumn(6)]
        public float Storage { get; set; }

        [LoadColumn(7)]
        public bool SSD { get; set; }

        [LoadColumn(8)]
        public float Weight { get; set; }

        [LoadColumn(9)]
        public float Price { get; set; }

    }
}
