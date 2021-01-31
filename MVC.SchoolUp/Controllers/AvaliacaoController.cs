using Microsoft.AspNetCore.Mvc;
using Modelo.Nucleo.Geral;
using Modelo.SchoolUp.Principal;
using Microsoft.AspNetCore.Http;
using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using Modelo.SchoolUp.Custom;
using Modelo.SchoolUp.Recursos;
using System.Threading.Tasks;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Collections.Generic;
using NPOI.HSSF.Util;

namespace MVC.SchoolUp.Controllers
{
    public class AvaliacaoController : MasterController
    {
        public AvaliacaoController() : base()
        {
            NomeFuncao = "SCHOOLUP_AVALIACAO";
        }

        public IActionResult GetSubPeriodo(string idPeriodo)
        {
            var idUsuario = HttpContext?.Session?.GetString("IdUsuario");
            var idEscola = HttpContext?.Session?.GetString("IdEscola");
            Guid idUsuarioGuid;
            if (!Guid.TryParse(idUsuario, out idUsuarioGuid))
            {
                TempData["PaginaAnterior"] = String.Format("{0}://{1}{2}{3}", Request.Scheme, Request.Host, Request.Path, Request.QueryString);
                TempData["MensagemErro"] = "Sessão expirada";
                return RedirectToAction("Login", "Acesso");
            }

            GlResposta<SubPeriodo> resultSubPeriodo = GetAux<SubPeriodo>("Servico/ObterSubPeriodo", idPeriodo);
            if (resultSubPeriodo.Succeeded == false)
            {
                TempData["CodigoErro"] = "";
                TempData["MensagemErro"] = resultSubPeriodo.Mensagem;
                return RedirectToAction("Erro", "Home");
            }

            return Ok(resultSubPeriodo);
        }

        public IActionResult GetTurmas(string idPeriodo)
        {
            var idUsuario = HttpContext?.Session?.GetString("IdUsuario");
            var idEscola = HttpContext?.Session?.GetString("IdEscola");
            Guid idUsuarioGuid;
            if (!Guid.TryParse(idUsuario, out idUsuarioGuid))
            {
                TempData["PaginaAnterior"] = String.Format("{0}://{1}{2}{3}", Request.Scheme, Request.Host, Request.Path, Request.QueryString);
                TempData["MensagemErro"] = "Sessão expirada";
                return RedirectToAction("Login", "Acesso");
            }

            GlResposta<Turma> resultTurma = GetAux<Turma>("Servico/ObterTurma", idPeriodo);
            if (resultTurma.Succeeded == false)
            {
                TempData["CodigoErro"] = "";
                TempData["MensagemErro"] = resultTurma.Mensagem;
                return RedirectToAction("Erro", "Home");
            }

            return Ok(resultTurma.Dados);
        }

        public IActionResult GetTurmasAluno(string idPeriodo, string idTurmaAluno)
        {
            var idUsuario = HttpContext?.Session?.GetString("IdUsuario");
            var idEscola = HttpContext?.Session?.GetString("IdEscola");
            Guid idUsuarioGuid;
            if (!Guid.TryParse(idUsuario, out idUsuarioGuid))
            {
                TempData["PaginaAnterior"] = String.Format("{0}://{1}{2}{3}", Request.Scheme, Request.Host, Request.Path, Request.QueryString);
                TempData["MensagemErro"] = "Sessão expirada";
                return RedirectToAction("Login", "Acesso");
            }

            GlResposta<Turma> resultTurma = GetAux<Turma>("Servico/ObterTurma", idPeriodo);
            resultTurma.Dados = resultTurma.Dados.Where(w => idTurmaAluno.Contains(w.Id.ToString())).ToList();
            if (resultTurma.Succeeded == false)
            {
                TempData["CodigoErro"] = "";
                TempData["MensagemErro"] = resultTurma.Mensagem;
                return RedirectToAction("Erro", "Home");
            }

            return Ok(resultTurma.Dados);
        }

        [HttpGet("Avaliacao/Exportar/{folderName}/{fileName}")]
        public async Task<IActionResult> Exportar(string folderName, string fileName)
        {
            string sWebRootFolder = folderName;
            string sFileName = fileName;
            var memory = new MemoryStream();
            
            using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", sFileName);
        }

        [HttpGet("Avaliacao/Exportar/{idAvaliacao}")]
        public async Task<IActionResult> Exportar(string idAvaliacao)
        {
            GlResposta<CmNotas> result = Get<CmNotas>("Avaliacao/ObterNotas", idAvaliacao);
            if (result == null)
            {
                TempData["CodigoErro"] = "";
                TempData["MensagemErro"] = "Ocorreu um erro desconhecido";
                return RedirectToAction("Erro", "Home");
            }
            List<CmNotas> notas = result.Dados;

            string sWebRootFolder = "PlanilhasNotas";
            string sFileName = $"{notas.FirstOrDefault().Turma}-{notas.FirstOrDefault().Disciplina}-{DateTime.Now.ToString("yyyyMMddHHmm")}.xlsx";
            var memory = new MemoryStream();
            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet($"{notas.FirstOrDefault().Turma}-{notas.FirstOrDefault().Disciplina}-{notas.FirstOrDefault().Bimestre}");
                IRow row;
                int linha = 1;
                int coluna = 0;

                ICellStyle boldStyle = workbook.CreateCellStyle();
                boldStyle.FillForegroundColor = HSSFColor.SeaGreen.Index;
                boldStyle.FillPattern = FillPattern.SolidForeground;
                
                row = excelSheet.CreateRow(linha++);
                row.CreateCell(coluna).SetCellValue("Número");
                row.GetCell(coluna++).CellStyle = boldStyle;
                row.CreateCell(coluna).SetCellValue("Matrícula");
                row.GetCell(coluna++).CellStyle = boldStyle;
                row.CreateCell(coluna).SetCellValue("Nome");
                row.GetCell(coluna++).CellStyle = boldStyle;
                row.CreateCell(coluna).SetCellValue("Nota");
                row.GetCell(coluna++).CellStyle = boldStyle;
                row.CreateCell(coluna).SetCellValue("Nota Recuperação");
                row.GetCell(coluna++).CellStyle = boldStyle;
                row.CreateCell(coluna).SetCellValue("Faltas");
                row.GetCell(coluna++).CellStyle = boldStyle;

                if (notas?.Any() == true)
                {
                    coluna = 100;
                    row.CreateCell(coluna).SetCellValue(notas.FirstOrDefault().IdTurma.ToString());
                    row.GetCell(coluna++).CellStyle.IsLocked = true;
                    
                    row.CreateCell(coluna).SetCellValue(notas.FirstOrDefault().IdDisciplina.ToString());
                    row.GetCell(coluna++).CellStyle.IsLocked = true;

                    row.CreateCell(coluna).SetCellValue(notas.FirstOrDefault().IdBimestre.ToString());
                    row.GetCell(coluna++).CellStyle.IsLocked = true;
                }

                foreach (var nota in notas)
                {
                    coluna = 0;

                    row = excelSheet.CreateRow(linha);
                    row.CreateCell(coluna).SetCellValue(linha++ - 1);
                    row.GetCell(coluna).SetCellType(CellType.String);
                    row.GetCell(coluna++).CellStyle.IsLocked = true;
                    row.CreateCell(coluna).SetCellValue(nota.MatriculaAluno);
                    row.GetCell(coluna).SetCellType(CellType.String);
                    row.GetCell(coluna++).CellStyle.IsLocked = true;
                    row.CreateCell(coluna).SetCellValue(nota.NomeAluno);
                    row.GetCell(coluna).SetCellType(CellType.String);
                    row.GetCell(coluna++).CellStyle.IsLocked = true;
                    row.CreateCell(coluna).SetCellValue(nota.Nota.HasValue ? nota.Nota.ToString() : "");
                    row.GetCell(coluna++).SetCellType(CellType.String);
                    row.CreateCell(coluna).SetCellValue(nota.NotaRecuperacao.HasValue ? nota.NotaRecuperacao.ToString() : "");
                    row.GetCell(coluna++).SetCellType(CellType.String);
                    row.CreateCell(coluna).SetCellValue(nota.Faltas.HasValue ? nota.Faltas.ToString() : "");
                    row.GetCell(coluna++).SetCellType(CellType.String);

                    row.CreateCell(100).SetCellValue(nota.Id.ToString());
                    row.GetCell(100).CellStyle.IsLocked = true;
                }
                for (coluna = 0; coluna < 6; coluna++)
                {
                    excelSheet.AutoSizeColumn(coluna);
                }
                                
                workbook.Write(fs);
            }
            using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", sFileName);
        }

        [HttpPost("Avaliacao/ImportarNotas/{idAvaliacao}")]
        //public async Task<IActionResult> Importar(string idAvaliacao)
        public IActionResult ImportarNotas([Bind()] CmNotas mdlDados)
        {
            IFormFile arquivo;
            string sWebRootFolder;
            string sFileName;
            List<CmNotas> notasAtuais;
            string idAvaliacao = mdlDados.IdAvaliacao.ToString();

            try
            {
                if (HttpContext?.Request?.Form?.Files?.Any() != true || String.IsNullOrEmpty(HttpContext?.Request?.Form?.Files[0]?.FileName))
                {
                    TempData["CodigoErro"] = "1";
                    TempData["MensagemErro"] = "Selecione um arquivo";
                    return RedirectToAction("Erro", "Home");
                }

                arquivo = HttpContext.Request.Form.Files[0];

                sWebRootFolder = "PlanilhasTemp";
                sFileName = $"Resultado-{HttpContext?.Request?.Form?.Files[0]?.FileName}";

                using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create))
                {
                    arquivo.CopyTo(stream);
                }

