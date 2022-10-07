using System;

namespace MyBasketballStats.Models.Abstract
{
    public abstract class GamePerfomance
    {
        public string Id { get; set; }
        public DateTime Date { get; set; }
        public int Points { get; set; }
        public int Assists { get; set; }
        public int Rebounds { get; set; }
        public int Steals { get; set; }
        public int Blockshots { get; set; }
        public int ThrowInGame { get; set; }
        public int SuccessedThrowInGame { get; set; }
        public int ThreePointersMade { get; set; }
        public int SuccessedThreePointers { get; set; }
        public int Minutes { get; set; }

        public string PersonId { get; set; }
        public Person Person { get; set; }
    }
}
