USE SARCFACIN
SELECT     CategoriasRecurso.CategoriaRecursoId, CategoriasRecurso.Descricao, count(CategoriasRecurso.CategoriaRecursoId) as TOTAL
FROM         Alocacao INNER JOIN
                      Recursos ON Alocacao.RecursoId = Recursos.RecursoId INNER JOIN
                      CategoriasRecurso ON Recursos.CategoriaId = CategoriasRecurso.CategoriaRecursoId
where Alocacao.AulaId is not null                      
group by CategoriasRecurso.Descricao, CategoriasRecurso.CategoriaRecursoId
order by TOTAL desc