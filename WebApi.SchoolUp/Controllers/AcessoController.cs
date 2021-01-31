using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using Comum;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Modelo.SchoolUp.Recursos;
using Modelo.Nucleo.Custom;
using Modelo.Nucleo.Geral;
using Modelo.Nucleo.Models;
using Negocio.Nucleo.Geral;
using Negocio.Nucleo.Permissao;
using Negocio.SchoolUp.Utilidades;
using Modelo.SchoolUp.Custom;

namespace WebApi.SchoolUp.Controllers
{
    [Produces("application/json")]
    [Authorize("Bearer")]
    [Route("apiUp/[controller]")]
    [Route("apiUp/[controller]/[action]")]
    public class AcessoController : Controller
    {
        public string NomeFuncao { get; } = "SCHOOLUP_ACESSO";
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
                    var headerAutorization = Request?.Headers["Authorization"];
                    if (headerAutorization?.Any() != true)                    {
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

        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login([FromBody] CmRegistro dadosRegistro,
            [FromServices]SigningConfigurations signingConfigurations,
            [FromServices]TokenConfigurations tokenConfigurations)
        {
            BllAutenticacao brAutenticacao = new BllAutenticacao();
            GlResposta<ApplicationUser> resposta = brAutenticacao.Login(dadosRegistro.Login, dadosRegistro.Senha).Result;
            if (resposta.Succeeded)
            {
                ClaimsIdentity identity = new ClaimsIdentity(
                    new GenericIdentity(dadosRegistro.Login, "Login"),
                    new[] {
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                        new Claim(JwtRegisteredClaimNames.UniqueName, dadosRegistro.Login)
                    }
                );

                DateTime dataCriacao = DateTime.Now;
                DateTime dataExpiracao = dataCriacao + TimeSpan.FromSeconds(tokenConfigurations.Seconds);

                var handler = new JwtSecurityTokenHandler();
                var securityToken = handler.CreateToken(new SecurityTokenDescriptor
                {
                    Issuer = tokenConfigurations.Issuer,
                    Audience = tokenConfigurations.Audience,
                    SigningCredentials = signingConfigurations.SigningCredentials,
                    Subject = identity,
                    NotBefore = dataCriacao,
                    Expires = dataExpiracao
                });
                var token = handler.WriteToken(securityToken);

                UsuarioAutenticacao usuarioAutenticacao = new UsuarioAutenticacao()
                {
                    IdUsuario = resposta.Id,
                    DataCriacao = dataCriacao,
                    DataExpiracao = dataExpiracao,
                    IdIdentificacao = Guid.NewGuid().ToString(),
                    Logout = false,
                    Token = token
                };
                BllHistorico brHistorico = new BllHistorico(Ip, NomeFuncao, "");
                Historico historico = brHistorico.GetHistorico;
                historico = brHistorico.MontarHistoricoJson(historico, usuarioAutenticacao, usuarioAutenticacao.IdUsuario);
                BllAcao.ObterTodos();
                historico.IdAcao = BllAcao.IdAcaoLogin;
                historico.IdUsuario = usuarioAutenticacao.IdUsuario;

                GlAutenticacao respostaAutenticacao = brAutenticacao.ArmazenarToken(usuarioAutenticacao, historico);
                if (respostaAutenticacao.Succeeded)
                {
                    respostaAutenticacao.Id = usuarioAutenticacao.IdUsuario;
                }
                return Ok(respostaAutenticacao);
            }

            return BadRequest(new GlAutenticacao() { Autenticado = false, Mensagem = Mensagens.UsuarioSenhaIncorreto });
        }

        [AllowAnonymous]
        [HttpPost("Logout")]
        public IActionResult Logout([FromBody] CmRegistro dadosRegistro)
        {
            UsuarioAutenticacao usuarioAutenticacao = new UsuarioAutenticacao() { Token = Token };
            BllAutenticacao brAutenticacao = new BllAutenticacao();
            BllHistorico brHistorico = new BllHistorico(Ip, NomeFuncao, "");
            Historico historico = brHistorico.GetHistorico;
            historico = brHistorico.MontarHistorico(historico, "Logout", Token);
            BllAcao.ObterTodos();
            historico.IdAcao = BllAcao.IdAcaoLogout;
            historico.IdUsuario = dadosRegistro.IdUsuario;

            GlAutenticacao respostaAutenticacao = brAutenticacao.Logout(usuarioAutenticacao, historico);
            if (respostaAutenticacao.Succeeded)
            {
                return Ok(respostaAutenticacao);
            }

            return BadRequest(respostaAutenticacao);
        }

        [HttpPut("AlterarSenha")]
        public IActionResult AlterarSenha([FromBody] CmRegistro dadosRegistro)
        {
            BllAutenticacao brAutenticacao = new BllAutenticacao();
            BllHistorico brHistorico = new BllHistorico(Ip, NomeFuncao, "");
            Historico historico = brHistorico.GetHistorico;
            historico = brHistorico.MontarHistorico(historico, "Alterar Senha", dadosRegistro.IdUsuario);
            BllAcao.ObterTodos();
            historico.IdAcao = BllAcao.IdAcaoAlterar;
            historico.IdUsuario = dadosRegistro.IdUsuario;

            var respostaAutenticacao = brAutenticacao.AlterarSenha(dadosRegistro).Result;
            if (respostaAutenticacao.Succeeded)
            {
                respostaAutenticacao.Mensagem = Mensagens.SenhaAlterada;
                return Ok(respostaAutenticacao);
            }

            return BadRequest(respostaAutenticacao);
        }

        [HttpPut("RecuperarSenha")]
        [AllowAnonymous]
        public IActionResult RecuperarSenha([FromBody] CmLogin cmLogin)
        {
            Random random = new Random();
            string alfabeticos = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            string numericos = "0123456789";
            string caracteres = "#@!$*&%";

            string alfabeticoSorteados = new string(Enumerable.Repeat(alfabeticos, 8).Select(s => s[random.Next(s.Length)]).ToArray());
            string numericosSorteados = new string(Enumerable.Repeat(numericos, 2).Select(s => s[random.Next(s.Length)]).ToArray());
            string caracteresSorteados = new string(Enumerable.Repeat(caracteres, 2).Select(s => s[random.Next(s.Length)]).ToArray());


            cmLogin.Senha = alfabeticoSorteados.Substring(0, 2) + numericosSorteados.Substring(0,1) + caracteresSorteados.Substring(0,1)
                + alfabeticoSorteados.Substring(2, 3) + caracteresSorteados.Substring(1, 1) + alfabeticoSorteados.Substring(5, 1) 
                + numericosSorteados.Substring(1, 1) + alfabeticoSorteados.Substring(6, 2);

            BllHistorico brHistorico = new BllHistorico(Ip, NomeFuncao, "");
            Historico historico = brHistorico.GetHistorico;
            historico = brHistorico.MontarHistorico(historico, "Resetar Senha", Configuracoes.IdUsuarioLogger);
            BllAcao.ObterTodos();
            historico.IdAcao = BllAcao.IdAcaoEnviar;
            historico.IdUsuario = Configuracoes.IdUsuarioLogger;

            string ambiente = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            BllEmail brEmail = new BllEmail(ambiente);
            GlResposta<CmLogin> glRespostaEmail = brEmail.SendEmailRecuperarSenha(historico, cmLogin);

            if (glRespostaEmail?.Succeeded != true)
            {
                return BadRequest(glRespostaEmail);
            }

            CmRegistro dadosRegistro = new CmRegistro()
            {
                IdUsuario = Guid.Empty.ToString(),
                Login = cmLogin.Login,
                Senha = cmLogin.Senha
            };

            BllAutenticacao brAutenticacao = new BllAutenticacao();

            var respostaAutenticacao = brAutenticacao.ResetarSenha(dadosRegistro).Result;
            if (respostaAutenticacao.Succeeded)
            {
                respostaAutenticacao.Mensagem = glRespostaEmail.Mensagem;
                return Ok(respostaAutenticacao);
            }

            respostaAutenticacao.Mensagem = Mensagens.EmailEnviadoSenhaNaoAlterada + respostaAutenticacao.Mensagem;

            return BadRequest(respostaAutenticacao);
        }

        [AllowAnonymous]
        [HttpGet("RenovarToken/{idUsuario}")]
        public IActionResult RenovarToken(string idUsuario, [FromServices]SigningConfigurations signingConfigurations,
            [FromServices]TokenConfigurations tokenConfigurations)
        {
            BllHistorico brHistorico = new BllHistorico(Ip, NomeFuncao, "");
            Historico historico = brHistorico.GetHistorico;
            historico = brHistorico.MontarHistorico(historico, "Renovar Token", idUsuario);
            try
            {
                ClaimsIdentity identity = new ClaimsIdentity(
                    new GenericIdentity(idUsuario, "Login"),
                    new[] {
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                        new Claim(JwtRegisteredClaimNames.UniqueName, idUsuario)
                    }
                );

                DateTime dataCriacao = DateTime.Now;
                DateTime dataExpiracao = dataCriacao + TimeSpan.FromSeconds(tokenConfigurations.Seconds);

                var handler = new JwtSecurityTokenHandler();
                var securityToken = handler.CreateToken(new SecurityTokenDescriptor
                {
                    Issuer = tokenConfigurations.Issuer,
                    Audience = tokenConfigurations.Audience,
                    SigningCredentials = signingConfigurations.SigningCredentials,
                    Subject = identity,
                    NotBefore = dataCriacao,
                    Expires = dataExpiracao
                });
                var tokenResposta = handler.WriteToken(securityToken);

                UsuarioAutenticacao usuarioAutenticacao = new UsuarioAutenticacao()
                {
                    IdUsuario = idUsuario,
                    DataCriacao = dataCriacao,
                    DataExpiracao = dataExpiracao,
                    IdIdentificacao = Guid.NewGuid().ToString(),
                    Logout = false,
                    Token = tokenResposta
                };
                historico = brHistorico.MontarHistoricoJson(historico, usuarioAutenticacao, usuarioAutenticacao.IdUsuario);
                BllAcao.ObterTodos();
                historico.IdAcao = BllAcao.IdAcaoLogin;
                historico.IdUsuario = Configuracoes.IdUsuarioLogger;

                GlAutenticacao respostaAutenticacao = new BllAutenticacao().ArmazenarToken(usuarioAutenticacao, historico);

                GlResposta<UsuarioAutenticacao> resposta = new GlResposta<UsuarioAutenticacao>() { Id = idUsuario, Succeeded = respostaAutenticacao.Succeeded };
                resposta.Dados = new List<UsuarioAutenticacao>();
                resposta.Dados.Add(new UsuarioAutenticacao() { Token = respostaAutenticacao.Token });
                return Ok(resposta);
            }
            catch (Exception excecao)
            {
                new BllErro().Inserir(historico, excecao);
                return BadRequest(new GlResposta<UsuarioAutenticacao>() { Succeeded = false, Mensagem = Mensagens.ErroInesperado });
            }
        }
    }
}