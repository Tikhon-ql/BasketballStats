namespace MyBasketballStats.Models.ManyToMany
{
    public class Friend
    {
        public string Id { get; set; }
        public string FirstFriendId { get; set; }
        public string SecondFriendId { get; set; }
    }
}
