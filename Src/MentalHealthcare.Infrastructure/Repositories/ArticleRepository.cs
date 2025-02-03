using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories;
using MentalHealthcare.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Infrastructure.Repositories
{
    public class ArticleRepository(MentalHealthDbContext dbContext) : IArticleRepository
    {
        public async Task<int> CreateArticleAsync(Article Article)
        {

          await  dbContext.Articles.AddAsync(Article);    

          await  dbContext.SaveChangesAsync();

            return Article.ArticleId;


        }

        public async Task DeleteArticleAsync(int ID_Article)
        {
            var Article = await dbContext.Articles.FindAsync(ID_Article);
            if (Article == null)
                throw new ResourceNotFound(nameof(Article), ID_Article.ToString());
            dbContext.Articles.Remove(Article);
            await dbContext.SaveChangesAsync(); }

        public async Task DeleteArticlePhotosUrlsAsync(int articleId)
        {var adimgs = dbContext.ArticleImageUrls
   .Where(img => img.ArticleId == articleId);
            dbContext.ArticleImageUrls.RemoveRange(adimgs);
            await dbContext.SaveChangesAsync();}

        public async Task<(int, IEnumerable<Article>)> GetAllArticlesAsync(string? search, int pageNumber, int pageSize)
        {var baseQuery = dbContext.Articles.AsQueryable();
          // Add Include after filtering
            baseQuery = baseQuery.Include(ad => ad.ArticleImageUrls);
         // Total count before pagination
            var totalCount = await baseQuery.CountAsync();
         // Apply ordering and pagination
            var Articles = await baseQuery
                .OrderBy(ad => ad.ArticleId) // Order by ID
                .Skip(pageSize * (pageNumber - 1)) // Pagination: Skip
                .Take(pageSize) // Pagination: Take
                .Select(ad => new Article
                { ArticleId = ad.ArticleId,
                  Title = ad.Title,
                  Content = ad.Content,
                  CreatedDate = ad.CreatedDate,
                  Author = new Author{Name = ad.Author.Name},
                  ArticleImageUrls = ad.ArticleImageUrls 
                 .Select(img => new ArticleImageUrl { ImageUrl = img.ImageUrl }) .ToList(),
                }).ToListAsync(); return (totalCount, Articles); }

        public async Task<Article> GetArticleByIdAsync(int Id)
        { var article = await dbContext.Articles
    .Where(a => a.ArticleId == Id)
    .Select(a => new Article
    {
        ArticleId = a.ArticleId,
        Title = a.Title,
        Content = a.Content,
        CreatedDate = a.CreatedDate,
        LastUploadImgCnt = a.LastUploadImgCnt,
        ArticleImageUrls = a.ArticleImageUrls
            .Select(img => new ArticleImageUrl { ImageUrl = img.ImageUrl })
            .ToList(),
        Author = new Author
        {
            AuthorId = a.Author.AuthorId,
            Name = a.Author.Name,
            ImageUrl = a.Author.ImageUrl,
            About = a.Author.About
        }  })
    .FirstOrDefaultAsync();

            if (article == null)
                throw new ResourceNotFound(nameof(Article), Id.ToString());return article;}

        public async Task UpdateArticleAsync(Article articles)
        {

          dbContext.Articles.Update(articles);

           await dbContext.SaveChangesAsync();    
        }
    }
}