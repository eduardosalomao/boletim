using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Modelo.SchoolUp.Principal;

namespace Acesso.SchoolUp.Contexts
{
    public partial class SchoolContext : DbContext
    {
        public SchoolContext()
        {
        }

        public SchoolContext(DbContextOptions<SchoolContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Aluno> Aluno { get; set; }
        public virtual DbSet<AlunoTurma> AlunoTurma { get; set; }
        public virtual DbSet<AreaConhecimento> AreaConhecimento { get; set; }
        public virtual DbSet<Avaliacao> Avaliacao { get; set; }
        public virtual DbSet<Diario> Diario { get; set; }
        public virtual DbSet<Disciplina> Disciplina { get; set; }
        public virtual DbSet<DisciplinaHorario> DisciplinaHorario { get; set; }
        public virtual DbSet<Empresa> Empresa { get; set; }
        public virtual DbSet<Ensino> Ensino { get; set; }
        public virtual DbSet<Escola> Escola { get; set; }
        public virtual DbSet<EscolaProfessor> EscolaProfessor { get; set; }
        public virtual DbSet<EscolaUsuario> EscolaUsuario { get; set; }
        public virtual DbSet<HorarioTurno> HorarioTurno { get; set; }
        public virtual DbSet<Inscricao> Inscricao { get; set; }
        public virtual DbSet<Notas> Notas { get; set; }
        public virtual DbSet<Periodo> Periodo { get; set; }
        public virtual DbSet<Professor> Professor { get; set; }
        public virtual DbSet<ProfessorDisciplina> ProfessorDisciplina { get; set; }
        public virtual DbSet<ProfessorDisponibilidade> ProfessorDisponibilidade { get; set; }
        public virtual DbSet<Responsavel> Responsavel { get; set; }
        public virtual DbSet<ResponsavelAluno> ResponsavelAluno { get; set; }
        public virtual DbSet<Serie> Serie { get; set; }
        public virtual DbSet<SubPeriodo> SubPeriodo { get; set; }
        public virtual DbSet<TipoAvaliacao> TipoAvaliacao { get; set; }
        public virtual DbSet<TipoRelacao> TipoRelacao { get; set; }
        public virtual DbSet<Turma> Turma { get; set; }
        public virtual DbSet<Turno> Turno { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var _configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
                optionsBuilder.UseSqlServer(_configuration.GetConnectionString("ConexaoSchoolUp"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Aluno>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Cpf).HasMaxLength(14);

                entity.Property(e => e.DataNascimento).HasColumnType("smalldatetime");

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.Matricula).HasMaxLength(20);

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.IdEscolaNavigation)
                    .WithMany(p => p.Aluno)
                    .HasForeignKey(d => d.IdEscola)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Aluno_Escola");
            });

            modelBuilder.Entity<AlunoTurma>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.HasOne(d => d.IdAlunoNavigation)
                    .WithMany(p => p.AlunoTurma)
                    .HasForeignKey(d => d.IdAluno)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AlunoTurma_Aluno");

