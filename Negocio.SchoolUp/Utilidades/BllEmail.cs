using System;
using System.Net;
using System.Net.Mail;
using Modelo.SchoolUp.Custom;
using Modelo.Nucleo.Geral;
using Modelo.SchoolUp.Recursos;
using Comum;
using Modelo.Nucleo.Models;
using Acesso.SchoolUp.Comum;

namespace Negocio.SchoolUp.Utilidades
{
    public class BllEmail
    {
        public string Ambiente { get; set; }
        public string EmailToTeste { get; set; }


        public const string hostMail = "smtp.gmail.com";
        public const int hostPort = 587;
        public const string hostPass = "sdRV1rt#3";
        public MailAddress hostAdress = new MailAddress("sender.virtualup@gmail.com", "Alternativa Colégio");

        public BllEmail(string ambiente)
        {
            Ambiente = ambiente;
            EmailToTeste = "teste.virtualup@gmail.com";
        }

        public GlResposta<CmLogin> SendEmailRecuperarSenha(Historico historico, CmLogin mdlAcesso)
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(mdlAcesso.Login))
                {
                    if (new BllAcesso().ExistLogin(mdlAcesso.Login))
                    {
                        MailAddress toAddress = new MailAddress(mdlAcesso.Login);
                        if (Ambiente != "Production")
                        {
                            toAddress = new MailAddress(EmailToTeste);
                        }
                        const string subject = "Alternativa Colégio - Recuperação de senha";

                        string body = $"<html><body>Prezado(a),<br /><br />Sua senha de acesso é: <b>{mdlAcesso.Senha}<b/> <br /><br />" +
                            $"Alternativa Colégio<html/><body/>";

                        var smtp = new SmtpClient
                        {
                            Host = hostMail,
                            Port = hostPort,
                            EnableSsl = true,
                            DeliveryMethod = SmtpDeliveryMethod.Network,
                            UseDefaultCredentials = false,
                            Credentials = new NetworkCredential(hostAdress.Address, hostPass)
                        };

                        using (var message = new MailMessage(hostAdress, toAddress)
                        {
                            //From = new MailAddress("sender.virtualup@gmail.com", "Alternativa Colégio"),
                            Subject = subject,
                            Body = body,
                            IsBodyHtml = true
                        })
                        {
                            smtp.Send(message);
                        }
                    }
                    else
                    {
                        return new GlResposta<CmLogin>() { Succeeded = false, Mensagem = Mensagens.EmailInexistente };
                    }
                }
                else
                {
                    return new GlResposta<CmLogin>() { Succeeded = false, Mensagem = String.Format(Mensagens.CampoObrigatorio, "Email") };
                }
            }
            catch (Exception excecao)
            {
                new BllErro().Inserir(historico, excecao);
                return new GlResposta<CmLogin>() { Succeeded = false, Mensagem = String.Format(Mensagens.MensagemRetorno, Mensagens.EmailNaoEnviado) };
            }

            try
            {
                DalSchool<Historico> dalSchoolUp = new DalSchool<Historico>();
                dalSchoolUp.InserirHistorico(historico);
            }
            catch (Exception excecao)
            {
                new BllErro().Inserir(historico, excecao);
                return new GlResposta<CmLogin>() { Succeeded = true, Mensagem = Mensagens.EmailEnviado };
            }

            return new GlResposta<CmLogin>() { Succeeded = true, Mensagem = Mensagens.EmailEnviado };
        }

//            public void SendEmailMarketing()
//        {
//            //var toAddress = new MailAddress("edu.s.gabriel@gmail.com", "Daniel Aguiar");
//            var toAddress = new MailAddress("danielaguiar@msn.com", "Daniel Aguiar");
//            var toCopy = new MailAddress("edu.s.gabriel@gmail.com", "Eduardo Salomão");
//            const string subject = "Solicitação de Treinamento";
//            //const string body = "<html><body><b>Nome do vendedor<b/> {0}<br />" +
//            //                    "<b>Razão social do cliente<b/> {1}<br />" +
//            //                    "<b>Tipo de treinamento escolhido<b/> {2}<br />" +
//            //                    "<b>Data do treinamento<b/> {3}<br />" +
//            //                    "<b>Nome do responsável<b/> {4}<br />" +
//            //                    "<b>E-mail do responsável<b/> {5}<br /><html/><body/>";

