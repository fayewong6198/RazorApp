using System.ComponentModel.DataAnnotations;

namespace aspnetapp.Models
{
    public class FeedItem
    {
        public int Id {get;set;}
        public Feed Feed {get;set;}
        public int FeedId {get;set;}
        public string Data {get;set;}
        public string Url {get;set;}

        public string Type {get;set;}
    }
}