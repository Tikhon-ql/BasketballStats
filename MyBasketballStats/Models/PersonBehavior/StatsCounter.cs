namespace MyBasketballStats.Models.PersonBehavior
{
    public static class StatsCounter
    {
        public static void CountStats(Person person,BasketballGamePerfomance perfomance)
        {
            person.TotalScores += perfomance.Points;
            person.TotalAssists += perfomance.Assists;
            person.TotalBlockshots += perfomance.Blockshots;
            person.TotalSteals += perfomance.Steals;
            person.TotalThrowInGame += perfomance.ThrowInGame;
            person.TotalSuccessedThrowInGame += perfomance.SuccessedThrowInGame;
            person.TotalThreePointersMade += perfomance.ThreePointersMade;
            person.TotalSuccessedThreePointers += perfomance.SuccessedThreePointers;
            person.TotalMinutes += perfomance.Minutes;
        }
    }
}