//            //const string body = "Nome do vendedor: {0}\n" +
//            //                    "Razão social do cliente: {1}\n" +
//            //                    "Tipo de treinamento escolhido: {2}\n" +
//            //                    "Data do treinamento: {3}\n" +
//            //                    "Nome do responsável: {4}\n" +
//            //                    "E-mail do responsável: {5}";


//            var smtp = new SmtpClient
//            {
//                Host = hostMail,
//                Port = hostPort,
//                EnableSsl = true,
//                DeliveryMethod = SmtpDeliveryMethod.Network,
//                UseDefaultCredentials = false,
//                Credentials = new NetworkCredential(hostAdress.Address, hostPass)
//            };
//            //using (var message = new MailMessage(fromAddress, toAddress)
//            //using (var message = new MailMessage(fromAddress, toAddress)
//            //{
//            //    Subject = subject,
//            //    Body = corpo
//            //})
//            //{
//            //    message.AlternateViews.Add(avCal);
//            //    smtp.Send(message);
//            //}

//            using (var message = new MailMessage(hostAdress, toAddress)
//            {
//                From = new MailAddress("daniela@levemais.test.tst", "Daniela Guiar"),
//                Subject = subject,
//                Body = @"<!doctype html>
//<html>
//  <head>
//    <meta charset='utf-8' />
// <meta name='viewport' content='width=device-width' />
//<style>

//.container {
//  max-width:600px!important;
//  display:block!important;
//  margin:0 auto!important; /* makes it centered */
//  clear:both!important;
//}

//.column-wrap { 
//  padding:0!important; 
//  margin:0 auto; 
//  max-width:600px!important;
//}

//.column {
//  width: 300px;
//  float:left;
//}
  
//   /* These are our tablet/medium screen media queries */
//        @media screen and (max-width: 630px){


//            /* Display block allows us to stack elements */                      
//            *[class='mobile-column'] {display: block;} 

//            /* Some more stacking elements */
//            *[class='mob-column'] {float: none !important;width: 100% !important;}     

//            /* Hide stuff */
//            *[class='hide'] {display:none !important;}          

//            /* This sets elements to 100% width and fixes the height issues too, a god send */
//            *[class='100p'] {width:100% !important; height:auto !important;}                    

//            /* For the 2x2 stack */         
//            *[class='condensed'] {padding-bottom:40px !important; display: block;}

//            /* Centers content on mobile */
//            *[class='center'] {text-align:center !important; width:100% !important; height:auto !important;}            

//            /* 100percent width section with 20px padding */
//            *[class='100pad'] {width:100% !important; padding:20px;} 

//            /* 100percent width section with 20px padding left & right */
//            *[class='100padleftright'] {width:100% !important; padding:0 20px 0 20px;} 

//            /* 100percent width section with 20px padding top & bottom */
//            *[class='100padtopbottom'] {width:100% !important; padding:20px 0px 20px 0px;} 


//        } 
//    </style>
//  </head>
//  <body>
//    <center>
//      <table>
//        <tr>
//            <td></td>
//          <td class='container'> </td>
   
//</tr></table>    
//      <table width='600' style='font-family: arial; font-size: 12px; color:#41453e;'>
//          <tr>
//              <td><p class='edit'>Escreva aqui um preview da sua mensagem</p></td>
//              <td align='right'><p><a href='#' class='web-link'>Ver no navegador</a></p></td>
//          </tr></table>
// <table cellspacing='0' cellpadding='0' width='600' bgcolor='#EEF0EF' border='0' class='100p'>
//<tr>
//    <td width='600' bgcolor='#EEF0EF' class='edit'>
//    <img class='pattern' src='mountain.png' width='600' height='400' align='center' style='padding-top: 0' />
//</td>
//    </tr>  
//               </table>
        
