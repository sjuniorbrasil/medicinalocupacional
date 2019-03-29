using System.ComponentModel.DataAnnotations;

namespace SaudeWeb.Models
{
    public class Risco
    {
        public Risco()
        {

        }
        [Key]
        public int Id { get; set; }

        [Required]
        public string Descricao { get; set; }

    }
}