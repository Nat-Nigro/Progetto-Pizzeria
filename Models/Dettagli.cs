namespace CiroKebab.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Dettagli")]
    public partial class Dettagli
    {
        [Key]
        public int idDettagli { get; set; }

        public int idOrdine_FK { get; set; }

        public int idProdotto_FK { get; set; }

        public int Quantita { get; set; }

        public virtual Ordini Ordini { get; set; }

        public virtual Prodotti Prodotti { get; set; }
    }
}