                using (FileStream fileStream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open, FileAccess.Read))
                {
                    try
                    {
                        GlResposta<CmNotas> result = Get<CmNotas>("Avaliacao/ObterNotas", idAvaliacao);
                        if (result == null)
                        {
                            TempData["CodigoErro"] = "2";
                            TempData["MensagemErro"] = "Ocorreu um erro desconhecido ao obter notas";
                            return RedirectToAction("Erro", "Home");
                        }
                        notasAtuais = result.Dados;
                    }
                    catch (Exception)
                    {
                        TempData["CodigoErro"] = "3";
                        TempData["MensagemErro"] = "Erro na obtenção das notas";
                        return RedirectToAction("Erro", "Home");
                    }

                    sWebRootFolder = "PlanilhasNotas";
                    using (var fileStreamResultado = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
                    {
                        try
                        {
                            IWorkbook workbook;
                            try
                            {
                                workbook = new XSSFWorkbook(fileStream);
                            }
                            catch (Exception excecao)
                            {
                                TempData["CodigoErro"] = "9";
                                TempData["MensagemErro"] = "Arquivo não é uma planilha";
                                return RedirectToAction("Erro", "Home");
                            }
                            
                            ISheet excelSheet = workbook.GetSheetAt(0);
                            IRow row;

                            row = excelSheet.GetRow(1);

                            var notaAtual = notasAtuais.FirstOrDefault();
                            string mensagemErro = String.Empty;

                            if (!notaAtual.IdTurma.ToString().Equals(row.GetCell(100).StringCellValue, StringComparison.OrdinalIgnoreCase))
                            {
                                mensagemErro += $"Esta planilha não pertence à Turma {notaAtual.Turma}!{Mensagens.CaracterePulaLinha}";
                            }

                            if (!notaAtual.IdDisciplina.ToString().Equals(row.GetCell(101).StringCellValue, StringComparison.OrdinalIgnoreCase))
                            {
                                mensagemErro += $"Esta planilha não pertence à Disciplina {notaAtual.Disciplina}!{Mensagens.CaracterePulaLinha}";
                            }

                            if (!notaAtual.IdBimestre.ToString().Equals(row.GetCell(102).StringCellValue, StringComparison.OrdinalIgnoreCase))
                            {
                                mensagemErro += $"Esta planilha não pertence ao {notaAtual.Bimestre}!{Mensagens.CaracterePulaLinha}";
                            }

                            if (mensagemErro != "")
                            {
                                TempData["CodigoErro"] = "8";
                                TempData["MensagemErro"] = mensagemErro;
                                return RedirectToAction("Erro", "Home");
                            }

                            IWorkbook workbookResultado = new XSSFWorkbook();
                            ISheet excelSheetResultado = workbookResultado.CreateSheet($"{notaAtual.Turma}-{notaAtual.Disciplina}-{notaAtual.Bimestre}");
                            IRow rowCreate = excelSheetResultado.CreateRow(1);

                            ICellStyle locked = workbookResultado.CreateCellStyle();
                            ICellStyle boldStyle = workbookResultado.CreateCellStyle();
                            locked.IsLocked = true;
                            boldStyle.FillForegroundColor = HSSFColor.SeaGreen.Index;
                            boldStyle.FillPattern = FillPattern.SolidForeground;

                            int coluna = 0;
                            rowCreate.CreateCell(coluna).SetCellValue("Número");
                            rowCreate.GetCell(coluna++).CellStyle = boldStyle;
                            rowCreate.CreateCell(coluna).SetCellValue("Matrícula");
                            rowCreate.GetCell(coluna++).CellStyle = boldStyle;
                            rowCreate.CreateCell(coluna).SetCellValue("Nome");
                            rowCreate.GetCell(coluna++).CellStyle = boldStyle;
                            rowCreate.CreateCell(coluna).SetCellValue("Nota");
                            rowCreate.GetCell(coluna++).CellStyle = boldStyle;
                            rowCreate.CreateCell(coluna).SetCellValue("Nota Recuperação");
                            rowCreate.GetCell(coluna++).CellStyle = boldStyle;
                            rowCreate.CreateCell(coluna).SetCellValue("Faltas");
                            rowCreate.GetCell(coluna++).CellStyle = boldStyle;
                            rowCreate.CreateCell(coluna).SetCellValue("Resultado");
                            rowCreate.GetCell(coluna).CellStyle = boldStyle;

                            List<CmNotas> listaNotasImport = new List<CmNotas>();
                                                       
                            for (int linha = 2; linha <= excelSheet.LastRowNum; linha++)
                            {
                                row = excelSheet.GetRow(linha);
                                if (row == null || (row?.Cells?.Any() != true))
                                {
                                    break;
                                }

                                try
                                {
                                    rowCreate = excelSheetResultado.CreateRow(linha);

                                    Guid id = String.IsNullOrEmpty(row.GetCell(100)?.ToString() ?? null) ? Guid.Empty : Guid.Parse(row.GetCell(100).ToString());
                                    Notas notaImport = new Notas()
                                    {
                                        Id = id
                                    };

                                    coluna = 0;
                                    row.GetCell(coluna).SetCellType(CellType.String);
                                    string numero = String.IsNullOrEmpty(row.GetCell(coluna)?.ToString() ?? null) ? "" : row.GetCell(coluna).ToString();
                                    rowCreate.CreateCell(coluna++).SetCellValue(numero);

                                    row.GetCell(coluna).SetCellType(CellType.String);
                                    string matriculaAluno = String.IsNullOrEmpty(row.GetCell(coluna)?.ToString() ?? null) ? "" : row.GetCell(coluna).ToString();
                                    rowCreate.CreateCell(coluna++).SetCellValue(matriculaAluno);

                                    row.GetCell(coluna).SetCellType(CellType.String);
                                    string nomeAluno = String.IsNullOrEmpty(row.GetCell(coluna)?.ToString() ?? null) ? "" : row.GetCell(coluna).ToString();
                                    rowCreate.CreateCell(coluna++).SetCellValue(nomeAluno);

                                    row.GetCell(coluna).SetCellType(CellType.String);
                                    if (String.IsNullOrEmpty(row.GetCell(coluna)?.ToString() ?? null))
                                    {
                                        notaImport.Nota = null;
                                    }
                                    else
                                    {
                                        notaImport.Nota = decimal.Parse(row.GetCell(coluna).ToString().Replace(".", ","));
                                    }
                                    rowCreate.CreateCell(coluna++).SetCellValue(notaImport.Nota?.ToString() ?? "");

                                    row.GetCell(coluna).SetCellType(CellType.String);
                                    if (String.IsNullOrEmpty(row.GetCell(coluna)?.ToString() ?? null))
                                    {
                                        notaImport.NotaRecuperacao = null;
                                    }
                                    else
                                    {
                                        notaImport.NotaRecuperacao = decimal.Parse(row.GetCell(coluna).ToString().Replace(".", ","));
                                    }
                                    rowCreate.CreateCell(coluna++).SetCellValue(notaImport.NotaRecuperacao?.ToString() ?? "");

                                    row.GetCell(coluna).SetCellType(CellType.String);
                                    if (String.IsNullOrEmpty(row.GetCell(coluna)?.ToString() ?? null))
                                    {
                                        notaImport.Faltas = null;
                                    }
                                    else
                                    {
                                        notaImport.Faltas = int.Parse(row.GetCell(coluna).ToString().Split(".").FirstOrDefault());
                                    }
                                    rowCreate.CreateCell(coluna++).SetCellValue(notaImport.Faltas?.ToString() ?? "");

                                    notaAtual = notasAtuais.Where(i => i.Id == id).FirstOrDefault();

                                    if (!notaAtual.MatriculaAluno.Equals(matriculaAluno, StringComparison.OrdinalIgnoreCase))
                                    {
                                        rowCreate.CreateCell(6).SetCellValue("Matrícula discrepante!");
                                        continue;
                                    }

                                    var resultImport = Put<Notas>("Avaliacao/GravarNotaCompleta",
                                        new Notas()
                                        {
                                            Id = notaImport.Id,
                                            Nota = notaImport.Nota,
                                            NotaRecuperacao = notaImport.NotaRecuperacao,
                                            Faltas = notaImport.Faltas
                                        });
                                    if (!resultImport.Succeeded)
                                    {
                                        rowCreate.CreateCell(6).SetCellValue("Dados incorretos. Nada foi gravado!");
                                        continue;
                                    }

                                    rowCreate.CreateCell(6).SetCellValue("Ok");
                                }
                                catch (Exception excecao)
                                {
                                    rowCreate.CreateCell(6).SetCellValue("Dados incorretos");
                                }
                            }

                            try
                            {
                                for (coluna = 0; coluna < 7; coluna++)
                                {
                                    excelSheetResultado.AutoSizeColumn(coluna);
                                }

                                for (int linha = 0; linha < excelSheetResultado.LastRowNum; linha++)
                                {
                                    for (coluna = 0; coluna < rowCreate.Cells.Count; coluna++)
                                    {
                                        rowCreate.GetCell(coluna).CellStyle = locked;
                                    }
                                }

                                workbookResultado.Write(fileStreamResultado);
                            }
                            catch (Exception excecao)
                            {
                                TempData["CodigoErro"] = "4";
                                TempData["MensagemErro"] = "Erro na gravação da planilha";
                                return RedirectToAction("Erro", "Home");
                            }
                        }
                        catch (Exception excecao)
                        {
                            TempData["CodigoErro"] = "5";
                            TempData["MensagemErro"] = "Erro na inicialização da planilha";
                            return RedirectToAction("Erro", "Home");
                        }
                    }
                }

                try
                {
                    var memory = new MemoryStream();

                    using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
                    {
                        stream.CopyToAsync(memory);
                    }
                    memory.Position = 0;

                    ViewBag.FolderName = sWebRootFolder;
                    ViewBag.FileName = sFileName;

                    //FileStreamResult arquivoDownload = File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", sFileName);

                    //return ImportarNotasRetorno(idAvaliacao, arquivoDownload);
                    return ImportarNotas(idAvaliacao);

                    //return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", sFileName); ;
                }
                catch (Exception excecao)
                {
                    TempData["CodigoErro"] = "6";
                    TempData["MensagemErro"] = "Erro no upload da planilha";
                    return RedirectToAction("Erro", "Home");
                }
            }
            catch (Exception excecao)
            {
                TempData["CodigoErro"] = "7";
                TempData["MensagemErro"] = "Erro na gravação da planilha";
                return RedirectToAction("Erro", "Home");
            }
        }

        public IActionResult GetGradeCalendario(string idTurma, string idSubPeriodo)
        {
            var idUsuario = HttpContext?.Session?.GetString("IdUsuario");
            var idEscola = HttpContext?.Session?.GetString("IdEscola");
            Guid idUsuarioGuid;
            if (!Guid.TryParse(idUsuario, out idUsuarioGuid))
            {
                TempData["PaginaAnterior"] = String.Format("{0}://{1}{2}{3}", Request.Scheme, Request.Host, Request.Path, Request.QueryString);
                TempData["MensagemErro"] = "Sessão expirada";
                return RedirectToAction("Login", "Acesso");
            }

            if (String.IsNullOrEmpty(idTurma))
            {
                ViewBag.IsSucesso = "false";
                ViewBag.LblMensagem = "Selecione uma turma";

                return PartialView("PartialGradeCalendario", new GlResposta<CmAvaliacao>() { Mensagem = Mensagens.SemRegistroEncontrado });
            }

            if (String.IsNullOrEmpty(idSubPeriodo))
            {
                ViewBag.IsSucesso = "false";
                ViewBag.LblMensagem = "Selecione um bimestre";

                return PartialView("PartialGradeCalendario", new GlResposta<CmAvaliacao>() { Mensagem = Mensagens.SemRegistroEncontrado });
            }

            GlResposta<CmAvaliacao> result = Get<CmAvaliacao>("Avaliacao/ObterGrade", new string[] { idTurma, idSubPeriodo });
            if (result == null)
            {
                TempData["CodigoErro"] = "";
                TempData["MensagemErro"] = "Ocorreu um erro desconhecido";
                return RedirectToAction("Erro", "Home");
            }
            if (!result.Succeeded)
            {
                ViewBag.IsSucesso = "false";
                ViewBag.LblMensagem = result.Mensagem;
            }
            else
            {
                if (result.Dados == null || result.Dados.Count == 0)
                {
                    ViewBag.IsSucesso = "true";
                    ViewBag.LblMensagem = result.Mensagem;
                }
            }

            return PartialView("PartialGradeCalendario", result);
        }

        public IActionResult GetCalendario(string idTurma, string idSubPeriodo)
        {
            var idUsuario = HttpContext?.Session?.GetString("IdUsuario");
            var idEscola = HttpContext?.Session?.GetString("IdEscola");
            Guid idUsuarioGuid;
            if (!Guid.TryParse(idUsuario, out idUsuarioGuid))
            {
                TempData["PaginaAnterior"] = String.Format("{0}://{1}{2}{3}", Request.Scheme, Request.Host, Request.Path, Request.QueryString);
                TempData["MensagemErro"] = "Sessão expirada";
                return RedirectToAction("Login", "Acesso");
            }

            Guid idTurmaGuid, idSubPeriodoGuid;

            if (!String.IsNullOrEmpty(idTurma))
            {
                if (!Guid.TryParse(idTurma, out idTurmaGuid))
                {
                    return BadRequest(new GlResposta<CmAvaliacao>() { Succeeded = false, Mensagem = Mensagens.FormatoIncorreto });
                }
            }
            if (!String.IsNullOrEmpty(idSubPeriodo))
            {
                if (!Guid.TryParse(idSubPeriodo, out idSubPeriodoGuid))
                {
                    return BadRequest(new GlResposta<CmAvaliacao>() { Succeeded = false, Mensagem = Mensagens.FormatoIncorreto });
                }
            }

            GlResposta<CmAvaliacao> result = Get<CmAvaliacao>("Avaliacao/ObterGrade", new string[] { idTurmaGuid.ToString(), idSubPeriodoGuid.ToString() });
            ViewBag.IsSucesso = "";
            if (result == null)
            {
                TempData["CodigoErro"] = "";
                TempData["MensagemErro"] = "Ocorreu um erro desconhecido";
                return RedirectToAction("Erro", "Home");
            }
            if (!result.Succeeded)
            {
                ViewBag.IsSucesso = "false";
                ViewBag.LblMensagem = result.Mensagem;
            }
            else
            {
                if (result.Dados == null || result.Dados.Count == 0)
                {
                    ViewBag.IsSucesso = "true";
                    ViewBag.LblMensagem = result.Mensagem;
                }
            }

            return PartialView("PartialCalendario", result);
        }

        public IActionResult GetGrade(string idTurma, string idSubPeriodo)
        {
            var idUsuario = HttpContext?.Session?.GetString("IdUsuario");
            var idEscola = HttpContext?.Session?.GetString("IdEscola");
            CarregarMenuObsoleto();
            Guid idUsuarioGuid;
            if (!Guid.TryParse(idUsuario, out idUsuarioGuid))
            {
                TempData["PaginaAnterior"] = String.Format("{0}://{1}{2}{3}", Request.Scheme, Request.Host, Request.Path, Request.QueryString);
                TempData["MensagemErro"] = "Sessão expirada";
                return RedirectToAction("Login", "Acesso");
            }

            if (String.IsNullOrEmpty(idTurma))
            {
                ViewBag.IsSucesso = "false";
                ViewBag.LblMensagem = "Selecione uma turma";

                return PartialView("PartialGrade", new GlResposta<CmAvaliacao>() { Mensagem = Mensagens.SemRegistroEncontrado });
            }

            if (String.IsNullOrEmpty(idSubPeriodo))
            {
                ViewBag.IsSucesso = "false";
                ViewBag.LblMensagem = "Selecione um bimestre";

                return PartialView("PartialGrade", new GlResposta<CmAvaliacao>() { Mensagem = Mensagens.SemRegistroEncontrado });
            }

            GlResposta<CmAvaliacao> result = Get<CmAvaliacao>("Avaliacao/ObterGrade", new string[] { idTurma, idSubPeriodo });
            if (result == null)
            {
                TempData["CodigoErro"] = "";
                TempData["MensagemErro"] = "Ocorreu um erro desconhecido";
                return RedirectToAction("Erro", "Home");
            }
            if (!result.Succeeded)
            {
                ViewBag.IsSucesso = "false";
                ViewBag.LblMensagem = result.Mensagem;
            }
            else
            {
                if (result.Dados == null || result.Dados.Count == 0)
                {
                    ViewBag.IsSucesso = "true";
                    ViewBag.LblMensagem = result.Mensagem;
                }
            }

            return PartialView("PartialGrade", result);
        }

        public IActionResult GetGradeDisciplina(string idTurma, string idSubPeriodo)
        {
            var idUsuario = HttpContext?.Session?.GetString("IdUsuario");
            var idEscola = HttpContext?.Session?.GetString("IdEscola");
            Guid idUsuarioGuid;
            if (!Guid.TryParse(idUsuario, out idUsuarioGuid))
            {
                TempData["PaginaAnterior"] = String.Format("{0}://{1}{2}{3}", Request.Scheme, Request.Host, Request.Path, Request.QueryString);
                TempData["MensagemErro"] = "Sessão expirada";
                return RedirectToAction("Login", "Acesso");
            }

            if (String.IsNullOrEmpty(idTurma))
            {
                ViewBag.IsSucesso = "false";
                ViewBag.LblMensagem = "Selecione uma turma";

                return PartialView("PartialGradeDisciplinas", new GlResposta<CmAvaliacao>() { Mensagem = Mensagens.SemRegistroEncontrado });
            }

            if (String.IsNullOrEmpty(idSubPeriodo))
            {
                ViewBag.IsSucesso = "false";
                ViewBag.LblMensagem = "Selecione um bimestre";

                return PartialView("PartialGradeDisciplinas", new GlResposta<CmAvaliacao>() { Mensagem = Mensagens.SemRegistroEncontrado });
            }

            GlResposta<CmAvaliacao> result = Get<CmAvaliacao>("Avaliacao/ObterGradeDisciplina", new string[] { idTurma, idSubPeriodo });
            if (result == null)
            {
                TempData["CodigoErro"] = "";
                TempData["MensagemErro"] = "Ocorreu um erro desconhecido";
                return RedirectToAction("Erro", "Home");
            }
            if (!result.Succeeded)
            {
                ViewBag.IsSucesso = "false";
                ViewBag.LblMensagem = result.Mensagem;
            }
            else
            {
                if (result.Dados == null || result.Dados.Count == 0)
                {
                    ViewBag.IsSucesso = "true";
                    ViewBag.LblMensagem = result.Mensagem;
                }
            }

            return PartialView("PartialGradeDisciplinas", result);
        }
        
        public IActionResult GetGradeImportar(string idTurma, string idSubPeriodo)
        {
            var idUsuario = HttpContext?.Session?.GetString("IdUsuario");
            var idEscola = HttpContext?.Session?.GetString("IdEscola");
            Guid idUsuarioGuid;
            if (!Guid.TryParse(idUsuario, out idUsuarioGuid))
            {
                TempData["PaginaAnterior"] = String.Format("{0}://{1}{2}{3}", Request.Scheme, Request.Host, Request.Path, Request.QueryString);
                TempData["MensagemErro"] = "Sessão expirada";
                return RedirectToAction("Login", "Acesso");
            }

            if (String.IsNullOrEmpty(idTurma))
            {
                ViewBag.IsSucesso = "false";
                ViewBag.LblMensagem = "Selecione uma turma";

                return PartialView("PartialGradeImportar", new GlResposta<CmAvaliacao>() { Mensagem = Mensagens.SemRegistroEncontrado });
            }

            if (String.IsNullOrEmpty(idSubPeriodo))
            {
                ViewBag.IsSucesso = "false";
                ViewBag.LblMensagem = "Selecione um bimestre";

                return PartialView("PartialGradeImportar", new GlResposta<CmAvaliacao>() { Mensagem = Mensagens.SemRegistroEncontrado });
            }

            GlResposta<CmAvaliacao> result = Get<CmAvaliacao>("Avaliacao/ObterGradeDisciplina", new string[] { idTurma, idSubPeriodo });
            if (result == null)
            {
                TempData["CodigoErro"] = "";
                TempData["MensagemErro"] = "Ocorreu um erro desconhecido";
                return RedirectToAction("Erro", "Home");
            }
            if (!result.Succeeded)
            {
                ViewBag.IsSucesso = "false";
                ViewBag.LblMensagem = result.Mensagem;
            }
            else
            {
                if (result.Dados == null || result.Dados.Count == 0)
                {
                    ViewBag.IsSucesso = "true";
                    ViewBag.LblMensagem = result.Mensagem;
                }
            }

            return PartialView("PartialGradeImportar", result);
        }

        [HttpGet("Avaliacao/Notas/{idAvaliacao}")]
        public IActionResult Notas(string idAvaliacao)
        {
            ViewBag.IsSucesso = "";
            GlResposta<string> resultPermissao = VerificarPermissao();
            if (resultPermissao?.Succeeded != true)
            {
                ViewBag.IsSucesso = "false";
                ViewBag.LblMensagem = resultPermissao.Mensagem;

                return View();
            }
            CarregarMenuObsoleto();

            var idUsuario = HttpContext?.Session?.GetString("IdUsuario");
            var idEscola = HttpContext?.Session?.GetString("IdEscola");
            Guid idUsuarioGuid;
            if (!Guid.TryParse(idUsuario, out idUsuarioGuid))
            {
                TempData["PaginaAnterior"] = String.Format("{0}://{1}{2}{3}", Request.Scheme, Request.Host, Request.Path, Request.QueryString);
                TempData["MensagemErro"] = "Sessão expirada";
                return RedirectToAction("Login", "Acesso");
            }
            Guid idAvaliacaoGuid;
            if (!Guid.TryParse(idAvaliacao, out idAvaliacaoGuid))
            {
                TempData["CodigoErro"] = "";
                TempData["MensagemErro"] = "Ocorreu um erro desconhecido";
                return RedirectToAction("Erro", "Home");
            }

            GlResposta<CmNotas> result = Get<CmNotas>("Avaliacao/ObterNotas", idAvaliacaoGuid.ToString());
            if (result == null)
            {
                TempData["CodigoErro"] = "";
                TempData["MensagemErro"] = "Ocorreu um erro desconhecido";
                return RedirectToAction("Erro", "Home");
            }
            if (!result.Succeeded)
            {
                ViewBag.IsSucesso = "false";
                ViewBag.LblMensagemGrid = result.Mensagem;
            }
            else
            {
                if (result.Dados == null || result.Dados.Count == 0)
                {
                    ViewBag.IsSucesso = "true";
                    ViewBag.LblMensagemGrid = result.Mensagem;
                }
            }

            return View(result);
        }

        [HttpGet("Avaliacao/Medias/{idAvaliacao}")]
        public IActionResult Medias(string idAvaliacao)
        {
            ViewBag.IsSucesso = "";
            GlResposta<string> resultPermissao = VerificarPermissao();
            if (resultPermissao?.Succeeded != true)
            {
                ViewBag.IsSucesso = "false";
                ViewBag.LblMensagem = resultPermissao.Mensagem;

                return View();
            }
            CarregarMenuObsoleto();

            var idUsuario = HttpContext?.Session?.GetString("IdUsuario");
            var idEscola = HttpContext?.Session?.GetString("IdEscola");
            Guid idUsuarioGuid;
            if (!Guid.TryParse(idUsuario, out idUsuarioGuid))
            {
                TempData["PaginaAnterior"] = String.Format("{0}://{1}{2}{3}", Request.Scheme, Request.Host, Request.Path, Request.QueryString);
                TempData["MensagemErro"] = "Sessão expirada";
                return RedirectToAction("Login", "Acesso");
            }
            Guid idAvaliacaoGuid;
            if (!Guid.TryParse(idAvaliacao, out idAvaliacaoGuid))
            {
                TempData["CodigoErro"] = "";
                TempData["MensagemErro"] = "Ocorreu um erro desconhecido";
                return RedirectToAction("Erro", "Home");
            }

            GlResposta<CmNotas> result = Get<CmNotas>("Avaliacao/ObterNotas", idAvaliacaoGuid.ToString());
            if (result == null)
            {
                TempData["CodigoErro"] = "";
                TempData["MensagemErro"] = "Ocorreu um erro desconhecido";
                return RedirectToAction("Erro", "Home");
            }
            if (!result.Succeeded)
            {
                ViewBag.IsSucesso = "false";
                ViewBag.LblMensagemGrid = result.Mensagem;
            }
            else
            {
                if (result.Dados == null || result.Dados.Count == 0)
                {
                    ViewBag.IsSucesso = "true";
                    ViewBag.LblMensagemGrid = result.Mensagem;
                }
            }

            return View(result);
        }

        [HttpGet("Avaliacao/ImportarNotas/{idAvaliacao}")]
        public IActionResult ImportarNotas(string idAvaliacao)
        {
            ViewBag.IsSucesso = "";
            GlResposta<string> resultPermissao = VerificarPermissao();
            if (resultPermissao?.Succeeded != true)
            {
                ViewBag.IsSucesso = "false";
                ViewBag.LblMensagem = resultPermissao.Mensagem;

                return View();
            }
            CarregarMenuObsoleto();

            var idUsuario = HttpContext?.Session?.GetString("IdUsuario");
            var idEscola = HttpContext?.Session?.GetString("IdEscola");
            Guid idUsuarioGuid;
            if (!Guid.TryParse(idUsuario, out idUsuarioGuid))
            {
                TempData["PaginaAnterior"] = String.Format("{0}://{1}{2}{3}", Request.Scheme, Request.Host, Request.Path, Request.QueryString);
                TempData["MensagemErro"] = "Sessão expirada";
                return RedirectToAction("Login", "Acesso");
            }
            Guid idAvaliacaoGuid;
            if (!Guid.TryParse(idAvaliacao, out idAvaliacaoGuid))
            {
                TempData["CodigoErro"] = "";
                TempData["MensagemErro"] = "Ocorreu um erro desconhecido";
                return RedirectToAction("Erro", "Home");
            }

            GlResposta<CmNotas> result = Get<CmNotas>("Avaliacao/ObterNotas", idAvaliacaoGuid.ToString());
            if (result == null)
            {
                TempData["CodigoErro"] = "";
                TempData["MensagemErro"] = "Ocorreu um erro desconhecido";
                return RedirectToAction("Erro", "Home");
            }
            if (!result.Succeeded)
            {
                ViewBag.IsSucesso = "false";
                ViewBag.LblMensagemGrid = result.Mensagem;
            }
            else
            {
                if (result.Dados == null || result.Dados.Count == 0)
                {
                    ViewBag.IsSucesso = "true";
                    ViewBag.LblMensagemGrid = result.Mensagem;
                }
            }

            return View(result);
        }

        [HttpGet("Avaliacao/ImportarNotasRetorno/{idAvaliacao}")]
        public IActionResult ImportarNotasRetorno(string idAvaliacao, FileStreamResult arquivo)
        {
            ViewBag.IsSucesso = "";
            GlResposta<string> resultPermissao = VerificarPermissao();
            if (resultPermissao?.Succeeded != true)
            {
                ViewBag.IsSucesso = "false";
                ViewBag.LblMensagem = resultPermissao.Mensagem;

                return View();
            }
            CarregarMenuObsoleto();

            var idUsuario = HttpContext?.Session?.GetString("IdUsuario");
            var idEscola = HttpContext?.Session?.GetString("IdEscola");
            Guid idUsuarioGuid;
            if (!Guid.TryParse(idUsuario, out idUsuarioGuid))
            {
                TempData["PaginaAnterior"] = String.Format("{0}://{1}{2}{3}", Request.Scheme, Request.Host, Request.Path, Request.QueryString);
                TempData["MensagemErro"] = "Sessão expirada";
                return RedirectToAction("Login", "Acesso");
            }
            Guid idAvaliacaoGuid;
            if (!Guid.TryParse(idAvaliacao, out idAvaliacaoGuid))
            {
                TempData["CodigoErro"] = "";
                TempData["MensagemErro"] = "Ocorreu um erro desconhecido";
                return RedirectToAction("Erro", "Home");
            }

            GlResposta<CmNotas> result = Get<CmNotas>("Avaliacao/ObterNotas", idAvaliacaoGuid.ToString());
            if (result == null)
            {
                TempData["CodigoErro"] = "";
                TempData["MensagemErro"] = "Ocorreu um erro desconhecido";
                return RedirectToAction("Erro", "Home");
            }
            if (!result.Succeeded)
            {
                ViewBag.IsSucesso = "false";
                ViewBag.LblMensagemGrid = result.Mensagem;
            }
            else
            {
                if (result.Dados == null || result.Dados.Count == 0)
                {
                    ViewBag.IsSucesso = "true";
                    ViewBag.LblMensagemGrid = result.Mensagem;
                }
                else
                {
                    ViewBag.IsSucesso = "true";
                    ViewBag.LblMensagemGrid = "Importação concluída!";
                }
            }

            return View("ImportarNotas", result);
        }

        [HttpPost("GravarNota")]
        public IActionResult GravarNota(string idNota, string nota)
        {
            var idUsuario = HttpContext?.Session?.GetString("IdUsuario");
            Guid idUsuarioGuid;
            if (!Guid.TryParse(idUsuario, out idUsuarioGuid))
            {
                TempData["PaginaAnterior"] = String.Format("{0}://{1}{2}{3}", Request.Scheme, Request.Host, Request.Path, Request.QueryString);
                TempData["MensagemErro"] = "Sessão expirada";
                return RedirectToAction("Login", "Acesso");
            }
            Guid idNotaGuid;
            GlResposta<Notas> resposta = new GlResposta<Notas>();
            if (!Guid.TryParse(idNota, out idNotaGuid))
            {
                return BadRequest(new GlResposta<Notas>() { Succeeded = false, Mensagem = Mensagens.FormatoIncorreto });
            }

            Notas mdlNotas = new Notas()
            {
                Id = idNotaGuid
            };

            decimal valorNota;
            if (String.IsNullOrEmpty(nota) == false)
            {
                if (!decimal.TryParse(nota, out valorNota))
                {
                    return BadRequest(new GlResposta<Notas>() { Succeeded = false, Mensagem = Mensagens.FormatoIncorreto });
                }
                mdlNotas.Nota = valorNota;
            }            

            GlResposta<Notas> result = Put<Notas>("Avaliacao/GravarNota", mdlNotas);

            if (result == null)
            {
                TempData["CodigoErro"] = "";
                TempData["MensagemErro"] = "Ocorreu um erro desconhecido";
                return RedirectToAction("Erro", "Home");
            }

            ViewBag.Id = result.Succeeded ? Guid.Parse(result.Id) : Guid.Empty;
            ViewBag.IsSucesso = result.Succeeded == false ? "false" : "";
            ViewBag.LblMensagem = result.Mensagem;

            return PartialView("JanelaMensagem");
        }


        [HttpPost("Avaliacao/GravarNotaRecuperacao")]
        public IActionResult GravarNotaRecuperacao(string idNota, string notaRecuperacao)
        {
            var idUsuario = HttpContext?.Session?.GetString("IdUsuario");
            Guid idUsuarioGuid;
            if (!Guid.TryParse(idUsuario, out idUsuarioGuid))
            {
                TempData["PaginaAnterior"] = String.Format("{0}://{1}{2}{3}", Request.Scheme, Request.Host, Request.Path, Request.QueryString);
                TempData["MensagemErro"] = "Sessão expirada";
                return RedirectToAction("Login", "Acesso");
            }
            Guid idNotaGuid;
            GlResposta<Notas> resposta = new GlResposta<Notas>();
            if (!Guid.TryParse(idNota, out idNotaGuid))
            {
                return BadRequest(new GlResposta<Notas>() { Succeeded = false, Mensagem = Mensagens.FormatoIncorreto });
            }

            Notas mdlNotas = new Notas()
            {
                Id = idNotaGuid
            };

            decimal valorNota;
            if (String.IsNullOrEmpty(notaRecuperacao) == false)
            {
                if (!decimal.TryParse(notaRecuperacao, out valorNota))
                {
                    return BadRequest(new GlResposta<Notas>() { Succeeded = false, Mensagem = Mensagens.FormatoIncorreto });
                }
                mdlNotas.NotaRecuperacao = valorNota;
            }

            GlResposta<Notas> result = Put<Notas>("Avaliacao/GravarNotaRecuperacao", mdlNotas);

            if (result == null)
            {
                TempData["CodigoErro"] = "";
                TempData["MensagemErro"] = "Ocorreu um erro desconhecido";
                return RedirectToAction("Erro", "Home");
            }

            ViewBag.Id = result.Succeeded ? Guid.Parse(result.Id) : Guid.Empty;
            ViewBag.IsSucesso = result.Succeeded == false ? "false" : "";
            ViewBag.LblMensagem = result.Mensagem;

            return PartialView("JanelaMensagem");
        }

        [HttpPost("GravarFaltas")]
        public IActionResult GravarFaltas(string idNota, string faltas)
        {
            var idUsuario = HttpContext?.Session?.GetString("IdUsuario");
            Guid idUsuarioGuid;
            if (!Guid.TryParse(idUsuario, out idUsuarioGuid))
            {
                TempData["PaginaAnterior"] = String.Format("{0}://{1}{2}{3}", Request.Scheme, Request.Host, Request.Path, Request.QueryString);
                TempData["MensagemErro"] = "Sessão expirada";
                return RedirectToAction("Login", "Acesso");
            }
            Guid idNotaGuid;
            GlResposta<Notas> resposta = new GlResposta<Notas>();
            if (!Guid.TryParse(idNota, out idNotaGuid))
            {
                return BadRequest(new GlResposta<Notas>() { Succeeded = false, Mensagem = Mensagens.FormatoIncorreto });
            }

            Notas mdlNotas = new Notas()
            {
                Id = idNotaGuid
            };

            int valorFaltas;
            if (String.IsNullOrEmpty(faltas) == false)
            {
                if (!int.TryParse(faltas, out valorFaltas))
                {
                    return BadRequest(new GlResposta<Notas>() { Succeeded = false, Mensagem = Mensagens.FormatoIncorreto });
                }
                mdlNotas.Faltas = valorFaltas;
            }

            GlResposta<Notas> result = Put<Notas>("Avaliacao/GravarFaltas", mdlNotas);

            if (result == null)
            {
                TempData["CodigoErro"] = "";
                TempData["MensagemErro"] = "Ocorreu um erro desconhecido";
                return RedirectToAction("Erro", "Home");
            }

            ViewBag.Id = result.Succeeded ? Guid.Parse(result.Id) : Guid.Empty;
            ViewBag.IsSucesso = result.Succeeded == false ? "false" : "";
            ViewBag.LblMensagem = result.Mensagem;

            return PartialView("JanelaMensagem");
        }

        //public IActionResult Edicao(Guid? id)
        //{
        //    ViewBag.IsSucesso = "";
        //    var idUsuario = HttpContext?.Session?.GetString("IdUsuario");
        //    var idEscola = HttpContext?.Session?.GetString("IdEscola");
        //    Guid idUsuarioGuid;
        //    if (!Guid.TryParse(idUsuario, out idUsuarioGuid))
        //    {
        //        TempData["PaginaAnterior"] = String.Format("{0}://{1}{2}{3}", Request.Scheme, Request.Host, Request.Path, Request.QueryString);
        //        TempData["MensagemErro"] = "Sessão expirada";
        //        return RedirectToAction("Login", "Acesso");
        //    }

        //    ViewBag.IdTurma = id.Value.ToString();
        //    GlResposta<CmDisciplinaHorario> result = Get<CmDisciplinaHorario>("Avaliacao/ObterGrade", id.Value.ToString());
        //    if (result == null)
        //    {
        //        TempData["CodigoErro"] = "";
        //        TempData["MensagemErro"] = "Ocorreu um erro desconhecido";
        //        return RedirectToAction("Erro", "Home");
        //    }
        //    if (!result.Succeeded)
        //    {
        //        ViewBag.IsSucesso = "false";
        //        ViewBag.LblMensagem = result.Mensagem;
        //    }
        //    else
        //    {
        //        if (result.Dados == null || result.Dados.Count == 0)
        //        {
        //            ViewBag.IsSucesso = "true";
        //            ViewBag.LblMensagem = result.Mensagem;
        //        }
        //    }

        //    return View(result);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Edicao(string idTurma)
        //{
        //    ViewBag.IsSucesso = "";
        //    var idUsuario = HttpContext?.Session?.GetString("IdUsuario");
        //    var idEscola = HttpContext?.Session?.GetString("IdEscola");
        //    Guid idUsuarioGuid;
        //    if (!Guid.TryParse(idUsuario, out idUsuarioGuid))
        //    {
        //        TempData["PaginaAnterior"] = String.Format("{0}://{1}{2}{3}", Request.Scheme, Request.Host, Request.Path, Request.QueryString);
        //        TempData["MensagemErro"] = "Sessão expirada";
        //        return RedirectToAction("Login", "Acesso");
        //    }

        //    idTurma = Request.Form["HdnIdTurma"];
        //    idTurma = HttpContext?.Session?.GetString("IdTurma");
        //    GlResposta<CmDisciplinaHorario> result = Get<CmDisciplinaHorario>("Disciplina/ObterGrade", idTurma);
        //    if (result == null)
        //    {
        //        TempData["CodigoErro"] = "";
        //        TempData["MensagemErro"] = "Ocorreu um erro desconhecido";
        //        return RedirectToAction("Erro", "Home");
        //    }
        //    if (!result.Succeeded)
        //    {
        //        ViewBag.IsSucesso = "false";
        //        ViewBag.LblMensagem = result.Mensagem;

        //        return PartialView("JanelaMensagem");
        //    }
        //    else
        //    {
        //        List<DisciplinaHorario> listDisciplinaHorario = new List<DisciplinaHorario>();
        //        int linha = 0;
        //        List<Guid> listGuids = result.Dados.Select(i => i.IdHorarioTurno.Value).ToList();
        //        listGuids = listGuids.Distinct().ToList();
        //        foreach (var horarioTurno in listGuids)
        //        {
        //            int coluna = 0;
        //            foreach (var diaSemana in Modelo.SchoolUp.Enumeracao.DiasSemana.ObterDiasSemana().OrderBy(o => o.dia))
        //            {
        //                string strDisciplina = Request.Form[$"Disc_{linha}_{coluna}"];
        //                string strProfessor = Request.Form[$"Prof_{linha}_{coluna}"];
        //                strDisciplina = strDisciplina?.Replace(",", "");
        //                strProfessor = strProfessor?.Replace(",", "");

        //                if (!String.IsNullOrEmpty(strDisciplina) || !String.IsNullOrEmpty(strProfessor))
        //                {
        //                    DisciplinaHorario disciplinaHorario = new DisciplinaHorario();
        //                    disciplinaHorario.Dia = diaSemana.dia;
        //                    disciplinaHorario.Excluido = false;
        //                    disciplinaHorario.IdHorarioTurno = horarioTurno;
        //                    disciplinaHorario.Id = Guid.NewGuid();
        //                    if (!String.IsNullOrEmpty(strDisciplina))
        //                    {
        //                        disciplinaHorario.IdDisciplina = Guid.Parse(strDisciplina);
        //                    }
        //                    if (!String.IsNullOrEmpty(strProfessor))
        //                    {
        //                        disciplinaHorario.IdProfessor = Guid.Parse(strProfessor);
        //                    }
        //                    disciplinaHorario.IdTurma = Guid.Parse(idTurma);
        //                    listDisciplinaHorario.Add(disciplinaHorario);
        //                }
        //                coluna++;
        //            }
        //            linha++;
        //        }
        //        GlResposta<DisciplinaHorario> resultGravar = Put<DisciplinaHorario>("Disciplina/GravarGrade", listDisciplinaHorario);
        //        if (!resultGravar.Succeeded)
        //        {
        //            ViewBag.IsSucesso = "false";
        //            ViewBag.LblMensagem = result.Mensagem;

        //            return PartialView("JanelaMensagem");
        //        }
        //    }

        //    ViewBag.IsSucesso = "true";
        //    ViewBag.LblMensagem = "Grade alterada";
        //    return PartialView("JanelaMensagem");
        //}

        [HttpGet("Avaliacao/Cadastro/{idTurma}/{idSubPeriodo}")]
        public IActionResult Cadastro(Guid? idTurma, Guid? idSubPeriodo)
        {
            ViewBag.IsSucesso = "";
            var idUsuario = HttpContext?.Session?.GetString("IdUsuario");
            var idEscola = HttpContext?.Session?.GetString("IdEscola");
            Guid idUsuarioGuid;
            if (!Guid.TryParse(idUsuario, out idUsuarioGuid))
            {
                TempData["PaginaAnterior"] = String.Format("{0}://{1}{2}{3}", Request.Scheme, Request.Host, Request.Path, Request.QueryString);
                TempData["MensagemErro"] = "Sessão expirada";
                return RedirectToAction("Login", "Acesso");
            }

            GlResposta<TipoAvaliacao> resultTipoAvaliacao = GetAux<TipoAvaliacao>("Servico/ObterTipoAvaliacao");
            if (resultTipoAvaliacao.Dados != null)
            {
                ViewBag.IdTipoAvaliacao = new SelectList(resultTipoAvaliacao.Dados?.ToList(), "Id", "Nome");
            }
            GlResposta<CmProfessorDisciplina> resultProfessorDisciplina = GetAux<CmProfessorDisciplina>("Servico/ObterTodosProfessorPorTurma", idTurma.Value.ToString());
            if (resultProfessorDisciplina.Dados != null)
            {
                ViewBag.IdProfessorDisciplina = new SelectList(resultProfessorDisciplina.Dados?.ToList(), "Id", "NomeProfessorDisciplina");
            }

            ViewBag.Id = Guid.Empty;
            ViewBag.IdTurma = idTurma;
            ViewBag.IdSubPeriodo = idSubPeriodo;

            return View();
        }

        [HttpGet("Avaliacao/Cadastro/{idAvaliacao}")]
        public IActionResult Cadastro(Guid? idAvaliacao)
        {
            ViewBag.IsSucesso = "";
            var idUsuario = HttpContext?.Session?.GetString("IdUsuario");
            var idEscola = HttpContext?.Session?.GetString("IdEscola");
            Guid idUsuarioGuid;
            if (!Guid.TryParse(idUsuario, out idUsuarioGuid))
            {
                TempData["PaginaAnterior"] = String.Format("{0}://{1}{2}{3}", Request.Scheme, Request.Host, Request.Path, Request.QueryString);
                TempData["MensagemErro"] = "Sessão expirada";
                return RedirectToAction("Login", "Acesso");
            }
            
            GlResposta<TipoAvaliacao> resultTipoAvaliacao = GetAux<TipoAvaliacao>("Servico/ObterTipoAvaliacao");
            if (resultTipoAvaliacao.Dados != null)
            {
                ViewBag.IdTipoAvaliacao = new SelectList(resultTipoAvaliacao.Dados?.ToList(), "Id", "Nome");
            }
            GlResposta<CmProfessorDisciplina> resultProfessorDisciplina = GetAux<CmProfessorDisciplina>("Servico/ObterTodosProfessorComDisciplina", idEscola);
            if (resultProfessorDisciplina.Dados != null)
            {
                ViewBag.IdProfessorDisciplina = new SelectList(resultProfessorDisciplina.Dados?.ToList(), "Id", "NomeProfessorDisciplina");
            }

            GlResposta<Avaliacao> result = Get<Avaliacao>("Avaliacao/Obter", idAvaliacao.Value.ToString());
            if (result == null)
            {
                TempData["CodigoErro"] = "";
                TempData["MensagemErro"] = "Ocorreu um erro desconhecido";
                return RedirectToAction("Erro", "Home");
            }
            if (!result.Succeeded)
            {
                ViewBag.IsSucesso = "false";
                ViewBag.LblMensagem = result.Mensagem;
            }
            else
            {
                Avaliacao avaliacao = result.Dados?.FirstOrDefault();
                ViewBag.Id = avaliacao.Id;
                ViewBag.IdTurma = avaliacao.IdTurma;
                ViewBag.IdSubPeriodo = avaliacao.IdSubPeriodo;
            }

            return View(result);
        }

        [HttpGet("Avaliacao/Editar")]
        [HttpGet("Avaliacao/Editar/{idAvaliacao}/{idPeriodo}/{periodoNome}")]
        public IActionResult Editar(string idAvaliacao, string idPeriodo, string periodoNome)
        {
            ViewBag.IsSucesso = "";
            var idUsuario = HttpContext?.Session?.GetString("IdUsuario");
            var idEscola = HttpContext?.Session?.GetString("IdEscola");
            Guid idUsuarioGuid;
            if (!Guid.TryParse(idUsuario, out idUsuarioGuid))
            {
                TempData["PaginaAnterior"] = String.Format("{0}://{1}{2}{3}", Request.Scheme, Request.Host, Request.Path, Request.QueryString);
                TempData["MensagemErro"] = "Sessão expirada";
                return RedirectToAction("Login", "Acesso");
            }

            //GlResposta<TipoAvaliacao> resultTipoAvaliacao = GetAux<TipoAvaliacao>("Servico/ObterTipoAvaliacao");
            //if (resultTipoAvaliacao.Dados != null)
            //{
            //    ViewBag.IdTipoAvaliacao = new SelectList(resultTipoAvaliacao.Dados?.ToList(), "Id", "Nome");
            //}
            //GlResposta<CmProfessorDisciplina> resultProfessorDisciplina = GetAux<CmProfessorDisciplina>("Servico/ObterTodosProfessorComDisciplina", idEscola);
            //if (resultProfessorDisciplina.Dados != null)
            //{
            //    ViewBag.IdProfessorDisciplina = new SelectList(resultProfessorDisciplina.Dados?.ToList(), "Id", "NomeProfessorDisciplina");
            //}

            GlResposta<Avaliacao> result = Get<Avaliacao>("Avaliacao/Obter", idAvaliacao);
            if (result == null)
            {
                TempData["CodigoErro"] = "";
                TempData["MensagemErro"] = "Ocorreu um erro desconhecido";
                return RedirectToAction("Erro", "Home");
            }
            if (!result.Succeeded)
            {
                ViewBag.IsSucesso = "false";
                ViewBag.LblMensagem = result.Mensagem;
            }
            else
            {
                Avaliacao avaliacao = result.Dados?.FirstOrDefault();
                ViewBag.Id = avaliacao.Id;
                ViewBag.IdTurma = avaliacao.IdTurma;
                ViewBag.IdSubPeriodo = avaliacao.IdSubPeriodo;
            }

            return Ok(result);
        }

        [HttpGet("Avaliacao/Criar")]
        public IActionResult Criar()
        {
            ViewBag.IsSucesso = "";
            var idUsuario = HttpContext?.Session?.GetString("IdUsuario");
            var idEscola = HttpContext?.Session?.GetString("IdEscola");
            Guid idUsuarioGuid;
            if (!Guid.TryParse(idUsuario, out idUsuarioGuid))
            {
                TempData["PaginaAnterior"] = String.Format("{0}://{1}{2}{3}", Request.Scheme, Request.Host, Request.Path, Request.QueryString);
                TempData["MensagemErro"] = "Sessão expirada";
                return RedirectToAction("Login", "Acesso");
            }


            GlResposta<string> resultPermissao = VerificarPermissao();
            if (resultPermissao?.Succeeded != true)
            {
                ViewBag.IsSucesso = "false";
                ViewBag.LblMensagem = resultPermissao.Mensagem;

                return View();
            }
            CarregarMenuObsoleto();

            GlResposta<Periodo> resultPeriodo = GetAux<Periodo>("Servico/ObterPeriodo", idEscola);
            if (resultPeriodo.Dados != null)
            {
                ViewBag.IdPeriodo = new SelectList(resultPeriodo.Dados?.ToList(), "Id", "Nome");
                Guid idPeriodo = resultPeriodo.Dados.FirstOrDefault().Id;
                GlResposta<Turma> resultTurma = GetAux<Turma>("Servico/ObterTurma", idPeriodo.ToString());
                ViewBag.IdTurma = new SelectList(resultTurma.Dados?.ToList(), "Id", "Nome");
                 
                GlResposta<SubPeriodo> resultSubPeriodo = GetAux<SubPeriodo>("Servico/ObterSubPeriodo", idPeriodo.ToString());
                ViewBag.IdSubPeriodo = new SelectList(resultSubPeriodo.Dados?.ToList(), "Id", "Nome");
            }

            GlResposta<TipoAvaliacao> resultTipoAvaliacao = GetAux<TipoAvaliacao>("Servico/ObterTipoAvaliacao");
            if (resultTipoAvaliacao.Dados != null)
            {
                ViewBag.IdTipoAvaliacao = new SelectList(resultTipoAvaliacao.Dados?.ToList(), "Id", "Nome");
            }
            GlResposta<CmProfessorDisciplina> resultProfessorDisciplina = GetAux<CmProfessorDisciplina>("Servico/ObterTodosProfessorComDisciplina", idEscola);
            if (resultProfessorDisciplina.Dados != null)
            {
                ViewBag.IdProfessorDisciplina = new SelectList(resultProfessorDisciplina.Dados?.ToList(), "Id", "NomeProfessorDisciplina");
            }

            //GlResposta<Avaliacao> result = Get<Avaliacao>("Avaliacao/Obter", idAvaliacao.Value.ToString());
            //if (result == null)
            //{
            //    TempData["CodigoErro"] = "";
            //    TempData["MensagemErro"] = "Ocorreu um erro desconhecido";
            //    return RedirectToAction("Erro", "Home");
            //}
            //if (!result.Succeeded)
            //{
            //    ViewBag.IsSucesso = "false";
            //    ViewBag.LblMensagem = result.Mensagem;
            //}
            //else
            //{
            //    Avaliacao avaliacao = result.Dados?.FirstOrDefault();
            //    ViewBag.Id = avaliacao.Id;
            //    ViewBag.IdTurma = avaliacao.IdTurma;
            //    ViewBag.IdSubPeriodo = avaliacao.IdSubPeriodo;
            //}

            return View();
        }

        [HttpGet("Avaliacao/CadastroCalendario")]
        public IActionResult CadastroCalendario()
        {
            ViewBag.IsSucesso = "";
            var idUsuario = HttpContext?.Session?.GetString("IdUsuario");
            var idEscola = HttpContext?.Session?.GetString("IdEscola");
            Guid idUsuarioGuid;
            if (!Guid.TryParse(idUsuario, out idUsuarioGuid))
            {
                TempData["PaginaAnterior"] = String.Format("{0}://{1}{2}{3}", Request.Scheme, Request.Host, Request.Path, Request.QueryString);
                TempData["MensagemErro"] = "Sessão expirada";
                return RedirectToAction("Login", "Acesso");
            }

            GlResposta<string> resultPermissao = VerificarPermissao();
            if (resultPermissao?.Succeeded != true)
            {
                ViewBag.IsSucesso = "false";
                ViewBag.LblMensagem = resultPermissao.Mensagem;

                return View();
            }
            CarregarMenuObsoleto();

            GlResposta<Periodo> resultPeriodo = GetAux<Periodo>("Servico/ObterPeriodo", idEscola);
            if (resultPeriodo.Dados != null)
            {
                ViewBag.IdPeriodo = new SelectList(resultPeriodo.Dados?.ToList(), "Id", "Nome");
                Guid idPeriodo = resultPeriodo.Dados.FirstOrDefault().Id;
                GlResposta<Turma> resultTurma = GetAux<Turma>("Servico/ObterTurma", idPeriodo.ToString());
                ViewBag.IdTurma = new SelectList(resultTurma.Dados?.ToList(), "Id", "Nome");

                GlResposta<SubPeriodo> resultSubPeriodo = GetAux<SubPeriodo>("Servico/ObterSubPeriodo", idPeriodo.ToString());
                ViewBag.IdSubPeriodo = new SelectList(resultSubPeriodo.Dados?.ToList(), "Id", "Nome");
            }

            GlResposta<TipoAvaliacao> resultTipoAvaliacao = GetAux<TipoAvaliacao>("Servico/ObterTipoAvaliacao");
            if (resultTipoAvaliacao.Dados != null)
            {
                ViewBag.IdTipoAvaliacao = new SelectList(resultTipoAvaliacao.Dados?.ToList(), "Id", "Nome");
                ViewBag.IdTipoAvaliacaoPuro = resultTipoAvaliacao.Dados?.ToList();
            }
            GlResposta<CmProfessorDisciplina> resultProfessorDisciplina = GetAux<CmProfessorDisciplina>("Servico/ObterTodosProfessorComDisciplina", idEscola);
            if (resultProfessorDisciplina.Dados != null)
            {
                ViewBag.IdProfessorDisciplina = new SelectList(resultProfessorDisciplina.Dados?.ToList(), "Id", "NomeProfessorDisciplina");
            }

            return View();
        }

        [HttpPost("Avaliacao/CadastroCalendario")]
        [ValidateAntiForgeryToken]
        public IActionResult CadastroCalendario([Bind("Id,IdSubPeriodo,IdProfessorDisciplina,IdTipoAvaliacao,IdTurma,Sigla,De,Ate")] Avaliacao mdlDados)
        {
            var idUsuario = HttpContext?.Session?.GetString("IdUsuario");
            Guid idUsuarioGuid;
            if (!Guid.TryParse(idUsuario, out idUsuarioGuid))
            {
                TempData["PaginaAnterior"] = String.Format("{0}://{1}{2}{3}", Request.Scheme, Request.Host, Request.Path, Request.QueryString);
                TempData["MensagemErro"] = "Sessão expirada";
                return RedirectToAction("Login", "Acesso");
            }
            GlResposta<Avaliacao> result = Post<Avaliacao>("Avaliacao/Criar", mdlDados);
            if (result == null)
            {
                TempData["CodigoErro"] = "";
                TempData["MensagemErro"] = "Ocorreu um erro desconhecido";
                return RedirectToAction("Erro", "Home");
            }

            ViewBag.Id = "";
            ViewBag.IsSucesso = result.Succeeded.ToString();
            ViewBag.LblMensagem = result.Mensagem;

            return PartialView("JanelaMensagemCalendario");
        }



        [HttpPost("Avaliacao/Cadastro/{idTurma}/{idSubPeriodo}")]
        [HttpPost("Avaliacao/Cadastro/{idAvaliacao}")]
        [ValidateAntiForgeryToken]
        public IActionResult Cadastro([Bind("Id,IdSubPeriodo,IdProfessorDisciplina,IdTipoAvaliacao,IdTurma,Sigla,De,Ate")] Avaliacao mdlDados)
        {
            var idUsuario = HttpContext?.Session?.GetString("IdUsuario");
            Guid idUsuarioGuid;
            if (!Guid.TryParse(idUsuario, out idUsuarioGuid))
            {
                TempData["PaginaAnterior"] = String.Format("{0}://{1}{2}{3}", Request.Scheme, Request.Host, Request.Path, Request.QueryString);
                TempData["MensagemErro"] = "Sessão expirada";
                return RedirectToAction("Login", "Acesso");
            }
            GlResposta<Avaliacao> result = null;
            Guid id = !String.IsNullOrEmpty(Request.Form["HdnIdChave"]) ? Guid.Parse(Request.Form["HdnIdChave"]) : mdlDados.Id;
            if (id.Equals(Guid.Empty))
            {
                result = Post<Avaliacao>("Avaliacao/Criar", mdlDados);
            }
            else
            {
                mdlDados.Id = id;
                result = Put<Avaliacao>("Avaliacao/Modificar", mdlDados);
            }

            if (result == null)
            {
                TempData["CodigoErro"] = "";
                TempData["MensagemErro"] = "Ocorreu um erro desconhecido";
                return RedirectToAction("Erro", "Home");
            }

            ViewBag.Id = result.Succeeded && result.Id != null ? Guid.Parse(result.Id) : id;
            ViewBag.IsSucesso = result.Succeeded.ToString();
            ViewBag.LblMensagem = result.Mensagem;

            return PartialView("JanelaMensagem");
        }

        [HttpGet("Avaliacao/CadastroDisciplina/{idTurma}/{idSubPeriodo}")]
        public IActionResult CadastroDisciplina(Guid? idTurma, Guid? idSubPeriodo)
        {
            ViewBag.IsSucesso = "";
            var idUsuario = HttpContext?.Session?.GetString("IdUsuario");
            var idEscola = HttpContext?.Session?.GetString("IdEscola");
            Guid idUsuarioGuid;
            if (!Guid.TryParse(idUsuario, out idUsuarioGuid))
            {
                TempData["PaginaAnterior"] = String.Format("{0}://{1}{2}{3}", Request.Scheme, Request.Host, Request.Path, Request.QueryString);
                TempData["MensagemErro"] = "Sessão expirada";
                return RedirectToAction("Login", "Acesso");
            }

            GlResposta<CmProfessorDisciplina> resultProfessorDisciplina = GetAux<CmProfessorDisciplina>("Servico/ObterTodosProfessorPorTurma", idTurma.Value.ToString());
            if (resultProfessorDisciplina.Dados != null)
            {
                ViewBag.IdProfessorDisciplina = new SelectList(resultProfessorDisciplina.Dados?.ToList(), "Id", "NomeProfessorDisciplina");
            }

            ViewBag.Id = Guid.Empty;
            ViewBag.IdTurma = idTurma;
            ViewBag.IdSubPeriodo = idSubPeriodo;

            return View();
        }

        [HttpGet("Avaliacao/CadastroDisciplina/{idAvaliacao}")]
        public IActionResult CadastroDisciplina(Guid? idAvaliacao)
        {
            ViewBag.IsSucesso = "";
            var idUsuario = HttpContext?.Session?.GetString("IdUsuario");
            var idEscola = HttpContext?.Session?.GetString("IdEscola");
            Guid idUsuarioGuid;
            if (!Guid.TryParse(idUsuario, out idUsuarioGuid))
            {
                TempData["PaginaAnterior"] = String.Format("{0}://{1}{2}{3}", Request.Scheme, Request.Host, Request.Path, Request.QueryString);
                TempData["MensagemErro"] = "Sessão expirada";
                return RedirectToAction("Login", "Acesso");
            }

            GlResposta<CmProfessorDisciplina> resultProfessorDisciplina = GetAux<CmProfessorDisciplina>("Servico/ObterTodosProfessorComDisciplina", idEscola);
            if (resultProfessorDisciplina.Dados != null)
            {
                ViewBag.IdProfessorDisciplina = new SelectList(resultProfessorDisciplina.Dados?.ToList(), "Id", "NomeProfessorDisciplina");
            }

            GlResposta<Avaliacao> result = Get<Avaliacao>("Avaliacao/Obter", idAvaliacao.Value.ToString());
            if (result == null)
            {
                TempData["CodigoErro"] = "";
                TempData["MensagemErro"] = "Ocorreu um erro desconhecido";
                return RedirectToAction("Erro", "Home");
            }
            if (!result.Succeeded)
            {
                ViewBag.IsSucesso = "false";
                ViewBag.LblMensagem = result.Mensagem;
            }
            else
            {
                Avaliacao avaliacao = result.Dados?.FirstOrDefault();
                ViewBag.Id = avaliacao.Id;
                ViewBag.IdTurma = avaliacao.IdTurma;
                ViewBag.IdSubPeriodo = avaliacao.IdSubPeriodo;
            }

            return View(result);
        }

        [HttpPost("Avaliacao/CadastroDisciplina/{idTurma}/{idSubPeriodo}")]
        [HttpPost("Avaliacao/CadastroDisciplina/{idAvaliacao}")]
        [ValidateAntiForgeryToken]
        public IActionResult CadastroDisciplina([Bind("Id,IdSubPeriodo,IdProfessorDisciplina,IdTurma")] Avaliacao mdlDados)
        {
            var idUsuario = HttpContext?.Session?.GetString("IdUsuario");
            Guid idUsuarioGuid;
            if (!Guid.TryParse(idUsuario, out idUsuarioGuid))
            {
                TempData["PaginaAnterior"] = String.Format("{0}://{1}{2}{3}", Request.Scheme, Request.Host, Request.Path, Request.QueryString);
                TempData["MensagemErro"] = "Sessão expirada";
                return RedirectToAction("Login", "Acesso");
            }
            GlResposta<Avaliacao> result = null;
            Guid id = !String.IsNullOrEmpty(Request.Form["HdnIdChave"]) ? Guid.Parse(Request.Form["HdnIdChave"]) : mdlDados.Id;
            if (id.Equals(Guid.Empty))
            {
                result = Post<Avaliacao>("Avaliacao/CriarDisciplina", mdlDados);
            }
            else
            {
                GlResposta<Avaliacao> resultAvaliacao = Get<Avaliacao>("Avaliacao/Obter", id.ToString());
                if (resultAvaliacao == null)
                {
                    TempData["CodigoErro"] = "";
                    TempData["MensagemErro"] = "Ocorreu um erro desconhecido";
                    return RedirectToAction("Erro", "Home");
                }
                if (!resultAvaliacao.Succeeded)
                {
                    ViewBag.IsSucesso = "false";
                    ViewBag.LblMensagem = resultAvaliacao.Mensagem;
                }
                else
                {
                    Avaliacao avaliacao = resultAvaliacao.Dados?.FirstOrDefault();
                    avaliacao.IdProfessorDisciplina = mdlDados.IdProfessorDisciplina;

                    result = Put<Avaliacao>("Avaliacao/Modificar", avaliacao);
                }
            }

            if (result == null)
            {
                TempData["CodigoErro"] = "";
                TempData["MensagemErro"] = "Ocorreu um erro desconhecido";
                return RedirectToAction("Erro", "Home");
            }

            ViewBag.Id = result.Succeeded ? Guid.Parse(result.Id) : id;
            ViewBag.IsSucesso = result.Succeeded.ToString();
            ViewBag.LblMensagem = result.Mensagem;

            return PartialView("JanelaMensagem");
        }

        [HttpGet("Avaliacao/{idTurma}/{idSubPeriodo}")]
        [HttpGet("Avaliacao")]
        public IActionResult Index(Guid? idTurma, Guid? idSubPeriodo)
        {
            ViewBag.IsSucesso = "";
            GlResposta<string> resultPermissao = VerificarPermissao();
            if (resultPermissao?.Succeeded != true)
            {
                ViewBag.IsSucesso = "false";
                ViewBag.LblMensagem = resultPermissao.Mensagem;

                return View();
            }
            CarregarMenuObsoleto();
            var idUsuario = HttpContext?.Session?.GetString("IdUsuario");
            var idEscola = HttpContext?.Session?.GetString("IdEscola");
            Guid idUsuarioGuid;
            if (!Guid.TryParse(idUsuario, out idUsuarioGuid))
            {
                TempData["PaginaAnterior"] = String.Format("{0}://{1}{2}{3}", Request.Scheme, Request.Host, Request.Path, Request.QueryString);
                TempData["MensagemErro"] = "Sessão expirada";
                return RedirectToAction("Login", "Acesso");
            }

            GlResposta<Periodo> resultPeriodo = GetAux<Periodo>("Servico/ObterPeriodo", idEscola);
            if (resultPeriodo.Dados != null)
            {
                ViewBag.IdPeriodo = new SelectList(resultPeriodo.Dados?.ToList(), "Id", "Nome");
                Guid idPeriodo = resultPeriodo.Dados.FirstOrDefault().Id;
                GlResposta<Turma> resultTurma = GetAux<Turma>("Servico/ObterTurma", idPeriodo.ToString());
                if (resultTurma.Dados != null)
                {
                    if (idTurma == null)
                    {
                        string idAlunoSelecionado = HttpContext?.Session?.GetString("IdAlunoSelecionado");
                        if (!String.IsNullOrWhiteSpace(idAlunoSelecionado))
                        {
                            CmAluno alunoSelecionado = ListaAlunos.Where(i => i.Id.ToString() == idAlunoSelecionado).FirstOrDefault();
                            ViewBag.IdTurma = new SelectList(resultTurma.Dados?.ToList(), "Id", "Nome", alunoSelecionado.IdTurma.ToString());
                            TempData["OcultarPanel"] = "true";
                        }
                        else
                        {
                            ViewBag.IdTurma = new SelectList(resultTurma.Dados?.ToList(), "Id", "Nome");
                        }
                    }
                    else
                    {
                        ViewBag.IdTurma = new SelectList(resultTurma.Dados?.ToList(), "Id", "Nome", idTurma.Value.ToString());
                    }
                }

                GlResposta<SubPeriodo> resultSubPeriodo = GetAux<SubPeriodo>("Servico/ObterSubPeriodo", idPeriodo.ToString());
                if (resultSubPeriodo.Dados != null)
                {
                    if (idSubPeriodo != null)
                    {
                        ViewBag.IdSubPeriodo = new SelectList(resultSubPeriodo.Dados?.ToList(), "Id", "Nome", idSubPeriodo.Value);
                    }
                    else if (String.IsNullOrEmpty(resultSubPeriodo.Id))
                    {
                        ViewBag.IdSubPeriodo = new SelectList(resultSubPeriodo.Dados?.ToList(), "Id", "Nome");
                    }
                    else
                    {
                        ViewBag.IdSubPeriodo = new SelectList(resultSubPeriodo.Dados?.ToList(), "Id", "Nome", resultSubPeriodo.Id);
                    }
                }
            }

            GlResposta<CmAvaliacao> result = new GlResposta<CmAvaliacao>();
            ViewBag.IsSucesso = "";

            return View(result);
        }

        [HttpGet("Avaliacao/Calendario/{idTurma}/{idSubPeriodo}")]
        [HttpGet("Avaliacao/Calendario")]
        public IActionResult Calendario(Guid? idTurma, Guid? idSubPeriodo)
        {
            ViewBag.IsSucesso = "";
            GlResposta<string> resultPermissao = VerificarPermissao();
            if (resultPermissao?.Succeeded != true)
            {
                ViewBag.IsSucesso = "false";
                ViewBag.LblMensagem = resultPermissao.Mensagem;

                return View();
            }
            CarregarMenuObsoleto();
            var idUsuario = HttpContext?.Session?.GetString("IdUsuario");
            var idEscola = HttpContext?.Session?.GetString("IdEscola");
            Guid idUsuarioGuid;
            if (!Guid.TryParse(idUsuario, out idUsuarioGuid))
            {
                TempData["PaginaAnterior"] = String.Format("{0}://{1}{2}{3}", Request.Scheme, Request.Host, Request.Path, Request.QueryString);
                TempData["MensagemErro"] = "Sessão expirada";
                return RedirectToAction("Login", "Acesso");
            }
                       
            GlResposta<Periodo> resultPeriodo = GetAux<Periodo>("Servico/ObterPeriodo", idEscola);
            if (resultPeriodo.Dados != null)
            {
                ViewBag.IdPeriodo = new SelectList(resultPeriodo.Dados?.ToList(), "Id", "Nome");
                Guid idPeriodo = resultPeriodo.Dados.FirstOrDefault().Id;
                GlResposta<Turma> resultTurma = GetAux<Turma>("Servico/ObterTurma", idPeriodo.ToString());
                if (resultTurma.Dados != null)
                {
                    if (idTurma == null)
                    {
                        ViewBag.IdTurma = new SelectList(resultTurma.Dados?.ToList(), "Id", "Nome");
                    }
                    else
                    {
                        ViewBag.IdTurma = new SelectList(resultTurma.Dados?.ToList(), "Id", "Nome", idTurma.Value.ToString());
                    }
                }

                GlResposta<SubPeriodo> resultSubPeriodo = GetAux<SubPeriodo>("Servico/ObterSubPeriodo", idPeriodo.ToString());
                if (resultSubPeriodo.Dados != null)
                {
                    if (idSubPeriodo != null)
                    {
                        ViewBag.IdSubPeriodo = new SelectList(resultSubPeriodo.Dados?.ToList(), "Id", "Nome", idSubPeriodo.Value);
                    }
                    else if (String.IsNullOrEmpty(resultSubPeriodo.Id))
                    {
                        ViewBag.IdSubPeriodo = new SelectList(resultSubPeriodo.Dados?.ToList(), "Id", "Nome");
                    }
                    else
                    {
                        ViewBag.IdSubPeriodo = new SelectList(resultSubPeriodo.Dados?.ToList(), "Id", "Nome", resultSubPeriodo.Id);
                    }
                }
            }

            ViewBag.IsSucesso = "";

            return View();
        }

        public IActionResult Disciplina()
        {
            ViewBag.IsSucesso = "";
            GlResposta<string> resultPermissao = VerificarPermissao();
            if (resultPermissao?.Succeeded != true)
            {
                ViewBag.IsSucesso = "false";
                ViewBag.LblMensagem = resultPermissao.Mensagem;

                return View();
            }
            CarregarMenuObsoleto();
            var idUsuario = HttpContext?.Session?.GetString("IdUsuario");
            var idEscola = HttpContext?.Session?.GetString("IdEscola");
            Guid idUsuarioGuid;
            if (!Guid.TryParse(idUsuario, out idUsuarioGuid))
            {
                TempData["PaginaAnterior"] = String.Format("{0}://{1}{2}{3}", Request.Scheme, Request.Host, Request.Path, Request.QueryString);
                TempData["MensagemErro"] = "Sessão expirada";
                return RedirectToAction("Login", "Acesso");
            }

            string idTurma = String.Empty;
            GlResposta<Periodo> resultPeriodo = GetAux<Periodo>("Servico/ObterPeriodo", idEscola);
            if (resultPeriodo.Dados != null)
            {
                ViewBag.IdPeriodo = new SelectList(resultPeriodo.Dados?.ToList(), "Id", "Nome");
                Guid idPeriodo = resultPeriodo.Dados.FirstOrDefault().Id;
                GlResposta<Turma> resultTurma = GetAux<Turma>("Servico/ObterTurma", idPeriodo.ToString());
                if (resultTurma.Dados != null)
                {
                    ViewBag.IdTurma = new SelectList(resultTurma.Dados?.ToList(), "Id", "Nome");
                }

                GlResposta<SubPeriodo> resultSubPeriodo = GetAux<SubPeriodo>("Servico/ObterSubPeriodo", idPeriodo.ToString());
                if (resultSubPeriodo.Dados != null)
                {
                    if (String.IsNullOrEmpty(resultSubPeriodo.Id))
                    {
                        ViewBag.IdSubPeriodo = new SelectList(resultSubPeriodo.Dados?.ToList(), "Id", "Nome");
                    }
                    else
                    {
                        ViewBag.IdSubPeriodo = new SelectList(resultSubPeriodo.Dados?.ToList(), "Id", "Nome", resultSubPeriodo.Id);
                    }
                }
            }

            GlResposta<CmAvaliacao> result = new GlResposta<CmAvaliacao>();
            ViewBag.IsSucesso = "";

            return View(result);
        }

        [HttpGet("Avaliacao/ImportarIndex")]
        public IActionResult ImportarIndex()
        {
            ViewBag.IsSucesso = "";
            GlResposta<string> resultPermissao = VerificarPermissao();
            if (resultPermissao?.Succeeded != true)
            {
                ViewBag.IsSucesso = "false";
                ViewBag.LblMensagem = resultPermissao.Mensagem;

                return View();
            }
            CarregarMenuObsoleto();
            var idUsuario = HttpContext?.Session?.GetString("IdUsuario");
            var idEscola = HttpContext?.Session?.GetString("IdEscola");
            Guid idUsuarioGuid;
            if (!Guid.TryParse(idUsuario, out idUsuarioGuid))
            {
                TempData["PaginaAnterior"] = String.Format("{0}://{1}{2}{3}", Request.Scheme, Request.Host, Request.Path, Request.QueryString);
                TempData["MensagemErro"] = "Sessão expirada";
                return RedirectToAction("Login", "Acesso");
            }

            string idTurma = String.Empty;
            GlResposta<Periodo> resultPeriodo = GetAux<Periodo>("Servico/ObterPeriodo", idEscola);
            if (resultPeriodo.Dados != null)
            {
                ViewBag.IdPeriodo = new SelectList(resultPeriodo.Dados?.ToList(), "Id", "Nome");
                Guid idPeriodo = resultPeriodo.Dados.FirstOrDefault().Id;
                GlResposta<Turma> resultTurma = GetAux<Turma>("Servico/ObterTurma", idPeriodo.ToString());
                if (resultTurma.Dados != null)
                {
                    ViewBag.IdTurma = new SelectList(resultTurma.Dados?.ToList(), "Id", "Nome");
                }

                GlResposta<SubPeriodo> resultSubPeriodo = GetAux<SubPeriodo>("Servico/ObterSubPeriodo", idPeriodo.ToString());
                if (resultSubPeriodo.Dados != null)
                {
                    if (String.IsNullOrEmpty(resultSubPeriodo.Id))
                    {
                        ViewBag.IdSubPeriodo = new SelectList(resultSubPeriodo.Dados?.ToList(), "Id", "Nome");
                    }
                    else
                    {
                        ViewBag.IdSubPeriodo = new SelectList(resultSubPeriodo.Dados?.ToList(), "Id", "Nome", resultSubPeriodo.Id);
                    }
                }
            }

            GlResposta<CmAvaliacao> result = new GlResposta<CmAvaliacao>();
            ViewBag.IsSucesso = "";

            return View(result);
        }

        public IActionResult Excluir(Guid? id)
        {
            var idUsuario = HttpContext?.Session?.GetString("IdUsuario");
            Guid idUsuarioGuid;
            if (!Guid.TryParse(idUsuario, out idUsuarioGuid))
            {
                TempData["PaginaAnterior"] = String.Format("{0}://{1}{2}{3}", Request.Scheme, Request.Host, Request.Path, Request.QueryString);
                TempData["MensagemErro"] = "Sessão expirada";
                return RedirectToAction("Login", "Acesso");
            }
            GlResposta<Avaliacao> result = Get<Avaliacao>("Avaliacao/Obter", id.ToString());
            if (result == null)
            {
                TempData["CodigoErro"] = "";
                TempData["MensagemErro"] = "Ocorreu um erro desconhecido";
                return RedirectToAction("Erro", "Home");
            }
            result = Put<Avaliacao>("Avaliacao/Apagar", result.Dados.FirstOrDefault());
            if (result == null)
            {
                TempData["CodigoErro"] = "";
                TempData["MensagemErro"] = "Ocorreu um erro desconhecido";
                return RedirectToAction("Erro", "Home");
            }

            ViewBag.IsSucesso = result.Succeeded.ToString();
            ViewBag.LblMensagem = result.Mensagem;

            return PartialView("JanelaMensagem");
        }
    }
}
