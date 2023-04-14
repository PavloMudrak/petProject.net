using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddStoredProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
			CREATE PROCEDURE dbo.GetCustomersWithSortingAndPaging
				@SearchTerm NVARCHAR(50) = '',
				@SortColumn NVARCHAR(50) = 'Name',
				@SortOrder NVARCHAR(50) = 'ASC',
				@PageNumber INT = 1,
				@PageSize INT = 10
			AS
			BEGIN
				SET NOCOUNT ON;

				DECLARE @TotalRows INT;

				SELECT @TotalRows = COUNT(*) FROM Customers
				WHERE (@SearchTerm = '' OR Name LIKE '%' + @SearchTerm + '%' OR CompanyName LIKE '%' + @SearchTerm + '%' OR Phone LIKE '%' + @SearchTerm + '%' OR Email LIKE '%' + @SearchTerm + '%');

				SELECT * FROM (
					SELECT ROW_NUMBER() OVER (ORDER BY
						CASE WHEN @SortColumn = 'Name' AND @SortOrder = 'ASC' THEN Name END ASC,
						CASE WHEN @SortColumn = 'Name' AND @SortOrder = 'DESC' THEN Name END DESC,
						CASE WHEN @SortColumn = 'CompanyName' AND @SortOrder = 'ASC' THEN CompanyName END ASC,
						CASE WHEN @SortColumn = 'CompanyName' AND @SortOrder = 'DESC' THEN CompanyName END DESC,
						CASE WHEN @SortColumn = 'Phone' AND @SortOrder = 'ASC' THEN Phone END ASC,
						CASE WHEN @SortColumn = 'Phone' AND @SortOrder = 'DESC' THEN Phone END DESC,
						CASE WHEN @SortColumn = 'Email' AND @SortOrder = 'ASC' THEN Email END ASC,
						CASE WHEN @SortColumn = 'Email' AND @SortOrder = 'DESC' THEN Email END DESC
					) AS RowNum, * FROM Customers
					WHERE (@SearchTerm = '' OR Name LIKE '%' + @SearchTerm + '%' OR CompanyName LIKE '%' + @SearchTerm + '%' OR Phone LIKE '%' + @SearchTerm + '%' OR Email LIKE '%' + @SearchTerm + '%')
			) AS CustomersWithRowNumbers
			WHERE RowNum BETWEEN ((@PageNumber - 1) * @PageSize) + 1 AND @PageNumber * @PageSize
			ORDER BY RowNum;
	
			SELECT @TotalRows AS TotalRows;
			END");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE dbo.GetCustomersWithSortingAndPaging");
        }
    }
}
