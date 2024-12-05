using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories;
using MentalHealthcare.Infrastructure.Persistence;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MentalHealthcare.Infrastructure.Repositories
{
    public class ArticleRepository(MentalHealthDbContext dbContext
        , ILogger<ArticleRepository> logger) : IArticleRepository
    {


        /// <summary>
        /// Creates a new article and saves it to the database.
        /// </summary>
        /// <param name="Article">The article entity to create.</param>
        /// <returns>The ID of the created article.</returns>
        /// <exception cref="DbUpdateException">Thrown when there is an error updating the database, including foreign key violations.</exception>
        /// <exception cref="Exception">Thrown when there is a general error.</exception>
        public async Task<int> CreateAsync(Article Article)
        {
            try
            {
                await dbContext.Articles.AddAsync(Article);
                await dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlException)
            {
                // TODO: Check for specific foreign key constraint violations
                foreach (SqlError error in sqlException.Errors)
                {
                    // TODO: SQL Server foreign key violation error number
                    if (error.Number == 547) // Foreign key violation error number
                    {
                        logger.LogError(ex, "Foreign key violation: {Message}", error.Message);
                        throw new ResourceNotFound(nameof(Author), Article.AuthorId.ToString());
                    }
                }

                // TODO: Log error details for further investigation
                logger.LogError(ex, "An error occurred while saving the Article.");
                throw;
            }
            catch (Exception ex)
            {
                // TODO: Log general exceptions for monitoring and debugging
                logger.LogError(ex, "An error occurred while saving the Article.");
                throw;
            }

            return Article.ArticleId;
        }

        /// <summary>
        /// Deletes the specified article from the database.
        /// </summary>
        /// <param name="article">The article to be deleted.</param>
        /// <returns>A task representing the asynchronous operation.
        /// The task result contains a string indicating the result of the operation.</returns>
        /// <remarks>
        /// TODO:
        /// - Add validation to check if the article exists before attempting to delete.
        /// - Consider implementing logging for successful and failed operations.
        /// </remarks>
        public async Task DeleteArticleAsync(int ID_Article)
        {

            try
            {
                var Selected_Article = await dbContext.Articles.FindAsync(ID_Article); 
                if (Selected_Article == null)
                { throw new ResourceNotFound(nameof(Article), ID_Article.ToString()); }
                dbContext.Articles.Remove(Selected_Article); await dbContext.SaveChangesAsync(); 

            }
            catch (Exception ex)
            {
             throw new Exception($"Failed Process! Error: {ex.Message}", ex); }
            }


        /// <summary>
        /// Retrieves a paginated and sorted list of articles based on search criteria.
        /// </summary>
        /// <param name="search">The search term to filter articles by title.</param>
        /// <param name="requestPageNumber">The page number for pagination.</param>
        /// <param name="requestPageSize">The number of articles per page.</param> 
        /// <param name="sortBy">The field to sort the articles by (e.g., "title", "date").</param>
        public async Task<(int, IEnumerable<Article>)> GetAllArticlesAsync(string? search, int requestPageNumber, int requestPageSize, string? sortBy)
        {
            var query = dbContext.Articles.AsQueryable();
            if (!string.IsNullOrEmpty(search)) 
            { query = query.Where(a => a.Title.Contains(search) || a.Content.Contains(search)); }


            switch (sortBy)
            {
                case "Title":
                    query = query.OrderBy(a => a.Title); break;
                case "CreatedDate":
                    query = query.OrderBy(a => a.CreatedDate); break;
                    // Add more cases as needed
                    default: query = query.OrderBy(a => a.ArticleId); break; }

          //TODO : Pagination validation
                    if (requestPageNumber < 1) requestPageNumber = 1;
            if (requestPageSize < 1) requestPageSize = 10; // Default page size

            search ??= string.Empty;
            search = search.ToLower();
            var baseQuery = dbContext.Articles
                .Where(r => r.Title.ToLower().Contains(search));
            //TODO : Apply sorting
            baseQuery = sortBy switch
            {
                "title" => baseQuery.OrderBy(r => r.Title),// Default sorting
                "date" => baseQuery.OrderBy(r => r.CreatedDate),
                _ => baseQuery.OrderBy(r => r.Title)
            };



            var totalCount = await baseQuery.CountAsync();
            var articles = await baseQuery
                .Skip(requestPageSize * (requestPageNumber - 1))
                .Take(requestPageSize)
                .ToListAsync();
            return (totalCount, articles);
        }

        /// <summary> ///
        /// Retrieves an article by its ID. ///
        /// </summary> ///
        /// <param name="Id">The ID of the article to retrieve.</param> ///
        /// <returns>The article that matches the given ID.</returns> /// 
        /// <exception cref="ResourceNotFound">
        /// Thrown when the article is not found.</exception>
        public async Task<Article> GetArticleByIdAsync(int Id)
        {
            var article = await dbContext.Articles
                .Where(A => A.ArticleId == Id)
                .Select(A => new Article {
                    ArticleId = A.ArticleId,
                    Title = A.Title,
                    Author = A.Author,
                    AuthorId = A.AuthorId,
                    Content = A.Content,
                    CreatedDate = A.CreatedDate,
                    PhotoUrl =A.PhotoUrl

                }).FirstOrDefaultAsync();
               
            if (article == null)
            {
                throw new ResourceNotFound(nameof(Article), Id.ToString());
            }

            return article;
        }

        /// <summary>
        /// Retrieves an article by its ID and title. 
        ///</summary>
        ///<param name="ArticleID">The ID of the article.</param>
        ///<param name="Article_title">The title of the article.</param> 
        ///<returns>The article matching the given ID and title.</returns>
        public async Task<Article> GetByTitleAsync(int? ArticleID, string Article_title)
        {
            var Article = dbContext.Articles
                  .Include(r => r.Title.ToLower().Contains(Article_title))
                  .FirstOrDefault(P => P.ArticleId == ArticleID);
            if (Article == null)
            { throw new ResourceNotFound(nameof(Podcast), Article_title); }
            return Article;
        }

        /// <summary>
        /// Checks if an article with the given title exists.
        /// </summary>
        /// <param name="title">The title of the article to check for existence.</param>
        /// <returns>True if an article with the given title exists, otherwise false.</returns>
        public async Task<bool> IsExistByTitle(string title)
        {
            //TODO : Validate the title parameter
            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentException("Title must not be null or empty.", nameof(title));
            }

            //TODO : Use a more efficient AnyAsync to check for existence and ensure case-insensitivity
            return await dbContext.Articles
                .AnyAsync(a => a.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
        }
        
        /// <summary>
        /// Updates an existing article.
        /// </summary>
        /// <param name="articles">The article entity with updated information.</param>
        /// <returns>A success message indicating the article has been updated.</returns>
        /// <exception cref="ArgumentException">Thrown when the article entity is null.</exception>
        /// <exception cref="DbUpdateException">Thrown when the update operation fails.</exception>
        public async Task<string> UpdateArticleAsync(Article articles)
        {
           

            // TODO: Add logging to capture the start of the update operation
            try
            {
                dbContext.Articles.Update(articles);
                await dbContext.SaveChangesAsync(); // Ensure changes are saved to the database

                // TODO: Log the successful update operation
                return "The Article has been updated successfully!";
            }
            catch (DbUpdateException ex)
            {
                // TODO: Log the exception details for debugging and monitoring
                throw new DbUpdateException("An error occurred while updating the article.", ex);
            }




        }
    }
}




/*
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 */