using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace appAPI.Data
{
    public class PENALIZATION
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }

        public string REASON { get; set; }
        public long USERID { get; set; }

        public long POINTS { get; set; }
        public DateTime DATE { get; set; }
    }
}
