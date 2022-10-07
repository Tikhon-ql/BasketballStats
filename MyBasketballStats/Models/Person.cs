using MyBasketballStats.Models.ManyToMany;
using System.Collections.Generic;

namespace MyBasketballStats.Models
{
    public class Person
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public string Email { get; set; }
        public int TotalScores { get; set; }
        public int TotalSteals { get; set; }
        public int TotalBlockshots { get; set; }
        public int TotalAssists { get; set; }
        public int TotalThrowInGame { get; set; }
        public int TotalSuccessedThrowInGame { get; set; }
        public int TotalThreePointersMade { get; set; }
        public int TotalSuccessedThreePointers { get; set; }
        public int TotalMinutes { get; set; }
        public Geoposition Geoposition { get; set; }
        public string GeopositionId { get; set; }
        public virtual List<Friend> Friends { get; set; } = new();
        public virtual List<BasketballGamePerfomance> GamePerfomances { get; set; } = new();
        public virtual List<Notification> Notifications { get; set; }
    }
}
