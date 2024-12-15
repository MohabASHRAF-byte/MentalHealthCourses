using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories;
using MentalHealthcare.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MentalHealthcare.Infrastructure.Repositories
{
    public class ArticleRepository(
        MentalHealthDbContext dbContext
        ) : IArticleRepository
    {
        public async Task<int> CreateAsync(Article Article)
        {
            await dbContext.Articles.AddAsync(Article);
            await dbContext.SaveChangesAsync();
            return Article.ArticleId;
        }
        public async Task DeleteArticleAsync(int ID_Article)
        {
            var Selected_Article = await dbContext.Articles.FindAsync(ID_Article);
            if (Selected_Article == null) { throw new ResourceNotFound(nameof(Article), ID_Article.ToString()); }
            dbContext.Articles.Remove(Selected_Article); await dbContext.SaveChangesAsync();
        }
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
                default: query = query.OrderBy(a => a.ArticleId); break;
            }

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
        public async Task<Article> GetArticleByIdAsync(int Id)
        {
            var article = await dbContext.Articles
                .Where(A => A.ArticleId == Id)
                .Select(A => new Article
                {
                    ArticleId = A.ArticleId,
                    Title = A.Title,
                    Author = A.Author,
                    AuthorId = A.AuthorId,
                    Content = A.Content,
                    CreatedDate = A.CreatedDate,
                    PhotoUrl = A.PhotoUrl

                }).FirstOrDefaultAsync();

            if (article == null)
            {
                throw new ResourceNotFound(nameof(Article), Id.ToString());
            }

            return article;
        }
        public async Task UpdateArticleAsync(Article articles)
        {
            dbContext.Articles.Update(articles);
            await dbContext.SaveChangesAsync();
        }
    }
}




