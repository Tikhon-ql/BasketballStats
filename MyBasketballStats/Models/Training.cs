using MyBasketballStats.Enums;

namespace MyBasketballStats.Models
{
    public class Training
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public TrainingType Type { get; set; }
        public string Url { get; set; }
    }
}