//<table cellspacing='0' cellpadding='0' width='600' bgcolor='#EEF0EF' border='0' class='100p'>
//<tr>
//    <td width='600' bgcolor='#EEF0EF'>
//  <h2 style='font-family: arial; color:#41453e; padding-left:20px' class='edit'> Lorem ipsum </h2>
//    <p style='font-family: arial; size: 14px; color:#41453e; padding: 20px 20px 20px 20px' class='edit'> Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.
//    </p>
//          <table cellspacing='0' border='0' cellpadding='8' width='240' align='center' style='padding: 15px 15px 15px 15px'>
//  <tr>
//    <td height='35' bgcolor='#0A5A8C' style='border-radius: 5px; -moz-border-radius: 5px; -webkit-border-radius: 5px; -khtml-border-radius: 5px; font-size: 16px; font-family: sans-serif; color: #333333; margin: 0; padding: 0; text-align: center;' class='vero-editable'>
//      <p class='edit'><a href='http://www.seulink.com' style='font-weight:bold; text-decoration:underline;color: #ffffff;text-decoration:none;'>Clique aqui &rarr;</a></p>
//</td>
//  </tr>
//</table>
//</td>
//    </tr>  
//               </table><table cellspacing='0' cellpadding='0' style='padding-right: 0px' width='600' bgcolor='#FFFFFF' border='0' class='100p'>
//<tr>
//          </tr></table>

//           <table cellspacing='0' cellpadding='0' style='padding-top: 10px' width='600' bgcolor='#3e423b' border='0' class='100p'>
//               <tr>
//                   <td align='center'> <p style='font-family: arial; size: 14px; color:#f5f1e6' class='edit'>ENCONTRE-NOS </p></td>
//                   </tr>
//               <tr>
//                   <td width='200' cellspacing='10' align='center' style='padding-bottom: 30px' class='edit'> <img src='facebook.png' width='30' height='30' />   <img src='twitter.png' width='30' height='30' /> <img src='email.png' width='30' height='30' /></td>
//</tr><tr>
//    <td width='600' bgcolor='#EEF0EF' style='padding: 15px 15px 15px 15px'>
//    <p class='edit' style='font-family: arial; font-size: 12px; color:#4d4d4d' align='center'> Se você não deseja mais receber nossas mensagens, <a href='#' class='unsubscribe'>descadastre-se</a>
//    </p>
//    </td>
//    </tr>
//          </table>
    
  
     
//  </center></body>
//</html>",
//                IsBodyHtml = true
//            })
//            {
//                message.To.Add(toCopy);
//                //message.To.Add(new MailAddress("juliana.barbosa@crbard.com", "Juliana Barbosa"));
//                //message.AlternateViews.Add(avCal);
//                smtp.Send(message);
//            }
//        }



//        public void SendEmailSolicitacao(string Nome, string RazaoSocial, string TipoTreinamento, string DataPrincipal
//            , string NomeResponsavel, string EmailResponsavel)
//        {
//            //var toAddress = new MailAddress("edu.s.gabriel@gmail.com", "Daniel Aguiar");
//            var toAddress = new MailAddress("danielaguiar@msn.com", "Daniel Aguiar");
//            var toCopy = new MailAddress("edu.s.gabriel@gmail.com", "Eduardo Salomão");
//            const string subject = "Solicitação de Treinamento";
//            //const string body = "<html><body><b>Nome do vendedor<b/> {0}<br />" +
//            //                    "<b>Razão social do cliente<b/> {1}<br />" +
//            //                    "<b>Tipo de treinamento escolhido<b/> {2}<br />" +
//            //                    "<b>Data do treinamento<b/> {3}<br />" +
//            //                    "<b>Nome do responsável<b/> {4}<br />" +
//            //                    "<b>E-mail do responsável<b/> {5}<br /><html/><body/>";

//            const string body = "Nome do vendedor: {0}\n" +
//                                "Razão social do cliente: {1}\n" +
//                                "Tipo de treinamento escolhido: {2}\n" +
//                                "Data do treinamento: {3}\n" +
//                                "Nome do responsável: {4}\n" +
//                                "E-mail do responsável: {5}";


//            string corpo = String.Format(body, Nome, RazaoSocial,
//                TipoTreinamento, DataPrincipal, NomeResponsavel, EmailResponsavel);

//            StringBuilder str = new StringBuilder();
//            str.AppendLine("BEGIN:VCALENDAR");
//            //str.AppendLine("PRODID:-//Ahmed Abu Dagga Blog");
//            str.AppendLine("PRODID:-//BBS");
//            str.AppendLine("VERSION:2.0");
//            str.AppendLine("METHOD:REQUEST");
//            str.AppendLine("BEGIN:VEVENT");


