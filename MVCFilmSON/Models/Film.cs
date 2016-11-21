using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCFilmSON.Models
{
   public class Film
    {
        [Key]
        public int FilmID { get; set; }
        public short Yil { get; set; }
        public string FilmAdi { get; set; }
        public string Yonetmen { get; set; }
        public string Konu { get; set; }
        [ForeignKey("FilminKategorisi")]
        public int KategoriID { get; set; }
        public string ResimURL { get; set; }
        public string Fragman { get; set; }
        public bool Yabanci { get; set; }

        public virtual Kategori FilminKategorisi { get; set; }
    }
    public class Kategori
    {
        [Key]
        public int KategoriID { get; set; }
        public string KategoriAdi { get; set; }

        public virtual List<Film> FilmListesi { get; set; }
    }
}
