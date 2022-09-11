using System.ComponentModel.DataAnnotations;

namespace CoupDeSonde.Models
{
    public class SurveyRequest
    {
        [Required]
        public Int32 Id { get; set; }
    }
}
