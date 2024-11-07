using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace appAPI.Data
{
    public class TRABAJOS
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }

        public string DESC { get; set; }

        public long PUNTUEK { get; set; }
    }
}
