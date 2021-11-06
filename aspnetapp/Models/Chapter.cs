using System.ComponentModel.DataAnnotations;

namespace aspnetapp.Models
{
    public class Chapter
    {
        public int Id {get;set;}
        public Post Post {get;set;}
        public int PostId {get;set;}
        public string ChapterNumber {get;set;}
        public string Content {get;set;}
    
    }
}