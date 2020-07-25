using System.ComponentModel.DataAnnotations;

namespace WebServer.Models
{
    public class ApiInputMessage
    {
        [Required(ErrorMessage = "Content should be not empty")]
        [StringLength(128)]
        public string Content { get; set; }
        [Required(ErrorMessage = "Number should be not empty")]
        public int Number { get; set; }
    }
}
