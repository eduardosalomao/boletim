using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Authorization;
using Modelo.SchoolUp.Principal;

namespace WebApi.SchoolUp.Controllers.Utils
{
    [Authorize("Bearer")]
    [Route("apiUp/Utils/[controller]")]
    [Route("apiUp/Utils/[controller]/[action]")]
    public class EmailController : Controller
    {
        public string NomeFuncao { get; } = "SCHOOLUP_ENVIOEMAIL";
        public string Ip
        {
            get
            {
                var ipRemoto = Request.HttpContext.Connection.RemoteIpAddress;
                if (ipRemoto == null)
                {
                    return "indefinido";
                }
                return ipRemoto.ToString();
            }
        }
        public string Token
        {
            get
            {
                try
                {
                    var headerAutorization = Request.Headers["Authorization"];
                    if (headerAutorization.Count == 0)
                    {
                        return "indefinido";
                    }
                    return headerAutorization.ToString().Replace("Bearer ", "");
                }
                catch (Exception)
                {
                    return "indefinido";
                }
            }
        }

        [HttpPost("Email/PrimeiroAcesso")]
        public IActionResult PrimeiroAcesso(Aluno mdlAluno)
        {
            //BllEmail brEmail = new BllEmail();
            //bool emailEnviado = brEmail.SendEmailPrimeiroAcesso(mdlAluno);
            //if (emailEnviado)
            //{
                return Ok();
            //}
            //else
            //{
            //    return BadRequest();
            //}
        }

        
    }
}
