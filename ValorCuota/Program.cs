using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValorCuota
{
    public class Program
    {
        static void Main(string[] args)
        {
            //Console.Write("Para salir escriba exit: "); // Prompt
            //string lsExit = Console.ReadLine(); // Get string from
            string lsExit = string.Empty;
            while (!lsExit.Trim().ToUpper().Equals("EXIT"))
            {
                CalcularCredito();
                Console.WriteLine();
                Console.WriteLine();
                Console.Write("Para salir escriba exit o enter para un nuevo credito: "); // Prompt
                lsExit = Console.ReadLine(); // Get string from
            }
        }

        private static void CalcularCredito()
        {
            Console.Write("Enter Fecha Otorgamiento (dd/mm/yyyy): "); // Prompt
            string lsFechaOtorgamiento = Console.ReadLine(); // Get string from 

            Console.Write("Enter Monto Liquido : "); // Prompt
            string lsmontoLiquido = Console.ReadLine(); // Get string from user

            Console.Write("Enter Monto Impuesto: "); // Prompt
            string lsmontoImpuesto = Console.ReadLine(); // Get string from user

            Console.Write("Enter Monto Notario: "); // Prompt
            string lsmontoNotario = Console.ReadLine(); // Get string from user

            Console.Write("Enter tasa Interes (ej: 1,35): "); // Prompt
            string lstasaInteres = Console.ReadLine().Replace(".",","); // Get string from user

            Console.Write("Enter numero Cuotas: "); // Prompt
            string lsnumeroCuotas = Console.ReadLine(); // Get string from user

            Calculo.SeekGoal(lsFechaOtorgamiento, lsmontoLiquido, lsmontoImpuesto, lsmontoNotario, lstasaInteres, lsnumeroCuotas);
        }
    }
}
