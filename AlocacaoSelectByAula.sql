ALTER PROCEDURE [dbo].[AlocacaoSelectByAula]
	
	@Aula uniqueidentifier,
	@Data datetime,
	@Horario nvarchar(2)


AS
BEGIN
if @Horario = 'E'
		set @Horario = 'EE'
	SET NOCOUNT ON;

	Select

--Alocacao
Alocacao.RecursoId,
Alocacao.Data,
Alocacao.Horario,
Alocacao.AulaId,
Alocacao.EventoId,

--Recursos
Recursos.Descricao,
Recursos.EstaDisponivel,

--Categorias Recurso
Recursos.CategoriaId,
CategoriasRecurso.Descricao as CatDescricao,

--Faculdades
Recursos.Vinculo,
Faculdades.Nome  as FaculdadeNome,
Faculdades.FaculdadeId

from Alocacao 
Inner join Recursos on (Alocacao.RecursoId = Recursos.RecursoId)
Inner join CategoriasRecurso on (CategoriasRecurso.CategoriaRecursoId = Recursos.CategoriaId)
Inner join Faculdades on (Faculdades.FaculdadeId = Recursos.Vinculo)

WHERE   data = @Data 
and horario = SUBSTRING(@Horario,1,1)
and Recursos.EstaDisponivel = 'TRUE'
and AulaId = @Aula

and  Alocacao.RecursoId in ( SELECT Alocacao.RecursoId FROM Alocacao WHERE 
								data = @Data and horario = SUBSTRING(@Horario,2,1))
order by Recursos.Descricao
	
END
