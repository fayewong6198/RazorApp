using System.ComponentModel.DataAnnotations;

namespace aspnetapp.Models
{

    public class Post
    {
        public int Id { get; set; }
        [Required]
        [StringLength(10, ErrorMessage = "Title is too long.")]
        public string Title { get; set; }
        public string Status { get; set; }
        public Post()
        {

        }


    }
}