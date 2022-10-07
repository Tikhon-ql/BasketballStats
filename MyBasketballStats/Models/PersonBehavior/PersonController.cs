using System;
using System.Collections.Generic;
using System.Linq;

namespace MyBasketballStats.Models.PersonBehavior
{
    public static class PersonController
    {
        public static string GetAllPointsAsString(Person person)
        {
            if (person.GamePerfomances.Count > 0)
            {
                string res = person.GamePerfomances[0].Points.ToString();
                for (int i = 1; i < person.GamePerfomances.Count; i++)
                    res += $" {person.GamePerfomances[i].Points.ToString()}";
                return res;
            }
            return "";
        }
        private static bool IsDatesAreBetween(DateTime date, DateTime beg, DateTime end)
        {
            if (date.Year >= beg.Year && date.Year <= end.Year)
            {
                if (date.Month >= beg.Month && date.Month <= end.Month)
                {
                    if (date.Day >= beg.Day && date.Day <= end.Day)
                        return true;
                }
            }
            return false;
        }
        public static string GetPointsBetweenAsString(Person person, DateTime begin, DateTime end)
        {
            List<BasketballGamePerfomance> list = person.GamePerfomances
                     .Where(p => IsDatesAreBetween(p.Date, begin, end))
                     .OrderBy(p => p.Date)
                     .ToList();
            if (list.Count > 0)
            {
                string res = list[0].Points.ToString();
                for (int i = 1; i < list.Count; i++)
                    res += $" {list[i].Points.ToString()}";
                return res;
            }
            return "";
        }
        public static string GetAllAssistsAsString(Person person)
        {
            if (person.GamePerfomances.Count > 0)
            {
                string res = person.GamePerfomances[0].Assists.ToString();
                for (int i = 1; i < person.GamePerfomances.Count; i++)
                    res += " " + person.GamePerfomances[i].Assists.ToString();
                return res;
            }
            return "";
        }

        public static string GetAvgBetween(Person person, DateTime begin, DateTime end)
        {
            List<BasketballGamePerfomance> list = person.GamePerfomances
                      .Where(p => IsDatesAreBetween(p.Date, begin, end))
                      .OrderBy(p => p.Date)
                      .ToList();
            if (list.Count > 0)
            {
                string res = (list[0].Points / Convert.ToDouble(list[0].Minutes)).ToString();
                for (int i = 1; i < list.Count; i++)
                    res += " " + (list[i].Points / Convert.ToDouble(list[i].Minutes)).ToString();
                return res;
            }
            return "";
        }

        public static string GetAssistsBetweenAsString(Person person, DateTime begin, DateTime end)
        {
            List<BasketballGamePerfomance> list = person.GamePerfomances
                      .Where(p => IsDatesAreBetween(p.Date, begin, end))
                      .OrderBy(p => p.Date)
                      .ToList();
            if (list.Count > 0)
            {
                string res = list[0].Assists.ToString();
                for (int i = 1; i < list.Count; i++)
                    res += " " + list[i].Assists.ToString();
                return res;
            }
            return "";
        }
        public static string GetAllStealsAsString(Person person)
        {
            if (person.GamePerfomances.Count > 0)
            {
                string res = person.GamePerfomances[0].Steals.ToString();
                for (int i = 1; i < person.GamePerfomances.Count; i++)
                    res += " " + person.GamePerfomances[i].Steals.ToString();
                return res;
            }
            return "";
        }
        public static string GetStealsBetweenAsString(Person person, DateTime begin, DateTime end)
        {
            List<BasketballGamePerfomance> list = person.GamePerfomances
                       .Where(p => IsDatesAreBetween(p.Date, begin, end))
                       .OrderBy(p => p.Date)
                       .ToList();
            if (list.Count > 0)
            {
                string res = list[0].Steals.ToString();
                for (int i = 1; i < list.Count; i++)
                    res += " " + list[i].Steals.ToString();
                return res;
            }
            return "";
        }
        public static string GetAllBlockshotsAsString(Person person)
        {
            if (person.GamePerfomances.Count > 0)
            {
                string res = person.GamePerfomances[0].Blockshots.ToString();
                for (int i = 1; i < person.GamePerfomances.Count; i++)
                    res += " " + person.GamePerfomances[i].Blockshots.ToString();
                return res;
            }
            return "";
        }
        public static string GetBlockshotsBetweenAsString(Person person, DateTime begin, DateTime end)
        {
            List<BasketballGamePerfomance> list = person.GamePerfomances
                     .Where(p => IsDatesAreBetween(p.Date, begin, end))
                     .OrderBy(p => p.Date)
                     .ToList();
            if (list.Count > 0)
            {
                string res = list[0].Blockshots.ToString();
                for (int i = 1; i < list.Count; i++)
                    res += " " + list[i].Blockshots.ToString();
                return res;
            }
            return "";
        }
        public static string GetAllReboundsAsString(Person person)
        {
            if (person.GamePerfomances.Count > 0)
            {
                string res = person.GamePerfomances[0].Rebounds.ToString();
                for (int i = 1; i < person.GamePerfomances.Count; i++)
                    res += $" {person.GamePerfomances[i].Rebounds}";
                return res;
            }
            return "";
        }
        public static string GetReboundsBetweenAsString(Person person, DateTime begin, DateTime end)
        {
            List<BasketballGamePerfomance> list = person.GamePerfomances
                     .Where(p => IsDatesAreBetween(p.Date, begin, end))
                     .OrderBy(p => p.Date)
                     .ToList();
            if (list.Count > 0)
            {
                string res = list[0].Rebounds.ToString();
                for (int i = 1; i < list.Count; i++)
                    res += $" {list[i].Rebounds}";
                return res;
            }
            return "";
        }
        public static string GetReboundsBetweenAsString(Person person, string begin, string end)
        {
            return "";
        }
        public static string GetGamesCurrentMounthsAsString(Person person)
        {
            if (person.GamePerfomances.Count > 0)
            {
                string res = "";
                person.GamePerfomances.OrderBy(p => p.Date.Year).ThenBy(p => p.Date.Month).ThenBy(p => p.Date.Day).ThenBy(p => p.Date.Hour)
                    .Where(p => p.Date.Month == DateTime.Now.Month && p.Date.Year == DateTime.Now.Year)
                    .ToList().ForEach(p => res += $"{p.Date.Day}.{p.Date.Month};");
                return res;
            }
            return "";
        }
        public static string GetGamesDaysBetweenAsString(Person person, DateTime begin, DateTime end)
        {
            List<BasketballGamePerfomance> list = person.GamePerfomances
                        .Where(p => IsDatesAreBetween(p.Date, begin, end))
                        .OrderBy(p => p.Date)
                        .ToList();
            if (list.Count > 0)
            {
                string res = list[0].Date.ToString("dd:MM");
                for (int i = 1; i < list.Count; i++)
                    res += $" {list[i].Date.ToString("dd:MM")}";
                return res;
            }
            return "";
        }
    }
}