                entity.HasOne(d => d.IdTurmaNavigation)
                    .WithMany(p => p.AlunoTurma)
                    .HasForeignKey(d => d.IdTurma)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AlunoTurma_Turma");
            });

            modelBuilder.Entity<AreaConhecimento>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Codigo).HasMaxLength(20);

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Avaliacao>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Ate).HasColumnType("smalldatetime");

                entity.Property(e => e.De).HasColumnType("smalldatetime");

                entity.Property(e => e.Peso).HasColumnType("decimal(6, 2)");

                entity.Property(e => e.Sigla)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.HasOne(d => d.IdProfessorDisciplinaNavigation)
                    .WithMany(p => p.Avaliacao)
                    .HasForeignKey(d => d.IdProfessorDisciplina)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Avaliacao_ProfessorDisciplina");

                entity.HasOne(d => d.IdSubPeriodoNavigation)
                    .WithMany(p => p.Avaliacao)
                    .HasForeignKey(d => d.IdSubPeriodo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Avaliacao_SubPeriodo");

                entity.HasOne(d => d.IdTipoAvaliacaoNavigation)
                    .WithMany(p => p.Avaliacao)
                    .HasForeignKey(d => d.IdTipoAvaliacao)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Avaliacao_TipoAvaliacao");

                entity.HasOne(d => d.IdTurmaNavigation)
                    .WithMany(p => p.Avaliacao)
                    .HasForeignKey(d => d.IdTurma)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Avaliacao_Turma");
            });

            modelBuilder.Entity<Disciplina>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Codigo).HasMaxLength(20);

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.IdAreaNavigation)
                    .WithMany(p => p.Disciplina)
                    .HasForeignKey(d => d.IdArea)
                    .HasConstraintName("FK_Disciplina_AreaConhecimento");

                entity.HasOne(d => d.IdEnsinoNavigation)
                    .WithMany(p => p.Disciplina)
                    .HasForeignKey(d => d.IdEnsino)
                    .HasConstraintName("FK_Disciplina_Ensino");

                entity.HasOne(d => d.IdEscolaNavigation)
                    .WithMany(p => p.Disciplina)
                    .HasForeignKey(d => d.IdEscola)
                    .HasConstraintName("FK_Disciplina_Escola");
            });

            modelBuilder.Entity<DisciplinaHorario>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.HasOne(d => d.IdDisciplinaNavigation)
                    .WithMany(p => p.DisciplinaHorario)
                    .HasForeignKey(d => d.IdDisciplina)
                    .HasConstraintName("FK_DisciplinaHorario_Disciplina");

                entity.HasOne(d => d.IdHorarioTurnoNavigation)
                    .WithMany(p => p.DisciplinaHorario)
                    .HasForeignKey(d => d.IdHorarioTurno)
                    .HasConstraintName("FK_DisciplinaHorario_HorarioTurno");

                entity.HasOne(d => d.IdProfessorNavigation)
                    .WithMany(p => p.DisciplinaHorario)
                    .HasForeignKey(d => d.IdProfessor)
                    .HasConstraintName("FK_DisciplinaHorario_Professor");

                entity.HasOne(d => d.IdTurmaNavigation)
                    .WithMany(p => p.DisciplinaHorario)
                    .HasForeignKey(d => d.IdTurma)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DisciplinaHorario_Turma");
            });

            modelBuilder.Entity<Empresa>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Cnpj)
                    .IsRequired()
                    .HasMaxLength(18);

                entity.Property(e => e.Codigo).HasMaxLength(20);

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.Facebook).HasMaxLength(100);

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.NomeCurto)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.RazaoSocial)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Site).HasMaxLength(50);

                entity.Property(e => e.Telefone).HasMaxLength(20);

                entity.Property(e => e.Twitter).HasMaxLength(100);

                entity.Property(e => e.WhatsApp).HasMaxLength(20);
            });

            modelBuilder.Entity<Ensino>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Codigo).HasMaxLength(20);

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Escola>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Cnpj)
                    .IsRequired()
                    .HasMaxLength(18);

                entity.Property(e => e.Codigo).HasMaxLength(20);

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.Facebook).HasMaxLength(100);

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.NomeCurto)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.RazaoSocial)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Site).HasMaxLength(50);

                entity.Property(e => e.Telefone).HasMaxLength(20);

                entity.Property(e => e.Twitter).HasMaxLength(100);

                entity.Property(e => e.WhatsApp).HasMaxLength(20);

                entity.HasOne(d => d.IdEmpresaNavigation)
                    .WithMany(p => p.Escola)
                    .HasForeignKey(d => d.IdEmpresa)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Escola_Empresa");
            });

            modelBuilder.Entity<EscolaProfessor>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Ate).HasColumnType("smalldatetime");

                entity.Property(e => e.De).HasColumnType("smalldatetime");

                entity.Property(e => e.Matricula).HasMaxLength(20);

                entity.HasOne(d => d.IdEscolaNavigation)
                    .WithMany(p => p.EscolaProfessor)
                    .HasForeignKey(d => d.IdEscola)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EscolaProfessor_Escola");

                entity.HasOne(d => d.IdProfessorNavigation)
                    .WithMany(p => p.EscolaProfessor)
                    .HasForeignKey(d => d.IdProfessor)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EscolaProfessor_Professor");
            });

            modelBuilder.Entity<EscolaUsuario>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.HasOne(d => d.IdEscolaNavigation)
                    .WithMany(p => p.EscolaUsuario)
                    .HasForeignKey(d => d.IdEscola)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EscolaUsuario_Escola");
            });

            modelBuilder.Entity<HorarioTurno>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Codigo).HasMaxLength(20);

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.IdTurnoNavigation)
                    .WithMany(p => p.HorarioTurno)
                    .HasForeignKey(d => d.IdTurno)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HorarioTurno_Turno");
            });

            modelBuilder.Entity<Inscricao>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.DataInscricao).HasColumnType("smalldatetime");

                entity.HasOne(d => d.IdAlunoNavigation)
                    .WithMany(p => p.Inscricao)
                    .HasForeignKey(d => d.IdAluno)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Inscricao_Aluno");

                entity.HasOne(d => d.IdDisciplinaNavigation)
                    .WithMany(p => p.Inscricao)
                    .HasForeignKey(d => d.IdDisciplina)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Inscricao_Disciplina");

                entity.HasOne(d => d.IdTurmaNavigation)
                    .WithMany(p => p.Inscricao)
                    .HasForeignKey(d => d.IdTurma)
                    .HasConstraintName("FK_Inscricao_Turma");
            });

            modelBuilder.Entity<Notas>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Nota).HasColumnType("decimal(6, 2)");

                entity.Property(e => e.NotaRecuperacao).HasColumnType("decimal(6, 2)");

                entity.HasOne(d => d.IdAvaliacaoNavigation)
                    .WithMany(p => p.Notas)
                    .HasForeignKey(d => d.IdAvaliacao)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Notas_Avaliacao");

                entity.HasOne(d => d.IdInscricaoNavigation)
                    .WithMany(p => p.Notas)
                    .HasForeignKey(d => d.IdInscricao)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Notas_Inscricao");
            });

            modelBuilder.Entity<Periodo>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Ate).HasColumnType("smalldatetime");

                entity.Property(e => e.Codigo).HasMaxLength(20);

                entity.Property(e => e.De).HasColumnType("smalldatetime");

                entity.Property(e => e.MediaAprovacao).HasColumnType("decimal(6, 2)");

                entity.Property(e => e.MediaAprovacaoFinal).HasColumnType("decimal(6, 2)");

                entity.Property(e => e.MediaReprovacao).HasColumnType("decimal(6, 2)");

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.HasOne(d => d.IdEscolaNavigation)
                    .WithMany(p => p.Periodo)
                    .HasForeignKey(d => d.IdEscola)
                    .HasConstraintName("FK_Periodo_Escola");
            });

            modelBuilder.Entity<Professor>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<ProfessorDisciplina>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Ate).HasColumnType("smalldatetime");

                entity.Property(e => e.De).HasColumnType("smalldatetime");

                entity.HasOne(d => d.IdDisciplinaNavigation)
                    .WithMany(p => p.ProfessorDisciplina)
                    .HasForeignKey(d => d.IdDisciplina)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProfessorDisciplina_Disciplina");

                entity.HasOne(d => d.IdEscolaProfessorNavigation)
                    .WithMany(p => p.ProfessorDisciplina)
                    .HasForeignKey(d => d.IdEscolaProfessor)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProfessorDisciplina_EscolaProfessor");
            });

            modelBuilder.Entity<ProfessorDisponibilidade>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.HasOne(d => d.IdProfessorNavigation)
                    .WithMany(p => p.ProfessorDisponibilidade)
                    .HasForeignKey(d => d.IdProfessor)
                    .HasConstraintName("FK_ProfessorDisponibilidade_Professor");
            });

            modelBuilder.Entity<Responsavel>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Cpf).HasMaxLength(14);

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.Identidade).HasMaxLength(20);

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.OrgaoIdentidade).HasMaxLength(10);

                entity.Property(e => e.Telefone).HasMaxLength(20);

                entity.Property(e => e.UfIdentidade)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.WhatsApp).HasMaxLength(20);
            });

            modelBuilder.Entity<ResponsavelAluno>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.HasOne(d => d.IdAlunoNavigation)
                    .WithMany(p => p.ResponsavelAluno)
                    .HasForeignKey(d => d.IdAluno)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ResponsavelAluno_Aluno");

                entity.HasOne(d => d.IdRelacaoNavigation)
                    .WithMany(p => p.ResponsavelAluno)
                    .HasForeignKey(d => d.IdRelacao)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ResponsavelAluno_TipoRelacao");

                entity.HasOne(d => d.IdResponsavelNavigation)
                    .WithMany(p => p.ResponsavelAluno)
                    .HasForeignKey(d => d.IdResponsavel)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ResponsavelAluno_Responsavel");
            });

            modelBuilder.Entity<Serie>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Codigo).HasMaxLength(20);

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.IdEnsinoNavigation)
                    .WithMany(p => p.Serie)
                    .HasForeignKey(d => d.IdEnsino)
                    .HasConstraintName("FK_Serie_Ensino");
            });

            modelBuilder.Entity<SubPeriodo>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Ate).HasColumnType("smalldatetime");

                entity.Property(e => e.Codigo).HasMaxLength(20);

                entity.Property(e => e.De).HasColumnType("smalldatetime");

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.HasOne(d => d.IdPeriodoNavigation)
                    .WithMany(p => p.SubPeriodo)
                    .HasForeignKey(d => d.IdPeriodo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SubPeriodo_Periodo");
            });

            modelBuilder.Entity<TipoAvaliacao>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Codigo).HasMaxLength(20);

                entity.Property(e => e.Descricao).HasMaxLength(1000);

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.HasOne(d => d.IdEscolaNavigation)
                    .WithMany(p => p.TipoAvaliacao)
                    .HasForeignKey(d => d.IdEscola)
                    .HasConstraintName("FK_TipoAvaliacao_Escola");
            });

            modelBuilder.Entity<TipoRelacao>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Codigo).HasMaxLength(20);

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Turma>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Codigo).HasMaxLength(20);

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.IdPeriodoNavigation)
                    .WithMany(p => p.Turma)
                    .HasForeignKey(d => d.IdPeriodo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Turma_Periodo");

                entity.HasOne(d => d.IdSerieNavigation)
                    .WithMany(p => p.Turma)
                    .HasForeignKey(d => d.IdSerie)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Turma_Serie");

                entity.HasOne(d => d.IdTurnoNavigation)
                    .WithMany(p => p.Turma)
                    .HasForeignKey(d => d.IdTurno)
                    .HasConstraintName("FK_Turma_Turno");
            });

            modelBuilder.Entity<Turno>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Codigo).HasMaxLength(20);

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.IdEscolaNavigation)
                    .WithMany(p => p.Turno)
                    .HasForeignKey(d => d.IdEscola)
                    .HasConstraintName("FK_Turno_Escola");
            });
        }
    }
}
