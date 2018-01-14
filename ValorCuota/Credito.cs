using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValorCuota
{
    public class Credito
    {
        public double tasaInteres;
        public int montoNotario;
        public int montoImpuesto;
        public int montoLiquido;
        public int montoBruto { get { return montoNotario + montoImpuesto + montoLiquido; } set { montoBruto = value; } } 
        public int mesesGracia;
        public DateTime fechaOtorgamiento;
        public int numeroCuotas;
        public int montoTotalCredito
        {
            get
            {
                return (int)(montoBruto + (tasaInteres * montoBruto));
            }
            set
            {
                montoTotalCredito = value;
            }
        }
        public LinkedList<Cupon> lstCupones;

        public Credito()
        {
            lstCupones = new LinkedList<Cupon>();
        }
    }
}
