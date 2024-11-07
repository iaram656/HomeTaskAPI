using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace appAPI.Data
{
    public class TAREA
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }

        public string DESCRIPTION { get; set; }
        public long USERID { get; set; }
        public DateTime LIMITDATE {  get; set; }
        public bool STATUS {  get; set; }
        public long TAREAID { get; set; }
    }
}
