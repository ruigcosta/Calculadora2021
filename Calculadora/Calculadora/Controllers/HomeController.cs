using Calculadora.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Calculadora.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }


        /// <summary>
        /// Invocação da calculadora em modo HttpGet
        /// Inicialização de diversas variáveis
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            ViewBag.operador = "";
            ViewBag.primeiroOperando = true;
            ViewBag.Visor = "0";
            ViewBag.limpaVisor = true.ToString();


            return View();
        }



        /// <summary>
        /// Invocação da calculadora em modo HttpPost
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Index(string visor, string btn, string operador, string primeiroOperando,
            bool limpaVisor)
        {
            //Dígitos
            if ("0123456789".Contains(btn))
            {
                visor = (visor == "0" || limpaVisor ? btn : visor + btn);
            }

            //Operações especiais
            else if ( "C=,".Contains(btn) || btn == "+/-" )
            {
                switch (btn)
                {
                    case "+/-":
                        if (visor.StartsWith("-")) visor = visor.Replace("-", "");
                        else visor = "-" + visor;
                        break;
                    case "C":
                        visor = "0";
                        break;
                    case ",":
                        if (!visor.Contains(",")) visor += ",";
                        break;
                    case "=":
                        break;
                    default:
                        break;
                }
            }

            //Opções aritméticas
            else if ( "+-:x".Contains(btn) )
            {

                if ( !string.IsNullOrEmpty(operador) )
                {
                    double operando1 = Convert.ToDouble(primeiroOperando);
                    double operando2 = Convert.ToDouble(visor);
                    switch (operador)
                    {
                        case "+":
                            visor = (operando1 + operando2) + "";
                            break;
                        case "-":
                            visor = (operando1 - operando2) + "";
                            break;
                        case ":":
                            visor = (operando1 / operando2) + "";
                            break;
                        case "x":
                            visor = (operando1 * operando2) + "";
                            break;
                        default:
                            break;
                    }
                    limpaVisor = true;
                }
                if (btn == "=")
                    operador = "";
                else
                    operador = btn;

                primeiroOperando = visor;

                //ViewBag.operador = operador;


            }
            ViewBag.Visor = visor;
            ViewBag.operador = operador;
            ViewBag.primeiroOperando = primeiroOperando;
            ViewBag.limpaVisor = true.ToString();

            return View();  
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
