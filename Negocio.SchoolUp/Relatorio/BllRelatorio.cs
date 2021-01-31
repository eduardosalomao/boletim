using Acesso.Nucleo.Geral;
using Acesso.SchoolUp.Relatorio;
using Comum;
using Modelo.Nucleo.Geral;
using Modelo.SchoolUp.Recursos;
using Modelo.SchoolUp.Relatorio;
using Negocio.Nucleo.Geral;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Negocio.SchoolUp.Relatorio
{
    public class BllRelatorio: BllAplicacao<RptComum>
    {
        public string IdUsuarioAcao { get; set; }
        public string Ip { get; set; }
        public string Funcao { get; set; }
        public string CodigoToken { get; set; }

        public BllRelatorio(string ip, string nomeFuncao, string token) : base(ip, nomeFuncao, token)
        {
            Ip = ip;
            Funcao = nomeFuncao;
            CodigoToken = token;
            IdUsuarioAcao = base.IdUsuario;
        }

        public BllRelatorio(string ip, string nomeFuncao) : base(ip, nomeFuncao)
        {
            IdUsuarioAcao = base.IdUsuario;
            Ip = ip;
            Funcao = nomeFuncao;
        }

        public GlResposta<RptAcessoResponsavel> ObterAcessoResponsavel(RptAcessoResponsavel mdlRptAcessoResponsavel)
        {
            GlResposta<RptAcessoResponsavel> resposta = new GlResposta<RptAcessoResponsavel>();
            DadosGravacao.IdAcao = BllAcao.IdAcaoAcessar;
            try
            {
                resposta = ValidarAutenticacaoPermissao<RptAcessoResponsavel>();
                if (!resposta.Succeeded)
                {
                    return resposta;
                }

                List<RptAcessoResponsavel> listaRptAcessoResponsavels = new DalRelatorio().ObterAcessoResponsavel(mdlRptAcessoResponsavel);

                if (listaRptAcessoResponsavels == null || listaRptAcessoResponsavels.Count == 0)
                {
                    return new GlResposta<RptAcessoResponsavel>() { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado };
                }
                resposta.Dados = listaRptAcessoResponsavels?.ToList();
                resposta.Succeeded = true;

                if (!IsAnonimo)
                {
                    DadosGravacao.IdAcao = BllAcao.IdAcaoAcessar;
                    DadosGravacao = BrHistorico.MontarHistoricoJson(DadosGravacao, mdlRptAcessoResponsavel, "ObterAcessoResponsavel()");
                    new DalGenerica<RptAcessoResponsavel>().InserirHistorico(DadosGravacao);
                }
            }
            catch (Exception excecao)
            {
                resposta.Succeeded = false;
                resposta.Mensagem = new BllErro().Inserir(DadosGravacao, excecao);
            }

            return resposta;
        }
    }
}
