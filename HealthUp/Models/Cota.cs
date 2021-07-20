﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthUp.Models
{
    public class Cota
    {

        public Cota()
        {


        }
        public Cota(string numsocio)
        {
            NumSocio = numsocio;
            DataRegisto = DateTime.Now.Date;
        }
        [Key]
        public int IdCota { get; set; }

        public string NumSocio { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Data de registo")]
        public DateTime? DataRegisto { get; set; }

        [ForeignKey(nameof(NumSocio))]
        [InverseProperty(nameof(Socio.Cotas))]
        public virtual Socio NumSocioNavigation { get; set; }

        [Required]
        [Display(Name = "Número de cotas pagas")]
        public int NumeroCotasPagas { get; set; } = 0;


        private int N_Meses { get => Math.Abs(DateTime.Now.Month - DataRegisto.GetValueOrDefault().Month + (12 * (DateTime.Now.Year - DataRegisto.GetValueOrDefault().Year))) + 1; set => N_Meses = value; }

        [NotMapped]
        public int NumeroCotasNaoPagas => N_Meses - NumeroCotasPagas;

        public bool AreCotasPagas()
        {
            if (NumeroCotasNaoPagas > 0)
            {
                return false;
            }
            return true;
        }
        public void AcertarCotas()
        {
            NumeroCotasPagas += NumeroCotasNaoPagas;
        }
    }
}