//            DateTime data = DateTime.ParseExact(DataPrincipal, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None);
//            str.AppendLine(string.Format("DTSTART:{0:yyyyMMddTHHmmssZ}", data.AddHours(9).ToUniversalTime().ToString("yyyyMMdd\\THHmmss\\Z")));
//            str.AppendLine(string.Format("DTSTAMP:{0:yyyyMMddTHHmmssZ}", DateTime.UtcNow));
//            str.AppendLine(string.Format("DTEND:{0:yyyyMMddTHHmmssZ}", data.AddHours(18).ToUniversalTime().ToString("yyyyMMdd\\THHmmss\\Z")));
//            str.AppendLine("LOCATION: RJ");
//            str.AppendLine(string.Format("UID:{0}", Guid.NewGuid()));
//            str.AppendLine(string.Format("DESCRIPTION:{0}", corpo));
//            str.AppendLine(string.Format("X-ALT-DESC;FMTTYPE=text/html:{0}", corpo));
//            str.AppendLine(string.Format("SUMMARY:{0}", subject));
//            str.AppendLine(string.Format("ORGANIZER:MAILTO:{0}", "BARD BIOPSY SYSTEM"));

//            str.AppendLine(string.Format("ATTENDEE;CN=\"{0}\";RSVP=TRUE:mailto:{1}", toAddress.DisplayName, toAddress.Address));

//            str.AppendLine("BEGIN:VALARM");
//            str.AppendLine("TRIGGER:-PT15M");
//            str.AppendLine("ACTION:DISPLAY");
//            str.AppendLine("DESCRIPTION:Reminder");
//            str.AppendLine("END:VALARM");
//            str.AppendLine("END:VEVENT");
//            str.AppendLine("END:VCALENDAR");
//            System.Net.Mime.ContentType ct = new System.Net.Mime.ContentType("text/calendar");
//            ct.Parameters.Add("method", "REQUEST");
//            AlternateView avCal = AlternateView.CreateAlternateViewFromString(str.ToString(), ct);

//            var smtp = new SmtpClient
//            {
//                Host = hostMail,
//                Port = hostPort,
//                EnableSsl = true,
//                DeliveryMethod = SmtpDeliveryMethod.Network,
//                UseDefaultCredentials = false,
//                Credentials = new NetworkCredential(hostAdress.Address, hostPass)
//            };
//            //using (var message = new MailMessage(fromAddress, toAddress)
//            //using (var message = new MailMessage(fromAddress, toAddress)
//            //{
//            //    Subject = subject,
//            //    Body = corpo
//            //})
//            //{
//            //    message.AlternateViews.Add(avCal);
//            //    smtp.Send(message);
//            //}

//            using (var message = new MailMessage(hostAdress, toAddress)
//            {
//                Subject = subject,
//                Body = corpo
//            })
//            {
//                message.To.Add(toCopy);
//                //message.To.Add(new MailAddress("juliana.barbosa@crbard.com", "Juliana Barbosa"));
//                //message.AlternateViews.Add(avCal);
//                smtp.Send(message);
//            }
//        }

//        public void SendEmailConfirmacao(string Nome, string RazaoSocial, string TipoTreinamento, string DataPrincipal
//            , string NomeResponsavel, string EmailResponsavel)
//        {
//            var toAddress = new MailAddress("danielaguiar@msn.com", "Daniel Aguiar");
//            var toCopy = new MailAddress("edu.s.gabriel@gmail.com", "Eduardo Salomão");
//            const string subject = "Confirmação de Treinamento";

//            const string body = "Nome do vendedor: {0}\n" +
//                                "Razão social do cliente: {1}\n" +
//                                "Tipo de treinamento escolhido: {2}\n" +
//                                "Data do treinamento: {3}\n" +
//                                "Nome do responsável: {4}\n" +
//                                "E-mail do responsável: {5}";

//            string corpo = String.Format(body, Nome, RazaoSocial,
//                TipoTreinamento, DataPrincipal, NomeResponsavel, EmailResponsavel);

//            StringBuilder str = new StringBuilder();
//            str.AppendLine("BEGIN:VCALENDAR");
//            //str.AppendLine("PRODID:-//Ahmed Abu Dagga Blog");
//            str.AppendLine("PRODID:-//BBS");
//            str.AppendLine("VERSION:2.0");
//            str.AppendLine("METHOD:REQUEST");
//            str.AppendLine("BEGIN:VEVENT");


