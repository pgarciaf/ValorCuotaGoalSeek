using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValorCuota
{
    public class Calculo
    {
        private const int factorRedondeo = 7;
        
        public static void SeekGoal(string lsFechaOtorgamiento, string lsmontoLiquido, string lsmontoImpuesto, string lsmontoNotario, string lstasaInteres, string lsnumeroCuotas)
        {
            Credito loCredito = new Credito
            {
                fechaOtorgamiento = DateTime.Parse(lsFechaOtorgamiento),
                mesesGracia = 1,
                montoLiquido = int.Parse(lsmontoLiquido),
                montoImpuesto = int.Parse(lsmontoImpuesto),
                montoNotario = int.Parse(lsmontoNotario),
                tasaInteres = (double.Parse(lstasaInteres) / 100),
                numeroCuotas = int.Parse(lsnumeroCuotas)

            };

            GenerarFechasPago(loCredito);

            Console.WriteLine(string.Format("fechaOtorgamiento: {0} | montoTotal: {1} | tasaInteres: {2}", loCredito.fechaOtorgamiento.ToShortDateString(), loCredito.montoBruto, loCredito.tasaInteres));

            GoalSeek(loCredito);
            Console.WriteLine();
            Console.WriteLine();

            var loCurrentNode = loCredito.lstCupones.First;
            while ((loCurrentNode != null)) //&& (currentNode.Value != desiredValue))
            {
                Console.WriteLine(string.Format("periodo: {0} | fechaPago: {1} | tasaInteres: {2} | montoInteres: {3} | cuota: {4} | Amort: {5} | montoAdeudado: {6}", loCurrentNode.Value.periodo, loCurrentNode.Value.fechaPago.ToShortDateString(), loCurrentNode.Value.tasaInteres, Math.Ceiling(loCurrentNode.Value.montoInteres), Math.Ceiling(loCurrentNode.Value.montoCuota), Math.Ceiling(loCurrentNode.Value.montoAmortizacion), Math.Ceiling(loCurrentNode.Value.montoCapitalAdeudado)));
                loCurrentNode = loCurrentNode.Next;
            }


        }

        private static void GoalSeek(Credito loCredito)
        {
            var loReiniciar = false;
            var currentNode = loCredito.lstCupones.First;
            var loTasaInteresAprox = loCredito.tasaInteres;
            decimal loMontoCuota = (decimal)((loTasaInteresAprox * loCredito.montoTotalCredito) / (1 - Math.Pow(1 + loTasaInteresAprox, loCredito.numeroCuotas * -1)));
            double factorAjuste = 100;
            int pasoNegativo = 0;
            int pasoPositivo = 0;

            while ((currentNode != null)) //&& (currentNode.Value != desiredValue))
            {
                currentNode.Value.montoCuota = loMontoCuota;
                // Pregunto si es el primer nodo
                if (currentNode.Previous == null)
                {
                    // En caso que sea un reinicio, se vuelve a dejar en falso para aumentar la tasa.
                    loReiniciar = false;
                    currentNode.Value.montoInteres = Math.Round((decimal)currentNode.Value.tasaInteres * loCredito.montoTotalCredito, 0);
                    currentNode.Value.montoAmortizacion = loMontoCuota - currentNode.Value.montoInteres;
                    currentNode.Value.montoCapitalAdeudado = loCredito.montoTotalCredito - currentNode.Value.montoAmortizacion;
                }
                else
                {
                    currentNode.Value.montoInteres = Math.Round((decimal)currentNode.Value.tasaInteres * currentNode.Previous.Value.montoCapitalAdeudado, 0);
                    currentNode.Value.montoAmortizacion = loMontoCuota - currentNode.Value.montoInteres;
                    currentNode.Value.montoCapitalAdeudado = currentNode.Previous.Value.montoCapitalAdeudado - currentNode.Value.montoAmortizacion;
                }

                // pregunto si es el ultimo
                if (currentNode.Next == null)
                {
                    if (Math.Round(currentNode.Value.montoCapitalAdeudado, factorRedondeo) == 0)
                        break;
                    else
                    {
                        if (currentNode.Value.montoCapitalAdeudado < 0)
                        {
                            loTasaInteresAprox = loTasaInteresAprox - (loTasaInteresAprox * 1 / factorAjuste);
                            pasoNegativo++;
                            factorAjuste = factorAjuste + factorAjuste * (pasoNegativo / 100);
                        }
                        else
                        {
                            loTasaInteresAprox = loTasaInteresAprox + (loTasaInteresAprox * 1 / factorAjuste);
                            pasoPositivo++;
                            factorAjuste = factorAjuste + factorAjuste * (pasoPositivo / 100);
                        }

                        Console.WriteLine("Int: {0} | Cuota: {1} | loTasaInteresAprox: {2} | FactorAjuste: {3} | montoCapitalAdeudado: {4}", pasoNegativo + pasoNegativo, currentNode.Value.montoCuota, loTasaInteresAprox, factorAjuste, currentNode.Value.montoCapitalAdeudado);
                        currentNode = loCredito.lstCupones.First;
                        loMontoCuota = (decimal)((loTasaInteresAprox * loCredito.montoTotalCredito) / (1 - Math.Pow(1 + loTasaInteresAprox, loCredito.numeroCuotas * -1)));
                        loReiniciar = true;
                        //Console.ReadLine();
                    }
                }

                if (!loReiniciar)
                    currentNode = currentNode.Next;
            }
        }

        private static void GenerarFechasPago(Credito loCredito)
        {
            for (int i = 0; i < loCredito.numeroCuotas; i++)
            {
                int dias = 0;
                DateTime fechaPago = DateTime.MaxValue;

                if (i == 0)
                {
                    fechaPago = loCredito.fechaOtorgamiento.AddMonths(loCredito.mesesGracia);
                    var diaPago = fechaPago.Day;
                    while (diaPago != 20)
                    {
                        fechaPago = fechaPago.AddDays(1);
                        diaPago = fechaPago.Day;
                    }

                    fechaPago = fechaHabilSiguiente(fechaPago);

                    dias = (int)(fechaPago - loCredito.fechaOtorgamiento).TotalDays;
                    loCredito.lstCupones.AddFirst(new Cupon { periodo = i + 1, dias = dias, fechaPago = fechaPago, tasaInteresCredito = loCredito.tasaInteres });
                }
                else
                {
                    DateTime fechaPagoAnt = loCredito.lstCupones.Last.Value.fechaPago;
                    fechaPago = fechaPagoAnt.AddDays(30);
                    var diaPago = fechaPago.Day;
                    while (fechaPago.Day != 20)
                        fechaPago = fechaPago.AddDays((diaPago < 20 ? +1 : -1));

                    fechaPago = fechaHabilSiguiente(fechaPago);
                    dias = (int)(fechaPago - fechaPagoAnt).TotalDays;
                    loCredito.lstCupones.AddLast(new Cupon { periodo = i + 1, dias = dias, fechaPago = fechaPago, tasaInteresCredito = loCredito.tasaInteres });
                }

            }
        }

        private static DateTime fechaHabilSiguiente(DateTime ldFecha)
        {
            while (ldFecha.DayOfWeek == DayOfWeek.Saturday || ldFecha.DayOfWeek == DayOfWeek.Sunday)
                ldFecha = ldFecha.AddDays(1);

            return ldFecha;
        }
    }
}
