using System.Collections.Generic;

namespace projet_API.DTOs
{
    public class ProductResponseDto
    {
        public string _id { get; set; }
        public string image { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public RatingDto rating { get; set; }
        public int priceCents { get; set; }
        public List<string> keywords { get; set; }
        public string type { get; set; }
        public string sizeChartLink { get; set; }
    }

    public class RatingDto
    {
        public double stars { get; set; }
        public int count { get; set; }
    }
}