//            DateTime data = DateTime.ParseExact(DataPrincipal, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None);
//            str.AppendLine(string.Format("DTSTART:{0:yyyyMMddTHHmmssZ}", data.AddHours(9).ToUniversalTime().ToString("yyyyMMdd\\THHmmss\\Z")));
//            str.AppendLine(string.Format("DTSTAMP:{0:yyyyMMddTHHmmssZ}", DateTime.UtcNow));
//            str.AppendLine(string.Format("DTEND:{0:yyyyMMddTHHmmssZ}", data.AddHours(18).ToUniversalTime().ToString("yyyyMMdd\\THHmmss\\Z")));
//            str.AppendLine("LOCATION: RJ");
//            str.AppendLine(string.Format("UID:{0}", Guid.NewGuid()));
//            str.AppendLine(string.Format("DESCRIPTION:{0}", corpo));
//            str.AppendLine(string.Format("X-ALT-DESC;FMTTYPE=text/html:{0}", corpo));
//            str.AppendLine(string.Format("SUMMARY:{0}", subject));
//            str.AppendLine(string.Format("ORGANIZER:MAILTO:{0}", "BARD BIOPSY SYSTEM"));

//            str.AppendLine(string.Format("ATTENDEE;CN=\"{0}\";RSVP=TRUE:mailto:{1}", toAddress.DisplayName, toAddress.Address));

//            str.AppendLine("BEGIN:VALARM");
//            str.AppendLine("TRIGGER:-PT15M");
//            str.AppendLine("ACTION:DISPLAY");
//            str.AppendLine("DESCRIPTION:Reminder");
//            str.AppendLine("END:VALARM");
//            str.AppendLine("END:VEVENT");
//            str.AppendLine("END:VCALENDAR");
//            System.Net.Mime.ContentType ct = new System.Net.Mime.ContentType("text/calendar");
//            ct.Parameters.Add("method", "REQUEST");
//            AlternateView avCal = AlternateView.CreateAlternateViewFromString(str.ToString(), ct);

//            var smtp = new SmtpClient
//            {
//                Host = hostMail,
//                Port = hostPort,
//                EnableSsl = true,
//                DeliveryMethod = SmtpDeliveryMethod.Network,
//                UseDefaultCredentials = false,
//                Credentials = new NetworkCredential(hostAdress.Address, hostPass)
//            };
//            //using (var message = new MailMessage(fromAddress, toAddress)
//            //using (var message = new MailMessage(fromAddress, toAddress)
//            //{
//            //    Subject = subject,
//            //    Body = corpo
//            //})
//            //{
//            //    message.AlternateViews.Add(avCal);
//            //    smtp.Send(message);
//            //}

//            using (var message = new MailMessage(hostAdress, toAddress)
//            {
//                Subject = subject,
//                Body = corpo
//            })
//            {
//                message.To.Add(toCopy);
//                //message.To.Add(new MailAddress("juliana.barbosa@crbard.com", "Juliana Barbosa"));
//                message.AlternateViews.Add(avCal);
//                smtp.Send(message);
//            }

