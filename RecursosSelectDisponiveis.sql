	ALTER PROCEDURE [dbo].[RecursosSelectDisponiveis] 
		
		@Data datetime,
		@Horario nvarchar(1)
		
	AS
	BEGIN
		SET NOCOUNT ON;
		SELECT Recursos.RecursoId, Recursos.CategoriaId, Recursos.Descricao,
		Recursos.Vinculo, Recursos.EstaDisponivel
		
		FROM Recursos
		INNER JOIN Alocacao on Recursos.RecursoId = Alocacao.RecursoId
		WHERE Alocacao.Data = @Data
		and
		Alocacao.Horario = @Horario
		and
		Alocacao.AulaId is NULL
		and
		Alocacao.EventoId is NULL
		and
		Recursos.EstaDisponivel = 'True'
		
END