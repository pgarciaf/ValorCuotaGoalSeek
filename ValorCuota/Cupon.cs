using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValorCuota
{
    public class Cupon
    {
        public const decimal diasMes = 30;
        public double tasaInteresCredito;
        public int periodo;
        public int dias;
        public DateTime fechaPago;
        public double tasaInteres
        {
            get
            {
                return tasaInteresCredito * dias / (double)diasMes;
            }
            set
            {
                tasaInteres = value;
            }
        }
        public decimal montoInteres;
        public decimal montoAmortizacion;
        public decimal montoCuota;
        public decimal montoCapitalAdeudado;
        public decimal montoPrepago;
    }
}
