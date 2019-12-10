using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DNMOFT.RNC.Context
{
    [Table("mContribuyentes")]
    public class mContribuyente
    {
        public mContribuyente(string[] datum)
        {
            if (datum.Length == 11)
            {
                RNC = datum[0];
                RazonSocial = datum[1];
                NombreComercial = datum[2];
                Categoria = datum[3];
                CalleAvenida = datum[4];
                Numero = datum[5];
                Sector = datum[6];
                Telefono = datum[7];
                Registrado = datum[8];
                Estado = datum[9];
                RegimenPagos = datum[10];
            }

            Actualizado = DateTime.Now;

        }
        [MaxLength(13)]
        public string RNC { get; set; }
        [MaxLength(70)]
        public string RazonSocial { get; set; }
        [MaxLength(70)]
        public string NombreComercial { get; set; }
        [MaxLength(40)]
        public string Categoria { get; set; }
        [MaxLength(70)]
        public string CalleAvenida { get; set; }
        [MaxLength(10)]
        public string Numero { get; set; }
        [MaxLength(70)]
        public string Sector { get; set; }
        [MaxLength(10)]
        public string Telefono { get; set; }
        [MaxLength(10)]
        public string Registrado { get; set; }
        [MaxLength(20)]
        public string Estado { get; set; }
        [MaxLength(10)]
        public string RegimenPagos { get; set; }
        public DateTime Actualizado { get; set; }

    }
}
