using System.Collections.Generic;
using FoodRescue.Web.Models;

namespace FoodRescue.Web.Services
{
    public class FoodAnalysisStore
    {
        private readonly List<FoodAnalysisResult> _results = new();

        public IReadOnlyList<FoodAnalysisResult> Results => _results;


        public void Add(string base64Image, string caption, string jsonTable)
        {
            _results.Add(new FoodAnalysisResult
            {
                ImageBase64 = base64Image,
                Caption = caption,
                JsonTable = jsonTable
            });
        }


        public void Clear() => _results.Clear();
    }
}
