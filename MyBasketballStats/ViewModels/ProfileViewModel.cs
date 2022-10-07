using MyBasketballStats.Enums;
using MyBasketballStats.Models;
using MyBasketballStats.Models.PersonBehavior;
using System;

namespace MyBasketballStats.ViewModels
{
    public class ProfileViewModel
    {
        //public User User { get; set; }
        public string Email { get; set; }
        public Person Person { get; set; }
        public DateTime Begin { get; set; }
        public DateTime End { get; set; }
        public ProfileAccessType AccessType { get; set; }
        public string Duration { get; set; }

        public string GetPointsBetween()
        {
            return PersonController.GetPointsBetweenAsString(Person, Begin, End);
        }
        public string GetAssistsBetween()
        {
            return PersonController.GetAssistsBetweenAsString(Person, Begin, End);
        }
        public string GetBlockshotsBetween()
        {
            return PersonController.GetBlockshotsBetweenAsString(Person, Begin, End);
        }
        public string GetStealsBetween()
        {
            return PersonController.GetStealsBetweenAsString(Person, Begin, End);
        }
        public string GetReboundsBetween()
        {
            return PersonController.GetReboundsBetweenAsString(Person, Begin, End);
        }   
        public string GetDaysBetween()
        {
            return PersonController.GetGamesDaysBetweenAsString(Person, Begin, End);
        }
        public string GetAvgBetween()
        {
            return PersonController.GetAvgBetween(Person,Begin,End);
        }
    }
}
