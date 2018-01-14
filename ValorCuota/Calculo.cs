using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValorCuota
{
    public class Calculo
    {
        
        public static void SeekGoal()
        {
            Credito loCredito = new Credito
            {
                fechaOtorgamiento = new DateTime(2017, 8, 21),
                mesesGracia = 1,
                montoLiquido = 800000,
                montoImpuesto = 6460,
                montoNotario = 1000,
                tasaInteres = (1.35 / 100),
                numeroCuotas = 14

            };

            //loCredito.montoTotalCredito = (int)(loCredito.montoBruto + (loCredito.tasaInteres * loCredito.montoBruto));

            loCredito.lstCupones.AddFirst(new Cupon { periodo = 1, dias = 60, fechaPago = new DateTime(2017, 10, 20), tasaInteresCredito = loCredito.tasaInteres });
            loCredito.lstCupones.AddLast(new Cupon { periodo = 2, dias = 31, fechaPago = new DateTime(2017, 11, 20) , tasaInteresCredito = loCredito.tasaInteres });
            loCredito.lstCupones.AddLast(new Cupon { periodo = 3, dias = 30, fechaPago = new DateTime(2017, 12, 20) , tasaInteresCredito = loCredito.tasaInteres });
            loCredito.lstCupones.AddLast(new Cupon { periodo = 4, dias = 33, fechaPago = new DateTime(2018, 01, 22) , tasaInteresCredito = loCredito.tasaInteres });
            loCredito.lstCupones.AddLast(new Cupon { periodo = 5, dias = 29, fechaPago = new DateTime(2018, 02, 20) , tasaInteresCredito = loCredito.tasaInteres });
            loCredito.lstCupones.AddLast(new Cupon { periodo = 6, dias = 28, fechaPago = new DateTime(2018, 03, 20) , tasaInteresCredito = loCredito.tasaInteres });
            loCredito.lstCupones.AddLast(new Cupon { periodo = 7, dias = 31, fechaPago = new DateTime(2018, 04, 20) , tasaInteresCredito = loCredito.tasaInteres });
            loCredito.lstCupones.AddLast(new Cupon { periodo = 8, dias = 31, fechaPago = new DateTime(2018, 05, 21) , tasaInteresCredito = loCredito.tasaInteres });
            loCredito.lstCupones.AddLast(new Cupon { periodo = 9, dias = 30, fechaPago = new DateTime(2018, 06, 20) , tasaInteresCredito = loCredito.tasaInteres });
            loCredito.lstCupones.AddLast(new Cupon { periodo = 10, dias = 30, fechaPago = new DateTime(2018, 07, 20) , tasaInteresCredito = loCredito.tasaInteres });
            loCredito.lstCupones.AddLast(new Cupon { periodo = 11, dias = 31, fechaPago = new DateTime(2018, 08, 20) , tasaInteresCredito = loCredito.tasaInteres });
            loCredito.lstCupones.AddLast(new Cupon { periodo = 12, dias = 31, fechaPago = new DateTime(2018, 09, 20) , tasaInteresCredito = loCredito.tasaInteres });
            loCredito.lstCupones.AddLast(new Cupon { periodo = 13, dias = 32, fechaPago = new DateTime(2018, 10, 22) , tasaInteresCredito = loCredito.tasaInteres });
            loCredito.lstCupones.AddLast(new Cupon { periodo = 14, dias = 29, fechaPago = new DateTime(2018, 11, 20) , tasaInteresCredito = loCredito.tasaInteres });

            Console.WriteLine(string.Format("fechaOtorgamiento: {0} | montoTotal: {1} | tasaInteres: {2}", loCredito.fechaOtorgamiento.ToShortDateString(), loCredito.montoBruto, loCredito.tasaInteres));

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
                if(currentNode.Next == null)
                {
                    if (Math.Round(currentNode.Value.montoCapitalAdeudado, 9) == 0)
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

                if(!loReiniciar)
                    currentNode = currentNode.Next;
            }

            var loCurrentNode = loCredito.lstCupones.First;
            while ((loCurrentNode != null)) //&& (currentNode.Value != desiredValue))
            {
                Console.WriteLine(string.Format("periodo: {0} | tasaInteres: {1} | montoInteres: {2} | cuota: {3} | Amort: {4} | montoAdeudado: {5}", loCurrentNode.Value.periodo, loCurrentNode.Value.tasaInteres, (int)loCurrentNode.Value.montoInteres, (int)loCurrentNode.Value.montoCuota, (int)loCurrentNode.Value.montoAmortizacion, (int)loCurrentNode.Value.montoCapitalAdeudado));
                loCurrentNode = loCurrentNode.Next;
            }


        }
    }
}