//        }

        //public void SendEmailSolicitacao(VupCliente cliente, VupUsuario vendedor, VupUsuario especialista, VupInscricao inscricao,
        //    VupTipoTreinamento tipoTreinamento)
        //{
        //    //var toAddress = new MailAddress("danielaguiar@msn.com", "Daniel Aguiar");
        //    //var toCopy = new MailAddress("edu.s.gabriel@gmail.com", "Eduardo Salomão");
        //    const string subject = "Solicitação de Treinamento";

        //    string bodyVendedor = "A sua solicitação de treinamento foi cadastrada com sucesso.\n\n" +
        //                        "Verifique atentamente as informações do treinamento cadastrado:\n\n" +
        //                        "Nome do vendedor: {0}\n" +
        //                        "Razão social do cliente: {1}\n" +
        //                        "Tipo de treinamento escolhido: {2}\n" +
        //                        "Data do treinamento: {3}\n" +
        //                        "Nome do responsável: {4}\n" +
        //                        "E-mail do responsável: {5}\n\n" +
        //                        "ATENÇÃO\n\n" +
        //                        "Aguarde o e-mail de confirmação da especialista clínica que será enviado nos próximos dias.";

        //    string bodyEspecialista = "Um treinamento foi solicitado.\n\n" +
        //                        "Verifique atentamente as informações do treinamento cadastrado:\n\n" +
        //                        "Nome do vendedor: {0}\n" +
        //                        "Razão social do cliente: {1}\n" +
        //                        "Tipo de treinamento escolhido: {2}\n" +
        //                        "Data do treinamento: {3}\n" +
        //                        "Nome do responsável: {4}\n" +
        //                        "E-mail do responsável: {5}\n\n" +
        //                        "Acesse o treinamento por este link: http://bbs.educaup.com.br/Confirmacao/Edit/" + inscricao.Id.ToString();


        //    string corpoVendedor = String.Format(bodyVendedor, vendedor.Nome, cliente.RazaoSocial,
        //        tipoTreinamento.Nome, inscricao.DataPrincipal.ToString("dd/MM/yyyy"), inscricao.NomeResponsavel, inscricao.EmailResponsavel);
        //    string corpoEspecialista = String.Format(bodyEspecialista, vendedor.Nome, cliente.RazaoSocial,
        //        tipoTreinamento.Nome, inscricao.DataPrincipal.ToString("dd/MM/yyyy"), inscricao.NomeResponsavel, inscricao.EmailResponsavel);

        //    var smtp = new SmtpClient
        //    {
        //        Host = hostMail,
        //        Port = hostPort,
        //        EnableSsl = true,
        //        DeliveryMethod = SmtpDeliveryMethod.Network,
        //        UseDefaultCredentials = false,
        //        Credentials = new NetworkCredential(hostAdress.Address, hostPass)
        //    };

        //    using (var message = new MailMessage(hostAdress, new MailAddress(vendedor.Email, vendedor.Nome))
        //    {
        //        Subject = subject,
        //        Body = corpoVendedor
        //    })
        //    {
        //        //message.To.Add(toCopy);
        //        smtp.Send(message);
        //    }
        //    //using (var message = new MailMessage(hostAdress, new MailAddress("Andreia.Gouveia@crbard.com", "Andreia Petito Gouveia"))
        //    using (var message = new MailMessage(hostAdress, new MailAddress(especialista.Email, especialista.Nome))
        //    {
        //        Subject = subject,
        //        Body = corpoEspecialista
        //    })
        //    {
        //        //message.To.Add(toCopy);
        //        smtp.Send(message);
        //    }
        //}

        //public void SendEmailConfirmacao(VupCliente cliente, VupUsuario vendedor, VupUsuario especialista, VupInscricao inscricao,
        //    VupTipoTreinamento tipoTreinamento, List<VupInscritos> inscritos)
        //{
        //    //var toAddress = new MailAddress("danielaguiar@msn.com", "Daniel Aguiar");
        //    //var toCopy = new MailAddress("edu.s.gabriel@gmail.com", "Eduardo Salomão");
        //    const string subject = "Confirmação de Treinamento";
        //    const string subjectCliente = "Treinamento Confirmado";

        //    string bodyAgenda = "Nome do vendedor: {0}\n" +
        //                        "Razão social do cliente: {1}\n" +
        //                        "Tipo de treinamento escolhido: {2}\n" +
        //                        "Data do treinamento: {3}\n" +
        //                        "Nome do responsável: {4}\n" +
        //                        "E-mail do responsável: {5}";

        //    string bodyCliente = "Prezado(a),\n\n" +
        //                        "Este é uma confirmação do treinamento do Encor Enspire a ser realizado no(s) dia(s) {0}" +
        //                        " no local {1} - {2} para {3} médicos, sendo eles:\n\n" +
        //                        "{4}\n" +
        //                        "Caso haja alguma alteração na agenda, por favor nos avisar com antecedência.\n\n\n" +
        //                        "Atenciosamente,\n\n" +
        //                        "Andreia Petito Gouveia | Especialista Clínica\n" +
        //                        "Email: Andreia.Gouveia@crbard.com\n" +
        //                        "Cel: (11) 97637-2031";

        //    string corpoAgenda = String.Format(bodyAgenda, vendedor.Nome, cliente.RazaoSocial,
        //        tipoTreinamento.Nome, inscricao.DataPrincipal.ToString("dd/MM/yyyy"), inscricao.NomeResponsavel, inscricao.EmailResponsavel);

        //    string listaInscritos = "";
        //    foreach (var item in inscritos)
        //    {
        //        listaInscritos += "- Dr. " + item.Nome + "\n";
        //    }

        //    string corpoCliente = String.Format(bodyCliente, inscricao.DataPrincipal.ToString("dd/MM/yyyy"), inscricao.Cidade,
        //        inscricao.UF, inscritos.Count, listaInscritos);

        //    StringBuilder str = new StringBuilder();
        //    str.AppendLine("BEGIN:VCALENDAR");
        //    //str.AppendLine("PRODID:-//Ahmed Abu Dagga Blog");
        //    str.AppendLine("PRODID:-//BBS");
        //    str.AppendLine("VERSION:2.0");
        //    str.AppendLine("METHOD:REQUEST");
        //    str.AppendLine("BEGIN:VEVENT");


        //    DateTime data = DateTime.ParseExact(inscricao.DataPrincipal.ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None);
        //    str.AppendLine(string.Format("DTSTART:{0:yyyyMMddTHHmmssZ}", data.AddHours(9).ToUniversalTime().ToString("yyyyMMdd\\THHmmss\\Z")));
        //    str.AppendLine(string.Format("DTSTAMP:{0:yyyyMMddTHHmmssZ}", DateTime.UtcNow));
        //    str.AppendLine(string.Format("DTEND:{0:yyyyMMddTHHmmssZ}", data.AddHours(18).ToUniversalTime().ToString("yyyyMMdd\\THHmmss\\Z")));
        //    str.AppendLine(string.Format("LOCATION: {0} - {1}", inscricao.Cidade, inscricao.UF));
        //    str.AppendLine(string.Format("UID:{0}", Guid.NewGuid()));
        //    str.AppendLine(string.Format("DESCRIPTION:{0}", corpoAgenda));
        //    str.AppendLine(string.Format("X-ALT-DESC;FMTTYPE=text/html:{0}", corpoAgenda));
        //    str.AppendLine(string.Format("SUMMARY:{0}", subject));
        //    str.AppendLine(string.Format("ORGANIZER:MAILTO:{0}", "BARD BIOPSY SYSTEM"));

        //    //str.AppendLine(string.Format("ATTENDEE;CN=\"{0}\";RSVP=TRUE:mailto:{1}", vendedor.Email, vendedor.Nome));
        //    //str.AppendLine(string.Format("ATTENDEE;CN=\"{0}\";RSVP=TRUE:mailto:{1}", especialista.Email, especialista.Nome));

        //    str.AppendLine(string.Format("ATTENDEE;ROLE=REQ-PARTICIPANT;CN=\"{0}\";RSVP=TRUE:mailto:{1}", vendedor.Email, vendedor.Nome));
        //    str.AppendLine(string.Format("ATTENDEE;ROLE=REQ-PARTICIPANT;CN=\"{0}\";RSVP=TRUE:mailto:{1}", especialista.Email, especialista.Nome));

        //    str.AppendLine("BEGIN:VALARM");
        //    str.AppendLine("TRIGGER:-PT15M");
        //    str.AppendLine("ACTION:DISPLAY");
        //    str.AppendLine("DESCRIPTION:Reminder");
        //    str.AppendLine("END:VALARM");
        //    str.AppendLine("END:VEVENT");
        //    str.AppendLine("END:VCALENDAR");
        //    System.Net.Mime.ContentType ct = new System.Net.Mime.ContentType("text/calendar");
        //    ct.Parameters.Add("method", "REQUEST");
        //    AlternateView avCal = AlternateView.CreateAlternateViewFromString(str.ToString(), ct);

        //    var smtp = new SmtpClient
        //    {
        //        Host = hostMail,
        //        Port = hostPort,
        //        EnableSsl = true,
        //        DeliveryMethod = SmtpDeliveryMethod.Network,
        //        UseDefaultCredentials = false,
        //        Credentials = new NetworkCredential(hostAdress.Address, hostPass)
        //    };

        //    using (var message = new MailMessage(hostAdress, new MailAddress(vendedor.Email, vendedor.Nome))
        //    {
        //        Subject = subject,
        //        Body = corpoAgenda
        //    })
        //    {
        //        message.To.Add(new MailAddress(especialista.Email, especialista.Nome));
        //        message.AlternateViews.Add(avCal);
        //        smtp.Send(message);
        //    }
        //    using (var message = new MailMessage(hostAdress, new MailAddress(inscricao.EmailResponsavel, inscricao.NomeResponsavel))
        //    {
        //        Subject = subjectCliente,
        //        Body = corpoCliente
        //    })
        //    {
        //        message.To.Add(new MailAddress(especialista.Email, especialista.Nome));
        //        //message.AlternateViews.Add(avCal);
        //        smtp.Send(message);
        //    }
        //}
    }
}
