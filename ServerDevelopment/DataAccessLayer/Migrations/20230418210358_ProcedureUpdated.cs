using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class ProcedureUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE PROCEDURE dbo.GetCustomersByAllFields
            	@SearchTerm NVARCHAR(50) = NULL,
            	@SortColumn NVARCHAR(50) = 'Name',
            	@SortOrder NVARCHAR(50) = 'ASC',
            	@PageNumber INT = 1,
            	@PageSize INT = 10,
                @TotalRows INT OUTPUT
            AS
            BEGIN
            	SET NOCOUNT ON;
            
            	SELECT @TotalRows = COUNT(*) FROM Customers
            	WHERE (@SearchTerm IS NULL OR Name LIKE '%' + @SearchTerm + '%'
            		OR CompanyName LIKE '%' + @SearchTerm + '%'
            		OR Phone LIKE '%' + @SearchTerm + '%'
            		OR EmailAddress LIKE '%' + @SearchTerm + '%');
            
            	SELECT * FROM (
            		SELECT ROW_NUMBER() OVER (ORDER BY
            			CASE WHEN @SortColumn = 'Name' AND @SortOrder = 'ASC' THEN Name END ASC,
            			CASE WHEN @SortColumn = 'Name' AND @SortOrder = 'DESC' THEN Name END DESC,
            			CASE WHEN @SortColumn = 'CompanyName' AND @SortOrder = 'ASC' THEN CompanyName END ASC,
            			CASE WHEN @SortColumn = 'CompanyName' AND @SortOrder = 'DESC' THEN CompanyName END DESC,
            			CASE WHEN @SortColumn = 'Phone' AND @SortOrder = 'ASC' THEN Phone END ASC,
            			CASE WHEN @SortColumn = 'Phone' AND @SortOrder = 'DESC' THEN Phone END DESC,
            			CASE WHEN @SortColumn = 'EmailAddress' AND @SortOrder = 'ASC' THEN EmailAddress END ASC,
            			CASE WHEN @SortColumn = 'EmailAddress' AND @SortOrder = 'DESC' THEN EmailAddress END DESC
            		) AS RowNum, * FROM Customers
            		WHERE (@SearchTerm IS NULL OR Name LIKE '%' + @SearchTerm + '%'
            			OR CompanyName LIKE '%' + @SearchTerm + '%'
            			OR Phone LIKE '%' + @SearchTerm + '%'
            			OR EmailAddress LIKE '%' + @SearchTerm + '%')
            	) AS CustomersWithRowNumbers
            	ORDER BY RowNum
            	OFFSET (@PageNumber - 1) * @PageSize ROWS
            	FETCH NEXT @PageSize ROWS ONLY;
            
            	SELECT @TotalRows AS TotalRows;
            END
            
            ");
            migrationBuilder.Sql("DROP PROCEDURE dbo.GetCustomersWithSortingAndPaging");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE dbo.GetCustomersByAllFields");
        }
    }
}
